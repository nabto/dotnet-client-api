using System;
using Xunit;
using Nabto.Client;
using System.IO;

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
        [Fact]
        public void testSetHomedir()
        {
            {
                string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(tempDirectory);
                NabtoClient nabto = new NabtoClient(false);
                nabto.HomeDirectory = tempDirectory;
                nabto.Startup();
                nabto.Dispose();
                Assert.True(File.Exists(Path.Combine(tempDirectory, "nabto_config.ini")));
                Directory.Delete(tempDirectory, true);
            }
        }
        [Fact]
        public void TestCreateAndUseCertificate()
        {
            {
                string profile = Guid.NewGuid().ToString();
                NabtoClient nabto = new NabtoClient();
                nabto.CreateSelfSignedProfile(profile, "bar");
                Session session = nabto.CreateSession(profile, "bar");
                nabto.Dispose();
            }
        }
    }
}
