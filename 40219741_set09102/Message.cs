using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _40219741_set09102
{
    public class Message
    {
        private String messageID;
        private String messageTxt;
        private String senderID;

        public String SenderID
        {
            get { return senderID; }
            set { senderID = value; }
        }
        public String MessageID
        {
            get { return messageID; }
            set
            {messageID = value; }
        }
        public String MessageTxt
        {
            get { return messageTxt; }
            set
            {messageTxt = value; }
        }
    }

    public class SMS : Message
    {
         
    }
    public class Email : Message
    {
        private String subject;
        public String Subject
        {
            get { return subject; }
            set { subject = value; }
        }
    }
    public class Tweet : Message
    {
        /*private String hashtags;
        public String Hashtags
        {
            get { return hashtags; }
            set { hashtags = value; }
        }*/
    }
}
