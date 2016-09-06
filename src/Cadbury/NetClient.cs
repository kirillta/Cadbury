using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Floxdc.Cadbury
{
    internal class NetClient
    {
        internal NetClient(HttpClient client, ILoggerFactory loggerFactory)
        {
            _client = client;
            _logger = loggerFactory.CreateLogger<NetClient>();
        }


        internal async Task<KeyValuePair<bool, string>> CheckUrl(TargetUrl targetUrl)
        {
            var isValid = false;
            var comment = string.Empty;

            try
            {
                int response;
                using (var stream = await _client.GetStreamAsync(targetUrl.Target))
                    response = stream.ReadByte();

                isValid = response > 0;
            }
            catch (Exception ex)
            {
                comment = ex.Message;
                _logger.LogWarning($"{targetUrl.Target}: {ex.Message}");
            }

            return new KeyValuePair<bool, string>(isValid, comment);
        }


        private readonly HttpClient _client;
        private readonly ILogger _logger;
    }
}
