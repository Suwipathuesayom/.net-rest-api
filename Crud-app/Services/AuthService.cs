
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<string> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userRepository.GetUserByUsernameAsync(registerDto.Username!);
        if (existingUser != null)
        {
            throw new Exception("Username already taken.");
        }

        var passwordHash = _passwordHasher.HashPassword(registerDto.Password!);
        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash
        };

        await _userRepository.AddUserAsync(user);
        return "Registration successful.";
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username!);
        if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password!, user.PasswordHash!))
        {
            throw new Exception("Invalid username or password.");
        }

        return "Login successful.";
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }
}

