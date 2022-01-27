
namespace DocumentTemplateManager.CLI.UserInteractors
{
    public class UserInteractionResult<T>
    {
        public UserInteractionResult(string errorMessage, bool isSuccess)
        {
            ErrorMessage = errorMessage;
            IsSuccess = isSuccess;
        }

        public UserInteractionResult(T result)
        {
            Result = result;
            IsSuccess = true;
        }

        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public T Result { get; }
    }
}
