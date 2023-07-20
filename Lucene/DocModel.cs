using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneConsoleApp.Models
{
    internal class DocModel
    {
        public string Page { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string Content { get; set; }   

    }
}
