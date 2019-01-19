namespace ChatComponents
{
    public class Message
    {
        public string Text { get; set; }
        public string Time { get; set; }

        public bool OwnMessage { get; set; }

        public Message(string text, string time, bool ownMessage = false)
        {
            Text = text;
            Time = time;
            OwnMessage = ownMessage;
        }
    }
}
