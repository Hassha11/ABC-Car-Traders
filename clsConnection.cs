using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ABC_Car_Traders
{
    internal class clsConnection
    {
        //public clsConnection() 
        //{
        //    SqlConnection connection;
        //    void openConnection()
        //    {
        //        connection = new SqlConnection();
        //        connection.ConnectionString = "Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;";
        //        connection.Open();
        //    }

        //}

        public class Conn
        {
            public static SqlConnection getCon()
            {
                string connectionString = "Data Source=DESKTOP-KLCTTP5\\SQLEXPRESS01;Initial Catalog=ABC_Cars;Integrated Security=True;";
                return new SqlConnection(connectionString);
            }
        }

    }
}
