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
        string ErrorMessages;
        byte[] FileBytes = null;
        string SelectionInfo;
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
