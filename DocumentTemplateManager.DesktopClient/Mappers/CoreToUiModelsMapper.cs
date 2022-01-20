using DocumentTemplateManager.Core.Models;
using DocumentTemplateManager.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTemplateManager.DesktopClient.Mappers
{
    internal static class CoreToUiModelsMapper
    {
        public static IEnumerable<TemplateInstanceConfigurationModel> MapCoreObjectsToUiModels(IEnumerable<TemplateInstantiationConfig> coreObjects)
        {
            var uiModels = coreObjects.Select(coreTemplate => new TemplateInstanceConfigurationModel()
            {
                TemplateFilePath = coreTemplate.TemplateFileName,
                TargetDirectoryPath = coreTemplate.TargetDirectoryPath,
                OutputFiles = new ObservableCollection<OutputFileConfigModel>(coreTemplate.OutputFiles
                .Select(f => new OutputFileConfigModel()
                {
                    FileName = f.FileName,
                    Variables = new ObservableCollection<VariableModel>(f.Data.Keys.Select(variableName=>new VariableModel ()
                    {
                        VariableName = variableName,
                        VariableValue = f.Data[variableName]
                    }))
                }))
            });
            return uiModels;
        }
    }
}
