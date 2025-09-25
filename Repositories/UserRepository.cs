using UzTube.Database;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models.DTO;
using UzTube.Services;

namespace UzTube.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IUserService _userService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public UserRepository(
        AppDbContext context,
        IUserService userService,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _userService = userService;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public Result Login(LoginDTO dto)
    {
        if (!_userService.Exists(dto.Email))
        {
            return new Result
            {
                Message = "Email not found",
                StatusCode = 404
            };
        }

        User user = _userService.GetByEmail(dto.Email);

        if (!_passwordHasher.Verify(user.PasswordHash, dto.Password, user.Salt))
        {
            return new Result
            {
                Message = "Email or Password not correct",
                StatusCode = 400
            };
        }

        string token = _jwtTokenService.GenerateToken(user);

        return new Result
        {
            Message = token,
            StatusCode = 200
        };
    }

    public Result Register(RegisterDTO dto)
    {
        if (_userService.Exists(dto.Email))
        {
            return new Result
            {
                Message = "Email already exists",
                StatusCode = 400
            };
        }

        string salt = Guid.NewGuid().ToString();
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
            Profile = userProfile
        };

        _context.Users.Add(newUser);
        _context.SaveChanges();

        return new Result
        {
            Message = "User registred successfully",
            StatusCode = 201
        };
    }

    public Result<UserGetDTO> UserProfile(int userId)
    {
        if (!_userService.Exists(userId))
        {
            return new Result<UserGetDTO>
            {
                Message = "User not found",
                StatusCode = 404
            };
        }

        UserGetDTO? user = _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserGetDTO
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.Profile.FirstName,
                LastName = u.Profile.LastName,
                PhoneNumber = u.Profile.PhoneNumber,
                Age = u.Profile.Age,
                Country = u.Profile.Country.Name,
                CreatedAt = u.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss")
            })
            .FirstOrDefault();

        return new Result<UserGetDTO>
        {
            StatusCode = 200,
            Data = user
        };
    }
}
