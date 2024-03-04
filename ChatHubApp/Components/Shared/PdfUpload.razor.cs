using ChatHubApp.Helpers;
using ChatHubApp.Services.FileUpload;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Components.Shared
{
   
    public partial class PdfUpload
    {

        [Parameter]
        public string PdfUrl { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        [Inject]
        public IFileUploadService fileUploadService { get; set; }
        private async Task HandlePdfSelected(InputFileChangeEventArgs e)
        {
            var pdfFiles = e.GetMultipleFiles();
            foreach (var pdfFile in pdfFiles)
            {
                if (pdfFile != null)
                {
                    using (var ms = pdfFile.OpenReadStream(pdfFile.Size))
                    {
                        var content = new MultipartFormDataContent();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        content.Add(new StreamContent(ms, Convert.ToInt32(pdfFile.Size)), "pdf", pdfFile.Name);
                        var res = await fileUploadService.UploadDocument(content); // Adjust the service method accordingly
                        if (res.Success)
                        {
                            PdfUrl = Path.Combine(AppConstants.staticsFiles.ToString(), res.Message); 
                            await OnChange.InvokeAsync(res.Message); 
                        }
                    }
                }
            }

           
        }
    }
}
