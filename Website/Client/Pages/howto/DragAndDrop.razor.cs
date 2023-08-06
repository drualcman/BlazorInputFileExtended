﻿using BlazorBasics.InputFileExtended.ValueObjects;
using System.Collections.Generic;

namespace Website.Client.Pages.howto;

public partial class DragAndDrop
{
    string Messages;
    string SelectText = "Choose File";
    string UploadText = "Upload";
    string ChosenText = "chosen";


    InputFileParameters ButtonParams;

    protected override void OnInitialized()
    {
        ButtonParams = new InputFileParameters()
        {
            DragAndDropOptions = new DragAndDropOptions
            {
                CanDropFiles = true,
            },
            ButtonOptions = new ButtonOptions
            {
                ButtonShow = true,
                CleanOnSuccessUpload = true
            }
        };
        ButtonParams.ButtonOptions.OnSubmit = UploadFles;
    }

    bool UploadFles(IReadOnlyList<FileUploadContent> files)
    {
        Messages = $"Image/s Uploaded {files.Count}";
        InvokeAsync(StateHasChanged);
        return true;
    }
}