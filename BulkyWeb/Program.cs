using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.    //dependency injection container
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();    //Default code line
builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();    //'(options => options.SignIn.RequireConfirmedAccount = true)' is removed so then Email is not confirmed


//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();      //Category Repo.is registered

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();     //Unit Of Work  registeration
builder.Services.AddRazorPages();  // Razer pages registration

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

app.UseAuthentication(); //is the 1st, it will check the validity of U/n and P/w and checkin the person
app.UseAuthorization();  //is the 2nd, if U/n and P/w valid now person can grant the access to the website with the given previlages
app.MapRazorPages();     //Razor Page Pipe Line Added
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
