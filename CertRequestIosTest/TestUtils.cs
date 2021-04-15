using Org.BouncyCastle.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;

using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace CertRequestIosTest
{
    public class TestUtils
    {
        public static X509Certificate2 GenerateCertificate(RSA rsa)
        {

            var random = new SecureRandom();
            var certificateGenerator = new X509V3CertificateGenerator();

            var serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
            certificateGenerator.SetSerialNumber(serialNumber);

            certificateGenerator.SetIssuerDN(new X509Name($"C=NL, O=SomeCompany, CN=Maria"));
            certificateGenerator.SetSubjectDN(new X509Name($"C=NL, O=SomeCompany, CN=Maria"));
            certificateGenerator.SetNotBefore(DateTime.UtcNow.Date);
            certificateGenerator.SetNotAfter(DateTime.UtcNow.Date.AddYears(1));


            AsymmetricCipherKeyPair keyPair = DotNetUtilities.GetRsaKeyPair(rsa);
            certificateGenerator.SetPublicKey(
                keyPair.Public
            );

            const string signatureAlgorithm = "SHA256WithRSA";
            var signatureFactory = new Asn1SignatureFactory(
                signatureAlgorithm,
                keyPair.Private
            );

            Org.BouncyCastle.X509.X509Certificate bouncyCert = certificateGenerator.Generate(signatureFactory);

            return new X509Certificate2(DotNetUtilities.ToX509Certificate(bouncyCert));
        }
    }
}
