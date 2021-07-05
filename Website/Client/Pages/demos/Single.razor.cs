using BlazorInputFileExtended;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Website.Client.Pages.demos
{
    public partial class Single
    {
        string ErrorsMessage;
        string UploadMessage;
        string CompletedMessage;
        string SaveMessage;

        InputFileExtended<bool> UploadImage;

        async Task Save()
        {
            SaveMessage = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            SaveMessage = "Image Uploaded!";
        }

        void Error(ArgumentException e) => ErrorsMessage = e.Message;

        void UploadFile(FileUploadEventArgs e) =>
            UploadMessage = $"File name: {e.File.Name} File type: {e.File.ContentType} Size: {e.File.Size} Action: {e.Action}";

        void Completed(FilesUploadEventArgs e) =>
            CompletedMessage = $". Files loadted: {e.Count} with total size: {e.Size} Action: {e.Action}";
    }
}
