using OK.Messaging.Common.Enumerations;
using System;

namespace OK.Messaging.Common.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public UserModel User { get; set; }

        public ActivityTypeEnum TypeId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}