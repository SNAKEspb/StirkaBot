using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public async Task<bool> messagesSendAsync(OutgoingMessage message, string groupId, string token, string apiVersion)
        {
            var urlBuilder = new UriBuilder(_url)
            {
                Path = "method/messages.send",
                Query = $"group_id={groupId}&access_token={token}&v={apiVersion}"
            };
            _logger.Log(NLog.LogLevel.Info, urlBuilder);
            var values = new Dictionary<string, string>
                {
                    { "random_id", message.random_id},
                    { "peer_id", message.peer_id.ToString()},
                    { "message", message.message},
                    { "attachment", message.attachment }
                };

            var content = new FormUrlEncodedContent(values);
            var response = await _httpClient.PostAsync(urlBuilder.Uri, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            _logger.Log(NLog.LogLevel.Info, responseBody);
            return true;
        }
    }
}
