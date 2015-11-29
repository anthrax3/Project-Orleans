using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebOrleans.Controllers
{
    public class testController : ApiController
    {
        public Task<string> Get()
        {
            return GrainClient.GrainFactory.GetGrain<IGrain11.IGrain1>(0).SayHello();   
        }
    }
}
