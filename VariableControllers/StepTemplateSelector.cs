using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ToDoApplication.Items;

namespace ToDoApplication.VariableControllers
{
    public class StepTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StepEntryTemplate { get; set; }
        public DataTemplate StepItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container) 
        {
            if (item is Step) 
            {
                return StepItemTemplate;
            }
            else 
            {
                return StepEntryTemplate;
            }
        }
    }
}
