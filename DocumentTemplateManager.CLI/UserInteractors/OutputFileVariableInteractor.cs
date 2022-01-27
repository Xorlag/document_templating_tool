using System;
using System.Collections.Generic;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    public class OutputFileVariableInteractor : ConsoleInteractorBase<KeyValuePair<string, string>>
    {
        public OutputFileVariableInteractor(int interactorLevel, string entityTitle) : base(interactorLevel, entityTitle)
        {
        }

        public override UserInteractionResult<KeyValuePair<string, string>> Interact()
        {
            WriteLog("Enter Variable Name");
            var variableName = ReadString();
            WriteLog("Enter Variable Value");
            var variableValue = ReadString();
            var result = new KeyValuePair<string, string>(variableName, variableValue);
            return new UserInteractionResult<KeyValuePair<string, string>>(result);
        }
    }
}
