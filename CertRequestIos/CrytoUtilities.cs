using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using EmbedIO;
using System.Threading;


namespace Moldes.CertRequest.IosCertRequest
{
   
    public class CryptoUtilities
    {
        public static byte[] GeneratePkcs12(X509Certificate2 cert, RSA rsa, String password, String alias)
        {
            if (cert == null)
            {
                throw new Exception("El certificado no puede ser nulo");
            }
            if (rsa == null)
            {
                throw new Exception("La clave privada no pyede ser nula");
            }
            if (password == null || password.Length < 1)
            {
                throw new Exception("La contrasena no puede ser nula ni vacia");
            }

            Pkcs12Store store = new Pkcs12StoreBuilder().Build();

            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);

            store.SetKeyEntry(
                alias: alias,
                new AsymmetricKeyEntry(
                    key: keyPair.Private
                ),
                chain: new X509CertificateEntry[] {
                    new X509CertificateEntry(
                        DotNetUtilities.FromX509Certificate(cert)
                    )
                }
            );

            var ms = new System.IO.MemoryStream();
            store.Save(ms, password.ToCharArray(), new SecureRandom());
            byte[] ret = ms.GetBuffer();
            ms.Close();
            return ret;

        }                                                    

        public static RSA GenerateKeyPair(int keySize)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
                if (rsa == null)
                {
                    throw new Exception("rsa no puede ser nulo");
                }
            rsa.KeySize = keySize;
                if (rsa.KeySize != keySize)
                {
                    throw new Exception("El tamaño de la clave no coincide con lo introducido");
                }
            return rsa;
        }

        public static byte[] GeneratePkcs10(RSA rsa, X509Name subject)
        {
            if (rsa == null)
            {
                throw new Exception("rsa no puede ser nulo");
            }
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
            if (keyPair == null)
            {
                throw new Exception("El par de claves no pueden ser nulas");
            }
            Pkcs10CertificationRequest csr = new Pkcs10CertificationRequest("SHA512WITHRSA", subject, keyPair.Public, null, keyPair.Private);
            return csr.GetDerEncoded();
        }
    }
}
