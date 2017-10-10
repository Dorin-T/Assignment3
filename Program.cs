using System;
using System.Diagnostics.PerformanceData;

namespace Assignment3
{
    public class Category
    {
        private int cid { get; set; }
        private string name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Category> categories = new List<Category>();
            categories.Add(new Category() {cid = 1, name = "Beverages"});
            categories.Add(new Category() {cid = 2, name = "Condiments"});
            categories.Add(new Category() {cid = 3, name = "Confections"});
            
            Console.WriteLine("Hello World! " + categorries[1]);
        }
    }

   
}
