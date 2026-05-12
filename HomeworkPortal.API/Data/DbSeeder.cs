using HomeworkPortal.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
                    var result = await roleManager.CreateAsync(new AppRole { Name = role });
                    if (!result.Succeeded) throw new Exception($"Rol oluşturulamadı ({role}): {string.Join(", ", result.Errors.Select(e => e.Description))}");
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
                var result = await userManager.CreateAsync(admin, "Admin.123!");
                if (!result.Succeeded) throw new Exception($"Admin oluşturulamadı: {string.Join(", ", result.Errors.Select(e => e.Description))}");

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
                    var result = await userManager.CreateAsync(teacher, $"Teacher{index}.123!");
                    if (!result.Succeeded) throw new Exception($"Öğretmen oluşturulamadı ({teacher.Email}): {string.Join(", ", result.Errors.Select(e => e.Description))}");

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
                    var result = await userManager.CreateAsync(student, $"Student{index}.123!");
                    if (!result.Succeeded) throw new Exception($"Öğrenci oluşturulamadı ({student.Email}): {string.Join(", ", result.Errors.Select(e => e.Description))}");

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

            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Temel Yeterlilik", Icon = "fas fa-square-root-alt" },
                    new Category { Name = "Alan Yeterlilik(Sayısal)", Icon = "fas fa-square-root-alt" },
                    new Category { Name = "Alan Yeterlilik(Sözel)", Icon = "fas fa-book" },
                    new Category { Name = "Alan Yeterlilik(Dil)", Icon = "fas fa-language" }
                };
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            if (!await context.Courses.AnyAsync())
            {
                var t1 = await context.Users.FirstOrDefaultAsync(u => u.Email == "teacher01@gmail.com");
                var t2 = await context.Users.FirstOrDefaultAsync(u => u.Email == "teacher02@gmail.com");
                var t3 = await context.Users.FirstOrDefaultAsync(u => u.Email == "teacher03@gmail.com");
                var t4 = await context.Users.FirstOrDefaultAsync(u => u.Email == "teacher04@gmail.com");
                var t5 = await context.Users.FirstOrDefaultAsync(u => u.Email == "teacher05@gmail.com");

                if (t1 != null && t2 != null && t3 != null && t4 != null && t5 != null)
                {
                    var courses = new List<Course>
                    {
                        new Course { Name = "Temel Matematik", Description = "TYT", TeacherId = t2.Id, CategoryId = 1 },
                        new Course { Name = "İleri Matematik", Description = "AYT", TeacherId = t2.Id, CategoryId = 2 },
                        new Course { Name = "Türkçe", Description = "TYT", TeacherId = t3.Id, CategoryId = 3 },
                        new Course { Name = "Edebiyat", Description = "AYT", TeacherId = t3.Id, CategoryId = 3 },
                        new Course { Name = "Temel Fizik", Description = "TYT", TeacherId = t1.Id, CategoryId = 1 },
                        new Course { Name = "İleri Fizik", Description = "AYT", TeacherId = t1.Id, CategoryId = 2 },
                        new Course { Name = "Temel Kimya", Description = "TYT", TeacherId = t4.Id, CategoryId = 1 },
                        new Course { Name = "İleri Kimya", Description = "AYT", TeacherId = t4.Id, CategoryId = 2 },
                        new Course { Name = "Temel Biyoloji", Description = "TYT", TeacherId = t5.Id, CategoryId = 1 },
                        new Course { Name = "İleri Biyoloji", Description = "AYT", TeacherId = t5.Id, CategoryId = 2 }
                    };

                    await context.Courses.AddRangeAsync(courses);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Kurslar oluşturulamadı çünkü öğretmenler veritabanında bulunamadı!");
                }
            }
        }
    }
}