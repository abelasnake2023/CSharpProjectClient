namespace CSharpProjectClient.Database.ALibSqlServer;

using System.Data.SqlClient;
using System.Diagnostics;





public class ALibDataReader : ALibADO
{
    //the following method Execute stored procedure
    public void ExecuteStoredProcedure(string procName, object[,] objParam = null)
    {
        //the sql command properties  
        SqlCommand cmd = SqlClientProp("StoredProcedure", procName);

        //Create the parameters
        if (objParam != null)
        {
            int innerLength = objParam.GetLength(0);
            SqlParameter[] sqlParam = new SqlParameter[objParam.Length / innerLength];
            for (byte i = 0; i < sqlParam.Length; i++)
            {
                sqlParam[i] = new SqlParameter();
            }
            bool allCreated = CreateSqlParameter(sqlParam, objParam);

            //Add the parameters to the SqlCommand object(cmd)
            if (allCreated)
            {
                AddSqlParameter(sqlParam, cmd);
            }
        }

        //Open Connection
        OpenForClosedConnection();

        //Execute the stored procedure
        int rowsAffected = cmd.ExecuteNonQuery();

        //Close the connection
        CloseForOpenedConnection();

        Debug.WriteLine("Rows Affected: " + rowsAffected); // Rows affected by `ExecuteNonQuery`
    }
    //the following method Execute Scalar function
    public object ExecuteScalarFunction(string funcName, object[,] objParam = null)
    {
        //Build the Argument name
        string argument = "";
        if (objParam != null)
        {
            argument = BuildArgumentName(objParam);
        }

        //the sql command properties  
        string cmdText = $"SELECT {funcName}({argument})";
        SqlCommand cmd = SqlClientProp("Text", cmdText);

        //Create the parameters
        if (objParam != null)
        {
            int innerLength = 3;
            SqlParameter[] sqlParam = new SqlParameter[objParam.Length / innerLength];
            for (byte i = 0; i < sqlParam.Length; i++)
            {
                sqlParam[i] = new SqlParameter();
            }
            bool allCreated = CreateSqlParameter(sqlParam, objParam);

            //Add the parameters to the SqlCommand object(cmd)
            if (allCreated)
            {
                AddSqlParameter(sqlParam, cmd);
            }
        }

        //Open Connection
        OpenForClosedConnection();

        //Execute the Scalar Function
        var result = cmd.ExecuteScalar();

        //Close the connection
        CloseForOpenedConnection();

        return result; // return the scalar value, that is returned from the Scalar function
    }
    //the following method Execute TableValued function
    public string ExecuteTableValuedFunction(string funcName, string columns = "*", object[,] objParam = null)
    {
        //Build the Argument name
        string argument = "";
        if (objParam != null)
        {
            argument = BuildArgumentName(objParam);
        }

        //the sql command properties  
        string cmdText = $"SELECT {columns} FROM {funcName}({argument})";
        SqlCommand cmd = SqlClientProp("Text", cmdText);

        //Create the parameters
        if (objParam != null)
        {
            int innerLength = 3;
            SqlParameter[] sqlParam = new SqlParameter[objParam.Length / innerLength];
            for (byte i = 0; i < sqlParam.Length; i++)
            {
                sqlParam[i] = new SqlParameter();
            }
            bool allCreated = CreateSqlParameter(sqlParam, objParam);

            //Add the parameters to the SqlCommand object(cmd)
            if (allCreated)
            {
                AddSqlParameter(sqlParam, cmd);
            }
        }

        //Open Connection
        OpenForClosedConnection();

        //Execute the Table valued Function
        SqlDataReader reader = cmd.ExecuteReader();

        //All the result set -> string
        string allRows = "";
        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                allRows += reader[i].ToString() + " ";
            }

            allRows += "\n";
        }

        //Close the connection
        CloseForOpenedConnection();

        return allRows; // return the result set as a string
    }
}