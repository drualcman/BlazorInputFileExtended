using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// InputFile Extension with all necessary to upload files.
    /// </summary>
    /// <typeparam name="TResponse">Model used when post action after upload file. This is the model returned the API call.</typeparam>
    public partial class InputFileExtended<TResponse> : ComponentBase
    {
        #region injections
        /// <summary>
        /// If want to use the upload must be have HttpClient.
        /// That's why is automatic injected
        /// </summary>
        [Inject] public HttpClient Client { get; set; }
        #endregion

        #region setup parameters
        #region upload management
        /// <summary>
        /// Set if we will accept multiple files uploaded or not
        /// </summary>
        [Parameter] public bool MultiFile { get; set; }

        /// <summary>
        /// Number maximum of files can be uploaded
        /// </summary>
        [Parameter] public int MaxUploatedFiles { get; set; } = 5;

        /// <summary>
        /// Maximum file size per each file
        /// </summary>
        [Parameter] public long MaxFileSize { get; set; } = 512000;
        #endregion

        #region input formating
        /// <summary>
        /// CSS InputFile
        /// </summary>
        [Parameter] public string InputCss { get; set; } = string.Empty;

        /// <summary>
        /// InputFile title
        /// </summary>
        [Parameter] public string InputTitle { get; set; } = string.Empty;
        /// <summary>
        /// File types accepted. Example: image/*
        /// </summary>
        [Parameter] public string InputFileTypes { get; set; } = string.Empty;
        #endregion

        #region button formating
        /// <summary>
        /// Show the save button
        /// </summary>
        [Parameter] public bool ButtonShow { get; set; } = true;
        /// <summary>
        /// CSS button save
        /// </summary>
        [Parameter] public string ButtonCss { get; set; } = string.Empty;

        /// <summary>
        /// Button text
        /// </summary>
        [Parameter] public string ButtonText { get; set; } = "Save";

        /// <summary>
        /// Button title
        /// </summary>
        [Parameter] public string ButtonTitle { get; set; } = string.Empty;
        #endregion

        #region review setup
        /// <summary>
        /// Inicate if the file it's a image
        /// </summary>
        [Parameter] public bool IsImage { get; set; }
        /// <summary>
        /// If IsImage = true this indicate if need to do a preview
        /// </summary>
        [Parameter] public bool ShowPreview { get; set; }
        /// <summary>
        /// CSS class for the preview image wrapper. Default image
        /// </summary>
        [Parameter] public string PreviewWrapperCss { get; set; } = "image";
        /// <summary>
        /// CSS class for the image file
        /// </summary>
        [Parameter] public string FileCss { get; set; }
        public byte[] FileBytes = null;
        #endregion

        #region post actions
        /// <summary>
        /// Form data to send in a post action with the files
        /// </summary>
        [Parameter] public MultipartFormDataContent FormData { get; set; }

        /// <summary>
        /// Used when send in a post action, this indicate the field name are expecting
        /// </summary>
        [Parameter] public string FormField { get; set; } = "files";

        /// <summary>
        /// End point to call in a post action
        /// </summary>
        [Parameter] public string EndPoint { get; set; }
        #endregion
        #endregion

        #region variables
        InputFileHandler Files;
        string ErrorMessages;
        #endregion

        #region setup
        protected override void OnInitialized()
        {
            Files = new InputFileHandler(Client);
            Files.OnUploaded += Files_OnUploaded;
            Files.OnUploadFile += Files_OnUploadFile;
            Files.OnUploadError += Files_OnUploadError;
            Files.OnAPIError += Files_OnAPIError;
        }
        protected override void OnParametersSet()
        {
            Files.SetMaxFiles(MaxUploatedFiles);
            Files.SetMaxFileSize(MaxFileSize);
            Files.SetFormField(FormField);

            if (IsImage && string.IsNullOrEmpty(InputFileTypes)) InputFileTypes = "image/*";
            else InputFileTypes = "*";
        }

        private void Files_OnAPIError(object sender, ArgumentException e)
        {
            ErrorMessages = e.Message;
        }

        #endregion

        #region events
        #region parameters
        /// <summary>
        /// When each file is uploaded
        /// </summary>
        [Parameter] public EventCallback<FileUploadEventArgs> OnUploadedFile { get; set; }

        /// <summary>
        /// When all files is uploaded
        /// </summary>
        [Parameter] public EventCallback<FilesUploadEventArgs> OnUploadComleted { get; set; }

        /// <summary>
        /// When some error occurs
        /// </summary>
        [Parameter] public EventCallback<ArgumentException> OnError { get; set; }

        /// <summary>
        /// When some error occurs
        /// </summary>
        [Parameter] public EventCallback<TResponse> OnSave { get; set; }
        #endregion

        #region handlers
        private async Task Files_OnUploaded(object sender, FilesUploadEventArgs e)
        {
            await OnUploadComleted.InvokeAsync(e);
        }

        private async Task Files_OnUploadFile(object sender, FileUploadEventArgs e)
        {
            FileBytes = await e.File.GetFileBytes();
            await OnUploadedFile.InvokeAsync(e);
        }

        private async Task Files_OnUploadError(object sender, ArgumentException e)
        {
            await OnError.InvokeAsync(e);
        }

        async Task Save()
        {
            if (string.IsNullOrEmpty(EndPoint))
            {
                await OnError.InvokeAsync(new ArgumentException("Don't have endpoint to call."));
            }
            else
            {
                if (FormData is not null) await OnSave.InvokeAsync(await Files.UploadAsync<TResponse>(EndPoint, FormData, !MultiFile));
                else await OnSave.InvokeAsync(await Files.UploadAsync<TResponse>(EndPoint, !MultiFile));
            }
        }
        #endregion
        #endregion

    }
}
