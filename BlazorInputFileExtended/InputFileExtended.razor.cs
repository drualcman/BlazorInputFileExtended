using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// InputFile Extension with all necessary to upload files.
    /// </summary>
    public partial class InputFileExtended : ComponentBase, IDisposable
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

        /// <summary>
        /// Clean all files after success upload
        /// </summary>
        [Parameter] public bool CleanOnSuccessUpload { get; set; }

        /// <summary>
        /// Message to show when files are selected
        /// </summary>
        [Parameter] public string SelectionText { get; set; }

        /// <summary>
        /// CSS class to personalize the selection text info
        /// </summary>
        [Parameter] public string SelectionCss { get; set; }

        #endregion

        #region input formating
        /// <summary>
        /// CSS InputFile
        /// </summary>
        [Parameter] public string InputCss { get; set; } = "input-file button-file";

        /// <summary>
        /// InputFile title
        /// </summary>
        [Parameter] public string InputTitle { get; set; } = string.Empty;
        /// <summary>
        /// File types accepted. Example: image/*
        /// </summary>
        [Parameter] public string InputFileTypes { get; set; } = string.Empty;

        /// <summary>
        /// Text to show for the file selection
        /// </summary>
        [Parameter] public RenderFragment InputContent { get; set; }
        #endregion

        #region button formating
        /// <summary>
        /// Show the save button
        /// </summary>
        [Parameter] public bool ButtonShow { get; set; } = false;
        /// <summary>
        /// CSS button save
        /// </summary>
        [Parameter] public string ButtonCss { get; set; } = "input-file button-upload";

        /// <summary>
        /// Button text
        /// </summary>
        [Parameter] public RenderFragment ButtonContent { get; set; }

        /// <summary>
        /// Button title
        /// </summary>
        [Parameter] public string ButtonTitle { get; set; } = string.Empty;
        #endregion

        #region review setup
        /// <summary>
        /// Inicate if the file it's a image
        /// </summary>
        [Parameter] public bool IsImage { get; set; } = true;
        /// <summary>
        /// If IsImage = true this indicate if need to do a preview
        /// </summary>
        [Parameter] public bool ShowPreview { get; set; } = true;
        /// <summary>
        /// CSS class for the preview image wrapper. Default image
        /// </summary>
        [Parameter] public string PreviewWrapperCss { get; set; } = "image";
        /// <summary>
        /// CSS class for the image file
        /// </summary>
        [Parameter] public string FileCss { get; set; }
        #endregion

        #region post actions
        /// <summary>
        /// Form data to send in a post action with the files
        /// </summary>
        [Parameter] public MultipartFormDataContent TargetFormDataContent { get; set; }

        /// <summary>
        /// Form data to send in a post action with the files
        /// </summary>
        [Parameter] public object TargetDataObject { get; set; }

        /// <summary>
        /// Used when send in a post action, this indicate the field name are expecting
        /// </summary>
        [Parameter] public string TargetFormFieldName { get; set; } = "files";

        /// <summary>
        /// End point to call in a post action
        /// </summary>
        [Parameter] public string TargetToPostFile { get; set; }
        #endregion
        #endregion

        #region variables
        /// <summary>
        /// Expose InputFileHandler to manage the files when the component have reference. Example to show all the images.
        /// </summary>
        public InputFileHandler Files { get; private set; }
        string ErrorMessages;
        byte[] FileBytes = null;
        string SelectionInfo;
        #endregion

        #region methods
        /// <summary>
        /// Clean all the files
        /// </summary>
        public void Clean()
        {
            Files.Clean();
            FileBytes = null;
            SelectionInfo = string.Empty;
        }
        #endregion

        #region setup
        /// <summary>
        /// Initialize the component
        /// </summary>
        protected override void OnInitialized()
        {
            Files = new InputFileHandler(Client);
            Files.OnUploaded += Files_OnUploaded;
            Files.OnUploadFile += Files_OnUploadFile;
            Files.OnUploadError += Files_OnUploadError;
            Files.OnAPIError += Files_OnAPIError;
            SelectionInfo = string.Empty;
        }
        /// <summary>
        /// Format the component with the properties
        /// </summary>
        protected override void OnParametersSet()
        {
            Files.SetMaxFiles(MaxUploatedFiles);
            Files.SetMaxFileSize(MaxFileSize);
            Files.SetFormField(TargetFormFieldName);

            if (IsImage && string.IsNullOrEmpty(InputFileTypes)) InputFileTypes = "image/*";
            else InputFileTypes = "*";

            if (string.IsNullOrEmpty(SelectionText)) SelectionText = "chosen";
            if (string.IsNullOrEmpty(SelectionCss)) SelectionCss = "info";
        }

        private void Files_OnAPIError(object sender, ArgumentException e)
        {
            ErrorMessages = e.Message;
            StateHasChanged();
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
        /// When upload is completed
        /// </summary>
        [Parameter] public EventCallback<object> OnSave { get; set; }
        #endregion

        #region handlers
        private void Files_OnUploaded(object sender, FilesUploadEventArgs e) =>
            OnUploadComleted.InvokeAsync(e);

        private async void Files_OnUploadFile(object sender, FileUploadEventArgs e)
        {
            FileBytes = await e.File.GetFileBytes();
            if (Files.Count > 0) SelectionInfo = $"{Files.Count} {SelectionText}";
            else SelectionInfo = string.Empty;
            await OnUploadedFile.InvokeAsync(e);
            StateHasChanged();
        }

        private void Files_OnUploadError(object sender, ArgumentException e) =>
            OnError.InvokeAsync(e);

        async Task Save()
        {
            if (string.IsNullOrEmpty(TargetToPostFile))
            {
                await OnError.InvokeAsync(new ArgumentException("Don't have endpoint to call."));
            }
            else if (Files.Count < 1)
            {
                await OnError.InvokeAsync(new ArgumentException("No files chosen"));
            }            
            else
            {
                if (TargetFormDataContent is not null) await OnSave.InvokeAsync(await Files.UploadAsync<object>(TargetToPostFile, TargetFormDataContent, !MultiFile));
                else if (TargetDataObject is not null) await OnSave.InvokeAsync(await Files.UploadAsync<object, object>(TargetToPostFile, TargetDataObject, !MultiFile));
                else await OnSave.InvokeAsync(await Files.UploadAsync<object>(TargetToPostFile, new MultipartFormDataContent(), !MultiFile));
                if (CleanOnSuccessUpload) Clean();
            }
            StateHasChanged();
        }
        #endregion
        #endregion

         /// <summary>
        /// Dispose action
        /// </summary>
        public void Dispose()
        {
            Files.Dispose();
        }
    }
}
