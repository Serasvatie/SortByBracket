using Microsoft.Win32;
using SordByBracket;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace SortByBracket.ViewModel
{
    public class MainViewModel : BaseNotifyPropertyChange
    {
        private Model.PathAndWords model;
        private ObservableCollection<string> _list;
        private int index;
        private bool isSorting = false;
        private string _addstring;
        private int percentage;

        public MainViewModel(Model.PathAndWords model)
        {
            this.model = model;
            _list = new ObservableCollection<string>(model.Words);
        }

        #region Declaration Command
        private ICommand browserinput;
        private ICommand browseroutput;
        private ICommand add;
        private ICommand remove;
        private ICommand sort;
        #endregion

        #region Command
        public ICommand Sort
        {
            get
            {
                if (sort == null)
                    sort = new RelayCommand(() => DoSort(), () => CanSort());
                    return sort;
            }
        }

        private bool CanSort()
        {
            return !isSorting;
        }

        private void DoSort()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync();
        }

        public int Percentage
        {
            get
            {
                return percentage;
            }
            set
            {
                percentage = value;
                OnPropertyChanged("Percentage");
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Percentage = e.ProgressPercentage;
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isSorting = !isSorting;
            MessageBox.Show("Opération de tri terminé", "Message");
            Percentage = 0;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            isSorting = true;
            SortingFactory sf = new SortingFactory(model, sender as BackgroundWorker);
            sf.DoSort();
        }

        public ICommand BrowserInput
        {
            get
            {
                if (browserinput == null)
                    browserinput = new RelayCommand(() => DoBrowserInput(), null);
                return browserinput;
            }
        }

        private void DoBrowserInput()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.ShowDialog();
            InputDir = fbd.SelectedPath;
        }

        public ICommand BrowserOutput
        {
            get
            {
                if (browseroutput == null)
                    browseroutput = new RelayCommand(() => DoBrowserOutput(), null);
                return browseroutput;
            }
        }

        private void DoBrowserOutput()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.ShowDialog();
            OutputDir = fbd.SelectedPath;
        }
        
        public ICommand Add
        {
            get
            {
                if (add == null)
                    add = new RelayCommand(() => DoAdd(), null);
                return add;
            }
        }

        private void DoAdd()
        {
            model.Words.Add(WordsToAdd);
            _list = new ObservableCollection<string>(model.Words);
            OnPropertyChanged("ListException");
        }

        public ICommand Remove
        {
            get
            {
                if (remove == null)
                    remove = new RelayCommand(() => DoRemove(), () => CanRemove());
                return remove;
            }
        }

        private void DoRemove()
        {
            model.Words.Remove(_list.ElementAt(SelectedIndex));
            _list.RemoveAt(SelectedIndex);
            SelectedIndex = 0;
        }

        private bool CanRemove()
        {
            return SelectedIndex >= 0;
        }
        #endregion

        #region Binding
        public string InputDir
        {
            get
            {
                return model.Input;
            }
            set
            {
                model.Input = value;
                OnPropertyChanged("InputDir");
            }
        }

        public string OutputDir
        {
            get
            {
                return model.Output;
            }
            set
            {
                model.Output = value;
                OnPropertyChanged("OutputDir");
            }
        }

        public string WordsToAdd
        {
            get
            {
                return _addstring;
            }
            set
            {
                _addstring = value;
                OnPropertyChanged("StringToAdd");
            }
        }

        public int SelectedIndex
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public ObservableCollection<string> ListException
        {
            get
            {
                return _list;
            }
        }
        #endregion
    }
}
