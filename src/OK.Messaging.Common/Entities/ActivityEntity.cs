using OK.Messaging.Common.Enumerations;

namespace OK.Messaging.Common.Entities
{
    public class ActivityEntity : BaseEntity
    {
        public int UserId { get; set; }

        public ActivityTypeEnum TypeId { get; set; }

        public string Description { get; set; }


        public virtual UserEntity User { get; set; }
    }
}