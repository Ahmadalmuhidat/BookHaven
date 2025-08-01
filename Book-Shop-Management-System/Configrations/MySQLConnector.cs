using MySql.Data.MySqlClient;

using System;

using System.Data;


namespace Book_Shop_Management_System.Configrations

{

    public class MySQLConnector

    {

        private readonly string _connectionString = "Server=localhost;Uid=root;Pwd=StrongPassword123!;Database=BookHevean";


        // Fetch without parameters

        public DataTable FetchData(string query)

        {

            return FetchData(query, null);

        }


        // Fetch with parameters

        public DataTable FetchData(string query, params MySqlParameter[] parameters)

        {

            var dt = new DataTable();


            try

            {

                using (var conn = new MySqlConnection(_connectionString))

                {

                    conn.Open();

                    using (var cmd = new MySqlCommand(query, conn))

                    {

                        if (parameters != null)

                        {

                            cmd.Parameters.AddRange(parameters);

                        }


                        using (var reader = cmd.ExecuteReader())

                        {

                            dt.Load(reader);

                        }

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"[FetchData] Error: {ex.Message}");

                throw;

            }


            return dt;

        }


        public bool InsertData(string query, params MySqlParameter[] parameters)

        {

            return ExecuteNonQuery(query, parameters);

        }


        public bool UpdateData(string query, params MySqlParameter[] parameters)

        {

            return ExecuteNonQuery(query, parameters);

        }


        public bool DeleteData(string query, params MySqlParameter[] parameters)

        {

            return ExecuteNonQuery(query, parameters);

        }


        private bool ExecuteNonQuery(string query, params MySqlParameter[] parameters)

        {

            try

            {

                using (var conn = new MySqlConnection(_connectionString))

                {

                    conn.Open();

                    using (var cmd = new MySqlCommand(query, conn))

                    {

                        if (parameters != null)

                        {

                            cmd.Parameters.AddRange(parameters);

                        }


                        int rowsAffected = cmd.ExecuteNonQuery();

                        return rowsAffected > 0;

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"[ExecuteNonQuery] Error: {ex.Message}");

                throw;

            }

        }

    }
}