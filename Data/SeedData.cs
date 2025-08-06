using LaborExchangeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LaborExchangeApi.Data;

public static class SeedData
{
    public static void SeedDatabase(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var maxRetries = 10;
            var delay = 2000;
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    Console.WriteLine("--> Attempting to apply migrations...");
                    context.Database.Migrate();
                    Console.WriteLine("--> Migrations applied successfully. Database is ready.");
                    break;
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    Console.WriteLine($"--> Could not connect to database, retrying... ({i + 1}/{maxRetries}) - Error: {ex.Message}");
                    if (i == maxRetries - 1)
                    {
                        Console.WriteLine("--> Max retries reached. Could not connect to database.");
                        throw;
                    }
                    Thread.Sleep(delay);
                }
            }

            Console.WriteLine("--> Attempting to apply migrations...");
            context.Database.SetCommandTimeout(180);
            context.Database.Migrate();

            if (context.Categories.Any())
            {
                Console.WriteLine("--> We already have data, no need to seed.");
                return;
            }

            Console.WriteLine("--> Seeding data...");

            var categories = new List<Category>
            {
                new() { Name = "Информационные технологии" },
                new() { Name = "Бухгалтерия и финансы" },
                new() { Name = "Маркетинг и реклама" },
                new() { Name = "Строительство и архитектура" },
                new() { Name = "Медицина и фармацевтика" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var companies = new List<Company>
            {
                new() { Name = "Google Inc.", ContactPerson = "Сергей Брин", Email = "hr@google.com", Phone = "111-222-333" },
                new() { Name = "Microsoft Corp.", ContactPerson = "Билл Гейтс", Email = "hr@microsoft.com", Phone = "444-555-666" },
                new() { Name = "Epam Systems", ContactPerson = "Аркадий Добкин", Email = "hr@epam.com", Phone = "777-888-999" }
            };
            context.Companies.AddRange(companies);
            context.SaveChanges();

            var unemployed = new List<Unemployed>
            {
                new() { FirstName = "Иван", LastName = "Петров", DateOfBirth = new DateOnly(1995, 5, 20), ContactEmail = "ivan.petrov@example.com", ContactPhone = "123-456-789" },
                new() { FirstName = "Мария", LastName = "Сидорова", DateOfBirth = new DateOnly(1998, 8, 15), ContactEmail = "maria.sidorova@example.com", ContactPhone = "987-654-321" },
                new() { FirstName = "Алексей", LastName = "Иванов", DateOfBirth = new DateOnly(1990, 1, 1), ContactEmail = "alex.ivanov@example.com", ContactPhone = "555-555-555" }
            };
            context.Unemployed.AddRange(unemployed);
            context.SaveChanges();

            var resumes = new List<Resume>
            {
                new()
                {
                    Title = "Junior .NET Developer",
                    Skills = "C#, ASP.NET Core, EF Core, SQL",
                    Experience = "1 год коммерческой разработки.",
                    UnemployedId = unemployed.First(u => u.FirstName == "Иван").Id,
                    CategoryId = categories.First(c => c.Name.Contains("технологии")).Id
                },
                new()
                {
                    Title = "Главный бухгалтер",
                    Skills = "1C, M.E.Doc, Excel, Налоговый кодекс",
                    Experience = "5 лет опыта работы главным бухгалтером.",
                    UnemployedId = unemployed.First(u => u.FirstName == "Мария").Id,
                    CategoryId = categories.First(c => c.Name.Contains("Бухгалтерия")).Id
                }
            };
            context.Resumes.AddRange(resumes);
            context.SaveChanges();

            var vacancies = new List<Vacancy>
            {
                new()
                {
                    Title = "Senior Backend Engineer (C#)",
                    Description = "Ищем опытного разработчика для работы над высоконагруженными сервисами.",
                    Salary = 150000,
                    CompanyId = companies.First(c => c.Name == "Google Inc.").Id,
                    CategoryId = categories.First(c => c.Name.Contains("технологии")).Id
                },
                new()
                {
                    Title = "Product Marketing Manager",
                    Description = "Требуется менеджер по маркетингу для продвижения нового продукта.",
                    Salary = 80000,
                    CompanyId = companies.First(c => c.Name == "Microsoft Corp.").Id,
                    CategoryId = categories.First(c => c.Name.Contains("Маркетинг")).Id
                },
                new()
                {
                    Title = "QA Automation Engineer",
                    Description = "Автоматизация тестирования веб-приложений.",
                    Salary = 75000,
                    CompanyId = companies.First(c => c.Name == "Epam Systems").Id,
                    CategoryId = categories.First(c => c.Name.Contains("технологии")).Id
                }
            };
            context.Vacancies.AddRange(vacancies);

            context.SaveChanges();

            Console.WriteLine("--> Data has been seeded successfully.");
        }
    }
}