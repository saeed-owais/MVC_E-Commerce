namespace API.Response
{
    public class ResponseHelper
    {
        public static ApiResponse<T> Success<T>(T data, string message = "Success")
        => new ApiResponse<T>(true, message, data);

        public static ApiResponse<T> Fail<T>(string message)
            => new ApiResponse<T>(false, message);
    }
}
