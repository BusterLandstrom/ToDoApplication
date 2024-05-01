using System.Windows;
using System.Windows.Controls;
using ToDoApplication.ViewModels;

namespace ToDoApplication.VariableControllers
{
    public class TaskTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TaskItemTemplate { get; set; }
        public DataTemplate TaskEntryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ToDoEntryViewModel)
            {
                return TaskEntryTemplate; // Return TaskEntryTemplate for TaskEntryViewModel
            }
            else
            {
                return TaskItemTemplate; // Return TaskItemTemplate for other items
            }
        }
    }
}
