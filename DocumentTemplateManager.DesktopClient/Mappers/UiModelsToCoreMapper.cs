using DocumentTemplateManager.Core.Models;
using DocumentTemplateManager.DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTemplateManager.DesktopClient.Mappers
{
    internal static class UiModelsToCoreMapper
    {
        public static IEnumerable<TemplateInstantiationConfig> MapUiTemplateModelToCoreObject(IEnumerable<TemplateInstanceConfigurationModel> uiTemplateModel)
        {
            var coreModelsTemplateConfigs = uiTemplateModel
                .Where(uiTemplateModel => uiTemplateModel.IsValid)
                .Select(uiTemplateModel =>
                {
                    return new TemplateInstantiationConfig()
                    {
                        TargetDirectoryPath = uiTemplateModel.TargetDirectoryPath,
                        TemplateFileName = uiTemplateModel.TemplateFilePath,
                        OutputFiles = uiTemplateModel.OutputFiles
                        .Where(uiFileModel => uiFileModel.IsValid)
                        .Select(MapUiFileModelToCoreObject)
                        .ToArray()
                    };
                });
            return coreModelsTemplateConfigs;
        }

        private static OutputFileConfig MapUiFileModelToCoreObject(OutputFileConfigModel uiFileModel)
        {
            var resultData = new Dictionary<string, string>();
            foreach (var uiVariableModel in uiFileModel.Variables
                .Where(variable => variable.IsValid))
            {
                resultData.Add(uiVariableModel.VariableName, uiVariableModel.VariableValue);
            }

            return new OutputFileConfig()
            {
                FileName = uiFileModel.FileName,
                Data = resultData
            };
        }
    }
}
