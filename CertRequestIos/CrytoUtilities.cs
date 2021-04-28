using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace Moldes.CertRequest.IosCertRequest
{
    /// <summary>
    /// The main Certificate Request class. Contains all methods for performing the generation of a digital certificate for the user.
    /// </summary>
    /// <list type="bullet">
    /// <term>Key Pair</term>
    /// <description>Generate a private and public key using RSA algorithm </description>
    /// <item>Generate Pkcs10</item>
    /// <description>Generate a Certificate Signing Request </description>
    /// </list>
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
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.KeySize = keySize;
            return rsa;
        }

        public static byte[] GeneratePkcs10(RSA rsa, X509Name subject)
        {
            // CertificateRequest pkcs10 = new CertificateRequest(subject, rsa, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            //return pkcs10.CreateSigningRequest(X509SignatureGenerator.CreateForRSA(rsa, signaturePadding: RSASignaturePadding.Pkcs1));

            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
            Pkcs10CertificationRequest csr = new Pkcs10CertificationRequest("SHA1WITHRSA", subject, keyPair.Public, null, keyPair.Private);
            return csr.GetDerEncoded();
        }

    }
}
