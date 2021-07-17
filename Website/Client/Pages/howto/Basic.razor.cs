using BlazorInputFileExtended;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Website.Client.Pages.howto
{
    public partial class Basic
    {
        string Messages;
        string InputMessage = "Choose Image";
        string Basic1 = "active";
        string Basic2 = string.Empty;
        string Basic3 = string.Empty;
        bool ShowUpload = false;
        bool CanDrop = false;

        InputFileExtended Files;

        /// <summary>
        /// Inject JavaScript interoperability
        /// </summary>
        [Inject] public IJSRuntime JavaScript { get; set; }

        async Task ChangeTab(int tab)
        {
            Files.Clean();
            switch (tab)
            {
                case 1:
                    ShowUpload = false;
                    Basic1 = string.Empty;
                    Basic2 = "active";
                    Basic3 = string.Empty;
                    CanDrop = false;
                    Files.DropZoneCss = string.Empty;
                    InputMessage = "Choose Image";
                    await Files.UnLoadDropScriptsAsync();
                    break;
                case 2:
                    ShowUpload = false;
                    Basic2 = string.Empty;
                    Basic1 = string.Empty;
                    Basic3 = "active";
                    CanDrop = true;
                    Files.DropZoneCss = "dropzone";
                    InputMessage = "Drag and drop or Choose Image";
                    await Files.LoadDropScriptsAsync();
                    break;
                default:
                    ShowUpload = false;
                    Basic1 = "active";
                    Basic2 = string.Empty;
                    Basic3 = string.Empty;
                    CanDrop = false;
                    Files.DropZoneCss = string.Empty;
                    InputMessage = "Choose Image";
                    await Files.UnLoadDropScriptsAsync();
                    break;
            }
            StateHasChanged();
        }

        async Task Save(HttpResponseMessage respose)
        {
            Messages = "Uploading image ... (Simulate 3 seconds)";
            await Task.Delay(3000);
            if (respose.IsSuccessStatusCode) Messages = $"Image Upload with result {await respose.Content.ReadFromJsonAsync<bool>()}";
            else Messages = $"Can't upload images.";
        }
    }
}
