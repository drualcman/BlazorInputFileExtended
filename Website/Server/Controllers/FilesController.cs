using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Website.Server.Controllers
{
    [Route("files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment Env;
        public FilesController(IWebHostEnvironment env)
        {
            Env = env;
        }

        [HttpPost]
        public bool Disco([FromForm] IEnumerable<IFormFile> files)
        {
            return true;
        }
    }
}
