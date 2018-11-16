using Template.CrossCutting;
using System.Data.Common;
using System.Data.SqlClient;

namespace Template.Infra
{
    public class ConnectionFactory
    {
        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection(ConnectionStrings.TemplateConnection);

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            return connection;
        }
    }
}
