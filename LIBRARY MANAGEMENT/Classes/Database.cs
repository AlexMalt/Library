using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LIBRARY_MANAGEMENT.Classes
{
    class Database
    {
        private static SqlConnection instance = null;
        private static object _lock = new object();
        

        public Database()
        {
           
        }

        public static SqlConnection Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new SqlConnection(@"Data Source=(LocalDB)\library;Integrated Security=True");
                    }
                    return instance;
                }
            }
        }
    }
}
