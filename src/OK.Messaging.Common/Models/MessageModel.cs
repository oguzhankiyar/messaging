namespace OK.Messaging.Common.Models
{
    public class MessageModel
    {
        public int Id { get; set; }

        public int FromUserId { get; set; }

        public UserModel FromUser { get; set; }

        public int ToUserId { get; set; }

        public UserModel ToUser { get; set; }

        public string Content { get; set; }
    }
}