using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        return user;
    }


    public async Task AddUserAsync(User user)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists.");
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }


    public Task GetUserAsync(User user)
    {
        _context.Users.Attach(user);
        return Task.CompletedTask;
    }

    // Get all users
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
}
