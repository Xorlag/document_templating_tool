using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentTemplateManager.DesktopClient.Models
{
    public class VariableModel
    {
        public string VariableName { get; set; }
        public string VariableValue { get; set; }

        public bool IsValid { get => !string.IsNullOrEmpty(VariableName) && !string.IsNullOrEmpty(VariableValue); }
    }
}
