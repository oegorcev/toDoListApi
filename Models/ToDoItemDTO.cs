using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListApi.Models
{
    public class ToDoItemDTO
    {
        public long Id { get; set; }
        public string TaskName { get; set; }
        public bool IsCompleted { get; set; }
    }
}
