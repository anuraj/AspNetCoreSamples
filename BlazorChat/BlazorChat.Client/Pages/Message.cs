using System;

namespace BlazorChat.Client.Pages
{
    public class Message
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}