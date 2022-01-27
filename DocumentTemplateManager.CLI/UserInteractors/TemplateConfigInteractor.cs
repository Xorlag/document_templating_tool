using DocumentTemplateManager.CLI.UserInteractors.Helpers;
using DocumentTemplateManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    public class TemplateConfigInteractor : ConsoleInteractorBase<TemplateInstantiationConfig>
    {
        public TemplateConfigInteractor(int interactorLevel, string entityTitle) : base(interactorLevel, entityTitle)
        {
        }

        public override UserInteractionResult<TemplateInstantiationConfig> Interact()
        {
            WriteLog("Beginning editing template config...");
            var templateConfig = new TemplateInstantiationConfig();
            var fileNameInteraction = ReadTemplateFilePath();
            if (fileNameInteraction.IsSuccess)
            {
                templateConfig.TemplateFileName = fileNameInteraction.Result;
            }
            else
            {
                return UserCancelledInput();
            }
            var directoryInteraction = ReadOuputDirectory();
            if (directoryInteraction.IsSuccess)
            {
                templateConfig.TargetDirectoryPath = directoryInteraction.Result;
            }
            else
            {
                return UserCancelledInput();
            }

            var nextLevelInteractor = new OutputFileConfigInteractor(_interactionLevel + 1, "output file");
            var outputFileConfigInteraction = nextLevelInteractor.InteractMany();
            if (outputFileConfigInteraction.IsSuccess)
            {
                templateConfig.OutputFiles = outputFileConfigInteraction.Result.ToArray();
            }
            return new UserInteractionResult<TemplateInstantiationConfig>(templateConfig);
        }

        private UserInteractionResult<string> ReadTemplateFilePath()
        {
            return ReadInput<string>(titleMessage: "Enter full path to template file:",
                errorTitle: "Template file does not exist.",
                convertValue: value => value,
                validateInput: value => File.Exists(value));
        }

        private UserInteractionResult<string> ReadOuputDirectory()
        {
            return ReadInput<string>(titleMessage: "Enter full path to target directory, where result files would be generated:",
                errorTitle: "Directory does not exist.",
                convertValue: value => value,
                validateInput: value => Directory.Exists(value));
        }
    }
}
