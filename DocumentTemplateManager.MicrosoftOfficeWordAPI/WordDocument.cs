using System.Collections.Generic;
using OpenXmlPowerTools;

namespace DocumentTemplateManager.MicrosoftOfficeWordAPI
{
    public class WordDocument
    {
        private readonly string _documentUri;

        public WordDocument(string documentUri)
        {
            _documentUri = documentUri;
        }

        public void FindAndReplaceMany(IDictionary<string, string> data)
        {
            var wmlDocumet = new WmlDocument(_documentUri);
            foreach (var key in data.Keys)
            {
                var variableName = $"[[{key}]]";
                var variableValue = data[key];
                wmlDocumet = wmlDocumet.SearchAndReplace(variableName, variableValue, true);
            }
            wmlDocumet.Save();
        }

    }
}
