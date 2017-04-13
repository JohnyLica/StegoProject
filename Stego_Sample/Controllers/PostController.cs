using System.Drawing;
using System.IO;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Stego_Sample.Controllers
{
    public class PostController : ApiController
    {
        private Bitmap _receivedBitmap;

        [HttpPost]
        public void RetrieveImage(Stream stream)
        {
        }

        [HttpGet]
        public string GetImage()
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            Stream stream = new MemoryStream(ImageToByte(_receivedBitmap));
            var bmpJson = JsonConvert.SerializeObject(stream);
            return bmpJson;
        }

        public static byte[] ImageToByte(Bitmap bmp)
        {
            var converter = new ImageConverter();
            return (byte[]) converter.ConvertTo(bmp, typeof (byte[]));
        }
    }
}