/*
 * RestCustomerClient1
 *
 * Author Michael Claudius, ZIBAT Computer Science
 * Version 1.0. 2016.04.12, 1.1 2016.04.15, 1.2 2016.11.01
 * Copyright 2015 by Michael Claudius
 * Revised 2016.11.10
 * All rights reserved
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace RestCustomerClient1
{
    class Program
    {
        private const string CustomersUri =
           "http://localhost:49972/CustomerService.svc/customers/";
        //public static int nextID = 0;
        static void Main(string[] args)
        {
            //GET all customers
            IList<Customer> cList = GetCustomersAsync().Result;
            Console.WriteLine(string.Join("\n", cList.ToString()));

            //Fast write out
            for (int i = 0; i < cList.Count; i++ )
                Console.WriteLine(cList[i].ToString());
            Console.WriteLine();

            Console.WriteLine("GET Give an id of customer to be found");
            string idStr = Console.ReadLine();
            int id = int.Parse(idStr);
            Customer customer = GetOneCustomerAsync(id).Result;
            Console.WriteLine(customer.ToString());
            Console.WriteLine();

            try
            {
                Console.WriteLine("GET Give an id of customer to be found. See exception if not found");
                idStr = Console.ReadLine();
                id = int.Parse(idStr);
                customer = GetOneCustomerAsync1(id).Result;
                Console.WriteLine(customer.ToString());
                Console.WriteLine();
            }
            catch (Exception ex)
            { Console.WriteLine(ex.ToString());
               // Console.WriteLine(ex.Message);
            }

            //POST a customer
            MyDate date = new MyDate(1808, 8, 8);
            Customer newCustomer = new Customer("HC", "Andersen", date);         
            customer = AddCustomerAsync(newCustomer).Result;
            Console.WriteLine(customer.ToString());

            for (int i = 0; i < cList.Count; i++)
                Console.WriteLine(cList[i].ToString());
            Console.WriteLine();
            
            //PUT a customer by ID
            Console.WriteLine("PUT Give an id of customer to be updated");
            idStr = Console.ReadLine();
            id = int.Parse(idStr);
            customer = GetOneCustomerAsync(id).Result;
            Console.WriteLine(customer.ToString());
            customer.FirstName = "Niels";
            Customer customer1 = UpdateCustomerAsync(customer).Result;
            Console.WriteLine(customer1.ToString());

            for (int i = 0; i < cList.Count; i++)
                Console.WriteLine(cList[i].ToString());
            Console.WriteLine();


            //DELETE a customer by ID
            Console.WriteLine("DELETE Give an id of customer to be deleted");
            idStr = Console.ReadLine();
            id = int.Parse(idStr);
            customer = DeleteOneCustomerAsync(id).Result;
            Console.WriteLine(customer.ToString()+"\n");

            //Fast writeout
            for (int i = 0; i < cList.Count; i++)
                Console.WriteLine(cList[i].ToString());
            Console.WriteLine();
            // When this was written out it looked like the customer was not deleted
            //because the write is done within main() and happens before the asynchronous delete-method
            //was activated. But later runs showed clearly that the list was actually modified
            //The solution is to encapsulate the write-sentences in an asynchronous method
            //See the printTest() method below


            //Fast write out
            for (int i = 0; i < cList.Count; i++)
                Console.WriteLine(cList[i].ToString());
            Console.WriteLine();        

        }

        private static async Task<IList<Customer>> GetCustomersAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(CustomersUri);
                IList<Customer> cList = JsonConvert.DeserializeObject<IList<Customer>>(content);
                return cList;
            }
        }

        private static async Task<Customer> GetOneCustomerAsync(int id)
        {
            string requestUri = CustomersUri + "/"+ id;
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(requestUri);
                Customer c = JsonConvert.DeserializeObject<Customer>(content);
                return c;
            }
        }

        private static async Task<Customer> GetOneCustomerAsync1(int id)
        {
            string requestUri = CustomersUri + "/" + id;
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUri);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Customer not found. Try another id");

                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                Customer c = JsonConvert.DeserializeObject<Customer>(str);
                return c;
            }
        }
        private static async Task<Customer> DeleteOneCustomerAsync(int id)
        {
            string requestUri = CustomersUri + id;      

                using (HttpClient client = new HttpClient())
            {
                
                HttpResponseMessage response = await client.DeleteAsync(requestUri);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Customer not found. Try another id");
                    
                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                Customer deletedCustomer = JsonConvert.DeserializeObject<Customer>(str);
                return deletedCustomer;
               
            }
        }


        private static async Task<Customer> AddCustomerAsync(Customer newCustomer)
        {
            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri(CustomersUri);
                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var jsonString = JsonConvert.SerializeObject(newCustomer);
                Console.WriteLine("JSON: " + jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(CustomersUri, content);
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Customer already exists. Try another id");
                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                Customer copyOfNewCustomer = JsonConvert.DeserializeObject<Customer>(str);
                return copyOfNewCustomer;
            }
        }

        private static async Task<Customer> UpdateCustomerAsync(Customer newCustomer)
        {
            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new Uri(CustomersUri);
                //client.DefaultRequestHeaders.Clear();
               // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonString = JsonConvert.SerializeObject(newCustomer);
                Console.WriteLine("JSON: " + jsonString);
                StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(CustomersUri, content);
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new Exception("Customer already exists. Try another id");
                }
                response.EnsureSuccessStatusCode();
                string str = await response.Content.ReadAsStringAsync();
                Customer updCustomer = JsonConvert.DeserializeObject<Customer>(str);
                return updCustomer;
            }
        }

        //Altenative using the built in method: PostAsJsonAsync 
        //private static async Task<Customer> AddCustomerAsyncAsJson(Customer newCustomer)
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        HttpResponseMessage response = await client.PostAsJsonAsync(CustomersUri, newCustomer);
        //        if (response.StatusCode == HttpStatusCode.Conflict)
        //        {
        //            throw new Exception("Customer already exists. Try another id");
        //        }
        //        response.EnsureSuccessStatusCode();
        //        string str = await response.Content.ReadAsStringAsync();
        //        Customer copyOfNewCustomer = JsonConvert.DeserializeObject<Customer>(str);

        //        return copyOfNewCustomer;
        //    }
        // }

        private static async Task<string> printTest()
        {
            using (HttpClient client = new HttpClient())
            {
                string content = await client.GetStringAsync(CustomersUri);
                IList<Customer> cList = JsonConvert.DeserializeObject<IList<Customer>>(content);
                
                for (int i = 0; i < cList.Count; i++)
                    Console.WriteLine(cList[i].ToString());
                Console.WriteLine();
            }

        return null;


    }
        
    }




      
    }



//My first code was rather messy, experimenting
// Actually I started out like this and later it was refactored with method responsible for
//each operation
//  var client = new HttpClient();
//client.BaseAddress = new Uri(CustomersUri);
//client.DefaultRequestHeaders.Clear();
//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//Console.WriteLine("Post /customers");
//Customer c = new Customer("Peter", "Levinsky", new MyDate(1972, 12, 24));

//var jsonString = JsonConvert.SerializeObject(c);
//Console.WriteLine("JSON" + jsonString);

//StringContent content = new StringContent(jsonString, Encoding.UTF8, "application/json");

//var response7 = client.PostAsync(CustomersUri, content).Result;
//if (response7.IsSuccessStatusCode)
//{
//    Console.WriteLine("POST DONE");
//    Console.WriteLine(response7.Content.ToString());
//}


