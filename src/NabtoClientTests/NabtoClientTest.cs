using System;
using Xunit;
using Nabto.Client;

namespace NabtoClientTests
{
    public class NabtoClientTest
    {
        [Fact]
        public void TestConstructor1()
        {
            {
                NabtoClient nabto = new NabtoClient();
                nabto.Dispose();
            }
        }
        [Fact]
        public void testConstructorNoStartup()
        {
            {
                NabtoClient nabto = new NabtoClient(false);
                nabto.Startup();
                nabto.Dispose();
            }
            
        }
    }
}