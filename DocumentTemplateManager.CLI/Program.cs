using DocumentTemplateManager.Core;
using System;

namespace MyApp
{
    public class Program
    {
        private const string CONFIG_FILE_URI = "InputData.json";
        static void Main(string[] args)
        {
            var directoryName = $@"Result_{DateTime.Now.ToString("ddMMyyyy_hhmmss")}";
            var service = new TemplateInstantiationService();
            service.GenerateTemplate(CONFIG_FILE_URI, directoryName);
        }
    }
}