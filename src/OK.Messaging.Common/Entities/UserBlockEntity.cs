namespace OK.Messaging.Common.Entities
{
    public class UserBlockEntity : BaseEntity
    {
        public int UserId { get; set; }

        public int BlockedUserId { get; set; }


        public virtual UserEntity User { get; set; }

        public virtual UserEntity BlockedUser { get; set; }
    }
}