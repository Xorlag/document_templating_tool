using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentTemplateManager.CLI.UserInteractors
{
    public interface IUserInteractor<T>
    {
        UserInteractionResult<T> Interact();
        UserInteractionResult<IEnumerable<T>> InteractMany();
    }
}
