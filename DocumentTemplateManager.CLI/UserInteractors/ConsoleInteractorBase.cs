
using DocumentTemplateManager.CLI.UserInteractors.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    public abstract class ConsoleInteractorBase<T> : IUserInteractor<T>
    {
        private const string INTERACTOR_LEVEL_SEPARATOR = ">>>>>>>>>>";

        private readonly string _entityTitle;
        protected readonly int _interactionLevel;

        protected ConsoleInteractorBase(int interactorLevel, string entityTitle)
        {
            _interactionLevel = interactorLevel;
            _entityTitle = entityTitle;
        }

        public UserInteractionResult<IEnumerable<T>> InteractMany()
        {
            bool finishInteraction = false;

            var outputFileList = new List<T>();
            while (!finishInteraction)
            {
                var userInteraction = Interact();
                if (userInteraction.IsSuccess)
                {
                    outputFileList.Add(userInteraction.Result);
                }
                else
                {
                    WriteLog(userInteraction.ErrorMessage);
                }
                WriteLog($"If you want to add one more {_entityTitle} interactively, enter '{UserInputOptions.INTERACTIVE_INPUT_OPTION}'");
                WriteLog($"If you want to Exit, enter '{UserInputOptions.EXIT_INPUT_OPTION}'");
                var inputString = ReadString();
                finishInteraction = inputString.ToUpper() != UserInputOptions.INTERACTIVE_INPUT_OPTION;
            }
            return new UserInteractionResult<IEnumerable<T>>(outputFileList.ToArray());
        }

        public abstract UserInteractionResult<T> Interact();

        protected UserInteractionResult<InputType> ReadInput<InputType>(string titleMessage, string errorTitle, Func<string, InputType> convertValue, Func<string, bool> validateInput = null)
        {
            bool isInputComplete = false;
            while (!isInputComplete)
            {
                WriteLog(titleMessage);
                var inputString = ReadString();
                var isInputValid = validateInput?.Invoke(inputString) ?? true;
                if (isInputValid)
                {
                    return new UserInteractionResult<InputType>(convertValue(inputString));
                }
                else
                {
                    WriteLog(errorTitle);
                    WriteLog($"If you want to enter it once again, enter '{UserInputOptions.REENTER_INPUT_OPTION}'. If you want to exit to outer menu, enter '{UserInputOptions.EXIT_INPUT_OPTION}'");
                    var userInput = ReadString();
                    switch (userInput.ToUpper())
                    {
                        case UserInputOptions.EXIT_INPUT_OPTION:
                            {
                                return new UserInteractionResult<InputType>("User canceled input", isSuccess: false);
                            }
                    }
                }
            }
            return new UserInteractionResult<InputType>("No user input", isSuccess: false);
        }

        protected UserInteractionResult<T> UserCancelledInput()
        {
            return new UserInteractionResult<T>("User cancelled input", isSuccess: false);
        }

        protected void WriteLog(string text)
        {
            var logBuilder = new StringBuilder();
            for (int i = 0; i < _interactionLevel; ++i)
            {
                logBuilder.Append(INTERACTOR_LEVEL_SEPARATOR);
            }
            logBuilder.Append(text);
            Console.WriteLine(logBuilder.ToString());
        }

        protected string ReadString()
        {
            var logBuilder = new StringBuilder();
            for (int i = 0; i < _interactionLevel; ++i)
            {
                logBuilder.Append(INTERACTOR_LEVEL_SEPARATOR);
            }
            Console.Write(logBuilder.ToString());
            return Console.ReadLine();
        }
    }
}
