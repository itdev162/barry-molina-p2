using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class List
    {
        public Guid Id { get; set;}
        [Required]
        public User User { get; set; }
        [Required]
        public string Title { get; set; }
        public ICollection<ListItem> Items { get; set; }
    }
}