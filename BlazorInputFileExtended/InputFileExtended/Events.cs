﻿using BlazorInputFileExtended.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
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
        [Parameter] public EventCallback<InputFileException> OnError { get; set; }

        /// <summary>
        /// When upload is completed
        /// </summary>
        [Parameter] public EventCallback<HttpResponseMessage> OnSave { get; set; }

        /// <summary>
        /// To setup correctly the urls to get the javascripts
        /// </summary>
        [Parameter] public EventCallback<InputFileChangeEventArgs> OnChange { get; set; }
        #endregion

        #region handlers
        private void Files_OnUploaded(object sender, FilesUploadEventArgs e) =>
            OnUploadComleted.InvokeAsync(e);

        private void Files_OnUploadFile(object sender, FileUploadEventArgs e)
        {
            e.File.SetFileBytes().Wait();
            FileBytes = e.File.FileBytes;
            if(Files.Count > 0) SelectionInfo = $"{Files.Count} {SelectionText}";
            else SelectionInfo = string.Empty;
            InvokeAsync(StateHasChanged);
            if(OnUploadedFile.HasDelegate)
                OnUploadedFile.InvokeAsync(e);
            if(AutoUpload &&
               !string.IsNullOrEmpty(TargetToPostFile))
                SendFile().Wait();        //send the file after upload
        }

        private void Files_OnUploadError(object sender, InputFileException e)
        {
            if(OnError.HasDelegate) OnError.InvokeAsync(e);
            else ErrorMessages =
                    $"{e.Message}" +
                    $"{(e.ExceptionType == ExceptionType.MaxSize ? $" File size {e.FileMbBytes.ToString("N2")}Mb ({e.FileBytes} bytes) overflow maximum size is {e.MaxFileMbBytes.ToString("N2")}Mb ({e.MaxFileBytes} bytes). " : "")}" +
                    $"{(e.ExceptionType == ExceptionType.MaxCount ? $" Max files selected {e.MaxFilesAllowed}. " : "")}";
        }
        #endregion

    }
}
