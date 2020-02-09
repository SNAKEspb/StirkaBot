using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot.VKBot.Models
{
    public class UpdateMessage : IIncomingMessage
    {
        public string type { get; set; }
        public UpdateMessageObject @object { get; set; }
        public int group_id { get; set; }
        public string event_id { get; set; }
        public string payload => @object != null && @object.message != null ? @object.message.payload.ToString() : null;

        public string peer_id => @object != null && @object.message != null ? @object.message.peer_id.ToString() : null;
        public string MessageType => type;
        public string from_id => @object != null &&  @object.message != null ? @object.message.from_id.ToString() : null;
        public string date => @object != null && @object.message != null ? @object.message.date.ToString() : null;//DateTime.Now.ToString();

        public string text => @object != null && @object.message != null ? @object.message.text : null;
        public List<dynamic> attachments => @object.message.attachments;
    }

    public class UpdateMessageObject : UpdateMessageData
    {
        public UpdateMessageData message;
        public UpdateMessageClientInfo client_info;

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
        //public UpdateMessageDataAction action { get; set; }
        public List<object> fwd_messages { get; set; }
        public bool important { get; set; }
        public int random_id { get; set; }
        public List<object> attachments { get; set; }
        public bool is_hidden { get; set; }
        public string payload { get; set; }

    }

    public class UpdateMessageClientInfo
    {
        List<object> button_actions { get; set; }
        public bool keyboard { get; set; }
        public bool inline_keyboard { get; set; }
        public int lang_id { get; set; }
    }

    public class UpdateMessageDataAction
    {
        public string type { get; set; }
        public int member_id { get; set; }
    }

    public class MenuButton
    {
        public MenuAction action { get; set; }
        public string color { get; set; } //optional
    }

    public class MenuAction
    {
        public string type { get; set; }
        public string payload { get; set; }
        public string label { get; set; } //optional
    }




}
