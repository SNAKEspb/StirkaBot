using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using StirkaBot.VKBot.Models;

namespace StirkaBot.Services
{
    public class VKService
    {
        static string _url { get; set; } = "https://api.vk.com/";
        static HttpClient _httpClient = new HttpClient();
        private NLog.Logger _logger;

        public VKService(NLog.Logger logger)
        {
            _logger = logger;
        }

        public async Task<string> sendRequest(IOutgoingMessage message, string method, VKBotOptions options) {
            var urlBuilder = new UriBuilder(_url)
            {
                Path = $"method/{method}",
                Query = $"group_id={options.groupId}&access_token={options.token}&v={options.apiVersion}"
            };
            _logger.Log(NLog.LogLevel.Info, urlBuilder);
            var values = message.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(t=> new { key = t.Name, value = t.GetValue(message, null)})
                .Where(t=> t.value != null)
                .ToDictionary(t => t.key, t => t.value.ToString());
            var content = new FormUrlEncodedContent(values);
            var response = await _httpClient.PostAsync(urlBuilder.Uri, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.Log(NLog.LogLevel.Info, responseBody);
            return responseBody;
        }

    }
}
