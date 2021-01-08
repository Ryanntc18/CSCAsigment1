using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;

using CSCTask6.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Stripe;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace CSCTask6.Controllers
{
    public class FirebaseController : ApiController
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "AuthKEY",
            BasePath = "BasePath"
        };

        IFirebaseClient client;

        [HttpGet]
        [Route("api/testdb")]
        public string TestAuth()
        {
            string msg = "Auth Failed";

            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                msg = "Auth Sucessful";
            }

            return msg;
        }

        [HttpPost]
        [Route("api/testcreate")]
        public async Task<string> CreateCustomerAsync(CustomerModel cust)
        {
            string msg = "Failed to create";
            client = new FireSharp.FirebaseClient(config);
            Trace.WriteLine(client);
            var data = cust;
            Trace.WriteLine(cust.Name);



            //SetResponse response = await client.SetAsync("customers/1", data);
            //CustomerModel result = response.ResultAs<CustomerModel>();
            FirebaseResponse response = await client.GetAsync("customers/1/Subscription");
            CustomerModel[] result = response.ResultAs<CustomerModel[]>();

            foreach(CustomerModel res in result)
            {
                if(res != null){
                    Trace.WriteLine("result: " + res.Name);
                }
                
            }

            if (result != null)
            {
                msg = "Created Sucessfully: "+result.Count();
            }

            return msg;
        }
    }
}