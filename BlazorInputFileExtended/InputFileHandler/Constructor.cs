using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorInputFileExtended.Helpers;

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
            if (httpClient is not null) HttpClient = httpClient;

            MaxAllowedFiles = maxFiles;
            MaxAllowedSize = maxSize;
            FormField = formField;
        }
    }
}
