using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Services.FileUpload
{
    public interface IFileUploadService
    {
        Task<GenericResponse<string>> UploadDocument(MultipartFormDataContent content);
        Task<GenericResponse<UploadResult>> UploadImageResult(MultipartFormDataContent content);
    }
}
