﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StirkaBot.VKBot.Models
{
    public class TextMessageHandler : IUpdatesHandler<IIncomingMessage>  
    {
        //todo:move statics to bot
        static string[] _messageTypes = new[] { "message_new", "message_reply" };

        public bool CanHandle(IIncomingMessage message, IVKBot bot)
        {
            return _messageTypes.Contains(message.MessageType.ToLowerInvariant())
                && !string.IsNullOrWhiteSpace(message.text);
        }
        public async Task HandleAsync(IIncomingMessage message, IVKBot bot)
        {
            await bot.processTextMessage(message);
        }
    }
}