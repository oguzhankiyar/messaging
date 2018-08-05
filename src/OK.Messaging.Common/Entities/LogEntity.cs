namespace OK.Messaging.Common.Entities
{
    public class LogEntity : BaseEntity
    {
        public string Level { get; set; }

        public int Thread { get; set; }

        public string Channel { get; set; }

        public string RequestId { get; set; }

        public int? UserId { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public string Exception { get; set; }

        public string IPAddress { get; set; }

        public string UserAgent { get; set; }


        public virtual UserEntity User { get; set; }
    }
}