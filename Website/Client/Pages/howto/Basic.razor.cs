using BlazorBasics.InputFileExtended;

namespace Website.Client.Pages.howto
{
    public partial class Basic
    {
        string Messages;

        async Task LoadFiles(FilesUploadEventArgs e)
        {
            Messages = "Manage image/s ... (Simulate 3 seconds upload)";
            await Task.Delay(3000);
            Messages = $"Image/s Uploaded {e.Count}";
        }
    }
}
