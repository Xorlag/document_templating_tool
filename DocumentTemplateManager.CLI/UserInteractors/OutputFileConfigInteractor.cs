using DocumentTemplateManager.CLI.UserInteractors.Helpers;
using DocumentTemplateManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    internal class OutputFileConfigInteractor : ConsoleInteractorBase<OutputFileConfig>
    {
        public OutputFileConfigInteractor(int interactorLevel, string entityTitle) : base(interactorLevel, entityTitle)
        {
        }

        public override UserInteractionResult<OutputFileConfig> Interact()
        {
            WriteLog("Beginning configuration of Output File...");
            var outputFileConfig = new OutputFileConfig();
            var outputFileNameInteraction = ReadOutputFileName();
            if (outputFileNameInteraction.IsSuccess)
            {
                outputFileConfig.FileName = outputFileNameInteraction.Result;
            }
            else
            {
                return UserCancelledInput();
            }

            var nextLevelInteractor = new OutputFileVariableInteractor(_interactionLevel + 1, "output file variable");
            var readVariablesInteraction = nextLevelInteractor.InteractMany();

            if (readVariablesInteraction.IsSuccess)
            {
                outputFileConfig.Data = readVariablesInteraction.Result.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
            }
            else
            {
                UserCancelledInput();
            }
            return new UserInteractionResult<OutputFileConfig>(outputFileConfig);
        }

        private UserInteractionResult<string> ReadOutputFileName()
        {
            return ReadInput<string>(titleMessage: "Enter output file name:",
                errorTitle: "Invalid file name",
                convertValue: value => value);
        }
    }
}
