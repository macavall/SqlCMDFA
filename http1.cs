using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;

namespace SqlCMDFA
{
    public class http1
    {
        [Function("http1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            var _logger = context.GetLogger("http1");

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            var str = "Server=tcp:casecountserver.database.windows.net,1433;Initial Catalog=EngineerCountDB8;Persist Security Info=False;User ID=casecountuser;Password={PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();
                var text = "Select TOP (1000) * FROM MemberBook1";

                using (SqlCommand cmd = new SqlCommand(text, conn))
                {
                    // Execute the command and log the # rows affected.
                    var rows = await cmd.ExecuteNonQueryAsync();
                    _logger.LogInformation($"{rows} rows were updated");
                }
            }

            return response;
        }
    }
}
