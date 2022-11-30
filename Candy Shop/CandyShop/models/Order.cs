using System;
using dbOperations;
using System.Data.SqlClient;


namespace models{
    class Order{
        private string orderId;
        private string userId;
        private string candieId;
        private int quantity;
        private Status status;

        public Order(string userId,string candieId,int quantity){
            this.orderId = "O";
            this.candieId = candieId;
            this.userId = userId;
            this.quantity = quantity;
            this.status = Status.INPROGRESS;
        }

        //Getters and Setters
        public void SetOrderId(string orderId){
            this.orderId = orderId;
        }
        public string GetOrderId(){
            return this.orderId;
        }
        public void SetUserId(string userId){
            this.userId = userId;
        }
        public string GetuserId(){
            return this.userId;
        }
        public void SetCandieId(string candieId){
            this.candieId = candieId;
        }
        public string GetCandieId(){
            return this.candieId;
        }
        public void SetQuantity(int quantity){
            this.quantity = quantity;
        }
        public int GetQuantity(){
            return this.quantity;
        }
        public void SetStatus(string status){
            this.status = (Status)Enum.Parse(typeof(Status), status,true);
        }
        public Status GetStatus(){
            return this.status;
        }

        public void AddOrderToDb(DbConnection dbConnection){

            Object reader = dbConnection.ExecuteQueryForScaler("SELECT COUNT(orderId) from orders;");

            int countOfNumberOfRowsinOrdersTable = (int)reader;
            countOfNumberOfRowsinOrdersTable++;

            this.SetOrderId(this.GetOrderId()+countOfNumberOfRowsinOrdersTable.ToString());

            dbConnection.ExecuteQuery("INSERT into orders VALUES('"+this.GetOrderId()+"','"+this.GetuserId()+"','"+this.GetCandieId()+"','"+this.GetQuantity()+"','"+this.GetStatus().ToString()+"');");

        }

        public void DisplayBill(DbConnection dbConnection){
            
            string userName = dbConnection.ExecuteQueryForScaler("SELECT userName from users where userId='"+this.GetuserId()+"';").ToString();
            decimal bill = Convert.ToDecimal(dbConnection.ExecuteQueryForScaler("SELECT price from candies where candieId='"+this.GetCandieId()+"';"))*this.GetQuantity();

            Console.WriteLine("\n\nHeyy "+userName.Trim()+" you need to pay "+bill+"\n\n");
            
        }

        public static void DisplayAllOrders(DbConnection dbConnection){

            Console.WriteLine("\n------------------ORDERS------------------\n");
            Console.WriteLine("\n\n OrderId\tUserId      \tUser Name \t  CandieId\t   Candy Name\t    \t        Quantity\t    Order Status");

            SqlDataReader reader = dbConnection.ExecuteQueryReader("SELECT orders.orderId,orders.userId,users.userName,candies.candieId,candies.candieName,orders.quantity,orders.orderStatus from candies,orders,users WHERE orders.candieId=candies.candieId and orders.userId=users.userId;");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("\n\n "+reader["orderId"].ToString()+"\t\t"+reader["userId"].ToString()+"\t\t"+reader["userName"].ToString()+"\t"+reader["candieId"].ToString()+"\t\t"+reader["candieName"].ToString()+"\t\t"+reader["quantity"].ToString()+"\t\t"+reader["orderStatus"].ToString());
                }
                reader.Close();    
            }
            else{
                Console.WriteLine("NONE");
            }



        }


    }
}