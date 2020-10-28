using System;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public AppUser Sender { get; set; }
        public int RecipientId { get; set; }
        // The following properites define the relationship between the AppUser and the message
        public string RecipientUsername { get; set; }
        public AppUser Recipient { get; set; }
        // Message specific properties
        public string Content { get; set; }
        public DateTime? DateRead { get; set; } // null if not read
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        //public DateTime MessageSent { get; set; } = DateTime.Now;
        // We'll only delete a message from server if both the sender and recipient have deleted the message
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}