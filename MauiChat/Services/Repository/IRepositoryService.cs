namespace MauiChat.Services
{
    public interface IRepositoryService
    {
        Task<int> InsertAsync<T>(T entity) where T : class, new();
    }
}
