using System;
using models;
using System.Data.SqlClient;
using dbOperations;
using validation;

class CandyShop{
    public static void DisplayWelcomeMessage(){

        string title = @"
 _____                    _         _____  _                   
/  __ \                  | |       /  ___|| |                  
| /  \/  __ _  _ __    __| | _   _ \ `--. | |__    ___   _ __  
| |     / _` || '_ \  / _` || | | | `--. \| '_ \  / _ \ | '_ \ 
| \__/\| (_| || | | || (_| || |_| |/\__/ /| | | || (_) || |_) |
 \____/ \__,_||_| |_| \__,_| \__, |\____/ |_| |_| \___/ | .__/ 
                              __/ |                     | |    
                             |___/                      |_|    
 ";

        Console.WriteLine(title);
        Console.WriteLine("1.User \n2.Admin\n3.Exit\n\n");
        Console.WriteLine("Enter your option : ");

    }

    

    public static void Start(DbConnection dbConnection){

        Boolean done = true;
        while(done){
            DisplayWelcomeMessage();
            int choice;
            choice=Convert.ToInt32(Console.ReadLine());
            switch(choice){
                case 1:
                    UserFlow(dbConnection);
                    break;

                case 2:
                    AdminFlow(dbConnection);
                    break;

                case 3:
                    done = false;
                    break;
                default:
                    break;
            }
        }

    }

    public static void DisplayRegisterOrLoginOptionMessage(){

        Console.WriteLine("\n\n1.Register for new users \n2.Login\n3.Go Back\n");
        Console.WriteLine("Enter your option : ");
    }

    public static void DisplayLoginMessage(User user){
        Console.WriteLine("\n\nHello "+user.GetUserName()+", Welcome to our CandyShop.");

    }


    public static void DisplayVieWOrdersOrOrderMessage(){
        Console.WriteLine("\n\n1.View my Orders \n2.Place Order\n3.Exit\n");
        Console.WriteLine("Enter your option:\n\n");
    }

    public static void UserFlow(DbConnection dbConnection){
        
        registerOrLoginFlow:
            DisplayRegisterOrLoginOptionMessage();
            
            int choice=Convert.ToInt32(Console.ReadLine());
            if(choice==3){
                return;
            }

            Console.WriteLine("\nEnter your name:\n");
            string name = Console.ReadLine();

            Console.WriteLine("\nEnter your phone number:\n");
            string phoneNumber = Console.ReadLine();
        if(!Validation.ValidatePhoneNumber(phoneNumber)){
            Console.WriteLine("\nEnter your phone number correctly\n");
            goto registerOrLoginFlow;
            
        }
        

        if(choice == 1){

            User user = new User(name,phoneNumber);       
            user.RegisterUser(dbConnection);
            Console.WriteLine($"\n\nHi {user.GetUserName()}, You have Registered Successfully :) Login To order \n\n");
                    
        }
        else if(choice == 2){

            User user = User.Login(dbConnection,name,phoneNumber);

            if(user!=null){

                VieWOrdersOrOrderFlow:
                    DisplayVieWOrdersOrOrderMessage();
                    choice = Convert.ToInt32(Console.ReadLine());

                if (choice == 1){
                    user.GetMyOrders(dbConnection);
                }
                else if (choice ==2){
                    DisplayLoginMessage(user);
                    DisplayMenu(dbConnection);
                    TakeOrder(dbConnection,user);
                    
                }
                else if(choice == 3){
                    return;
                }
                goto VieWOrdersOrOrderFlow;
            }
            else
            {
                Console.WriteLine("Indvalid User details");
                goto registerOrLoginFlow;
            }
        }
    }

    public static void TakeOrder(DbConnection dbConnection,User user){

        Boolean done = true;

        do{

            Console.WriteLine("\nEnter your choice candieId : \n");
            string candieId = Console.ReadLine();
            candieId = candieId.ToUpper();

            Console.WriteLine("\nEnter the number of candies you need : \n");
            int quantity = Convert.ToInt32(Console.ReadLine());

            Order order = new Order(user.GetuserId(),candieId,quantity);
            order.AddOrderToDb(dbConnection);
            order.DisplayBill(dbConnection);

            done = false;

        }while(done);

    }

    public static void DisplayMenu(DbConnection dbConnection){

        Console.WriteLine("\n------------------Menu------------------\n");
        Console.WriteLine("\n\n candieId\t\tcandieName\t \tprice");

        SqlDataReader reader = dbConnection.ExecuteQueryReader("SELECT * from candies;");

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Console.WriteLine("\n\n "+reader["candieId"].ToString()+"\t \t"+reader["candieName"].ToString()+"\t \t "+reader["price"].ToString());
            }
            reader.Close();    
        }

    }

    public static void DisplayAdminWelcomeMessage(DbConnection dbConnection){
        Console.WriteLine("\n\nWelcome back to work!! :D");
        Console.WriteLine("You have got "+dbConnection.ExecuteQueryForScaler("SELECT COUNT(orderId) FROM ORDERS where orderStatus='INPROGRESS';").ToString()+" orders to Deliver\n\n");
    }
    public static void AdminFlow(DbConnection dbConnection){

        Admin admin = new Admin();

        Console.WriteLine("\n\nEnter username : ");
        string userName= Console.ReadLine();
        Console.WriteLine("\n\nEnter password :");
        string password = Console.ReadLine();

        if(userName.Equals(admin.GetUserName()) && password.Equals(admin.GetPassword())){

            DisplayAdminWelcomeMessage(dbConnection);
            Order.DisplayAllOrders(dbConnection);
            admin.updateOrders(dbConnection);

        }
        else{
            Console.WriteLine("Invalid Admin Details");
        }

        

    }



    
}