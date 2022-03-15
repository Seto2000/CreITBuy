﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CreITBuy.Infrastructure.Data.Models
{
    public class Product
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(36)]
        public string AuthorId { get; set; }
        [Required]
        [ForeignKey(nameof(AuthorId))]
        public User Author { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }
        [Required]
        [Range(0,100000)]
        public decimal Price { get; set; }
        public DateTime PostedOn { get; set; }
    }
}