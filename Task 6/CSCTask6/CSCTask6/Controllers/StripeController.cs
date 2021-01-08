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
using Stripe.Checkout;


namespace CSCTask6.Controllers
{
    public class StripeController : ApiController
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "02igQ44z8HsHe2Ufq01IbBNfbVBdq6EUhI7aeynB",
            BasePath = "https://csctask6-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

       

        [HttpGet]
        [Route("api/paysub")]
        public async Task<string> createSubsAsync()
        {
            StripeConfiguration.ApiKey = "sk_test_51I5B7mLaXDcHpE2BKAPeNbNK9ZiHUVIaTI6D6YuXe1Mloa2F3GiIGXG0piKKs2vROyem94HywDr9ysJK0I8w0NRk00SZjaoGWh";
            Trace.WriteLine("Pay for a sub");

            var options = new SessionCreateOptions
            {
                // See https://stripe.com/docs/api/checkout/sessions/create
                
                SuccessUrl = "https://localhost:44314/Home/Charge?msg=pass",
                CancelUrl = "https://localhost:44314/Home/Charge?msg=fail",
                PaymentMethodTypes = new List<string>
            {
                "card",
            },
                    Mode = "subscription",
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            Price = "price_1I5Bi7LaXDcHpE2BqbEBl8zK",
                            // For metered billing, do not pass quantity
                            Quantity = 1,
                        },
                    },
            };// End of options

            var service = new SessionService();
            var session = await service.CreateAsync(options);
            var sessionid = session.Id;
            client = new FireSharp.FirebaseClient(config);

            var data = new FirebaseInput
            {
                Name = "Pay for sub",
                Id = session.Id
            };

            Trace.WriteLine("sessId :"+ session.CustomerId);
            SetResponse response = client.Set("customers/1/orders/2", sessionid);
            string result = response.ResultAs<string>();

            return sessionid;
        }

        [HttpGet]
        [Route("api/checksub")]
        public async Task<FirebaseInput> checkSubsAsync()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseInput msg = new FirebaseInput {
                Name = "None",
                Id = "None"
            };

           FirebaseResponse response = await client.GetAsync("customers/1/Subscription");
           FirebaseInput[] result = response.ResultAs<FirebaseInput[]>();

            foreach (FirebaseInput res in result)
            {
                
                if (res != null)
                {
                    Trace.WriteLine("result in get: " + res.Name);
                    Trace.WriteLine("result in get: " + res.Id);
                    msg = res;
                }
               

            }

            
            Trace.WriteLine("result for get: " + msg.Id);
            return msg;
        }

        [HttpPost]
        [Route("api/createCus")]
        public string CreateCus()
        {
            var msg = "failed to create account";

            StripeConfiguration.ApiKey = "sk_test_51I5B7mLaXDcHpE2BKAPeNbNK9ZiHUVIaTI6D6YuXe1Mloa2F3GiIGXG0piKKs2vROyem94HywDr9ysJK0I8w0NRk00SZjaoGWh";

            var options = new AccountCreateOptions
            {
                Type = "custom",
                Country = "US",
                Email = "jenny.rosen@example.com",
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions
                    {
                        Requested = true,
                    },
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true,
                    },
                },
            };
            var service = new AccountService();
            service.Create(options);

            if(service != null)
            {
                msg = "Account created";
            }
            

            return msg;
        }

        [HttpGet]
        [Route("api/createsub")]
        public async Task<string> CreateSubAsync()
        {
            var msg = "failed to create Subscription";

            StripeConfiguration.ApiKey = "sk_test_51I5B7mLaXDcHpE2BKAPeNbNK9ZiHUVIaTI6D6YuXe1Mloa2F3GiIGXG0piKKs2vROyem94HywDr9ysJK0I8w0NRk00SZjaoGWh";

            var options = new SubscriptionCreateOptions
            {
                Customer = "cus_IhyONtmViXXMfa",
                Items = new List<SubscriptionItemOptions>
            {
            new SubscriptionItemOptions
            {
                Price = "price_1I5Bi7LaXDcHpE2BqbEBl8zK",
                Quantity = 1,
            },
            },
                    };

            var service = new SubscriptionService();
            var subscription = service.Create(options).Id;

            client = new FireSharp.FirebaseClient(config);
            var data = new FirebaseInput
            {
                Name = "Netflix Basic",
                Id = subscription
            };
            SetResponse response = await client.SetAsync("customers/1/Subscription/1", data);
            FirebaseInput result = response.ResultAs<FirebaseInput>();

            return msg;
        }

        [HttpPost]
        [Route("api/updatesub")]
        public async Task<string> UpdateSubAsync(FirebaseInput sub)
        {
            var msg = "Failed to upgrade account";

            StripeConfiguration.ApiKey = "sk_test_51I5B7mLaXDcHpE2BKAPeNbNK9ZiHUVIaTI6D6YuXe1Mloa2F3GiIGXG0piKKs2vROyem94HywDr9ysJK0I8w0NRk00SZjaoGWh";

            var service = new SubscriptionService();
            Trace.WriteLine("Sub id in update: "+sub.Id);
            Subscription subscription = service.Get(sub.Id);

            

            string PriceId = "";
            string type = sub.Name;
            string SubId = sub.Id;

            Trace.WriteLine(SubId);
            
            if (type == "Netflix Basic")
            {
                PriceId = "price_1I5Bi7LaXDcHpE2BqbEBl8zK";
            }

            if (type == "Netflix Enhanced")
            {
                PriceId = "price_1I5re3LaXDcHpE2B3XKR9hGK";
            }

            var BCA = SubscriptionBillingCycleAnchor.Unchanged;

            var items = new List<SubscriptionItemOptions> {
                new SubscriptionItemOptions {
                    Id = subscription.Items.Data[0].Id,
                    Price = PriceId
                    },
            };

            var options = new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                ProrationBehavior = "create_prorations",
                Items = items,
                BillingCycleAnchor = BCA
            };

            subscription = service.Update(sub.Id, options);

            var data = sub;

            client = new FireSharp.FirebaseClient(config);
            SetResponse response = await client.SetAsync("customers/1/Subscription/1", data);
            FirebaseInput result = response.ResultAs<FirebaseInput>();

            msg = "Stripe req: "+subscription.StripeResponse.RequestId + " /nFireBase res: "+result.Name;
            return msg;
        }

        [HttpPost]
        [Route("api/cancelsub")]
        public async Task<string> cancelSubAsync(FirebaseInput sub)
        {
            var msg = "Failed to upgrade account";

            // Set your secret key. Remember to switch to your live secret key in production!
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.ApiKey = "sk_test_51I5B7mLaXDcHpE2BKAPeNbNK9ZiHUVIaTI6D6YuXe1Mloa2F3GiIGXG0piKKs2vROyem94HywDr9ysJK0I8w0NRk00SZjaoGWh";

            var service = new SubscriptionService();
            var cancelOptions = new SubscriptionCancelOptions
            {
                InvoiceNow = false,
                Prorate = false,
            };
            Subscription subscription = service.Cancel(sub.Id, cancelOptions);

            client = new FireSharp.FirebaseClient(config);
            var data = new FirebaseInput
            {
                Name = "None",
                Id = "1"
            };
            SetResponse response = await client.SetAsync("customers/1/Subscription/1", data);
            FirebaseInput result = response.ResultAs<FirebaseInput>();

            msg = subscription.StripeResponse.RequestId;

            return msg;
        }

       
    }
}
