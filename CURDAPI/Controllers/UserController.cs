using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CURDAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private readonly SqlConnection conn;

        public UserController()
        {
            string cs = "Data Source=DESKTOP-8D8TGL4\\SQLEXPRESS; Initial Catalog=permissionTask; Integrated Security=true";
            conn = new SqlConnection(cs);
        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            List<User> users = new List<User>();

            if (conn != null)
            {
                conn.Open();
                try
                {

                    SqlCommand cmd = new SqlCommand("getAllUsers", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        int fields = rd.FieldCount;
                        while (rd.Read())
                        {
                            User obj = new User();
                            obj.id = Convert.ToInt32(rd["id"]);
                            obj.first_name = Convert.ToString(rd["first_name"]) != "" ? Convert.ToString(rd["first_name"]) : "";
                            obj.last_name = Convert.ToString(rd["last_name"]) != "" ? Convert.ToString(rd["last_name"]) : "";
                            obj.age = Convert.ToString(rd["age"]) != "" ? Convert.ToInt32(rd["age"]) : 0;
                            obj.gender = Convert.ToString(rd["gender"]) != "" ? Convert.ToString(rd["gender"]) : "" ;
                            obj.phone_no = Convert.ToString(rd["phone_no"]) != "" ? Convert.ToString(rd["phone_no"]) : "";
                            obj.email = Convert.ToString(rd["email"]) != "" ? Convert.ToString(rd["email"]) : "";
                            obj.role = Convert.ToString(rd["role"]) != "" ? Convert.ToString(rd["role"]) : "";
                            obj.password = Convert.ToString(rd["password"]) != "" ? Convert.ToString(rd["password"]) : "";
                            users.Add(obj);
                        }
                    }
                    conn.Close();
                    
                    return users;
                }
                catch
                {
                    return users;
                }
            }
            else
            {
                return users;
            }
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User obj = new User();

            if (conn != null)
            {
                conn.Open();
                try
                {

                    SqlCommand cmd = new SqlCommand("getOneUser", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            obj.id = Convert.ToInt32(rd["id"]);
                            obj.first_name = Convert.ToString(rd["first_name"]) != "" ? Convert.ToString(rd["first_name"]) : "";
                            obj.last_name = Convert.ToString(rd["last_name"]) != "" ? Convert.ToString(rd["last_name"]) : "";
                            obj.age = Convert.ToString(rd["age"]) != "" ? Convert.ToInt32(rd["age"]) : 0;
                            obj.gender = Convert.ToString(rd["gender"]) != "" ? Convert.ToString(rd["gender"]) : "";
                            obj.phone_no = Convert.ToString(rd["phone_no"]) != "" ? Convert.ToString(rd["phone_no"]) : "";
                            obj.email = Convert.ToString(rd["email"]) != "" ? Convert.ToString(rd["email"]) : "";
                            obj.role = Convert.ToString(rd["role"]) != "" ? Convert.ToString(rd["role"]) : "";
                            obj.password = Convert.ToString(rd["password"]) != "" ? Convert.ToString(rd["password"]) : "";
                        }
                    }
                    conn.Close();

                    return obj;
                }
                catch
                {
                    return obj;
                }
            }
            else
            {
                return obj;
            }
        }

        //// POST api/<UserController>
        [HttpPost]
        public void Post(string role, string[] permissions, string password)
        {
            if(conn != null)
            {
                conn.Open();

                try
                {
                    Console.WriteLine(permissions[0], permissions[1]);

                    SqlCommand getRole = new SqlCommand("SELECT id FROM roles WHERE name = @name", conn);
                    getRole.Parameters.AddWithValue("@name", role);
                    int role_id = Convert.ToInt32(getRole.ExecuteScalar());

                    SqlCommand addCmd = new SqlCommand("addUser", conn);
                    addCmd.CommandType = CommandType.StoredProcedure;
                    addCmd.Parameters.AddWithValue("@role", role_id);
                    addCmd.Parameters.AddWithValue("@pass", password);

                    int emp_id = Convert.ToInt32(addCmd.ExecuteScalar());

                    int[] permission_id = new int[permissions.Length];

                    for (int i = 0; i < permissions.Length; i++)
                    {
                        SqlCommand permissionCmd = new SqlCommand("SELECT id FROM permissions WHERE name = @name", conn);
                        permissionCmd.Parameters.AddWithValue("@name", permissions[i]);
                        using (SqlDataReader rd = permissionCmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                permission_id[i] = Convert.ToInt32(rd["id"]);
                            }
                        }
                    }
                    //"list_personal_details","edit_personal_details"

                    foreach (var item in permission_id)
                    {
                        SqlCommand addPrmission = new SqlCommand("assignPermission", conn);
                        addPrmission.CommandType = CommandType.StoredProcedure;
                        addPrmission.Parameters.AddWithValue("@user_id", emp_id);
                        addPrmission.Parameters.AddWithValue("@permission_id", item);

                        int permissionResult = addPrmission.ExecuteNonQuery();

                    }
                    conn.Close();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    conn.Close();
                }

            }
        }

        //// PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, string first_name, string last_name, int age, string gender, string phone_no, string email, string role, string password)
        {
            if(conn != null)
            {
                conn.Open();

                if (gender == "Male")
                    gender = "m";
                else
                    gender="f";

                try
                {
                    SqlCommand getRoleId = new SqlCommand("SELECT id FROM roles WHERE name = @role", conn);
                    getRoleId.Parameters.AddWithValue("@role", role);
                    int role_id = Convert.ToInt32(getRoleId.ExecuteScalar());

                    SqlCommand addDetails = new SqlCommand("updateDetails", conn);
                    addDetails.CommandType = CommandType.StoredProcedure;
                    addDetails.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    addDetails.Parameters.Add("@name", SqlDbType.VarChar).Value = first_name;
                    addDetails.Parameters.Add("@surname", SqlDbType.VarChar).Value = last_name;
                    addDetails.Parameters.Add("@age", SqlDbType.Int).Value = age;
                    addDetails.Parameters.Add("@gender", SqlDbType.Char).Value = gender;
                    addDetails.Parameters.Add("@no", SqlDbType.VarChar).Value = phone_no;
                    addDetails.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                    addDetails.Parameters.Add("@role", SqlDbType.Int).Value = role_id;
                    addDetails.Parameters.Add("@pass", SqlDbType.VarChar).Value = password;

                    int result = addDetails.ExecuteNonQuery();

                    conn.Close();
                }
                catch
                {
                    conn.Close();
                }

                
            }
        }

        [HttpGet("/getRolePermissions")]
        public RolePermission GetRole()
        {
            RolePermission obj = new RolePermission();

            if (conn != null)
            {
                conn.Open();
                try
                {
                    SqlCommand countRoles = new SqlCommand("SELECT count(*) FROM roles", conn);
                    int roleCount = Convert.ToInt32(countRoles.ExecuteScalar());
                    string[] roleNames = new string[roleCount];
                    SqlCommand getRoles = new SqlCommand("SELECT name FROM roles", conn);
                    using (SqlDataReader rd = getRoles.ExecuteReader())
                    {
                        int count = 0;
                        while (rd.Read())
                        {
                            roleNames[count] = Convert.ToString(rd["name"]);
                            count++;
                        }
                    }

                    SqlCommand countPermissions = new SqlCommand("SELECT count(*) FROM permissions", conn);
                    int permissionCount = Convert.ToInt32(countPermissions.ExecuteScalar());
                    string[] permissionNames = new string[permissionCount];
                    SqlCommand getPermissions = new SqlCommand("SELECT name FROM permissions", conn);
                    using (SqlDataReader rd = getPermissions.ExecuteReader())
                    {
                        int count = 0;
                        while (rd.Read())
                        {
                            permissionNames[count] = Convert.ToString(rd["name"]);
                            count++;
                        }
                    }

                    SqlCommand getIdCount = new SqlCommand("SELECT count(*) FROM users", conn);
                    int idCount = Convert.ToInt32(getIdCount.ExecuteScalar());
                    int[] ids = new int[idCount];

                    SqlCommand getIds = new SqlCommand("SELECT id FROM users", conn);
                    using (SqlDataReader rd = getIds.ExecuteReader())
                    {
                        int count = 0;
                        while (rd.Read())
                        {
                            ids[count] = Convert.ToInt32(rd["id"]);
                            count++;
                        }
                    }

                    obj.roles = roleNames;
                    obj.permissions = permissionNames;
                    obj.ids = ids;

                    conn.Close();

                    return obj;
                }
                catch
                {
                    return obj;
                }
            }
            else
            {
                return obj;
            }
        }

        [HttpGet("/getRolePermissions/{id}")]
        public RolePermission GetRole(int id)
        {
            RolePermission obj = new RolePermission();

            if (conn != null)
            {
                conn.Open();
                try
                {
                    string[] role = new string[1];
                    SqlCommand getRole = new SqlCommand("SELECT name FROM roles WHERE id IN (SELECT role_id FROM users WHERE id = @id)", conn);
                    getRole.Parameters.AddWithValue("@id", id);
                    role[0] = Convert.ToString(getRole.ExecuteScalar());

                    SqlCommand countPermissions = new SqlCommand("SELECT count(*) FROM user_has_permissions WHERE user_id = @id", conn);
                    countPermissions.Parameters.AddWithValue("@id", id);
                    int permissionCount = Convert.ToInt32(countPermissions.ExecuteScalar());
                    string[] permissionNames = new string[permissionCount];
                    SqlCommand getPermissions = new SqlCommand("SELECT name FROM permissions WHERE id IN (SELECT permission_id FROM user_has_permissions WHERE user_id = @id)", conn);
                    getPermissions.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader rd = getPermissions.ExecuteReader())
                    {
                        int count = 0;
                        while (rd.Read())
                        {
                            permissionNames[count] = Convert.ToString(rd["name"]);
                            count++;
                        }
                    }

                    obj.roles = role;
                    obj.permissions = permissionNames;

                    conn.Close();

                    return obj;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    return obj;
                }
            }
            else
            {
                return obj;
            }

            return obj;
        }

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
