using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubApp.Helpers
{
   
    public static class DocumentViewer
    {
        public static void OpenDocumentInViewer(byte[] documentBytes)
        {
            try
            {
                string filePath;
                // Save the Document to a file
                
                 filePath = SaveDocumentToFile(documentBytes, "document.pdf");

                // Launch the default Document viewer
                Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening PDF: {ex.Message}");
            }
        }

        private static string SaveDocumentToFile(byte[] pdfBytes, string fileName)
        {
            // Save the Document byte array to a file
            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            File.WriteAllBytes(filePath, pdfBytes);

            return filePath;
        }
    }
}
