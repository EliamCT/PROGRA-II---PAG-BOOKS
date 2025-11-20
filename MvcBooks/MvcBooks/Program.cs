using Microsoft.EntityFrameworkCore;
using MvcBooks.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Conexión a la base de datos (BooksContext)
builder.Services.AddDbContext<BooksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BooksContext")));

var app = builder.Build();

// ====== SEED DATA (cargar datos iniciales) ======
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}
// ===============================================

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // CSS, JS, imágenes

app.UseRouting();

app.UseAuthorization();

// 🚀 CAMBIO: Que el proyecto inicie en /Books/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Books}/{action=Index}/{id?}");

app.Run();