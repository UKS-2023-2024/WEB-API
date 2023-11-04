
using Newtonsoft.Json;

namespace WEB_API.ExceptionHandling;

public class ExceptionMessagePresenter
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    
    public ExceptionMessagePresenter(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }


    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}