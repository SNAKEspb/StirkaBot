using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot.VKBot.Models
{
    public class ConfirmationHandler : IUpdatesResultHandler<IIncomingMessage>
    {
        public bool CanHandle(IIncomingMessage message, IVKBot bot)
        {
            return message.MessageType == "confirmation";
        }
        public async Task<HandlerResult> HandleAsync(IIncomingMessage message, IVKBot bot)
        {
            return new HandlerResult() { message = bot.confimationCode};
        }

    }
}
