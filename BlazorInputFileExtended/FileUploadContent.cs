using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{

    /// <summary>
    /// Manage the file upload
    /// </summary>
    public class FileUploadContent
    {
        /// <summary>
        /// The name of the file as specified by the browser.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The last modified date as specified by the browser.
        /// </summary>
        public DateTimeOffset LastModified { get; set; }
        /// <summary>
        /// The size of the file in bytes as specified by the browser.
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// The MIME type of the file as specified by the browser.
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// File bites
        /// </summary>
        public StreamContent FileStreamContent { get; set; }
        /// <summary>
        /// Get the bytes from the stream
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> GetFileBytes() =>
            await FileStreamContent.ReadAsByteArrayAsync();
    }
}
