using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToDoApplication.Items;

namespace ToDoApplication.ViewModels
{

    public interface IToDoViewModel
    {
        // Define common properties or methods here
    }


    public class ToDoItemViewModel : IToDoViewModel, INotifyPropertyChanged
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public bool Done { get; set; }
        public List<Step> Steps { get; set; }
        public int StepsDone { get; set; }
        public int TotalSteps { get; set; }


        // Properties for ToDoItemViewModel
        // ...

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ToDoItemsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<IToDoViewModel> Items { get; } = new ObservableCollection<IToDoViewModel>();


        public event PropertyChangedEventHandler PropertyChanged;

        public ToDoEntryViewModel TaskEntry { get; } = new ToDoEntryViewModel();

        public ToDoItemsViewModel() 
        {
            Items.Add(TaskEntry);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
