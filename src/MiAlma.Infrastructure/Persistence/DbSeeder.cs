using MiAlma.Domain.Entities;

namespace MiAlma.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static readonly Guid TestUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        public const string TestUserEmail = "test@mialma.dev";
        public const string TestUserPassword = "Password123!";

        public static async Task SeedAsync(MiAlmaDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Id = TestUserId,
                    Email = TestUserEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(TestUserPassword),
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (!context.Rfps.Any())
            {
                context.Rfps.AddRange(
                    new Rfp
                    {
                        Id = Guid.NewGuid(),
                        Title = "Modernización de infraestructura de red municipal",
                        Agency = "Secretaría de Tecnología - Ciudad de Bogotá",
                        Description = "Se solicita propuesta para la renovación de la infraestructura de red de las oficinas municipales, incluyendo cableado, switches y conectividad redundante.",
                        Deadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Rfp
                    {
                        Id = Guid.NewGuid(),
                        Title = "Sistema de gestión documental para archivo judicial",
                        Agency = "Rama Judicial",
                        Description = "Desarrollo e implementación de un sistema de gestión documental digital para el archivo central, con capacidades de búsqueda y control de acceso.",
                        Deadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(45)),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Rfp
                    {
                        Id = Guid.NewGuid(),
                        Title = "Mantenimiento de vías secundarias - Zona Norte",
                        Agency = "Instituto Nacional de Vías",
                        Description = "Contratación de servicios de mantenimiento preventivo y correctivo de vías secundarias en la zona norte del departamento.",
                        Deadline = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(60)),
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
