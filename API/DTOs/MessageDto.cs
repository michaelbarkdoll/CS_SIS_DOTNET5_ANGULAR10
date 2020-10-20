using System;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public int RecipientId { get; set; }
        // The following properites define the relationship between the AppUser and the message
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }
        // Message specific properties
        public string Content { get; set; }
        public DateTime? DateRead { get; set; } // null if not read
        public DateTime MessageSent { get; set; }

    }
}