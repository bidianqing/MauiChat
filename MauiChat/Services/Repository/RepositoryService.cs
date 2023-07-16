namespace MauiChat.Services
{
    public class RepositoryService : IRepositoryService
    {
        private SQLiteAsyncConnection connection;
        private readonly string _databasePath;

        public RepositoryService(string databasePath)
        {
            _databasePath = databasePath;
        }

        private async Task Init()
        {
            if (connection is not null)
                return;

            connection = new SQLiteAsyncConnection(_databasePath);

            await Task.CompletedTask;
        }

        public async Task<int> InsertAsync<T>(T entity) where T : class, new()
        {
            // Call Init()
            await Init();

            return await connection.InsertAsync(entity);
        }

        public async Task<int> DeleteAsync<T>(T entity)
        {
            // Call Init()
            await Init();

            return await connection.DeleteAsync(entity);
        }

        public async Task QueryAsync<T>(string sql) where T : new()
        {
            // Call Init()
            await Init();

            await connection.QueryAsync<T>(sql);
        }

        public async Task ExecuteAsync(string sql)
        {
            // Call Init()
            await Init();

            await connection.ExecuteAsync(sql);
        }

        public async Task<int> ExecuteScalaAsync(string sql)
        {
            // Call Init()
            await Init();

            return await connection.ExecuteScalarAsync<int>(sql);
        }
    }
}
