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

        public string stepName { get; set; }
        public bool stepDone { get; set; }
        public Status stepStatus { get; set; }

        public void SetDone() { stepDone = true; stepStatus = App.accountManager.; }
    }

    public class StepEntry : StepViewModel
    {
        public string stepName { get; set; }

        // Add more if needed
    }
}
