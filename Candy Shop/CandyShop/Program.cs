using System;
using System.Data.SqlClient;
using dbOperations;
using models;

class Program{
    public static void Main(string[] args){

        String s = @"Server=localhost\SQLEXPRESS;Database=CandyShop;Trusted_Connection=True";

        DbConnection dbConnection = new DbConnection(s);
        dbConnection.OpenConnection();
        CandyShop.Start(dbConnection);
        
    }
}
