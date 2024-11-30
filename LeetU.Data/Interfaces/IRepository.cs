using Microsoft.EntityFrameworkCore;

public interface IRepository
{
    DbSet<T> Set<T>() where T : class;
}