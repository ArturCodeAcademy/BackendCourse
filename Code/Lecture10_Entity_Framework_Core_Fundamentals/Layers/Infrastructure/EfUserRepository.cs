using Lecture10.Application;
using Lecture10.Domain;
using Microsoft.EntityFrameworkCore;

namespace Lecture10.Infrastructure;

public sealed class EfUserRepository : IUserRepository
{
    private readonly ExpenseDbContext context;

    public EfUserRepository(ExpenseDbContext context)
    {
        this.context = context;
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        return context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Username == username);
    }

    public async Task AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }
}
