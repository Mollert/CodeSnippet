using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SnippetApp.Models
{
    public class Snippet
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name="Programming Language")]
        public string ProgrammingLanguage { get; set; }

        [Display(Name="Code Snippet")]       
        public string CodeSnippet { get; set; }
    }

}