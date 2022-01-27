using DocumentTemplateManager.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    public class ConfigFileInputInteractor : ConsoleInteractorBase<IEnumerable<TemplateInstantiationConfig>>
    {
        public ConfigFileInputInteractor(int interactorLevel, string entityTitle) : base(interactorLevel, entityTitle)
        {
        }

        public override UserInteractionResult<IEnumerable<TemplateInstantiationConfig>> Interact()
        {
            WriteLog("Plese enter full path to a file:");

            var readConfigUrlInteraction = ReadConfigFilePathInteraction();

            if (readConfigUrlInteraction.IsSuccess)
            {
                string json = File.ReadAllText(readConfigUrlInteraction.Result);
                var templateInstantiationConfigs = JsonSerializer.Deserialize<IEnumerable<TemplateInstantiationConfig>>(json);
                return new UserInteractionResult<IEnumerable<TemplateInstantiationConfig>>(templateInstantiationConfigs);
            }
            else
            {
                return UserCancelledInput();
            }
        }

        private UserInteractionResult<string> ReadConfigFilePathInteraction()
        {
            return ReadInput<string>(titleMessage: "Enter full path to config file:",
                errorTitle: "Config file does not exist.",
                convertValue: value => value,
                validateInput: value => File.Exists(value));
        }
    }
}
