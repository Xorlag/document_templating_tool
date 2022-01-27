using DocumentTemplateManager.CLI.UserInteractors.Helpers;
using DocumentTemplateManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    public class MainUserInteractor : ConsoleInteractorBase<IEnumerable<TemplateInstantiationConfig>>
    {
        public MainUserInteractor(int interactorLevel, string entityTitle) : base(interactorLevel, entityTitle)
        {
        }

        public override UserInteractionResult<IEnumerable<TemplateInstantiationConfig>> Interact()
        {
            WriteLog($"If you want to paste Config File path, enter '{UserInputOptions.FILE_PATH_INPUT_OPTION}'");
            WriteLog($"If you want to fill template configuration interactively, enter '{UserInputOptions.INTERACTIVE_INPUT_OPTION}'");
            WriteLog($"If you want to Exit, enter '{UserInputOptions.EXIT_INPUT_OPTION}'");

            var inputString = Console.ReadLine();
            switch (inputString.ToUpper())
            {
                case UserInputOptions.FILE_PATH_INPUT_OPTION:
                    {
                        return ReadAppConfigurationFromFile();
                    }
                case UserInputOptions.INTERACTIVE_INPUT_OPTION:
                    {
                        var nextLevelInteractor = new TemplateConfigInteractor(_interactionLevel + 1, entityTitle: "template configuration");
                        return nextLevelInteractor.InteractMany();
                    }
                case UserInputOptions.EXIT_INPUT_OPTION:
                default:
                    {
                        return UserCancelledInput();
                    }
            }
        }

        private UserInteractionResult<IEnumerable<TemplateInstantiationConfig>> ReadAppConfigurationFromFile()
        {
            var nextUserInteractor = new ConfigFileInputInteractor(_interactionLevel + 1, "application configuration");
            var interactionResult = nextUserInteractor.Interact();
            return interactionResult;
        }
    }
}
