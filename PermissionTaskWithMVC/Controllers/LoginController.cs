using Microsoft.AspNetCore.Mvc;
using PermissionTaskWithMVC.Models;
using System.Collections.Specialized;
using System.Data.SqlClient;
using PermissionTaskWithMVC.Controllers;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace PermissionTaskWithMVC.Controllers
{
    public class LoginController : Controller
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public static List<string> permissions = new List<string>();
        public static int id;

        [HttpPost]
        public IActionResult ValidatePass(LoginModel data)
        {
            if (Authentication.IsAuthenticated(data.id, data.password))
            {
                permissions = [];

                SqlConnection conn = null;
                conn = Connection.getConnection();

                if (conn != null)
                {
                    conn.Open();

                    try
                    {
                        SqlCommand getUserPermissions = new SqlCommand("SELECT name FROM permissions WHERE id IN (SELECT permission_id FROM user_has_permissions WHERE user_id = @id);", conn);
                        getUserPermissions.Parameters.AddWithValue("@id", data.id);

                        SqlCommand getRolePermissions = new SqlCommand("SELECT name FROM permissions WHERE id IN (SELECT permission_id FROM role_has_permissions WHERE role_id = (SELECT role_id FROM users WHERE id = @id));", conn);
                        getRolePermissions.Parameters.AddWithValue("@id", data.id);

                        using (SqlDataReader rd = getUserPermissions.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                if (!permissions.Contains(Convert.ToString(rd["name"])))
                                {
                                    LoginController.permissions.Add(Convert.ToString(rd["name"]));
                                }
                            }
                        }

                        using (SqlDataReader rd = getRolePermissions.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                if (!permissions.Contains(Convert.ToString(rd["name"])))
                                {
                                    LoginController.permissions.Add(Convert.ToString(rd["name"]));
                                }
                            }
                        }

                        foreach (var item in permissions)
                        {
                            Console.WriteLine(item);
                        }
                        LoginController.id = data.id;
                        conn.Close();

                        return RedirectToAction("showDashboard");
                    }
                    catch
                    {
                        conn.Close();
                        return RedirectToAction("error");
                    }
                }
                else
                {
                    return RedirectToAction("error");
                }
            }
            else
            {
                return RedirectToAction("incoreectPass");
            }
        }
        
        [HttpGet]
        public IActionResult showDashboard()
        {
            return View("Services");
        }

        [HttpGet]
        public IActionResult incoreectPass()
        {
            return View("IncorrectPass");
        }

        [HttpGet]
        public IActionResult error()
        {
            return View();
        }
    }
}
