using Microsoft.JSInterop;
using System;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// InputFile Extension with all necessary to upload files.
    /// </summary>
    public partial class InputFileExtended : IDisposable
    {
        #region variables
        string APIErrorMessages;
        string ErrorMessages;
        byte[] FileBytes = null;
        string SelectionInfo;
        #endregion

        /// <summary>
        /// Dispose action
        /// </summary>
        public async void Dispose()
        {
            Files.Dispose();
            await UnLoadDropScriptsAsync();
        }
    }
}
