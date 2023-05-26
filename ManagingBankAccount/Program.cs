using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
using System.Data;

namespace ManagingBankAccounts;

public class Program{

    public static void Main(string[] args){

        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        SqlConnection sqlConnection = new SqlConnection(configuration.GetSection("ConnectionStrings").Value);

        
        ManagingAccounts managingAccounts = new ManagingAccounts(sqlConnection);


        string? choice = string.Empty;

        while(choice != "Exit"){

            System.Console.WriteLine("-----------------------------------\n"+
                                     "1-Display\n"+
                                     "2-Print\n"+
                                     "3-Add\n"+
                                     "4-Show\n"+
                                     "5-Update name\n"+
                                     "6-Update balance\n"+
                                     "7-Count\n"+
                                     "8-Delete\n"+
                                     "9-Exit\n"+
                                     "-----------------------------------");
            System.Console.WriteLine();

            System.Console.Write("Enter your choice: ");
            choice = Console.ReadLine(); 

            if(choice == "Exit"){
                System.Console.WriteLine("Bye");
                break;
            }
            
            switch(choice){
                case "Display":
                   managingAccounts.Display();
                   System.Console.WriteLine();
                break;
                //--------------------------------------------------

                case "Print":
                       managingAccounts.Print();
                       System.Console.WriteLine();
                break;
                //--------------------------------------------------

                case "Show":
                   foreach(DataRow row in managingAccounts){
                       System.Console.WriteLine("{ ID:"+Convert.ToInt16(row["Id"])+
                                        " Name:"+Convert.ToString(row["Name"])+ 
                                        " Balance:"+Convert.ToDecimal(row["Balance"])+
                                        " }");
                   }
                break;
                //---------------------------------------------------
                
                case "Add":
                            System.Console.Write("Enter your name: ");
                            string? AddName = Console.ReadLine();
                            System.Console.WriteLine();
                            System.Console.Write("Enter your balance: ");
                            Decimal AddBalance = Convert.ToDecimal(Console.ReadLine());
                            
                            if(AddName!=null)
                            managingAccounts.Add(AddName, AddBalance);
                            else
                            System.Console.WriteLine("Null name!!!"); 
                break;
                //-------------------------------------------------------------------

                case "Update name":
                     System.Console.Write("Enter your Id: ");
                     int UpdateId = Convert.ToInt32(Console.ReadLine());
                     System.Console.WriteLine();
                     System.Console.Write("Enter your new name: ");
                     string? UpdateName = Console.ReadLine();
                     if(UpdateName!=null)
                     managingAccounts.Update(UpdateId ,UpdateName);
                     System.Console.WriteLine();
                break;
                //-------------------------------------------------------------------

                case "Update balance":
                    System.Console.Write("Enter your Id: ");
                    int UpdateBId = Convert.ToInt32(Console.ReadLine());
                    System.Console.WriteLine();
                    System.Console.Write("Enter your new balance: ");
                    decimal UpdateBalance = Convert.ToDecimal(Console.ReadLine());
                    managingAccounts.Update(UpdateBId ,UpdateBalance);
                    System.Console.WriteLine();
                break;
                //--------------------------------------------------------------------
                
                case "Count":
                    System.Console.WriteLine(managingAccounts.Count); 
                break;
                //---------------------------------------------------------------------
                
                case "Delete":
                     System.Console.Write("Enter you Id: ");
                     int DId = Convert.ToInt32(Console.ReadLine());
                     managingAccounts.Delete(DId); 
                break;
                //---------------------------------------------------------------------

                default:
                    System.Console.WriteLine("Enter valid command");
                break; 
            }

        }

    }






}