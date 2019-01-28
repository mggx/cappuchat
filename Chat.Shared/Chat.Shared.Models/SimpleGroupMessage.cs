namespace Chat.Shared.Models
{
    public class SimpleGroupMessage : SimpleMessage
    {
        public SimpleGroupMessage(SimpleUser sender, string message) : base(sender, message)
        //public SimpleGroupMessage(SimpleUser sender, string message, int reactions) : base(sender, message, reactions)
        {
        }
    }
}
