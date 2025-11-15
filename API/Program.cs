
using AutoMapper;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAdminProductService, AdminProductService>();
            builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IAddressService, AddressService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IAOrderService, AOrderService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            builder.Services.AddAutoMapper(typeof(ReviewProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(PaymentProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(OrderProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(OrderHistoryProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(ApplicationUserProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(OrderItemProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(CartItemProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(AdminCategoryService).Assembly);
            builder.Services.AddScoped<IAdminOrderService, AdminOrderService>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddHttpClient();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "QuizSystem API v1");

                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
