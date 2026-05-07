using Microsoft.AspNetCore.Identity;
using HomeworkPortal.API.Models;

namespace HomeworkPortal.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedDataAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            // 0. ROLLER
            string[] roles = { "Admin", "Teacher", "Student" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new AppRole { Name = role });
                }
            }

            // 1. ADMIN OLUŞTURMA
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

            // 2. ÖĞRETMENLERİ OLUŞTURMA
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

            // 3. ÖĞRENCİLERİ OLUŞTURMA 
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
        public static async Task SeedBadgesAsync(AppDbContext context)
        {
            var systemBadges = new List<Models.Badge>
            {
                new Models.Badge { Name = "İlk Kan", Description = "İlk ödevini başarıyla teslim ettin! Mükemmel bir başlangıç.", Icon = "fas fa-seedling text-success", ConditionType = "SUBMISSION_COUNT", RequiredCount = 1 },
                new Models.Badge { Name = "Hızlı Çaylak", Description = "3. ödevini teslim ettin. Isınmaya başladın!", Icon = "fas fa-running text-info", ConditionType = "SUBMISSION_COUNT", RequiredCount = 3 },
                new Models.Badge { Name = "Çalışkan", Description = "5. ödev teslimini yaptın. Disiplin senin göbek adın!", Icon = "fas fa-fire text-warning", ConditionType = "SUBMISSION_COUNT", RequiredCount = 5 },
                new Models.Badge { Name = "İstikrarlı", Description = "10. ödevini teslim ettin. Bu kararlılıkla seni kimse tutamaz!", Icon = "fas fa-shield-alt text-primary", ConditionType = "SUBMISSION_COUNT", RequiredCount = 10 },
                new Models.Badge { Name = "Usta", Description = "25 ödev teslimi! Sen artık bu işin ustasısın.", Icon = "fas fa-star text-warning", ConditionType = "SUBMISSION_COUNT", RequiredCount = 25 },
                new Models.Badge { Name = "Efsane", Description = "50 ödev teslimi! Adın sistemin altın sayfalarına yazıldı.", Icon = "fas fa-dragon text-danger", ConditionType = "SUBMISSION_COUNT", RequiredCount = 50 },
                new Models.Badge { Name = "Durdurulamaz Makine", Description = "100 ödev! Sen bir insan olamazsın, saygılar...", Icon = "fas fa-robot text-secondary", ConditionType = "SUBMISSION_COUNT", RequiredCount = 100 }
            };

            var existingBadgeNames = context.Badges.Select(b => b.Name).ToList();

            var badgesToAdd = systemBadges.Where(b => !existingBadgeNames.Contains(b.Name)).ToList();

            if (badgesToAdd.Any())
            {
                await context.Badges.AddRangeAsync(badgesToAdd);
                await context.SaveChangesAsync();
            }
        }
    }
}