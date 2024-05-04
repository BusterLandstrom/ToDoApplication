using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToDoApplication.Items;
using ToDoApplication.ViewModels;

namespace ToDoApplication.Views
{
    /// <summary>
    /// Interaction logic for NewTaskView.xaml
    /// </summary>
    public partial class NewTaskView : UserControl
    {
        public NewTaskView()
        {
            InitializeComponent();
        }

        public void SetDataContext(NewTaskViewModel newTaskViewModel) 
        {
            DataContext = newTaskViewModel;
        }

        public NewTaskViewModel GetDataContext() 
        {
            return (NewTaskViewModel)DataContext;
        }
    }
}
