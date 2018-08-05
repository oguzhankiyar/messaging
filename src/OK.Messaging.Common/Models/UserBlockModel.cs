namespace OK.Messaging.Common.Models
{
    public class UserBlockModel
    {
        public int UserId { get; set; }

        public UserModel User { get; set; }

        public int BlockedUserId { get; set; }

        public UserModel BlockedUser { get; set; }
    }
}