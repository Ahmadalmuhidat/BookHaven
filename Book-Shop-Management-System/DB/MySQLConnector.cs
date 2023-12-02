using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Book_Shop_Management_System.DB
{
    class MySQLConnector
    {
        private MySqlConnection Connector { get; set; }
        public MySQLConnector()
        {
            var connstr = "Server=localhost;Uid=root;Pwd=root;database=book_system";
            this.Connector = new MySqlConnection(connstr);
        }

        public DataTable FetchData(String query)
        {
            try
            {
                using (var conn = this.Connector)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        DataTable dt = new DataTable();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                            return dt;
                        }
                    }
                } 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool InsertData(String query, String[] values)
        {
            try
            {
                String valuesParams = " Values ";
                valuesParams += "(";
                for (int i = 0; i < values.Length; i++)
                {
                    if (i == (values.Length - 1))
                    {
                        valuesParams += "@param" + (i + 1);
                    }
                    else
                    {
                        valuesParams += "@param" + (i + 1) + ",";
                    }
                }
                valuesParams += ");";
                query += valuesParams;

                using (var conn = this.Connector)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        for (int i = 0; i < values.Length; i++)
                        {
                            cmd.Parameters.AddWithValue("@param" + (i + 1), values[i]);
                        }

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool DeleteData(String query)
        {
            try
            {
                using (var conn = this.Connector)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public bool UpdateData(String query)
        {
            try
            {
                using (var conn = this.Connector)
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = query;
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void closeConnection()
        {
            Connector.Close();
        }
    }
}