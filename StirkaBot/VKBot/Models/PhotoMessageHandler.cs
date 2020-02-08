using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StirkaBot.VKBot.Models
{
    public class PhotoMessageHandler : IUpdatesHandler<IIncomingMessage>
    {
        public bool CanHandle(IIncomingMessage message, IVKBot bot)
        {
            return message.attachments.Any(x => x.type == "photo");
        }
        public async Task HandleAsync(IIncomingMessage message, IVKBot bot)
        {
        }
    }
}
