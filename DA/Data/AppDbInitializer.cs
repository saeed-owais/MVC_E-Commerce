using DA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Data
{
    public class AppDbInitializer
    {
        // هذه هي الميثود التي سنستدعيها من Program.cs
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // 1. جلب الخدمات التي نحتاجها
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 2. تعريف الرتب (Roles)
            string adminRole = "Admin";
            string customerRole = "Customer";

            // 3. إنشاء الرتب إذا لم تكن موجودة
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            if (!await roleManager.RoleExistsAsync(customerRole))
            {
                await roleManager.CreateAsync(new IdentityRole(customerRole));
            }

            // 4. إنشاء مستخدم الأدمن
            if (await userManager.FindByEmailAsync("admin@ecommerce.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@ecommerce.com",
                    Email = "admin@ecommerce.com",
                    FullName = "Admin User",
                    EmailConfirmed = true // تأكيد الإيميل مباشرة
                };

                // إنشاء المستخدم
                var result = await userManager.CreateAsync(adminUser, "Admin123!"); // ◀️ كلمة سر قوية

                if (result.Succeeded)
                {
                    // 5. إضافة المستخدم إلى رتبة "Admin"
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
