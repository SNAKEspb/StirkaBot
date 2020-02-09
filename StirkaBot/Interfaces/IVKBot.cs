using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot
{
    public interface IVKBot
    {
        string confimationCode { get; }
        //Task<bool> SendMessageAsync(IOutgoingMessage message);
        Task processTextMessage(IIncomingMessage message);
        //Task processMenuMessage(IIncomingMessage message);
        //Task processPhotoMessage(IIncomingMessage message);
    }
}
