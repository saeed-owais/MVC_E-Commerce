using BLL.Mapper;
using BLL.Services;
using BLL.Services.Address;
using BLL.Services.AdminCategory;
using BLL.Services.AdminProduct;
using BLL.Services.Cart;
using BLL.Services.Cartitem;
using BLL.Services.Category;
using BLL.Services.Order;
using BLL.Services.Order_Service;
using BLL.Services.Product;
using BLL.Services.Review_Service;
using DA;
using DA.Models;
using DAL.Data;
using DAL.Interfaces;
using DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace E_Commerce_MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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


            builder.Services.AddHttpContextAccessor();

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IAdminProductService, AdminProductService>();
            builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<BLL.Services.Order.IOrderService, OrderService>();
            builder.Services.AddScoped<IAddressService,AddressService >();
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

            builder.Services.AddHttpClient();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await AppDbInitializer.SeedRolesAndAdminAsync(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database seeding.");
                }
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "Admin",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
