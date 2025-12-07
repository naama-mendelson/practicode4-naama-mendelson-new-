using System.ComponentModel.DataAnnotations.Schema; // חובה להוסיף!

namespace TodoApi.Models;

public partial class Item
{
    // חשוב לוודא שאתה משתמש ב-long (או ב-int) כפי שהוגדר ב-DB
    // הוספת ה-Annotation הזו פותרת את הבעיה:
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public string? Name { get; set; }

    public bool IsComplete { get; set; }
}