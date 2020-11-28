using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Domain
{
    [Owned]
    public class ListItem
    {
        [Key]
        public Guid _id { get; set; }
        [Required]
        public string Desc { get; set; }
        public string Url { get; set; }
    }
}