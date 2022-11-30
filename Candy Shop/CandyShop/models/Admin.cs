using System;
using dbOperations;
using System.Data.SqlClient;

namespace models{
    class Admin{

        private string userName="Admin";
        private string password ="Admin@123";

        public string GetUserName(){
            return this.userName;
        }
        public string GetPassword(){
            return this.password;
        }

        public void updateOrders(DbConnection dbConnection){

            Boolean done = true;

            do{

                Console.WriteLine("\n\nDo you want to update orders? (YES/NO):");

                string input = Console.ReadLine();
                input = input.ToUpper();

                if(input.Equals("YES")){

                    Console.WriteLine("\n\nEnter orderid :");
                    string orderId= Console.ReadLine();
                    orderId=orderId.ToUpper();

                    Console.WriteLine("\n\nEnter order status that you want to update to (DELIVERED/FAILED):");
                    string orderStatus= Console.ReadLine();
                    orderStatus = orderStatus.ToUpper();

                    dbConnection.ExecuteQuery("UPDATE orders SET orderStatus='"+orderStatus+"' WHERE orderId='"+orderId+"';");
                    
                    Console.WriteLine("\nUpdated order status of orderId :"+orderId+" to "+orderStatus);

                }
                else if(input.Equals("NO")){
                    done = false;
                }
                else{
                    Console.WriteLine("\n Invalid Input");
                }

            }while(done);
        }
    }
}