using Microsoft.EntityFrameworkCore;
using RAG2;
using RAG2.Context;

var builder = WebApplication.CreateBuilder(args);

// 添加服務到容器
builder.Services.AddDistributedMemoryCache(); // 使用內存作為 Session 存儲
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session 超時設置
    options.Cookie.HttpOnly = true; // 防止客戶端腳本訪問 Cookies
    options.Cookie.IsEssential = true; // 標記 Cookie 為基本服務以符合 GDPR
});
builder.Services.AddControllersWithViews(); // 如果是 MVC 應用

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session services to the DI container.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // e.g., Set the session timeout.
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    
});
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MariaDB"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MariaDB"))));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // 啟用 Session 中間件
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();