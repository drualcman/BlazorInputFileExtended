using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// InputFile Extension with all necessary to upload files.
    /// </summary>
    public partial class InputFileExtended : IDisposable
    {
        #region variables
        string APIErrorMessages;
        byte[] FileBytes = null;
        string SelectionInfo;
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
            if (!CanDropFiles)
            {
                DropZoneCss = string.Empty;
                Dropping = string.Empty;
            }
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

            if (string.IsNullOrEmpty(SelectionText)) SelectionText = "chosen";
            if (string.IsNullOrEmpty(SelectionCss)) SelectionCss = "info";
        }

        private void Files_OnAPIError(object sender, ArgumentException e)
        {
            APIErrorMessages = e.Message;
            StateHasChanged();
        }

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
