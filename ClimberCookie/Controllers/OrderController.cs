using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace ClimberCookie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Route("CreateOrder/{custID}/{shippingID}")]
        public string CreateOrder(int custID, int shippingID)
        {
            var connectionstring = "Host=localhost;Username=postgres;Password=ClimberCookie!93;Database=ClimberCookie";
            using(var conn = new NpgsqlConnection(connectionstring))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("Select * from customer",conn))
                {
                    using (var read = cmd.ExecuteReader())
                    {
                        var tmp = "";
                        while (read.Read())
                        {
                            tmp = (string)read.GetValue(read.GetOrdinal("custname"));
                        }
                        return tmp;

                    }
                }
            }
        }

    }
}