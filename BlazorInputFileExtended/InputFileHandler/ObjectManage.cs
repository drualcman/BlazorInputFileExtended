using System.Net.Http;

namespace BlazorInputFileExtended
{
    public partial class InputFileHandler
    {

        /// <summary>
        /// Set HttpClient if is not from the constructor
        /// </summary>
        /// <param name="httpClient"></param>
        public void SetHttpClient(HttpClient httpClient) => HttpClient = httpClient;
        /// <summary>
        /// Set the max allowed files
        /// </summary>
        /// <param name="maxfile"></param>
        public void SetMaxFiles(int maxfile) => MaxAllowedFiles = maxfile;
        /// <summary>
        /// Set the max file size allowed
        /// </summary>
        /// <param name="maxSize"></param>
        public void SetMaxFileSize(long maxSize) => MaxAllowedSize = maxSize;
        /// <summary>
        /// Set the field name for the form when upload files
        /// </summary>
        /// <param name="field"></param>
        public void SetFormField(string field) => FormField = field;

    }
}
