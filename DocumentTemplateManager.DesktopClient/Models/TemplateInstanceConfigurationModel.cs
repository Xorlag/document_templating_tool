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
    public class TemplateInstanceConfigurationModel : INotifyPropertyChanged
    {
        private const string DEFAULT_HEADER_VALUE = "New Template";

        private string _templateFilePath;
        private string _templateHeader;
        private OutputFileConfigModel _selectedFileConfig;

        public TemplateInstanceConfigurationModel()
        {
            AddOutputFileCommand = new CommonCommand(AddOutputFile);
            RemoveOutputFileCommand = new CommonCommand(RemoveOutputFile);
            var defaultFileConfig = new OutputFileConfigModel();
            OutputFiles = new ObservableCollection<OutputFileConfigModel> { defaultFileConfig };
            SelectedFileConfig = defaultFileConfig;
            TemplateHeader = DEFAULT_HEADER_VALUE;
        }

        public OutputFileConfigModel SelectedFileConfig
        {
            get { return _selectedFileConfig; }
            set
            {
                _selectedFileConfig = value;
                OnPropertyChanged(nameof(SelectedFileConfig));
            }
        }

        public string TemplateHeader
        {
            get
            {
                return _templateHeader;
            }
            set
            {
                _templateHeader = value;
                OnPropertyChanged(nameof(TemplateHeader));
            }
        }
        public string TemplateFilePath
        {
            get
            {
                return _templateFilePath;
            }
            set
            {
                _templateFilePath = value;
                OnPropertyChanged(nameof(TemplateFilePath));
            }
        }

        public bool IsValid { get => !string.IsNullOrEmpty(TargetDirectoryPath) && !string.IsNullOrEmpty(TemplateFilePath); }

        public string TargetDirectoryPath { get; set; }
        public ICommand AddOutputFileCommand { get; }
        public ICommand RemoveOutputFileCommand { get; }
        public ObservableCollection<OutputFileConfigModel> OutputFiles { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void AddOutputFile(object? param)
        {
            var defaultOuputFileConfig = new OutputFileConfigModel();
            OutputFiles.Add(defaultOuputFileConfig);
            SelectedFileConfig = defaultOuputFileConfig;
        }

        private void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(TemplateFilePath):
                    {
                        TemplateHeader = TemplateFilePath?.Split('\\').LastOrDefault() ?? DEFAULT_HEADER_VALUE;
                        break;
                    }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RemoveOutputFile(object? param)
        {
            var outputFileConfig = param as OutputFileConfigModel;
            if (outputFileConfig != null && OutputFiles.Contains(outputFileConfig))
            {
                OutputFiles.Remove(outputFileConfig);
                if (!OutputFiles.Any())
                {
                    var defaultFileConfig = new OutputFileConfigModel();
                    OutputFiles.Add(defaultFileConfig);
                    SelectedFileConfig = defaultFileConfig;
                }
            }
        }
    }
}
