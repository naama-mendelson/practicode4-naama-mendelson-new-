using Microsoft.EntityFrameworkCore;
using TodoApi.Models; // ודא שזהו המרחב השמות הנכון עבור TodoDbContext

var builder = WebApplication.CreateBuilder(args);

// ********** הוספת קוד לטיפול בפורט של Render (מתחיל כאן) **********
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        // גורם לשרת להאזין לפורט ש-Render מספק (כגון 10000)
        options.ListenAnyIP(int.Parse(port));
    });
}
// ********** סוף קוד לטיפול בפורט של Render **********


// הוספת שירותים ל-Container.

// ********** 1. הוספת CORS לשירותים (Services) **********
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy  =>
        {
            // מאפשר לכל בקשה מה-Frontend ב-localhost:3000 להתחבר ל-API
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// ******************************************************


// הגדרת DBContext עם מחרוזת החיבור החדשה
builder.Services.AddDbContext<TodoDbContext>(options =>
{
    // 1. קבלת מחרוזת החיבור "ToDoDB" מקובץ appsettings.json
    // הערה: ב-Render, מחרוזת החיבור נלקחת ממשתנה הסביבה שהגדרנו: ConnectionStrings__ToDoDB
    var connectionString = builder.Configuration.GetConnectionString("ToDoDB");

    // 2. שימוש במחרוזת החיבור כדי להתחבר ל-MySQL
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ********** 2. הפעלת מדיניות CORS (מעל UseAuthorization) **********
app.UseCors("CorsPolicy"); 
// ******************************************************************

app.UseAuthorization();

app.MapControllers();

app.Run();