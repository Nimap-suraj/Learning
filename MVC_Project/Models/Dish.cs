using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_Project.Models;

public partial class Dish
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? ImageUrl { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [ForeignKey("DishId")]
    [InverseProperty("Dishes")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
