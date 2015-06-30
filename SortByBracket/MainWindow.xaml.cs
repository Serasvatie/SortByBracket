using SortByBracket.Model;
using SortByBracket.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace SortByBracket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PathAndWords model;

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("config.xml"))
            {
                XmlSerializer xs = new XmlSerializer(typeof(PathAndWords));
                using (StreamReader sr = new StreamReader("config.xml"))
                {
                    model = xs.Deserialize(sr) as PathAndWords;
                }
            }
            else
                model = new PathAndWords();

            MainViewModel viewmodel = new MainViewModel(model);
            this.DataContext = viewmodel;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            XmlSerializer xs = new XmlSerializer(typeof(PathAndWords));
            using (StreamWriter sw = new StreamWriter("config.xml", false))
            {
                xs.Serialize(sw, model);
            }
        }
    }
}
