using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using AssemblyChecker;
using Block = AssemblyChecker;
using System.IO;

namespace AssemblyBrowser
{
    class ViewButton:INotifyPropertyChanged
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public CommandButton ButtonCommand { get; set; }
        public ViewButton()
        {
            ButtonCommand = new CommandButton(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnExecute()
        {

            var fileDialog = new OpenFileDialog
            {
                //Filter = "Assemblies|*.dll;*.exe",
                Title = "Select assembly БУБУБУБ",
                Multiselect = false
            };

            var isOpen = fileDialog.ShowDialog();

            if (isOpen == null)
            {
                FileName = "File hasn't been chosen. ";
                return;
            }

            if (isOpen.Value)
            {
                FileName = fileDialog.FileName;
                CreateTree(FileName);
            }
        }

        private List<ContainerInAssembly> _namespaces;
        public List<ContainerInAssembly> Namespaces
        {
            get
            {
                return _namespaces;
            }
            set
            {
                _namespaces = value;
                OnPropertyChanged(nameof(Namespaces));
            }
        }
        private readonly AssemblyCheck assemblyCheck = new AssemblyCheck();

        private void CreateTree(string FileName)
        {
            Namespaces = null;
            try           
            {
                Namespaces = assemblyCheck.GetAssemblyInfo(FileName);
            }            
            catch (Exception e)
            {
                    MessageBox.Show("Error");
                
                return;
            }

            OnPropertyChanged("Signature");
            OnPropertyChanged("Members");
            OnPropertyChanged(nameof(Namespaces));
        }
    }
}
