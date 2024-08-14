namespace CashFlow.Communication.Responses
{
    public class ResponseErrorJson
    {
        public ResponseErrorJson(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
