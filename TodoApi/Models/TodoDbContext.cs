using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TodoApi.Models;

public partial class TodoDbContext : DbContext
{
    // משאיר את הבנאים (Constructors) כפי שהם
    public TodoDbContext()
    {
    }

    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }

    // ********** השינוי הראשון: ניקוי OnConfiguring **********
    // כעת ה-DbContext יקבל את האפשרויות שלו (מחרוזת החיבור) דרך Program.cs (DI).
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // אם ה-DbContext אינו מוגדר, ניתן להשאיר כאן מחרוזת פיתוח
        // או להשאיר את הבלוק הזה ריק לחלוטין.
        // הוספתי תנאי כדי לשמור על האפשרות לשימוש מקומי במקרה הצורך, אך זה לא ישפיע על Production.
        if (!optionsBuilder.IsConfigured)
        {
            // שומרים את המחרוזת המקומית הישנה רק לצורכי Scaffolding/בדיקות מקומיות
            optionsBuilder.UseMySql("server=localhost;port=3306;database=tododb;uid=root;pwd=Mn0527690845@", 
                Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));
        }
    }
    // ***********************************************

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // ********** השינוי השני והקריטי ביותר: תיקון שם הטבלה **********
            // מחפש את הטבלה בשם "Items" (עם I גדולה) כפי שנוצרה ב-CLI.
            entity.ToTable("Items"); 
            // *************************************************************

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}