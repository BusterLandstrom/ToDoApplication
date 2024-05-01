using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Views
{
    /// <summary>
    /// Interaction logic for ToDoListView.xaml
    /// </summary>
    public partial class ToDoListView : UserControl
    {
        public IEnumerable<ToDoItemViewModel> ItemsSource
        {
            get { return (IEnumerable<ToDoItemViewModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ToDoItemViewModel>), typeof(ToDoListView), new PropertyMetadata(null));


        public ToDoListView()
        {
            InitializeComponent();
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
