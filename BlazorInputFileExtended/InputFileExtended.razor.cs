using System;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// InputFile Extension with all necessary to upload files.
    /// </summary>
    public partial class InputFileExtended : IDisposable, IAsyncDisposable
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

        public async ValueTask DisposeAsync() 
        {
            await UnLoadDropScriptsAsync();
        }
    }
}
