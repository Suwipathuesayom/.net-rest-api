public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);

    // Get Users
     Task<List<User>> GetAllUsersAsync();
}
