using ChatHubApp.Helpers;
using ChatHubApp.Services.FileUpload;
using ChatHubApp.Services.Message;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace ChatHubApp.Components.Shared
{
    public partial class ImageUpload
    {

        [Parameter]
        public string ImgUrl { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        [Inject]
        public IFileUploadService fileUploadService { get; set; }
        private async Task HandleImageSelected(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null)
                {
                    var resizedFile = await imageFile.RequestImageFileAsync("image/png", 300, 500);

                    using (var ms = resizedFile.OpenReadStream(resizedFile.Size))
                    {
                        var content = new MultipartFormDataContent();
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                        content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", imageFile.Name);
                        var res =  await fileUploadService.UploadDocument(content);
                        if(res.Success)
                        {

                            ImgUrl =  Path.Combine(AppConstants.staticsFiles.ToString(), res.Message);
                            await OnChange.InvokeAsync(res.Message);
                        }
                      
                    }
                }
            }
        }
    }
}
