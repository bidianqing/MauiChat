namespace MauiChat.Services
{
    public interface IRepositoryService
    {
        Task<int> InsertAsync<T>(T entity) where T : class, new();

        Task<List<T>> QueryAsync<T>(string sql) where T : new();
    }
}
