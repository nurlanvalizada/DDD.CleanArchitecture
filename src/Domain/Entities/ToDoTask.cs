using System;
using AppDomain.Common.Entities;
using AppDomain.Enums;

namespace AppDomain.Entities
{
    public class ToDoTask : BaseEntity<int>, ICreationAudited, IModificationAudited, IDeletionAudited, ISoftDelete
    {
        public string Name { get; set; }

        public bool IsCompleted { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public TaskState State { get; set; } = TaskState.Active;


        public DateTime CreatedDate { get; set; }

        public long? CreatedUserId { get; set; }


        public DateTime? LastModifiedDate { get; set; }

        public long? LastModifiedUserId { get; set; }


        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public long? DeletedUserId { get; set; }


        //navigation properties
        public int AssignedPersonId { get; set; }
        public Person Person { get; set; }
    }
}
