using System.Data.SqlClient;

namespace PermissionTaskWithMVC
{
    public class Connection
    {
        public static SqlConnection getConnection() 
        {
            var cs = "Data Source=DESKTOP-8D8TGL4\\SQLEXPRESS; Initial Catalog=permissionTask; Integrated Security=true";
            var conn = new SqlConnection(cs);
            return conn;
        }
    }
}
