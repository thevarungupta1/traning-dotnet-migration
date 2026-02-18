using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

public class DataService
{
    private readonly IConfiguration _config;

    public DataService(IConfiguration config)
    {
        _config = config;
    }


    public async Task<IEnumerable<string>> GetDataAsyc()
    {
        using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return await conn.QueryAsync<string>("SELECT Name FROM SampleTable");
    }

    // public List<string> GetData()
    // {
    //     var list = new List<string>();
    //     var connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    //     using (var conn = new SqlConnection(connString))
    //     {
    //         conn.Open();
    //         var cmd = new SqlCommand("SELECT Name FROM SampleTable", conn);
    //         var reader = cmd.ExecuteReader();

    //         while (reader.Read())
    //         {
    //             list.Add(reader.GetString(0));
    //         }
    //     }

    //     return list;
    // }
}
