using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DocumentTemplateManager.Core.Models;
using DocumentTemplateManager.MicrosoftOfficeWordAPI;

namespace DocumentTemplateManager.Core
{
    public class TemplateInstantiationService
    {
        public void GenerateTemplate(string configFile, string targetDirectoryPath)
        {
            using (StreamReader reader = new StreamReader(configFile))
            {
                string json = reader.ReadToEnd();
                var templateInstantiationConfigs = JsonSerializer.Deserialize<IEnumerable<TemplateInstantiationConfig>>(json);
                Directory.CreateDirectory(targetDirectoryPath);
                foreach (var inputTemplateConfig in templateInstantiationConfigs)
                {
                    foreach (var outputFileConfig in inputTemplateConfig.OutputFiles)
                    {
                        var resultFileName = $@"{targetDirectoryPath}/{outputFileConfig.FileName}.docx";
                        File.Copy(inputTemplateConfig.TemplateFileName, resultFileName);
                        var wordDocument = new WordDocument(resultFileName);
                        wordDocument.FindAndReplaceMany(outputFileConfig.Data);
                    }
                }
            }
        }
        public void GenerateTemplate(IEnumerable<TemplateInstantiationConfig> templateInstantiationConfigItems)
        {
            foreach (var templateConfig in templateInstantiationConfigItems)
            {
                Directory.CreateDirectory(templateConfig.TargetDirectoryPath);
                foreach (var outputFileConfig in templateConfig.OutputFiles)
                {
                    var resultFileName = $@"{templateConfig.TargetDirectoryPath}/{outputFileConfig.FileName}.docx";
                    File.Copy(templateConfig.TemplateFileName, resultFileName);
                    var wordDocument = new WordDocument(resultFileName);
                    wordDocument.FindAndReplaceMany(outputFileConfig.Data);
                }
            }
        }
    }
}
