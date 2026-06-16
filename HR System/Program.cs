using HR_System.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// 1. קריאת הגדרות החיבור מתוך ה-appsettings.json
var connectionString = builder.Configuration["MongoDB:ConnectionString"];
var databaseName = builder.Configuration["MongoDB:DatabaseName"];

// 2. רישום ה-MongoClient כ-Singleton (מופע אחד לכל המערכת)
builder.Services.AddSingleton<IMongoClient>(new MongoClient(connectionString));

// 3. רישום ה-IMongoDatabase (זה מה שחסר לך כדי לפתור את השגיאה)
builder.Services.AddScoped<IMongoDatabase>(sp =>
    sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));

// 4. רישום הסרוויסים שלך
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ISystemService, SystemService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();