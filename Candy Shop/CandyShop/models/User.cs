using System;
using dbOperations;
using System.Data.SqlClient;

namespace models{
    class User{
        private string userId;
        private string userName;
        private string phoneNumber;

        public User(string userName,string phoneNumber){
            this.userId="U";
            this.userName= userName;
            this.phoneNumber = phoneNumber;
        }

        //Getters and Setters for the fields
        public void SetUserId(string userId){
            this.userId = userId;
        }
        public string GetuserId(){
            return this.userId;
        }

        public void SetUserName(string userName){
            this.userName = userName;
        }
        public string GetUserName(){
            return this.userName;
        }

        public void SetPhoneNumber(string phoneNumber){
            this.phoneNumber = phoneNumber;
        }
        public string GetPhoneNumber(){
            return this.phoneNumber;
        }

        public void RegisterUser(DbConnection dbConnection){

            // Getting count of number of rows in user table and updating it to plus one 
            //to set the userId to "U1","U2"... soon.
            Object reader = dbConnection.ExecuteQueryForScaler("SELECT COUNT(userName) from users;");
            int countOfNumberOfRowsinUserTable = (int)reader;
            countOfNumberOfRowsinUserTable++;
            this.SetUserId(this.GetuserId()+countOfNumberOfRowsinUserTable.ToString());
            
            //Inserting data into users table
            dbConnection.ExecuteQuery("INSERT INTO USERS VALUES('"+this.GetuserId()+"','"+this.GetUserName()+"','"+this.GetPhoneNumber()+"');");

        }

        public static User Login(DbConnection dbConnection, string userName, string phoneNumber){

            User user = new User(userName,phoneNumber);

            string userId = dbConnection.ExecuteQueryForScaler("select userid from users where username='"+user.GetUserName()+"' and phonenumber='"+user.GetPhoneNumber()+"';").ToString();
            if(userId!=null){
                user.SetUserId(userId);
                return user;
            }

            return null;
            
        }

        public void GetMyOrders(DbConnection dbConnection){
            SqlDataReader reader = dbConnection.ExecuteQueryReader("SELECT orders.orderId,candies.candieName,orders.quantity,orders.orderStatus from candies,orders WHERE orders.userId='"+this.GetuserId()+"' and orders.candieId=candies.candieId;");


            if (reader.HasRows)
            {
                Console.WriteLine("\n\nDisplaying your orders ");
                Console.WriteLine("\n\n OrderId\t \tCandie Name\t \tQuantity\t \tOrder Status");
                while (reader.Read())
                {
                    Console.WriteLine("\n\n "+reader["orderId"].ToString()+"\t \t\t"+reader["candieName"].ToString()+"\t \t"+reader["quantity"].ToString()+"\t \t"+reader["orderStatus"].ToString());
                }
            }
            else{
                Console.WriteLine("\n\nNo orders placed yet\n");
            }
            reader.Close();
        }





    }
}