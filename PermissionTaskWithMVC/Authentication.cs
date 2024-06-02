using System.Data;
using System.Data.SqlClient;

namespace PermissionTaskWithMVC
{
    public class Authentication
    {
        public static bool IsAuthenticated(int id, string pass)
        {
            SqlConnection conn = null;
            conn = Connection.getConnection();
            if(conn != null)
            {
                conn.Open();
                string actualPass;
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT password FROM users WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("id", id);
                    actualPass = Convert.ToString(cmd.ExecuteScalar());
                }
                catch
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }


                if (actualPass == pass)
                {
                    return true;
                }
                return false;
            }
            else
            {
                Console.WriteLine("Can't Connect to the database");
                return false;
            }
        }
    }
}
