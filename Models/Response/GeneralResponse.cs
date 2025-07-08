using System.Net;

namespace TwitterClone_API.Models.Response
{
    public class GeneralResponse
    {
        public bool Success { get; set; }
        public object Response { get; set; }
       
        public void SetResponse(bool success, object response)
        {
            Success = success;
            Response = response;
        }
    }
}
