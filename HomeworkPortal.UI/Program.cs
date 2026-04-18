using HomeworkPortal.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. Session ve Token Okuma Ýþlemleri Ýçin Zorunlu Servisler
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 2. Token Çözücü Servisimiz (Aþama 5 Kuralý)
builder.Services.AddScoped<ITokenParserService, TokenParserService>();

// 3. API Ýletiþim Katmaný (Backend'e giden istekleri yönetir)
builder.Services.AddTransient<AuthHeaderHandler>();
builder.Services.AddHttpClient<ApiClient>(opt => {
    opt.BaseAddress = new Uri("https://localhost:7113/");
})
.AddHttpMessageHandler<AuthHeaderHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();