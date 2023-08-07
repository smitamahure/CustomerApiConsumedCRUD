using ConsumeApiTask1.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;

namespace ConsumeApiTask1.Controllers
{
    public class CustomersController : Controller
    {
        private string apiUrl = "https://getinvoices.azurewebsites.net/api/";
        public IActionResult Index()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(apiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = httpClient.GetAsync("Customers").Result;
                    httpClient.Dispose();
                    if (response.IsSuccessStatusCode)
                    {
                        string stringData = response.Content.ReadAsStringAsync().Result;
                        customers = JsonConvert.DeserializeObject<List<Customer>>(stringData);
                    }
                    else
                    {
                        TempData["error"] = $"{response.ReasonPhrase}";
                    }

                    
                }

            }
            catch (Exception ex)
            {
                TempData["expection"] = ex.Message;
            }

            return View(customers);
            
        }

        public IActionResult AddCustomer()
        {
            Customer customer = new Customer();
            return View(customer);
        }


        [HttpPost]
        public IActionResult AddCustomer(Customer model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(apiUrl);
                        var data = JsonConvert.SerializeObject(model);
                        var contentData = new StringContent(data, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = httpClient.PostAsync("Customer", contentData).Result;
                         if (response.IsSuccessStatusCode)
                         {
                             TempData["success"] = "Customer added successfully!";
                         }
                         else
                         {
                             TempData["error"] = "Error adding customer: " + response.ReasonPhrase;
                         }
                        
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ModelState is not Valid!");
                    return View(model);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateCustomer(int id)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(apiUrl);

                HttpResponseMessage response = httpClient.GetAsync($"Customer/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var customerData = response.Content.ReadAsStringAsync().Result;
                    var existingCustomer = JsonConvert.DeserializeObject<Customer>(customerData);

                    return View(existingCustomer); // Pass the existing customer data to the update view
                }
                else
                {
                    TempData["error"] = "Error retrieving customer data.";
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(apiUrl);

                        var data = JsonConvert.SerializeObject(model);
                        var contentData = new StringContent(data, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = httpClient.PostAsync($"Customer/{model.Id}", contentData).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["success"] = "Customer updated successfully!";
                        }
                        else
                        {
                            TempData["error"] = "Error updating customer.";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "ModelState is not valid!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred: " + ex.Message;
                // Log the exception for further investigation
            }

            return RedirectToAction("Index");
        }

        public IActionResult DeleteCustomer(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                HttpResponseMessage response = client.DeleteAsync($"Customer/{id}").Result;
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "Customer deleted successfully!";
                }
                else
                {
                    TempData["error"] = "Record is not deleted succesfully.";
                }
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult CreateCustomerList()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(apiUrl);
                    var response =  client.GetAsync("CreateCustomerList").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["success"] = "Customer list created successfully!";
                    }
                    else
                    {
                        TempData["error"] = $"Error creating customer list: {response.ReasonPhrase}";
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"An error occurred: {ex.Message}";
                }
            }
           

            return RedirectToAction("Index", "Customers");
        }
    }
}
