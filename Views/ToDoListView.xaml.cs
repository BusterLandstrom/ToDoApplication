using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToDoApplication.Items;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Views
{
    /// <summary>
    /// Interaction logic for ToDoListView.xaml
    /// </summary>
    public partial class ToDoListView : UserControl
    {
        public IEnumerable<ToDoItem> ItemsSource
        {
            get { return (IEnumerable<ToDoItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable<ToDoItem>), typeof(ToDoListView), new PropertyMetadata(null));


        public ToDoListView()
        {
            InitializeComponent();
        }

        public void SetDataContex(ToDoItemsViewModel todoItemsViewModel) 
        {
            this.DataContext = todoItemsViewModel;
        }

        // These will be fixed and actaully added once implemented better (currently handled in the XAML code)
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
