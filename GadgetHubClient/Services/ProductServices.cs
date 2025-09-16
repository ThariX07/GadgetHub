using GadgetHub.Models;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GadgetHubClient.Services
{
    public class ProductServices
    {
        private readonly string baseUrl = "https://localhost:44376/"; // Update your API URL if different

        public List<Product> GetAllProducts()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = client.GetAsync("api/Products").Result;
                response.EnsureSuccessStatusCode();
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Product>>(json);
            }
        }

        public Product GetProductById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = client.GetAsync($"api/Products/{id}").Result;
                response.EnsureSuccessStatusCode();
                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Product>(json);
            }
        }

        public bool AddProduct(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/Products", content).Result;
                return response.IsSuccessStatusCode;
            }
        }

        public bool RegisterCustomer(Customer customer, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var json = JsonConvert.SerializeObject(customer);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    // Match the API you implemented above:
                    var response = client.PostAsync("api/Customers/Register", content).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        errorMessage = response.Content.ReadAsStringAsync().Result;
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }


        public dynamic LoginDistributor(string email, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var loginData = new { Email = email, Password = password };
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/Distributors/Login", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Login failed: {response.StatusCode} - {errorContent}");
                }
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<dynamic>(result);
            }
        }

        public dynamic LoginCustomer(string email, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var loginData = new { Email = email, Password = password };
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("api/Customers/Login", content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new HttpRequestException($"Login failed: {response.StatusCode} - {errorContent}");
                }
                var result = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<dynamic>(result);
            }
        }

        // ✅ NEW: Get customer by ID
        public Customer GetCustomerById(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = client.GetAsync($"api/Customers/{id}").Result;
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error fetching customer: " + response.StatusCode);
                }

                var json = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Customer>(json);
            }
        }

        // ✅ NEW: Update customer profile
        public bool UpdateCustomer(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var json = JsonConvert.SerializeObject(customer);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync($"api/Customers/{customer.Id}", content).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
