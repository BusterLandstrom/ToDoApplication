using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToDoApplication.Items;

namespace ToDoApplication.ViewModels
{
    public class NewTaskViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Status> _statusList;
        public ObservableCollection<Status> StatusList
        {
            get { return _statusList; }
            set
            {
                _statusList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusList)));
            }
        }

        private Status _selectedStatus;
        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
            }
        }

        public NewTaskViewModel()
        {

        }
    }
}
