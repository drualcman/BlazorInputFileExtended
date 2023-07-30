using System.Collections.Generic;
using System.Net.Http;

namespace BlazorInputFileExtended
{
    public partial class InputFileHandler
    {
        /// <summary>
        /// Need to use the post actions
        /// </summary>
        protected HttpClient HttpClient;

        int MaxAllowedFiles;
        long MaxAllowedSize;
        string FormField;

        /// <summary>
        /// All files uploaded
        /// </summary>
        protected List<FileUploadContent> UploadedFiles = new List<FileUploadContent>();

    }
}
