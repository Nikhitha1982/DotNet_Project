using System;
using System.Data.SqlClient;

namespace dbOperations{
    class DbConnection{
        private string connectionString;
        private SqlConnection connection;
        private SqlCommand command;



        public DbConnection(String connectionString){
            this.connectionString = connectionString;
            connection = new SqlConnection(this.connectionString);
        }

        public void OpenConnection(){

            try{
                this.connection.Open();
            }
            catch(SqlException e){
                Console.WriteLine(e.ToString());
            }

        }

        public void ExecuteQuery(string query){

            this.command = new SqlCommand(query, this.connection);

            command.ExecuteNonQuery();

        }

        public Object ExecuteQueryForScaler(string query){
            this.command = new SqlCommand(query,this.connection);
            Object output = command.ExecuteScalar();
            return output;
        }


        public SqlDataReader ExecuteQueryReader(string query){
            this.command = new SqlCommand(query, this.connection);

            SqlDataReader output = command.ExecuteReader();
            return output;
        }



        public void CloseConnection(){

            try{
                this.connection.Close();
            }
            catch(SqlException e){
                Console.WriteLine(e.ToString());
            }

        }


    }
}