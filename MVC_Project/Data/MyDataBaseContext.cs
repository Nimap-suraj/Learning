using System;
using System.Collections.Generic;
using MVC_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace MVC_Project.Data;

public partial class MyDataBaseContext : DbContext
{
    public MyDataBaseContext()
    {
    }

    public MyDataBaseContext(DbContextOptions<MyDataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Dish> Dishes { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-TBOGTIK\\SQLEXPRESS;Initial Catalog=My_DataBase;Integrated Security=True;Pooling=False;Encrypt=False;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dish>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dishes__3214EC0721037C47");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasMany(d => d.Ingredients).WithMany(p => p.Dishes)
                .UsingEntity<Dictionary<string, object>>(
                    "DishIngredient",
                    r => r.HasOne<Ingredient>().WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DishIngre__Ingre__534D60F1"),
                    l => l.HasOne<Dish>().WithMany()
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__DishIngre__DishI__52593CB8"),
                    j =>
                    {
                        j.HasKey("DishId", "IngredientId").HasName("PK__DishIngr__A369A475CBD269CE");
                        j.ToTable("DishIngredients");
                    });
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC07B5E79678");

            entity.Property(e => e.IsAllergen).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
