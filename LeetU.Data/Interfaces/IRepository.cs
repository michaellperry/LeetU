public interface IRepository
{
    IQueryable<T> Set<T>() where T : class;
}