namespace DocumentTemplateManager.Core.Models
{
    public class TemplateInstantiationConfig
    {
        public string TemplateFileName { get; set; }
        public string TargetDirectoryPath { get; set; }
        public OutputFileConfig[] OutputFiles { get; set; }
    }
}
