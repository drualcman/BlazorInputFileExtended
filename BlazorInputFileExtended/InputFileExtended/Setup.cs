using BlazorInputFileExtended.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
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

        private void Files_OnAPIError(object sender, InputFileException e)
        {
            APIErrorMessages = e.Message;
            StateHasChanged();
        }
    }
}
