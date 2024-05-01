using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Items
{
    public class Step
    {
        public string stepName { get; set; }
        public bool stepDone { get; set; }
        public Status stepStatus { get; set; }

        public void SetDone(Status done) { stepDone = true; stepStatus = done; }
    }
}
