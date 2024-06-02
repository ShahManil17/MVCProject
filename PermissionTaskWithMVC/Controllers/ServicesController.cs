using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PermissionTaskWithMVC.Models;

namespace PermissionTaskWithMVC.Controllers
{
    public class ServicesController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7110/api");
        private readonly HttpClient _client;
        
        public ServicesController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpPost]
        public IActionResult list_all_users()
        {
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/User").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var userList = JsonConvert.DeserializeObject<List<UserViewModel>>(data);
                ViewBag.userList = userList;
            }

            return View();
        }

        [HttpPost]
        public IActionResult edit_user_details(int id)
        {
            ViewBag.id = id;
            Console.WriteLine(id);
            return View();
        }

        [HttpPost]
        public IActionResult add_new_user()
        {
            return View();
        }

        [HttpPost]
        public IActionResult list_personal_details(int id)
        {
            if(id == 0)
            {
                ViewBag.userList = null;
            }
            else
            {
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/User/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    var userList = JsonConvert.DeserializeObject<UserViewModel>(data);
                    //var userList = JsonConvert.DeserializeObject<List<UserViewModel>>(data);

                    ViewBag.userList = userList;
                }
            }
            return View();
        }

        //[HttpPost]
        //public IActionResult display_personal_details(int id)
        //{
            
            
        //    return View("list_personal_details");
        //}

        [HttpPost]
        public IActionResult assign_permissions()
        {
            return View();
        }

        [HttpPost]
        public IActionResult edit_personal_details(int id)
        {
            ViewBag.id = id;
            return View();
        }

        [HttpPost]
        public IActionResult remove_permission()
        {
            return View();
        }
    }
}
