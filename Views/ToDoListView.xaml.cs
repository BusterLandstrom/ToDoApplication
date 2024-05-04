using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ToDoApplication.Items;
using ToDoApplication.ViewModels;
using ZstdSharp.Unsafe;

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
            DataContext = todoItemsViewModel;
        }

        public ToDoItemsViewModel GetDataContex() 
        {
            return (ToDoItemsViewModel)DataContext;
        }
    }
}
