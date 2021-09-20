using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApi.Models
{
    public class ToDoItem
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
        public string Secret { get; set; }
    }
}
