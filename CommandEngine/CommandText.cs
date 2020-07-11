using System;
namespace CommandEngine
{
    public class Message
    {
        public string Text { get; set; }

        public Message() { }

        public Message(string text)
        {
            Text = text;
        }
    }
}
