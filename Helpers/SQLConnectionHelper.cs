using System.Data.SqlClient;

namespace SPANTask.Helpers
{
    public abstract class SQLConnectionHelper
    {
        protected SqlConnection Connection;
        public SQLConnectionHelper(string connectionString)
        {

            try
            {
                // Creating Connection  
                Connection = new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Command not valid. " + e);
            }

        }
    }
}
