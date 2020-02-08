using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot.VKBot.Models
{
    public class UpdateMessage : IIncomingMessage
    {
        public string type { get; set; }
        public UpdateMessageData @object { get; set; }
        public int group_id { get; set; }

        public string peer_id => @object != null ? @object.peer_id.ToString() : null;
        public string MessageType => type;
        public string from_id => @object != null ? @object.from_id.ToString() : null;
        public string date => @object != null ? @object.date.ToString() : null;//DateTime.Now.ToString();

        public string text => @object != null ? @object.text : null;
        public List<dynamic> attachments => @object.attachments;
    }

    public class UpdateMessageData
    {
        public int date { get; set; }
        public int from_id { get; set; }
        public int id { get; set; }
        public int @out { get; set; }
        public int peer_id { get; set; }
        public string text { get; set; }
        public int conversation_message_id { get; set; }
        public UpdateMessageDataAction action { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public int random_id { get; set; }
        public List<object> attachments { get; set; }
        public bool is_hidden { get; set; }
    }

    public class UpdateMessageDataAction
    {
        public string type { get; set; }
        public int member_id { get; set; }
    }




}
