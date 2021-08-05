using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
