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
        // Método que genera un archivo con extensión p12 a partir de un certificado X509, la contraseña pública y privada
        //del usuario y un alias.
        public static byte[] GeneratePkcs12(X509Certificate2 cert, RSA rsa, String password, String alias)
        {
            //Se realizan unos controles de error sobre los parámetros introducidos en el método. De este modo
            //se consiguen evitar valores vacíos o nulos. 
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

            // Creamos una variable de almacenamiento con extesión p12 para introducir el certificado y la clave privada del usuario
            // asignando a esta una contraseña.
            Pkcs12Store store = new Pkcs12StoreBuilder().Build();

            //Extraemos la clave pública y privada y la guardamos en una variable.
            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);

            //Guardamos la clave privada, el certificado y un alias del usuario dentro del p12.
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
           
            //Guardamos el p12
            var ms = new System.IO.MemoryStream();
            store.Save(ms, password.ToCharArray(), new SecureRandom());
            Console.WriteLine(password.ToCharArray());
            byte[] ret = ms.ToArray();
            ms.Close();
            return ret;
        }                                                    

        //Método que genera un par de claves criptográficas (pública y privada). 
        public static RSA GenerateKeyPair(int keySize)
        {
            //Generamos las claves y controlamos posibles errores que puedan surgir
            //durante la generación de estas.
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

        //Método que genera una solicitud de certificado en formato PKCS#10 a partir del par de claves generadas por el usuario.
        public static byte[] GeneratePkcs10(RSA rsa, X509Name subject)
        {
            //Implementamos un control de errores en la generación del p10
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
            if (csr == null)
            {
                throw new Exception("El certificado no puede ser nulo");
            }
            return csr.GetDerEncoded();
        }
    }
}
