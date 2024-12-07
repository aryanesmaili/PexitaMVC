using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PexitaMVC.Application.Interfaces;
using PexitaMVC.Application.MapperConfigs;
using PexitaMVC.Core.Entites;
using PexitaMVC.Core.Interfaces;
using PexitaMVC.Infrastructure.Data;
using PexitaMVC.Infrastructure.Repositories;
using PexitaMVC.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

string connectionString = builder.Environment.IsDevelopment() ? "DBConnectionString" : "DBProductionConnectiongString";
builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)));

builder.Services.AddIdentity<UserModel, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders();

// Register Application Services
builder.Services.AddTransient<IBillService, BillService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Register Domain Interfaces
builder.Services.AddTransient<IBillRepository, BillRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();

// Register Mappers.
builder.Services.AddAutoMapper(typeof(BillMapperConfig));
builder.Services.AddAutoMapper(typeof(UserMapperConfig));
builder.Services.AddAutoMapper(typeof(PaymentMapperConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
