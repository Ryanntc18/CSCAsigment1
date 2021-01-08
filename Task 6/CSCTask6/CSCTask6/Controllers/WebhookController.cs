using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CSCTask6.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Stripe;

[Route("webhook")]
public class WebhookController : ApiController
{

    IFirebaseConfig config = new FirebaseConfig
    {
        AuthSecret = "Firebase Auth Key",
        BasePath = "https://csctask6-default-rtdb.firebaseio.com/"
    };

    IFirebaseClient client;

    [HttpPost]
    public async Task<IHttpActionResult> Index(HttpContext context)
    {
       
        client = new FireSharp.FirebaseClient(config);
        Trace.WriteLine("Event has occured");

        Stream req = await Request.Content.ReadAsStreamAsync();
        req.Seek(0, SeekOrigin.Begin);
        string json = new StreamReader(req).ReadToEnd();

        string msg = "Failed";

        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);
            
            FirebaseResponse response = await client.GetAsync("customers/1/Events");            

            FirebaseInput[] result = response.ResultAs<FirebaseInput[]>();

            int amt = result.Count();
            Trace.WriteLine("Get Event count");

            FirebaseInput data = new FirebaseInput
            {
                Name = stripeEvent.Type,
                Id = stripeEvent.Id
            };
            
            Trace.WriteLine("Event Occured: "+ stripeEvent.Type);
            SetResponse response1 = await client.SetAsync("customers/1/Events/"+amt, data);
            
            Trace.WriteLine("Event Succesfully saved to database");

            FirebaseInput result1 = response1.ResultAs<FirebaseInput>();

            msg = "Event " + result1.Name + " Logged";

            return Ok(msg);
        }
        catch (StripeException)
        {
            return BadRequest();
        }


    }
}
