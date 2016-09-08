using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Floxdc.Cadbury
{
    internal class Core
    {
        internal Core(ILoggerFactory loggerFactory)
        {
            _httpClient = new HttpClient { MaxResponseContentBufferSize = 1 };

            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger("Cadbury");
        }

        internal async Task Run(string[] args)
        {
            var taskNumber = Environment.ProcessorCount;
            var path = Utilites.GetCsvPath(args);

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("No path specified!");

            var fileProcessor = new FileProcessor(_loggerFactory);
            var urls = await fileProcessor.ReadCsv(path);

            foreach (var url in urls)
                InitialUrls.Add(url);

            var tasks = new List<Task>();

            for (var i = 0; i < taskNumber; i++)
            {
                var task = Task.Run(async () => await CheckUrls());
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            await fileProcessor.WriteCsv(TestedUrls.ToArray(), path);

            Console.WriteLine("Done.");
            Console.ReadLine();
        }


        private async Task CheckUrls()
        {
            var client = new NetClient(_httpClient, _loggerFactory);

            TargetUrl url;
            if (InitialUrls.TryTake(out url))
            {
                var checkResult = await client.CheckUrl(url);
                url.IsValid = checkResult.Key;
                url.Comment = checkResult.Value;

                TestedUrls.Add(url);

                _logger.LogTrace($"New URL state: {url.Target} -> {url.IsValid} {url.Comment}");
            }
        }


        private static readonly ConcurrentBag<TargetUrl> InitialUrls = new ConcurrentBag<TargetUrl>(); 
        private static readonly ConcurrentBag<TargetUrl> TestedUrls = new ConcurrentBag<TargetUrl>(); 

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
    }
}
