using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ToDoApplication.Views
{
    /// <summary>
    /// Interaction logic for ToDoListView.xaml
    /// </summary>
    public partial class ToDoListView : UserControl
    {
        public ToDoListView()
        {

        }

        // Remove these event handlers since they are now defined in XAML
        //private void NewTaskTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if (NewTaskTextBox.Text == "Enter new task name...")
        //    {
        //        NewTaskTextBox.Text = string.Empty;
        //        NewTaskTextBox.Foreground = Brushes.Black;
        //    }
        //}

        //private void NewTaskTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(NewTaskTextBox.Text))
        //    {
        //        NewTaskTextBox.Text = "Enter new task name...";
        //        NewTaskTextBox.Foreground = Brushes.Gray;
        //    }
        //}

        //private void NewTaskTextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        string newTaskName = NewTaskTextBox.Text;
        //        // Add logic to handle adding a new task here
        //    }
        //}
    }
}
