using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StirkaBot.Services;
using StirkaBot.VKBot.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;

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
        public async Task<bool> sendMessageAsync(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                return await vkService.messagesSendAsync(tmessage, _options.groupId, _options.token, _options.apiVersion);
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "SendMessageAsync error");
                return false;
            }

        }

    }
}
