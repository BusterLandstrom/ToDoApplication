﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.ViewModels
{
    public class ToDoEntryViewModel : IToDoViewModel
    {
        public string TaskName { get; set; }

        // Add funciton to creata a new task by using this entry item
    }
}
