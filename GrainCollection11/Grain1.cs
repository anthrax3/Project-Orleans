using System.Threading.Tasks;
using Orleans;
using IGrain11;
using System;

namespace GrainCollection11
{
    /// <summary>
    /// Grain implementation class Grain1.
    /// </summary>
    public class Grain1 : Grain, IGrain11.IGrain1
    {
        public Task<string> SayHello()
        {
            return Task.FromResult("Hello World!");
        }
    }
}
