using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// Return file name and file stream per each file uploaded
    /// </summary>
    public class FileUploadEventArgs : EventArgs
    {
        /// <summary>
        /// File uploaded with all the data
        /// </summary>
        public FileUploadContent File { get; set; }
        /// <summary>
        /// Index in the object
        /// </summary>
        public int FileIndex { get; set; }
        /// <summary>
        /// Action used
        /// </summary>
        public string Action { get; set; }
    }
}
