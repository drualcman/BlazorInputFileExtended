using System.Net.Http;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// Manage upload files
    /// </summary>
    public partial class InputFileHandler
    {
        /// <summary>
        /// Can upload files
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="maxFiles">Maximum files allowed to upload</param>
        /// <param name="maxSize">Maximum file size to upload</param>
        /// <param name="formField">Form content name to upload the file</param>
        public InputFileHandler(HttpClient httpClient = null, int maxFiles = 5, long maxSize = 512000, string formField = "files")
        {
            if(httpClient is not null) HttpClient = httpClient;

            MaxAllowedFiles = maxFiles;
            MaxAllowedSize = maxSize;
            FormField = formField;
        }
    }
}
