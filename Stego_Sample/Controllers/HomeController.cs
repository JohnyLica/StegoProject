using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;
using Stego_Sample.Models;

namespace Stego_Sample.Controllers
{
    public class HomeController : ApiController
    {
        private Bitmap _receivedBitmap;

        [HttpPost]
        public void RetrieveImage(byte[] streamBytes, byte[] streamMessage)
        {
            var receivedMessage = Encoding.UTF8.GetString(streamMessage, 0, streamMessage.Length);
            using (var mStream = new MemoryStream())
            {
                mStream.Write(streamBytes, 0, streamBytes.Length);
                mStream.Seek(0, SeekOrigin.Begin);
                var bm = new Bitmap(mStream);
                _receivedBitmap = bm;
            }

            SteganographyHelper.MergeText(receivedMessage, _receivedBitmap);
        }

        [HttpGet]
        public string GetImage()
        {
            SteganographyHelper.ExtractText(_receivedBitmap);
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