using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTemplateManager.CLI.UserInteractors.Helpers
{
    internal class UserInputHelper
    {
        public static UserInteractionResult<T> ReadInput<T>(string titleMessage, string errorTitle, Func<string, T> convertValue, Func<string, bool> validateInput = null)
        {
            bool isInputComplete = false;
            while (!isInputComplete)
            {
                Console.WriteLine(titleMessage);
                var inputString = Console.ReadLine();
                var isInputValid = validateInput?.Invoke(inputString) ?? true;
                if (isInputValid)
                {
                    return new UserInteractionResult<T>(convertValue(inputString));
                }
                else
                {
                    Console.WriteLine(errorTitle);
                    Console.WriteLine($"If you want to enter it once again, enter '{UserInputOptions.REENTER_INPUT_OPTION}'. If you want to exit to outer menu, enter '{UserInputOptions.EXIT_INPUT_OPTION}'");
                    var userInput = Console.ReadLine();
                    switch (userInput.ToUpper())
                    {
                        case UserInputOptions.EXIT_INPUT_OPTION:
                            {
                                return new UserInteractionResult<T>("User canceled input", isSuccess: false);
                            }
                    }
                }
            }
            return new UserInteractionResult<T>("No user input", isSuccess: false);
        }

        //public static UserInteractionResult<T> UserCancelledInput<T>()
        //{
        //    return new UserInteractionResult<T>("User cancelled input", isSuccess: false);
        //}

        //public static UserInteractionResult<IEnumerable<T>> InteractMany<T, I>(string entityTitle) 
        //    where I : IUserInteractor<T>, new()
        //{
        //    bool finishInteraction = false;
        //    var nextUserInteractor = new I();
        //    var outputFileList = new List<T>();
        //    while (!finishInteraction)
        //    {
        //        var templateConfigInteraction = nextUserInteractor.Interact();
        //        if (templateConfigInteraction.IsSuccess)
        //        {
        //            outputFileList.Add(templateConfigInteraction.Result);
        //        }
        //        else
        //        {
        //            Console.WriteLine(templateConfigInteraction.ErrorMessage);
        //        }
        //        Console.WriteLine($"If you want to add one more {entityTitle} interactively, enter '{UserInputOptions.INTERACTIVE_INPUT_OPTION}'");
        //        Console.WriteLine($"If you want to Exit, enter '{UserInputOptions.EXIT_INPUT_OPTION}'");
        //        var inputString = Console.ReadLine();
        //        finishInteraction = inputString.ToUpper() != UserInputOptions.INTERACTIVE_INPUT_OPTION;
        //    }
        //    return new UserInteractionResult<IEnumerable<T>>(outputFileList.ToArray());
        //}
    }
}
