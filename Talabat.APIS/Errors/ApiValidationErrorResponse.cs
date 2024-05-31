namespace Talabat.APIS.Errors
{
    public class ApiValidationErrorResponse :ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorResponse() :base(400)
        {
            
        }
    }
}
