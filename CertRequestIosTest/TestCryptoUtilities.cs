using Moldes.CertRequest.IosCertRequest;
using System;
using NUnit.Framework;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using CertRequestServer;
using System.Threading;
using UIKit;
using Foundation;

namespace CertRequestIosTest
{
    [TestFixture]
    public class MyTest
    {
        private static byte[] pkcs12;
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
            byte[] p12 = CryptoUtilities.GeneratePkcs12(cert, rsa, "12341234", "maria");
            Console.WriteLine(Convert.ToBase64String(p12));
        }

        [Test]
        public void TestServer()
        {
            Server.EmbedioServer(new byte[] { 0x00, 0x00, 0x00, 0x00 });
        }

        [Test]
        public void TestApp()
        {
            // Generamos el par de claves
            RSA rsa = CryptoUtilities.GenerateKeyPair(2048);

            // Generamos la solicitud CSR PKCS#10
            byte[] p10 = CryptoUtilities.GeneratePkcs10(rsa, new X509Name("cn=Maria"));

            // Le llega a la CA y la CA nos genera un certificado
            X509Certificate2 cert = TestUtils.GenerateCertificate(rsa);

            // Me llega el certificado de la CA, y junto con la clave privada de antes
            // genero el PKCS#12
            byte[] p12 = CryptoUtilities.GeneratePkcs12(cert, rsa, "12341234", "maria"); // Contraseña fija, va también en el XML de perfil de configuración

            // Publicamos el PKCS#12 en servidor web
            // Asignacion a variable global estatica para leerla desde el hilo
            pkcs12 = p12;

            // Abrimos un hilo con el servidor web
            Thread thread = new Thread(WorkThreadFunction);
            thread.Start();

            // Damos tiempo al servidor a arrancar
            Thread.Sleep(3000);

            // Abrimos el navegador apuntando al perfil de configuración en localhost 
            UIApplication.SharedApplication.OpenUrl(new NSUrl("http://localhost:9696/certificado.mobileconfig"));
        }

        public void WorkThreadFunction()
        {
            try
            {
                Server.EmbedioServer(pkcs12);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ejecutando el servidor en un hilo: " + ex);
            }
        }
    }
}
