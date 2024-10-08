using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;  // Import for using LINQ queries
using BCrypt.Net;  // You can install BCrypt.Net-Next package for password hashing


public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _unitOfWork.Users.GetAllAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Hash the password before saving it in the database
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        return await _unitOfWork.Users.AddAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await _unitOfWork.Users.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _unitOfWork.Users.DeleteAsync(id);
    }

    // Authentication method that validates username and password
    public async Task<User> Authenticate(string username, string password)
    {
        // Find the user by username
        var user = (await _unitOfWork.Users.GetAllAsync())
                    .FirstOrDefault(u => u.Username == username);

        // If user not found or password is incorrect, return null
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return null;

        // Authentication successful
        return user;
    }
}