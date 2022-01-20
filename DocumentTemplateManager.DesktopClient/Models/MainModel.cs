using DocumentTemplateManager.Core;
using DocumentTemplateManager.Core.Models;
using DocumentTemplateManager.DesktopClient.Commands;
using DocumentTemplateManager.DesktopClient.Mappers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace DocumentTemplateManager.DesktopClient.Models
{
    internal class MainModel : INotifyPropertyChanged
    {
        private TemplateInstanceConfigurationModel _selectedTemplateConfig;
        private ObservableCollection<TemplateInstanceConfigurationModel> _templateInstanceConfigurations;
        public ICommand AddTemplateConfigCommand { get; }
        public ICommand RemoveTemplateConfigCommand { get; }
        public ICommand GenerateFilesCommand { get; }
        public ICommand ExportConfigFileCommand { get; }
        public ICommand ImportConfigFileCommand { get; }

        public bool IsValid { get => TemplateInstanceConfigurations.Any(uiTemplateModel => uiTemplateModel.IsValid); }
        public MainModel()
        {
            AddTemplateConfigCommand = new CommonCommand(AddTemplateConfig);
            RemoveTemplateConfigCommand = new CommonCommand(RemoveTemplateConfig);
            GenerateFilesCommand = new CommonCommand(GenerateFiles);
            ExportConfigFileCommand = new CommonCommand(ExportConfigFile);
            ImportConfigFileCommand = new CommonCommand(ImportConfigFile);
            var defaultTemplateConfig = new TemplateInstanceConfigurationModel();
            TemplateInstanceConfigurations = new ObservableCollection<TemplateInstanceConfigurationModel>()
            {
                defaultTemplateConfig
            };
            SelectedTemplateConfig = defaultTemplateConfig;
        }

        public TemplateInstanceConfigurationModel SelectedTemplateConfig
        {
            get
            {
                return _selectedTemplateConfig;
            }
            set
            {
                _selectedTemplateConfig = value;
                OnPropertyChanged(nameof(SelectedTemplateConfig));
            }
        }

        public ObservableCollection<TemplateInstanceConfigurationModel> TemplateInstanceConfigurations
        {
            get
            {
                return _templateInstanceConfigurations;
            }
            set
            {
                _templateInstanceConfigurations = value;
                OnPropertyChanged(nameof(TemplateInstanceConfigurations));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void AddTemplateConfig(object? param)
        {
            var defaultTemplateConfig = new TemplateInstanceConfigurationModel();
            TemplateInstanceConfigurations.Add(defaultTemplateConfig);
            SelectedTemplateConfig = defaultTemplateConfig;
        }

        private void RemoveTemplateConfig(object? param)
        {
            var templateConfig = param as TemplateInstanceConfigurationModel;
            if (templateConfig != null && TemplateInstanceConfigurations.Contains(templateConfig))
            {
                TemplateInstanceConfigurations.Remove(templateConfig);
                if (!TemplateInstanceConfigurations.Any())
                {
                    var defaultTemplateConfig = new TemplateInstanceConfigurationModel();
                    TemplateInstanceConfigurations.Add(defaultTemplateConfig);
                    SelectedTemplateConfig = defaultTemplateConfig;
                }
            }
        }

        private void GenerateFiles(object? param)
        {
            if (IsValid)
            {
                var coreModelsTemplateConfigs = UiModelsToCoreMapper.MapUiTemplateModelToCoreObject(TemplateInstanceConfigurations);
                var templateInstantiationService = new TemplateInstantiationService();
                templateInstantiationService.GenerateTemplate(coreModelsTemplateConfigs);
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "Finished Generating Files", "Info");
            }
            else
            {
                System.Windows.MessageBox.Show(System.Windows.Application.Current.MainWindow, "No Valid Template found", "Warning");
            }
        }

        private void ExportConfigFile(object? param)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json";
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var coreModelsTemplateConfigs = UiModelsToCoreMapper.MapUiTemplateModelToCoreObject(TemplateInstanceConfigurations);
                var serializedCoreObjectsJson = JsonSerializer.Serialize(coreModelsTemplateConfigs);
                File.WriteAllText(saveFileDialog.FileName, serializedCoreObjectsJson);
            }
        }

        private void ImportConfigFile(object? param)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var serializedCoreObjectsJson = File.ReadAllText(openFileDialog.FileName);
                var coreObjects = JsonSerializer.Deserialize<IEnumerable<TemplateInstantiationConfig>>(serializedCoreObjectsJson);
                var uiTemplateModels = CoreToUiModelsMapper.MapCoreObjectsToUiModels(coreObjects);
                TemplateInstanceConfigurations = new ObservableCollection<TemplateInstanceConfigurationModel>(uiTemplateModels);

                if (TemplateInstanceConfigurations.Any())
                {
                    SelectedTemplateConfig = TemplateInstanceConfigurations.First();
                    foreach (var templateModel in TemplateInstanceConfigurations)
                    {
                        if (templateModel.OutputFiles.Any())
                        {
                            templateModel.SelectedFileConfig = templateModel.OutputFiles.First();
                        }
                    }
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
