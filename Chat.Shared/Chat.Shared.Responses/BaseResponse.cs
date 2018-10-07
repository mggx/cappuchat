namespace Chat.Responses
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public BaseResponse()
        {
        }

        public BaseResponse(bool success)
        {
            Success = success;
        }

        public BaseResponse(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
