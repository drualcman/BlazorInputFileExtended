using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorInputFileExtended
{
    public partial class InputFileExtended
    {
        /// <summary>
        /// Expose InputFileHandler to manage the files when the component have reference. Example to show all the images.
        /// </summary>
        public InputFileHandler Files { get; private set; }
        /// <summary>
        /// Know the Id assigned to the input file to use from some external CSS or JAVASCRIPT when has reference name
        /// </summary>
        public readonly string InputFileId = Guid.NewGuid().ToString();
    }
}
