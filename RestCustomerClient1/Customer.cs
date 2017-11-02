using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RestCustomerClient1
{
     public class Customer
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int ID { get; set; }

        public MyDate DateOfBirth { get; set; }


        public Customer()
        { //Start data generation

        }

        public Customer(String firstName)
        {
            this.ID = 0;
            this.FirstName = firstName; }

        public Customer(String firstName, String lastName, MyDate date)
        {
            this.ID = 0;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = date;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append(ID); sb.Append(", ");
            sb.Append(FirstName); sb.Append(", ");
            sb.Append(LastName); sb.Append(", ");
            sb.Append(DateOfBirth.ToString()); sb.Append(", ");
            return sb.ToString();

        }



    }
}
