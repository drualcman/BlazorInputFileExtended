﻿using BlazorInputFileExtended;
using BlazorInputFileExtended.Exceptions;

namespace Website.Client.Pages.version2.pages.howto
{
    public partial class Handler
    {
        [Inject]
        public HttpClient Client { get; set; }

        InputFileHandler FileHandler;
        string Messages;

        protected override void OnInitialized()
        {
            FileHandler = new InputFileHandler(Client);
            FileHandler.OnUploadError += FileHandler_OnUploadError;
            FileHandler.OnUploadFile += FileHandler_OnUploadFile;
        }

        #region InputFile
        private void FileHandler_OnUploadFile(object sender, BlazorInputFileExtended.FileUploadEventArgs e)
        {
            FileHandler.Add(e.File);
            Messages = $"File {e.File.Name} chosen. Total size {e.File.Size}.";
        }

        private void FileHandler_OnUploadError(object sender, InputFileException e)
        {
            Messages = $"Exception: {e.Message} on {e.ParamName}";
        }

        #endregion

        #region InputFileExtended
        void OnError(InputFileException e)
        {
            Messages = $"Exception: {e.Message} on {e.ParamName}";
        }

        void OnSelect(BlazorInputFileExtended.FileUploadEventArgs e)
        {
            FileHandler.Add(e.File);
            Messages = $"File {e.File.Name} chosen. Total size {e.File.Size}.";
        }
        #endregion

        async Task Upload()
        {
            Messages = $"Start upload simulating ...";
            bool result = await FileHandler.UploadAsync<bool>("files");
            await Task.Delay(3000);
            Messages = $"Files Upload with result {result}";
        }
    }
}
