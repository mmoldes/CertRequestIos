using Moldes.CertRequest.IosCertRequest;
using System;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;

namespace CertRequestIosTest
{
    [TestFixture]
    public class MyTest
    {
        [Test]
        public void TestGenerateKeyPair()
        {
            RSA rsa = CryptoUtilities.GenerateKeyPair(2048);
            Console.WriteLine(rsa.ToXmlString(true));
        }

        [Test]
        public void TestGeneratePkcs10()
        {
            RSA rsa = CryptoUtilities.GenerateKeyPair(2048);
            byte[] p10 = CryptoUtilities.GeneratePkcs10(rsa, new X509Name("cn=Maria"));
            Console.WriteLine(System.Convert.ToBase64String(p10));
        }

        [Test]
        public void TestGenerateX509()
        {
            RSA rsa = CryptoUtilities.GenerateKeyPair(2048);
            X509Certificate2 cert = TestUtils.GenerateCertificate(rsa);
            Console.WriteLine(cert.ToString());
        }

        [Test]
        public void TestGeneratePkcs12()
        {
            RSA rsa = CryptoUtilities.GenerateKeyPair(2048);
            X509Certificate2 cert = TestUtils.GenerateCertificate(rsa);
            byte[] p12 = CryptoUtilities.GeneratePkcs12(cert, rsa, "pass", "maria");
            Console.WriteLine(System.Convert.ToBase64String(p12));
        }
    }
}
