using System.Collections;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ManagingBankAccounts;

public class Account{

    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Balance { get; set; }

}

public class ManagingAccounts : ICollection
{
    readonly SqlConnection connection;

    public ManagingAccounts(SqlConnection sqlConnection){
        connection = sqlConnection;
    }

    public int Count => Accounts_Count();
    public int Accounts_Count(){
        var sqlSyntax = "SELECT Id FROM Account";
        SqlDataAdapter adapter = new SqlDataAdapter(sqlSyntax, connection);

        connection.Open();
           DataTable dt = new DataTable();
           adapter.Fill(dt);
        connection.Close();

        return dt.Rows.Count;
    }


    public bool Add(string name, Decimal balance){

        SqlParameter Name = new SqlParameter{
            ParameterName = "@Name",
            SqlDbType = System.Data.SqlDbType.VarChar,
            Direction = System.Data.ParameterDirection.Input,
            Value = name
        };

        SqlParameter Balance = new SqlParameter{
            ParameterName = "@Balance",
            SqlDbType = System.Data.SqlDbType.Decimal,
            Direction = System.Data.ParameterDirection.Input,
            Value = balance
        };

        SqlCommand command = new SqlCommand("AddAccount", connection);
        command.CommandType= System.Data.CommandType.StoredProcedure;

        command.Parameters.Add(Name);
        command.Parameters.Add(Balance);

        connection.Open();

            try{
                command.ExecuteScalar();
            }catch(Exception e){
                System.Console.WriteLine(e.Message);
                connection.Close();
                return false;
            }
            finally{
                connection.Close();
            }
        return true;     
    }


    public bool Update(int id, string name){

         var sqlSyntax = "UPDATE Account SET "+$"Name=@Name "+$"WHERE Id=@Id";

         SqlParameter Name = new SqlParameter{
            ParameterName="@Name",
            SqlDbType = System.Data.SqlDbType.VarChar,
            Direction = System.Data.ParameterDirection.Input,
            Value = name
         };             

         SqlParameter Id = new SqlParameter{
            ParameterName = "@Id",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Input,
            Value = id
         };

         SqlCommand sqlCommand = new SqlCommand(sqlSyntax, connection);

         sqlCommand.Parameters.Add(Name);
         sqlCommand.Parameters.Add(Id);

         sqlCommand.CommandType = System.Data.CommandType.Text;
         
         connection.Open();

            try{
                sqlCommand.ExecuteScalar();
            }catch(Exception e){
                System.Console.WriteLine(e.Message);
                connection.Close();
                return false;
            }
            finally{
                connection.Close();
            }
        
        return true;
    }


    public bool Update(int id, decimal balance){
        var sqlSyntax = "UPDATE Account SET "+$"Balance=@Balance "+$"WHERE Id=@Id";

         SqlParameter Balance = new SqlParameter{
            ParameterName="@Balance",
            SqlDbType = System.Data.SqlDbType.VarChar,
            Direction = System.Data.ParameterDirection.Input,
            Value = balance
         };             

         SqlParameter Id = new SqlParameter{
            ParameterName = "@Id",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Input,
            Value = id
         };

         SqlCommand sqlCommand = new SqlCommand(sqlSyntax, connection);

         sqlCommand.Parameters.Add(Balance);
         sqlCommand.Parameters.Add(Id);

         sqlCommand.CommandType = System.Data.CommandType.Text;
         
         connection.Open();

            try{
                sqlCommand.ExecuteScalar();
            }catch(Exception e){
                System.Console.WriteLine(e.Message);
                connection.Close();
                return false;
            }
            finally{
                connection.Close();
            }
        
        return true;
    }


    public bool Delete(int Id){

       SqlParameter ID = new SqlParameter{
          ParameterName="@Id",
          SqlDbType = System.Data.SqlDbType.Int,
          Direction = System.Data.ParameterDirection.Input,
          Value = Id
       };

       SqlCommand command = new SqlCommand("DeleteAccount", connection);
       command.CommandType = System.Data.CommandType.StoredProcedure;

       command.Parameters.Add(ID);

       connection.Open();
           
           try{
               
               command.ExecuteScalar();

           }catch(Exception e){
               System.Console.WriteLine(e.Message);
               connection.Close();
               return false;
           }
           finally{
              connection.Close();
           }

        return true;
    }


    public void Display(){
        
        var sqlSyntax = "SELECT * FROM Account";

        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlSyntax, connection);

        connection.Open();

           DataTable dataTable = new DataTable(); 
           sqlDataAdapter.Fill(dataTable);
           

        connection.Close();

        foreach(DataRow row in dataTable.Rows){
            System.Console.WriteLine("{ ID:"+Convert.ToInt16(row["Id"])+
                                        " Name:"+Convert.ToString(row["Name"])+ 
                                        " Balance:"+Convert.ToDecimal(row["Balance"])+
                                        " }");
        }
    }

    public void Print(){
        var sqlSyntax = "SELECT * FROM Account";
        SqlCommand sqlCommand = new SqlCommand(sqlSyntax, connection);


        connection.Open();

           SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(); 
           while(sqlDataReader.Read()){

               System.Console.WriteLine("{ ID:"+sqlDataReader.GetInt32("Id")+
                                        " Name:"+sqlDataReader.GetString("Name")+ 
                                        " Balance:"+sqlDataReader.GetDecimal("Balance")+
                                        " }");

           }

        connection.Close();

    }
    
    public bool Transfer(int From, int To, Decimal balance){

        SqlCommand sqlCommand = connection.CreateCommand();
        sqlCommand.CommandType = CommandType.Text;

        connection.Open();
            
            SqlTransaction sqlTransaction = connection.BeginTransaction();
            sqlCommand.Transaction = sqlTransaction;

            try{
                sqlCommand.CommandText = "UPDATE Account SET "+
                                         $"Balance=Balance-@Balance WHERE "+
                                         $"Id=@fromId";

                SqlParameter Balance = new SqlParameter{
                    ParameterName = "@Balance",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = balance
                };

                SqlParameter fromId = new SqlParameter{
                    ParameterName = "@fromId",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = From
                };

                sqlCommand.Parameters.Add(Balance);
                
                sqlCommand.Parameters.Add(fromId);
                sqlCommand.ExecuteScalar();

                sqlCommand.CommandText = "UPDATE Account SET "+
                                         $"Balance=Balance+@Balance WHERE "+
                                         $"Id=@toId";

                
                
                SqlParameter toId = new SqlParameter{
                    ParameterName = "@toId",
                    SqlDbType = SqlDbType.Decimal,
                    Direction = ParameterDirection.Input,
                    Value = To
                };
                
                sqlCommand.Parameters.Add(toId);
                sqlCommand.ExecuteScalar();

                sqlTransaction.Commit();

            }catch(Exception e){
                System.Console.WriteLine(e.Message);
                connection.Close();
                return false;
            }finally{
                connection.Close();
            }

        return true;

    }


    public IEnumerator GetEnumerator()
    {
        var sqlSyntax = "SELECT * FROM Account";

        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlSyntax, connection);

        connection.Open();

           DataTable dataTable = new DataTable(); 
           sqlDataAdapter.Fill(dataTable);
           

        connection.Close();

        return dataTable.Rows.GetEnumerator(); 
    }

    public bool IsSynchronized => throw new NotImplementedException();

    public object SyncRoot => throw new NotImplementedException();

    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }
}