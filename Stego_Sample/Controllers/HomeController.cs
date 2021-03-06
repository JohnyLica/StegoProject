﻿using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Stego_Sample.Models;

namespace Stego_Sample.Controllers
{
    public class HomeController : ApiController
    {
        private Bitmap _receivedBitmap;

        [HttpPost]
		[Route("retrieveImage/{message}")]
		public async Task<HttpResponseMessage> PostFile(string message)
		{
			HttpResponseMessage response = Request.CreateResponse();
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			var rootDirectory = HttpContext.Current.Server.MapPath("~/Images");
			var provider = new MultipartFormDataStreamProvider(rootDirectory);

			try
			{
				await Request.Content.ReadAsMultipartAsync(provider);
				File.Move(provider.FileData[0].LocalFileName, Path.Combine(rootDirectory, $"{message}.jpg"));
				_receivedBitmap = (Bitmap)Image.FromFile($"{message}.jpg"); //TODO google it
				response.StatusCode = HttpStatusCode.Created;
				return null;
			}
			catch (Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
			}
		}

		[HttpGet]
        [Route("getImage")]
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