using System;
using DocumentTemplateManager.CLI.UserInteractors;
using DocumentTemplateManager.Core;

namespace MyApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var mainInteractor = new MainUserInteractor(interactorLevel: 0, entityTitle:"application config");
            var templateConfigListInteraction = mainInteractor.Interact();
            if (templateConfigListInteraction.IsSuccess)
            {
                var documentTemplatingService = new TemplateInstantiationService();
                documentTemplatingService.GenerateTemplate(templateConfigListInteraction.Result);
            }
            else
            {
                Console.WriteLine(templateConfigListInteraction.ErrorMessage);
            }
        }
    }
}