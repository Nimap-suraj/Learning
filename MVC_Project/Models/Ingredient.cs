using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MVC_Project.Models;

public partial class Ingredient
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    public bool? IsAllergen { get; set; }

    [ForeignKey("IngredientId")]
    [InverseProperty("Ingredients")]
    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
