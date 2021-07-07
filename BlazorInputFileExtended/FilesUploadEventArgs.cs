using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    /// <summary>
    /// Return all files uploaded
    /// </summary>
    public class FilesUploadEventArgs : EventArgs
    {
        /// <summary>
        /// Files uploaded
        /// </summary>
        public SortedDictionary<int, FileUploadContent> Files { get; set; }
        /// <summary>
        /// Total size of all the files uploated
        /// </summary>
        public long Size { get; set; }
        /// <summary>
        /// Number of the files uploated
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Action used
        /// </summary>
        public string Action { get; set; }
    }

}
