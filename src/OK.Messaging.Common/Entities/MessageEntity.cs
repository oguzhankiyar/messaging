namespace OK.Messaging.Common.Entities
{
    public class MessageEntity : BaseEntity
    {
        public int FromUserId { get; set; }

        public int ToUserId { get; set; }
        
        public string Content { get; set; }


        public virtual UserEntity FromUser { get; set; }

        public virtual UserEntity ToUser { get; set; }
    }
}