using DocumentTemplateManager.DesktopClient.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DocumentTemplateManager.DesktopClient.Models
{
    public class OutputFileConfigModel : INotifyPropertyChanged
    {
        private string _fileName;

        public OutputFileConfigModel()
        {
            AddVariableCommand = new CommonCommand(AddVariable);
            RemoveVariableCommand = new CommonCommand(RemoveVariable);
            Variables = new ObservableCollection<VariableModel>()
            {
                new VariableModel()
            };
            FileName = "New Output File";
        }
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                OnPropertyChanged(nameof(FileName));
            }
        }

        public ObservableCollection<VariableModel> Variables { get; set; }

        public ICommand AddVariableCommand { get; }
        public ICommand RemoveVariableCommand { get; }

        public bool IsValid { get => !string.IsNullOrEmpty(FileName) && Variables.Any(); }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void AddVariable(object? param)
        {
            Variables.Add(new VariableModel());
        }

        private void RemoveVariable(object? param)
        {
            var variable = param as VariableModel;
            if (variable != null && Variables.Contains(variable))
            {
                Variables.Remove(variable);
                if (!Variables.Any())
                {
                    Variables.Add(new VariableModel());
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
