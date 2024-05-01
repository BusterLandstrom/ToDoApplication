using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToDoApplication.Items;

namespace ToDoApplication.ViewModels
{
    public class ToDoItemViewModel : INotifyPropertyChanged
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
        public ObservableCollection<ToDoItemViewModel> Items { get; } = new ObservableCollection<ToDoItemViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
