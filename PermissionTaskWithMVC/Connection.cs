using System.Data.SqlClient;

namespace PermissionTaskWithMVC
{
    public class Connection
    {
        public static SqlConnection getConnection() 
        {
            var cs = "Data Source=DESKTOP-GU6T46F\\SQLEXPRESS; Initial Catalog=permissionTaskMvc; Integrated Security=true";
            var conn = new SqlConnection(cs);
            return conn;
        }
    }
}
