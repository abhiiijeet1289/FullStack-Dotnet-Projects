using System.Data.SqlClient;

namespace BookstoreApp.Data
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public static void SafeExecute(Action action, Action<Exception>? onError = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                throw;
            }
        }

        public static T SafeExecute<T>(Func<T> func, Action<Exception>? onError = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                throw;
            }
        }
    }
}