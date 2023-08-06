using BlazorInputFileExtended;
using BlazorInputFileExtended.Exceptions;

namespace Website.Client.Pages.version2.pages.demos
{
    public partial class Multiple
    {
        string ErrorsMessage;
        string UploadMessage;
        string CompletedMessage;
        string SaveMessage;

        InputFileExtended Files;

        async Task Save(HttpResponseMessage respose)
        {
            SaveMessage = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            if(respose.IsSuccessStatusCode) SaveMessage = $"Image Upload with result {await respose.Content.ReadFromJsonAsync<bool>()}";
            else SaveMessage = $"Can't upload images.";
            Files.Clean();
        }

        void Error(InputFileException e) => ErrorsMessage = e.Message;

        void UploadFile(BlazorInputFileExtended.FileUploadEventArgs e) =>
            UploadMessage = $"File name: {e.File.Name} File type: {e.File.ContentType} Size: {e.File.Size} Action: {e.Action}";

        void Completed(BlazorInputFileExtended.FilesUploadEventArgs e) =>
            CompletedMessage = $". Files loaded: {e.Count} with total size: {e.Size} Action: {e.Action}";
    }
}
