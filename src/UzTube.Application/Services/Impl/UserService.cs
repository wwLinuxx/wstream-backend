using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UzTube.Application.Exeptions;
using UzTube.Application.Helpers.Interfaces;
using UzTube.Application.Models;
using UzTube.Application.Models.Role;
using UzTube.Application.Models.User;
using UzTube.DataAccess.Persistence;
using UzTube.DataAccess.Repositories;
using UzTube.Entities;
using UzTube.Interfaces;
using UzTube.Models.DTO;
using UzTube.Shared.Services;

namespace UzTube.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IClaimService _claimService;

    public UserService(
        DatabaseContext context,
        IConfiguration configuration,
        IUserRepository userRepository,
        IPasswordHelper passwordHelper,
        IClaimService claimService)
    {
        _context = context;
        _configuration = configuration;
        _userRepository = userRepository;
        _passwordHelper = passwordHelper;
        _claimService = claimService;
    }

    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel dto)
    {
        //if (await _userRepository.CheckUserByEmailAsync(dto.Email))
        //    throw new BadRequestException("Email is already registered");

        string salt = _passwordHelper.GenerateSalt();
        string passwordHash = _passwordHelper.Encrypt(dto.Password, salt);

        UserProfile userProfile = new UserProfile
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            Age = dto.Age,
            CountryId = dto.CountryId
        };

        User newUser = new User
        {
            Email = dto.Email,
            PasswordHash = passwordHash,
            Salt = salt,
            UserProfile = userProfile
        };

        await _userRepository.AddAsync(newUser);

        return new CreateUserResponseModel { Id = newUser.Id };
    }

    public async Task<LoginResponseModel> LoginAsync(LoginUserModel dto)
    {
        User? user = await _userRepository.GetUserByEmailAsync(dto.Email);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!_passwordHelper.Verify(user.PasswordHash, dto.Password, user.Salt))
            throw new BadRequestException("Email or Password not correct");

        var permissions = await _userRepository.GetUserAllPermissionsAsync(user.Id);

        string token = JwtHelper.GenerateToken(user, permissions, _configuration);

        return new LoginResponseModel
        {
            Email = user.Email,
            Token = token
        };
    }

    public async Task<UserResponseModel> GetMeAsync()
    {
        Guid userId = Guid.Parse(_claimService.GetUserId());

        IQueryable<User> query = _userRepository.QueryUsers();

        UserResponseModel? user = await query
            .Where(u => u.Id == userId)
            .Select(u => new UserResponseModel
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
                    .Select(ur => new RoleResponseModel
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name
                    }).ToList()
            })
            .FirstOrDefaultAsync();

        if (user == null)
            throw new NotFoundException("User not found");

        return user;
    }

    public async Task<List<UserResponseModel>> GetUsersAsync()
    {
        IQueryable<User> query = _userRepository.QueryUsers();

        List<UserResponseModel>? users = await query
            .OrderBy(p => p.Id)
            .Select(u => new UserResponseModel
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
            throw new NotFoundException("User not found");

        return users;
    }

    public async Task<PaginatedList<UserListResponseModel>> GetUsersListAsync(PageOption option)
    {
        IQueryable<User> query = _context.Users;

        if (!string.IsNullOrEmpty(option.Search))
            query = query.Where(u => u.UserProfile.FirstName.Contains(option.Search.Trim(), StringComparison.OrdinalIgnoreCase));

        List<UserListResponseModel> users = await query
            .Skip((option.PageNumber - 1) * option.PageSize)
            .Take(option.PageSize)
            .Select(u => new UserListResponseModel
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
            throw new NotFoundException("Users not found");

        int pagesCount = await _context.Posts.CountAsync();

        return PaginatedList<UserListResponseModel>.Create(
            users, 
            pagesCount, 
            option.PageNumber, 
            option.PageSize);
    }

    public async Task<UserResponseModel> GetUserProfileByIdAsync(Guid id)
    {
        IQueryable<User> query = _userRepository.QueryUsers();

        UserResponseModel? user = await query
            .Where(u => u.Id == id)
            .Select(u => new UserResponseModel
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
            throw new NotFoundException("User not found");

        return user;
    }

    public async Task<UserResponseModel> SearchUserByQueryAsync(Guid id)
    {
        IQueryable<User> query = _userRepository.QueryUsers();

        UserResponseModel? user = await query
            .Include(u => u.UserProfile)
            .Where(u => u.Id == id)
            .Select(u => new UserResponseModel
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
            throw new NotFoundException("User not found");

        return user;
    } //TODO: name bo'yicha qidirish

    public async Task<UpdateUserProfileResponseModel> UpdateUserProfileByIdAsync(Guid id, UpdateUserProfileModel dto)
    {
        IQueryable<User> query = _userRepository.QueryUsers();

        User? user = await query
            .Include(p => p.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new NotFoundException("User not found");

        user.UserProfile.FirstName = dto.FirstName;
        user.UserProfile.LastName = dto.LastName;
        user.UserProfile.PhoneNumber = dto.PhoneNumber;
        user.UserProfile.Age = dto.Age;
        user.UserProfile.CountryId = dto.CountryId;

        await _userRepository.UpdateAsync(user);

        return new UpdateUserProfileResponseModel { Id = user.Id };
    }

    public async Task<UpdateUserPasswordResponseModel> UpdateUserPasswordByIdAsync(Guid id, UpdateUserPasswordModel dto)
    {
        User? user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        if (!_passwordHelper.Verify(user.PasswordHash, dto.OldPassword, user.Salt))
            throw new BadRequestException("Old Password not correct");

        if (dto.NewPassword != dto.ConfirmPassword)
            throw new BadRequestException("NewPassword and ConfirmPassword not equal");

        string newSalt = _passwordHelper.GenerateSalt();
        string newHashedPassword = _passwordHelper.Encrypt(dto.NewPassword, newSalt);

        user.Salt = newSalt;
        user.PasswordHash = newHashedPassword;

        await _userRepository.UpdateAsync(user);

        return new UpdateUserPasswordResponseModel { Id = id };
    }

    public async Task<UpdateUserRoleResponseModel> UpdateUserRoleByIdAsync(Guid id, UpdateUserRoleModel dto)
    {
        if (!await _userRepository.CheckUserByIdAsync(id))
            throw new NotFoundException("User not found");

        List<UserRole> userRoles = await _userRepository.GetUserAllRolesListAsync(id);

        _context.UserRoles.RemoveRange(userRoles);

        List<UserRole> newUserRoles = dto.RoleIds.Select(rid => new UserRole
        {
            UserId = id,
            RoleId = rid
        })
        .ToList();

        await _context.UserRoles.AddRangeAsync(newUserRoles);
        await _context.SaveChangesAsync();

        return new UpdateUserRoleResponseModel { Id = id };
    }

    public async Task<DeleteUserResonseModel> DeleteUserByIdAsync(Guid id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        user.IsDeleted = true;
        user.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new DeleteUserResonseModel { Result = "Success" };
    }

    public async Task<RestoreUserResponseModel> RestoreUserByIdAsync(Guid id)
    {
        User? user = await _userRepository.GetUserByIdAsync(id);

        if (user == null)
            throw new NotFoundException("User not found");

        user.IsDeleted = false;
        user.DeletedAt = null;

        await _context.SaveChangesAsync();

        return new RestoreUserResponseModel { Id = id };
    }
}
