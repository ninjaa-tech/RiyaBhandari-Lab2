using RiyaBhandari_Lab2.Data;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();


app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");


// Seed in-memory data once at app 
InMemoryData.Seed();


app.Run();