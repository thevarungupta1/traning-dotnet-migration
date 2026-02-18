using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

public class DataService
{
    public List<string> GetData()
    {
        var list = new List<string>();
        var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        using (var conn = new SqlConnection(connString))
        {
            conn.Open();
            var cmd = new SqlCommand("SELECT Name FROM SampleTable", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(reader.GetString(0));
            }
        }

        return list;
    }
}
