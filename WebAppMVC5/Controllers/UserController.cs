using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using WebAppMVC5.Models;
using Newtonsoft.Json;
using System.Text;

namespace WebAppMVC5.Controllers
{
    public class UserController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44324/api");
        HttpClient Client;
        public UserController()
        {
            Client = new HttpClient();
            Client.BaseAddress = baseAddress;
        }

        // GET: User
        public ActionResult Index()
        {
            List<UserModelView> modellist = new List<UserModelView>();
            HttpResponseMessage response = Client.GetAsync(Client.BaseAddress + "/User").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modellist = JsonConvert.DeserializeObject<List<UserModelView>>(data);
                return View(modellist);
            }
            return View();
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(UserModelView model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage message = Client.PostAsync(Client.BaseAddress+"/user", content).Result;
            if(message.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
       
        public ActionResult Edit(int id)
        {
            UserModelView modellist = new UserModelView();
            HttpResponseMessage response = Client.GetAsync(Client.BaseAddress + "/user/" +id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modellist = JsonConvert.DeserializeObject<UserModelView>(data);
                
            }
            return View("Create", modellist);
        }

        [HttpPost]
        public ActionResult Edit(UserModelView model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage message = Client.PutAsync(Client.BaseAddress + "/user/"+model.Id, content).Result;
            if (message.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

       // [HttpDelete]
        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = Client.DeleteAsync(Client.BaseAddress + "/user/"+id).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}