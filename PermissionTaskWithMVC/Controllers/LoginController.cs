using Microsoft.AspNetCore.Mvc;
using PermissionTaskWithMVC.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;

namespace PermissionTaskWithMVC.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public IActionResult ValidatePass(LoginModel data)
        {
            if(Authentication.IsAuthenticated(data.id, data.password))
            {
                SqlConnection conn = null;
                conn = Connection.getConnection();

                if(conn != null)
                {
                    conn.Open();

                    try
                    {
                        SqlCommand getPermissions = new SqlCommand("getPermissions", conn);
                        getPermissions.CommandType = CommandType.StoredProcedure;
                        getPermissions.Parameters.AddWithValue("@id", data.id);


                        List<string> permissions = new List<string>();

                        using (SqlDataReader rd = getPermissions.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                permissions.Add(Convert.ToString(rd["name"]));
                            }
                        }
                        foreach (var item in permissions)
                        {
                            Console.WriteLine(item);
                        }
                        ViewBag.permissions = permissions;
                        ViewBag.id = data.id;
                        conn.Close();
                        return View("Services");
                    }
                    catch
                    {
                        conn.Close();
                        return View("~/Views/Shared/ConnectionError");
                    }



                }
                else
                {
                    return View("~/Views/Shared/ConnectionError");
                }
            }
            else
            {
                return View("IncorrectPass");
            }
        }
    }
}
