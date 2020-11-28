using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class List
    {
        [Key]
        public Guid _id { get; set;}
        public Guid User { get; set; }
        [Required]
        public string Title { get; set; }
        public ICollection<ListItem> Items { get; set; }
    }
}