using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StirkaBot.Services;
using StirkaBot.VKBot.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using System.Runtime.Caching;

namespace StirkaBot.VKBot
{
    public class VKBot : IVKBot
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        private VKService vkService;
        private VKBotOptions _options;

        public VKBot(IOptions<VKBotOptions> options)
        {
            vkService = new VKService(_logger);
            _options = options.Value;
        }

        public string confimationCode => _options.confirmationCode;
        public async Task<string> sendMessageAsync(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                return await vkService.sendRequest(tmessage, "messages.send", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
                return null;
            }

        }

        public class HistoryRequest : IOutgoingMessage
        {
            public int offset { get; set; }
            public int count { get; set; }
            public string user_id { get; set; }
            public string peer_id { get; set; }
            public string start_message_id { get; set; }
            public string rev { get; set; }
            public string extended { get; set; }
            public string fields { get; set; }
            public string group_id { get; set; }
        }

        public async Task messagesGetHistory(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                var request = new HistoryRequest
                {
                    //offset = 0,
                    count = 1,
                    //user_id = "1556462",
                    peer_id = tmessage.peer_id,
                    //start_message_id = "0",
                    //rev = "1",
                    //extended = 
                    //fields = 
                    group_id = _options.groupId,
                };
                await vkService.sendRequest(request, "messages.getHistory", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
            }
        }

        public class ConversationRequest : IOutgoingMessage
        {
            public int offset { get; set; }
            public int count { get; set; }
            public string filter { get; set; }
            public string start_message_id { get; set; }
            public string extended { get; set; }
            public string fields { get; set; }
            public string group_id { get; set; }

            public string major_sort_id { get; set; }
        }

        public async Task<string> messagesGetConversationsAsync(IOutgoingMessage message)
        {
            try
            {
                return await vkService.sendRequest(message, "messages.getConversations", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
                return null;
            }
        }

        public class MembersRequest : IOutgoingMessage
        {
            public int offset { get; set; }
            public int count { get; set; }
            public string filter { get; set; }
            public string fields { get; set; }
            public string group_id { get; set; }
            public string sort { get; set; }
        }


        public async Task groupsGetMembers(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                var request = new ConversationRequest
                {
                    offset = 0,
                    count = 10,
                    filter = "managers",
                    //fields = 
                    group_id = _options.groupId,
                    //major_sort_id = 
                };
                await vkService.sendRequest(request, "groups.getMembers", _options);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
            }
        }

        public class UsersRequest : IOutgoingMessage
        {
            public string user_ids { get; set; }
            public string fields { get; set; }
            public string name_case { get; set; }
        }

        public async Task<string> usersGetAsync(IOutgoingMessage message)
        {
            try
            {
                //ObjectCache cache = MemoryCache.Default;
                //var tmessage = (UsersRequest)message;
                //if (cache[tmessage.user_ids] != null)
                //{
                //    return cache[tmessage.user_ids];
                //}

                return await vkService.sendRequest(message, "users.get", _options);
                //cache.Add(cacheKey, message, DateTime.Now.AddMinutes(5));
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
                return null;
            }
        }

    }
}
