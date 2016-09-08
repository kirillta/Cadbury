using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Floxdc.Cadbury
{
    internal class FileProcessor
    {
        internal FileProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FileProcessor>();

        }


        internal async Task<TargetUrl[]> ReadCsv(string path)
        {
            var urls = new List<TargetUrl>();

            var stream = new FileStream(path, FileMode.Open);
            using (var reader = new StreamReader(stream, Encoding.Unicode))
            {
                string line;
                await reader.ReadLineAsync();

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var targetUrl = GetTargetUrl(line);
                    urls.Add(targetUrl);
                    _logger.LogTrace($"Loaded: {targetUrl.Target} : {targetUrl.IsValid}");
                }
            }

            return urls.ToArray();
        }


        internal async Task WriteCsv(TargetUrl[] targetUrls, string path)
        {
            var root = Path.GetDirectoryName(path);
            var exstrnsion = Path.GetExtension(path);
            var name = Path.GetFileNameWithoutExtension(path) + DateTime.Now.ToString(CultureInfo.InvariantCulture);

            name = Path.GetInvalidFileNameChars()
                .Select(c => c.ToString())
                .Aggregate(name, (target, invalidSymbol) => target.Replace(invalidSymbol, ""));

            var newPath = Path.Combine(root, Path.ChangeExtension(name, exstrnsion));

            var stream = new FileStream(newPath, FileMode.OpenOrCreate);

            using (var writer = new StreamWriter(stream, Encoding.Unicode))
            {
                await writer.WriteLineAsync("Target,IsValid,Comment");

                foreach (var url in targetUrls)
                    await writer.WriteLineAsync($"{url.Target},{url.IsValid},{url.Comment}");
            }
        }


        private TargetUrl GetTargetUrl(string parsedLine)
        {
            var segments = parsedLine.Split(',');
            segments[0] = segments[0].Trim();

            if (segments.Length == 2)
            {
                segments[1] = segments[1].Trim();

                bool isValid;
                var isParsed = bool.TryParse(segments[1], out isValid);

                if (isParsed)
                    return new TargetUrl
                    {
                        IsValid = isValid,
                        Target = segments[0]
                    };
            }

            if (segments.Length > 2)
                _logger.LogWarning($"{Environment.NewLine}This block contains more elements when expected: \"{segments[0]}\"{Environment.NewLine}");

            return new TargetUrl
            {
                IsValid = null,
                Target = segments[0]
            };
        }


        private readonly ILogger _logger;
    }
}