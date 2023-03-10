using System.IO;
using EjesUI.Models;
using Newtonsoft.Json;

namespace EjesUI.Services
{
    internal class PdfService
    {
        private ApiService api;
        private readonly AppConfig appConfig;
        
        public PdfService() { 
            this.api = new ApiService();
            this.appConfig = new AppConfig();
        }

        public string Generate(Pdf pdfData) {
            string payloadPdf = JsonConvert.SerializeObject(pdfData);
            dynamic? pdfResult = api.Post("/generate-pdf", payloadPdf);

            return pdfResult.filename.Value;
        }

        public void Download(string filename)
        {
            string downloadURL = $"{this.appConfig.DefaultDownloadPath}{filename}.pdf";

            dynamic? rawPdf = api.Get("/view-pdf", ("filename", $"{filename}.pdf"));

            File.WriteAllBytes(downloadURL, rawPdf);
        }
    }
}
