using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABC_Car_Traders
{
    public class clsSQLQuerys
    {
        //public class SQL
        //{
        //    public static DataSet getDataSet(string query, SqlConnection connection, string tblCustomers)
        //    {
        //        DataSet ds = new DataSet();

        //        try
        //        {
        //            using (SqlCommand cmd = new SqlCommand(query, connection))
        //            {
        //                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
        //                {
        //                    adapter.Fill(ds, tblCustomers);
        //                }
        //            }
        //            return ds; 
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error: " + ex.Message);
        //            return null; 
        //        }
        //    }

        //    int CusID { get; set; }

        //    public static string strCustomers()
        //    {
        //        return "SELECT * FROM Customers WHERE CusID = CusID ";
        //    }
        //}

        public string getSingleData(string strSQL, SqlConnection Conn)
        {
            string s = "";
            SqlCommand cmdgetSingleData = new SqlCommand();
            cmdgetSingleData.CommandText = strSQL;
            cmdgetSingleData.Connection = Conn;

            Conn.Open();

            object result = cmdgetSingleData.ExecuteScalar();

            Conn.Close();

            if (result == null)
            { s = ""; }
            else
            { s = cmdgetSingleData.ExecuteScalar().ToString(); }
            return s;
        }
    }
}
