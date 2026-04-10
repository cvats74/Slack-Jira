using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using WorkFlowPro.Domain.Common;

namespace WorkFlowPro.Domain.Entities
{
    public class Organization :BaseEntity
    {
        public Organization() 
        {
            Projects = new List<Project>();
            Members = new List<User>();
        
        }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public string? Website { get; set; }
        public bool IsActive { get; set; } = true;

        //Navigation
        public ICollection<User> Members { get; set; }
        public ICollection<Project> Projects { get; set; }
    }
}
