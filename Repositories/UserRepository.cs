using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public UserRepository(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result> Login(LoginDTO dto)
    {
        User user = await _userService.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            return new Result
            {
                Message = "Email not found",
                StatusCode = 404
            };
        }

        if (!_passwordHasher.Verify(user.PasswordHash, dto.Password, user.Salt))
        {
            return new Result
            {
                Message = "Email or Password not correct",
                StatusCode = 400
            };
        }

        string token = await _jwtTokenService.GenerateToken(user);

        return new Result
        {
            Message = token,
            StatusCode = 200
        };
    }

    public async Task<Result> Register(RegisterDTO dto)
    {
        if (await _userService.ExistsAsync(dto.Email))
        {
            return new Result
            {
                Message = "Email is already registered",
                StatusCode = 400
            };
        }

        string salt = _passwordHasher.GenerateSalt();
        string passwordHash = _passwordHasher.Encrypt(dto.Password, salt);

        UserProfile userProfile = new UserProfile
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Age = dto.Age,
            CountryId = dto.Country
        };

        User newUser = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            UserProfile = userProfile
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User registred successfully",
            StatusCode = 201
        };
    }

    public async Task<Result<UserGetDTO>> Me()
    {
        int userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

        UserGetDTO? user = await _context.Users
            //.Include(u => u.UserRoles)
            .Where(u => u.Id == userId)
            .Select(u => new UserGetDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                PhoneNumber = u.UserProfile.PhoneNumber,
                Age = u.UserProfile.Age,
                Country = u.UserProfile.Country.Name,
                CreatedAt = u.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                Roles = u.UserRoles
                    .OrderBy(p => p.RoleId)
                    .Select(ur => new RoleGetDTO
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new Result<UserGetDTO>
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<UserGetDTO>
        {
            StatusCode = 200,
            Data = user
        };
    }

    public async Task<Result<List<UserGetDTO>>> GetAllUsers()
    {
        List<UserGetDTO>? users = await _context.Users
            .OrderBy(p => p.Id)
            .Select(u => new UserGetDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                PhoneNumber = u.UserProfile.PhoneNumber,
                Age = u.UserProfile.Age,
                Country = u.UserProfile.Country.Name,
                CreatedAt = u.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss")
            })
            .ToListAsync();

        if (users.Count == 0)
        {
            return new Result<List<UserGetDTO>>
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<List<UserGetDTO>>
        {
            StatusCode = 200,
            Data = users
        };
    }

    public async Task<Result<UserGetDTO>> GetUserProfileById(int id)
    {
        UserGetDTO? user = await _context.Users
            .Where(u => u.Id == id)
            .Select(u => new UserGetDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                PhoneNumber = u.UserProfile.PhoneNumber,
                Age = u.UserProfile.Age,
                Country = u.UserProfile.Country.Name,
                CreatedAt = u.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss")
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new Result<UserGetDTO>
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<UserGetDTO>
        {
            StatusCode = 200,
            Data = user
        };
    }

    public async Task<Result<UserGetDTO>> SearchUserByQuery(int id)
    {
        UserGetDTO? user = await _context.Users
            .Include(u => u.UserProfile)
            .Where(u => u.Id == id)
            .Select(u => new UserGetDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                PhoneNumber = u.UserProfile.PhoneNumber,
                Age = u.UserProfile.Age,
                Country = u.UserProfile.Country.Name,
                CreatedAt = u.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss")
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new Result<UserGetDTO>
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<UserGetDTO>
        {
            Message = "From Database",
            StatusCode = 200,
            Data = user
        };
    }

    public async Task<Result> UpdateUserProfileById(int id, UserProfileUpdateDTO dto)
    {
        User? user = await _context.Users
            .Include(p => p.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return new Result
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        user.UserProfile.FirstName = dto.FirstName;
        user.UserProfile.LastName = dto.LastName;
        user.UserProfile.PhoneNumber = dto.PhoneNumber;
        user.UserProfile.Age = dto.Age;
        user.UserProfile.CountryId = dto.Country;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User updated successfully",
            StatusCode = 200
        };
    }

    public async Task<Result> UpdateUserPasswordById(int id, UserPasswordUpdateDTO dto)
    {
        User? user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return new Result
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        if (!_passwordHasher.Verify(user.PasswordHash, dto.OldPassword, user.Salt))
        {
            return new Result
            {
                Message = "Old Password not correct",
                StatusCode = 400
            };
        }

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            return new Result
            {
                Message = "NewPassword and ConfirmPassword not equal",
                StatusCode = 400
            };
        }

        string newSalt = _passwordHasher.GenerateSalt();
        string newHashedPassword = _passwordHasher.Encrypt(dto.NewPassword, newSalt);

        user.Salt = newSalt;
        user.PasswordHash = newHashedPassword;

        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User password updated successfully",
            StatusCode = 200
        };
    }

    public async Task<Result> UpdateUserRoleById(int id, UserRoleUpdateDTO dto)
    {
        if (!await _userService.ExistsAsync(id))
        {
            return new Result
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        List<UserRole> userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == id)
            .ToListAsync();

        _context.UserRoles.RemoveRange(userRoles);

        List<UserRole> newUserRoles = dto.RoleIds.Select(rid => new UserRole
        {
            UserId = id,
            RoleId = rid
        })
        .ToList();

        await _context.UserRoles.AddRangeAsync(newUserRoles);
        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User roles updated successfully",
            StatusCode = 200
        };
    }
    
    public async Task<Result> DeleteUserById(int id)
    {
        User user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return new Result
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User deleted successfully",
            StatusCode = 200
        };
    }

    public async Task<Result> RestoreUserById(int id)
    {
        User user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return new Result
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        user.IsDeleted = false;
        user.DeletedAt = null;

        await _context.SaveChangesAsync();

        return new Result
        {
            Message = "User restored successfully",
            StatusCode = 200
        };
    }
}
