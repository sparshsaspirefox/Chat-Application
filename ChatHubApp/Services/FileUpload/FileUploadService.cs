using ChatHubApp.Helpers;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatHubApp.Services.FileUpload
{
    public class FileUploadService:IFileUploadService
    {
        private readonly HttpClient _httpClient;
        public FileUploadService(HttpClient httpClient)
        {

            _httpClient = httpClient;

        }
        public async Task<GenericResponse<string>> UploadDocument(MultipartFormDataContent content)
        {
            try
            {
                string jsonResult = string.Empty;
                var response = await _httpClient.PostAsync(AppConstants.baseAddress + "/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    jsonResult = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<GenericResponse<string>>(jsonResult, options);
                    return json;
                }
                else
                {
                    return new GenericResponse<string> { Success=false,Error="Api error"};
                }


            }
            catch (Exception ex)
            {
                return new GenericResponse<string> { Success = false, Error = ex.Message };
            }

        }
        public async Task<GenericResponse<UploadResult>> UploadImageResult(MultipartFormDataContent content)
        {
            try
            {
                string jsonResult = string.Empty;
                //var response = await _httpClient.PostAsync(AppConstants.baseAddress + "/upload", content);
                var response = await _httpClient.PostAsync(AppConstants.baseAddress + "/file", content);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };
                    jsonResult = await response.Content.ReadAsStringAsync();
                    var json = JsonSerializer.Deserialize<GenericResponse<UploadResult>>(jsonResult, options);
                    // var json = JsonSerializer.Deserialize<GenericResponse<string>>(jsonResult, options);
                    return json;
                }
                else
                {
                    return new GenericResponse<UploadResult> { Success = false, Error = "Api error" };
                }


            }
            catch (Exception ex)
            {
                return new GenericResponse<UploadResult> { Success = false, Error = ex.Message };
            }

        }
    }
}
