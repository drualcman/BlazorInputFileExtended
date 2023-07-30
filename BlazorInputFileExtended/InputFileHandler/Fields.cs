using System.Net.Http;

namespace BlazorInputFileExtended
{
    public partial class InputFileHandler
    {

        /// <summary>
        /// Last File stream uploaded
        /// </summary>
        public StreamContent UploadedImage;
        /// <summary>
        /// Last File name uploaded
        /// </summary>
        public string FileName;

    }
}
