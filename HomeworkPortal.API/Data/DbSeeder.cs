using Microsoft.AspNetCore.Identity;
using HomeworkPortal.API.Models;

namespace HomeworkPortal.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDataAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // 0. ROLLER (Buna dokunmuyoruz, arka planda oluşsunlar)
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            // 1. ADMIN OLUŞTURMA (ID'si 1000... ile başlıyor, en üste geçecek)
            if (await userManager.FindByEmailAsync("admin@gmail.com") == null)
            {
                var admin = new AppUser
                {
                    Id = "10000000-0000-0000-0000-000000000000",
                    UserName = "Admin00",
                    Email = "admin@gmail.com",
                    FirstName = "Admin",
                    LastName = "TrueAdmin"
                };
                await userManager.CreateAsync(admin, "Admin.123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // 2. ÖĞRETMENLERİ OLUŞTURMA (ID'leri 2000... ile başlıyor)
            for (int i = 1; i <= 10; i++)
            {
                string index = i.ToString("D2"); // 01, 02...
                if (await userManager.FindByEmailAsync($"teacher{index}@gmail.com") == null)
                {
                    var teacher = new AppUser
                    {
                        Id = $"20000000-0000-0000-0000-0000000000{index}",
                        UserName = $"Teacher_{index}",
                        Email = $"teacher{index}@gmail.com",
                        FirstName = "Teacher",
                        LastName = $"Teacher{index}"
                    };
                    await userManager.CreateAsync(teacher, $"Teacher{index}.123!");
                    await userManager.AddToRoleAsync(teacher, "Teacher");
                }
            }

            // 3. ÖĞRENCİLERİ OLUŞTURMA (ID'leri 3000... ile başlıyor)
            for (int i = 1; i <= 10; i++)
            {
                string index = i.ToString("D2"); // 01, 02...
                if (await userManager.FindByEmailAsync($"student{index}@gmail.com") == null)
                {
                    var student = new AppUser
                    {
                        Id = $"30000000-0000-0000-0000-0000000000{index}",
                        UserName = $"Student_{index}",
                        Email = $"student{index}@gmail.com",
                        FirstName = $"User{index}",
                        LastName = $"Student{index}"
                    };
                    await userManager.CreateAsync(student, $"Student{index}.123!");
                    await userManager.AddToRoleAsync(student, "Student");
                }
            }
        }
    }
}