using System.Text;
using BLL.Mapper;
using BLL.Services;
using BLL.Services.Address;
using BLL.Services.AdminCategory;
using BLL.Services.AdminOrderService;
using BLL.Services.AdminProduct;
using BLL.Services.AdminUserService;
using BLL.Services.Cart;
using BLL.Services.Cartitem;
using BLL.Services.Category;
using BLL.Services.Order;
using BLL.Services.Order_Service;
using BLL.Services.Product;
using BLL.Services.Review_Service;
using DA;
using DA.Models;
using DAL.Interfaces;
using DAL.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:IssuerIP"],
        ValidAudience = builder.Configuration["JWT:AudienceIP"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddScoped<IAdminProductService, AdminProductService>();
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();

builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ReviewProfile).Assembly);
builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);
builder.Services.AddAutoMapper(typeof(OrderHistoryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ApplicationUserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CartItemProfile).Assembly);
builder.Services.AddAutoMapper(typeof(OrderItemProfile).Assembly);


builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();

app.Run();
