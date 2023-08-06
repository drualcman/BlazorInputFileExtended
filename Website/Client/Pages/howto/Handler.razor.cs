using BlazorBasics.InputFileExtended.Handlers;

namespace Website.Client.Pages.howto
{
    public partial class Handler
    {
        InputFileHandler FileHandler;
        string Messages;

        protected override void OnInitialized()
        {
            FileHandler = new InputFileHandler();
            FileHandler.OnUploadFile += FileHandler_OnUploadFile;
        }

        private void FileHandler_OnUploadFile(object sender, FileUploadEventArgs e)
        {
            FileHandler.Add(e.File);
            Messages = $"File {e.File.Name} chosen. Total size {e.File.Size}.";
        }

        async Task Upload()
        {
            Messages = $"Start upload simulating ...";
            foreach(FileUploadContent file in FileHandler)
            {
                //upload each image
                Messages = $"Uploading image {file.Name}";
                await Task.Delay(1000);
            }
            Messages = $"Files Upload";
        }
    }
}
