using System.Collections.Generic;
using AppDomain.Common.Entities;
using AppDomain.ValueObjects;

namespace AppDomain.Entities
{
    public class Person : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public short Age { get; set; }

        public Address Address { get; set; }

        public ICollection<ToDoTask> Tasks { get; set; }
    }
}
