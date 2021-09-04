using BlazorInputFileExtended;
using BlazorInputFileExtended.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Website.Client.Pages.demos
{
    public partial class Single
    {
        string ErrorsMessage;
        string UploadMessage;
        string CompletedMessage;
        string SaveMessage;
        string SelectText = "Choose File";
        string UploadText = "Upload";

        async Task Save(HttpResponseMessage respose)
        {
            SaveMessage = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            if (respose.IsSuccessStatusCode) SaveMessage = $"Image Upload with result {await respose.Content.ReadFromJsonAsync<bool>()}";
            else SaveMessage = $"Can't upload images.";
        }

        void Error(InputFileException e) => ErrorsMessage = e.Message;

        void UploadFile(FileUploadEventArgs e) =>
            UploadMessage = $"File name: {e.File.Name} File type: {e.File.ContentType} Size: {e.File.Size} Action: {e.Action}";

        void Completed(FilesUploadEventArgs e) =>
            CompletedMessage = $". Files loadted: {e.Count} with total size: {e.Size} Action: {e.Action}";
    }
}
