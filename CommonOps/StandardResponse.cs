namespace CommonOps
{

    public enum GenericMessage
    {
        Success,
        Error,
        TokenError
    }

    public class StandardResponse<T>
    {
        public bool status { get; set; }
        public string message { get; set; }
        public T? data { get; set; }
        public int code { get; set; }


        public static StandardResponse<T> SuccessMessage(string message = "", dynamic data = null, bool status = true)
        {
            return new StandardResponse<T>
            { 
              status = status, 
              message = message ?? GenericMessage.Success.ToString(),
              data = (T)data 
            };
        }
        public static StandardResponse<T> ErrorMessage(string message = "",  int code = 0)
        {
            return new StandardResponse<T>
            {
                status = false,
                message = message ?? GenericMessage.Error.ToString(),
                code = code
            };
        }
    }
}
