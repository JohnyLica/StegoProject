using System.Drawing;
using System.IO;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stego_Sample.Controllers
{
    public class PostController : ApiController
    {
        [HttpPost]
        public void RetrieveImage(Stream stream)
        {
            
        }

        [HttpGet]
        public void GetImage()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = nullva
        }
    }
}