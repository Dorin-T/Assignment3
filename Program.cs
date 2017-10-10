using System;
using System.Collections.Generic;

namespace Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Category> category = new List<Category>();
            category.Add( new Category {cid = 1, name = "Beverages"} );
            category.Add( new Category {cid = 2, name = "Condiments"} );
            category.Add( new Category {cid = 3, name = "Confections"} );
            Console.WriteLine("Created list: ");
            foreach (Category c in category) {
                Console.WriteLine(c.cid + " " + c.name);
            }
        }
    }
}
