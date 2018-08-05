namespace OK.Messaging.Common.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }
    }
}