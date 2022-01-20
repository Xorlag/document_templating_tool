using System.Collections.Generic;

namespace DocumentTemplateManager.Core.Models
{
    public class OutputFileConfig
    {
        public string FileName { get; set; }  
        public Dictionary<string, string> Data { get; set; }
    }
}
