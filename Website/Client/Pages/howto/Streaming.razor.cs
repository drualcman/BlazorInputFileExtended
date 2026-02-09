using BlazorBasics.InputFileExtended.ValueObjects;

namespace Website.Client.Pages.howto;
public partial class Streaming
{
    InputFileParameters ButtonParams; protected override void OnInitialized()
    {
        ButtonParams = new InputFileParameters()
        {
            EnableStreaming = true,
            AllowPasteFiles = false,
            MaxFileSize = 2L * 1024 * 1024, // 2 MB
            MaxUploatedFiles = 1,
            ShowFileList = false,
            MultiFile = false,
            InputFileTypes = ".mp4,.webm,.ogg,.mov,.mkv",
            ButtonOptions = new ButtonOptions
            {
                ButtonShow = false,
                CleanOnSuccessUpload = true
            },
            PreviewOptions = new PreviewOptions
            {
                ShowPreview = false,
                IsImage = false
            }
        };
    }
    private void HandleFileSelection(FilesUploadEventArgs e)
    {
        Console.WriteLine($"HandleFileSelection called {e.Action}");
    }
}