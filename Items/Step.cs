using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ToDoApplication.Items
{
    public interface StepViewModel
    {
        // Fill if needed
    }

    public class Step : INotifyPropertyChanged, StepViewModel
    {

        private event PropertyChangedEventHandler _propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged
        {
            add { _propertyChanged += value; }
            remove { _propertyChanged -= value; }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObjectId Id { get; set; }

        private string _stepName;
        public string StepName 
        {
            get {  return _stepName; }
            set 
            {
                _stepName = value;
                OnPropertyChanged(nameof(StepName));
            }
        }
        private bool _stepDone;
        public bool StepDone 
        {
            get { return _stepDone; }
            set 
            {
                _stepDone = value;
                OnPropertyChanged(nameof(StepDone));
            }
        }
        private Status _stepStatus;
        public Status StepStatus 
        {
            get { return _stepStatus; }
            set 
            {
                _stepStatus = value;
                OnPropertyChanged(nameof(StepStatus));
            }
        }

        public async void ToggleDone() 
        {
            StepDone = !StepDone;
            if (StepDone == true)
            {
                var statusItems = await App.accountManager.GetStatusItems();
                var doneStatus = statusItems[0];
                var databaseDoneStatus = doneStatus.GetStatus(2); // This is just the "Done" status from the database it is bad to do it this way not valid at all so I need to fix it later!!
                StepStatus = databaseDoneStatus;
            }
            else
            {
                var statusItems = await App.accountManager.GetStatusItems();
                var doneStatus = statusItems[0];
                var databaseDoneStatus = doneStatus.GetStatus(0); // This is just the "To do" status from the database it is bad to do it this way not valid at all so I need to fix it later!!
                StepStatus = databaseDoneStatus;
            }

            App.accountManager.currentTodoitem.UpdateItem();
        }
    }

    public class StepEntry : StepViewModel
    {
        public string stepName { get; set; }

        // Add more if needed
    }
}
