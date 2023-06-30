using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
        #region Configuration
        /// <summary>
        /// Enable is can drop files
        /// </summary>
        [Parameter] public bool CanDropFiles { get; set; }

        /// <summary>
        /// Css when drop a file
        /// </summary>
        [Parameter] public string DropZoneCss { get; set; } = "dropzone";

        /// <summary>
        /// Css when drop a file
        /// </summary>
        [Parameter] public string DroppingCss { get; set; } = "dropzone-drag";
        #endregion

        #region variables
        ElementReference DropZone;
        ElementReference InputContainer;

        IJSObjectReference DragAndDropScript;
        IJSObjectReference DragAndDropInstance;
        #endregion

        #region Setup
        /// <summary>
        /// Setup the drag and drop support
        /// </summary>
        /// <param name="firstRender"></param>
        /// <returns></returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (CanDropFiles)
                {
                    await LoadDropScriptsAsync();
                }

            }
        }
        #endregion

        #region Management
        /// <summary>
        /// Add the scripts fro can drop files
        /// </summary>
        /// <returns></returns>
        public async Task LoadDropScriptsAsync()
        {
            string url = Navigation.BaseUri;
            // if can drop need to load some JavaScript
            DragAndDropScript = await JavaScript.InvokeAsync<IJSObjectReference>("import", $"{url}_content/BlazorInputFileExtended/DragAndDrop1316.js");
            //for initialize the drop zone
            DragAndDropInstance = await DragAndDropScript.InvokeAsync<IJSObjectReference>("DragAndDrop", DropZone, InputContainer);
            CanDropFiles = true;
        }

        /// <summary>
        /// Remove drag and drop options
        /// </summary>
        /// <returns></returns>
        public async Task UnLoadDropScriptsAsync()
        {
            // unload the JavaScript for drag and drop
            if (DragAndDropInstance is not null)
            {
                try
                {
                    await DragAndDropInstance.InvokeVoidAsync("Dispose");
                    await DragAndDropInstance.DisposeAsync();
                }
                catch { }
            }

            if (DragAndDropScript is not null)
            {      
                try
                {
                    await DragAndDropScript.DisposeAsync();
                }
                catch { }
            }
            CanDropFiles = false;
        }

        string Dropping;
        /// <summary>
        /// Change class to know we are in the drag area
        /// </summary>
        void DragEnter()
        {
            if (CanDropFiles) Dropping = DroppingCss;
            else Dropping = string.Empty;
        }
        /// <summary>
        /// Remove the class because we are not in the drag area
        /// </summary>
        void DragLeave() => Dropping = string.Empty;
        #endregion

    }
}
