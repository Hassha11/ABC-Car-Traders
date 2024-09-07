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
        public static class SYS
        {
        private static int id;
        private static DateTime date;

        public static int SYS_ID
        {
            get { return id; }
            set { id = value; }
        }

        public static DateTime SYS_DATE
        {
            get { return date; }
            set { date = value; }
        }
    }
}
