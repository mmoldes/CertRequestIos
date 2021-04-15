using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Moldes.CertRequest.IosCertRequest
{
    public class CryptoUtilities
    {
        public static byte[] GeneratePkcs12(X509Certificate2 cert, RSA rsa, String password)
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

        }

        public static RSA GenerateKeyPair(int keySize)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.KeySize = keySize;
            return rsa;
        }

        public static byte[] GeneratePkcs10(RSA rsa, X500DistinguishedName subject)
        {
            CertificateRequest pkcs10 = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            return pkcs10.CreateSigningRequest(X509SignatureGenerator.CreateForRSA(rsa, signaturePadding: RSASignaturePadding.Pkcs1));
        }

    }
}
