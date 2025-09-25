using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Repositories;
using UzTube.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionSQL = builder.Configuration.GetConnectionString("DefaultConnection");
var secretKey = builder.Configuration["JwtOptions:SecretKey"];

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer YOUR_TOKEN')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseNpgsql(connectionSQL);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


var app = builder.Build();

// ==== PERMISSIONS SYNC ====
using (var scope = app.Services.CreateScope())
{
    var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // DB permissions
    HashSet<Permission> dbPermissions = _context.Permissions.ToHashSet();

    // Enum permissions
    List<SystemPermissions> enumPermissions = Enum.GetValues(typeof(SystemPermissions))
                              .Cast<SystemPermissions>()
                              .ToList();

    // 1. Enumda bor, DB da yo‘q => qo‘shamiz
    foreach (SystemPermissions permission in enumPermissions)
    {
        if (!dbPermissions.Any(p => p.Id == (int)permission))
        {
            _context.Permissions.Add(new Permission
            {
                Id = (int)permission,
                Name = permission.ToString(),
                Description = permission + " - Permission"
            });

            Console.WriteLine($"Added: {permission}");
        }
    }

    // 2. DB da bor, Enumda yo‘q => o‘chiramiz
    foreach (Permission dbPermission in dbPermissions)
    {
        if (!enumPermissions.Any(e => (int)e == dbPermission.Id))
        {
            _context.Permissions.Remove(dbPermission);
            Console.WriteLine($"Removed: {dbPermission.Name}");
        }
    }

    // 3. Ikkala joyda ham bor, lekin Name farq qiladi => yangilaymiz
    foreach (Permission dbPermission in dbPermissions)
    {
        SystemPermissions enumMatch = enumPermissions.FirstOrDefault(e => (int)e == dbPermission.Id);

        if (enumMatch != default && dbPermission.Name != enumMatch.ToString())
        {
            dbPermission.Name = enumMatch.ToString();
            dbPermission.Description = enumMatch + " - Permission";
            _context.Permissions.Update(dbPermission);

            Console.WriteLine($"Changed: {enumMatch}");
        }
    }

    _context.SaveChanges();
}

// ==== ROLES SEED ====
using (var scope = app.Services.CreateScope())
{
    var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    List<Role> defaultRoles = new List<Role>
    {
        new Role
        {
            Id = 1,
            Name = "root",
            Description = "root - Role",
            RolePermissions = new List<RolePermission>
            {
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewUsers },
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ManageUser },
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewRoles },
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ManageRoles },
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewPermissions },
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ViewAuditLogs },
                new RolePermission { RoleId = 1, PermissionId = (int)SystemPermissions.ManageSystem }
            }
        },
        new Role
        {
            Id = 2,
            Name = "Admin",
            Description = "Admin - Role",
            RolePermissions = new List<RolePermission>
            {
                new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ViewUsers },
                new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ManageUser },
                new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ViewRoles },
                new RolePermission { RoleId = 2, PermissionId = (int)SystemPermissions.ManageRoles }
            }
        }
    };

    HashSet<string> dbRoles = _context.Roles
        .Select(dr => dr.Name)
        .ToHashSet();

    foreach (Role defaultRole in defaultRoles)
    {
        if (!dbRoles.Contains(defaultRole.Name))
        {
            _context.Roles.Add(defaultRole);
        }
    }

    _context.SaveChanges();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
