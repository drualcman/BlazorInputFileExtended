using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
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
        /// Get the context from the form
        /// </summary>
        [CascadingParameter] public EditContext Context { get; set; }

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
    }
}
