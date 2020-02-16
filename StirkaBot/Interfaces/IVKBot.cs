using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot
{
    public interface IVKBot
    {
        string confimationCode { get; }
        Task<string> sendMessageAsync(IOutgoingMessage message);
        Task<string> messagesGetConversationsAsync(IOutgoingMessage message);
        Task<string> usersGetAsync(IOutgoingMessage message);
        //Task processTextMessage(IIncomingMessage message);
        //Task processMenuMessage(IIncomingMessage message);
        //Task processPhotoMessage(IIncomingMessage message);
    }
}
