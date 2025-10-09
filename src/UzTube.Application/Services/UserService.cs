using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models;
using UzTube.Models.DTO;

namespace UzTube.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiResult> Login(LoginDTO dto)
    {
        User user = await .GetUserByEmailAsync(dto.Email);

        if (user == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "Email not found",
                StatusCode = 404
            };
        }

        if (!_passwordHasher.Verify(user.PasswordHash, dto.Password, user.Salt))
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "Email or Password not correct",
                StatusCode = 400
            };
        }

        string token = await _jwtTokenService.GenerateTokenAsync(user);

        return new ApiResult
        {
            Succeed = true,
            Message = token,
            StatusCode = 200
        };
    }

    public async Task<ApiResult> Register(RegisterDTO dto)
    {
        if (await _userService.ExistsAsync(dto.Email))
        {
            return new ApiResult
            {
                Succeed = false,
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

        return new ApiResult
        {
            Succeed = true,
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
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<UserGetDTO>
        {
            Succeed = true,
            StatusCode = 200,
            Data = user
        };
    }

    public async Task<Result<List<UserGetDTO>>> GetAllUsersAsync()
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
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<List<UserGetDTO>>
        {
            Succeed = true,
            StatusCode = 200,
            Data = users
        };
    }

    public async Task<Result<UserGetDTO>> GetUserProfileByIdAsync(int id)
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
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<UserGetDTO>
        {
            Succeed = true,
            StatusCode = 200,
            Data = user
        };
    }

    public async Task<Result<UserGetDTO>> SearchUserByQueryAsync(int id)
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
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        return new Result<UserGetDTO>
        {
            Succeed = true,
            Message = "From Database",
            StatusCode = 200,
            Data = user
        };
    }

    public async Task<ApiResult> UpdateUserProfileByIdAsync(int id, UserProfileUpdateDTO dto)
    {
        User? user = await _context.Users
            .Include(p => p.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return new ApiResult
            {
                Succeed = false,
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

        return new ApiResult
        {
            Succeed = true,
            Message = "User updated successfully",
            StatusCode = 200
        };
    }

    public async Task<ApiResult> UpdateUserPasswordByIdAsync(int id, UserPasswordUpdateDTO dto)
    {
        User? user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        if (!_passwordHasher.Verify(user.PasswordHash, dto.OldPassword, user.Salt))
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "Old Password not correct",
                StatusCode = 400
            };
        }

        if (dto.NewPassword != dto.ConfirmPassword)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "NewPassword and ConfirmPassword not equal",
                StatusCode = 400
            };
        }

        string newSalt = _passwordHasher.GenerateSalt();
        string newHashedPassword = _passwordHasher.Encrypt(dto.NewPassword, newSalt);

        user.Salt = newSalt;
        user.PasswordHash = newHashedPassword;

        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "User password updated successfully",
            StatusCode = 200
        };
    }

    public async Task<ApiResult> UpdateUserRoleByIdAsync(int id, UserRoleUpdateDTO dto)
    {
        if (!await _userService.ExistsAsync(id))
        {
            return new ApiResult
            {
                Succeed = false,
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

        return new ApiResult
        {
            Succeed = true,
            Message = "User roles updated successfully",
            StatusCode = 200
        };
    }
    
    public async Task<ApiResult> DeleteUserByIdAsync(int id)
    {
        User user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "User deleted successfully",
            StatusCode = 200
        };
    }

    public async Task<ApiResult> RestoreUserByIdAsync(int id)
    {
        User user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return new ApiResult
            {
                Succeed = false,
                Message = "User not found",
                StatusCode = 404
            };
        }

        user.IsDeleted = false;
        user.DeletedAt = null;

        await _context.SaveChangesAsync();

        return new ApiResult
        {
            Succeed = true,
            Message = "User restored successfully",
            StatusCode = 200
        };
    }
}
