using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System;
using Octokit.Internal;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.IO;

namespace ASCOM.Alpaca.Simulators.SSL
{
    public class SelfCert
    {
        internal static X509Certificate2 BuildSelfSignedServerCertificate(string FriendlyName, string Password)
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={FriendlyName}");

            using (RSA rsa = RSA.Create(2048))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)
                {
                    CertificateExtensions = {
                        new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false),
                        new X509EnhancedKeyUsageExtension(
                                                new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false),
                        sanBuilder.Build() }
                };
                  
                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    certificate.FriendlyName = FriendlyName;
                }

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, Password), Password, X509KeyStorageFlags.Exportable);
            }
        }

        internal static void SaveCertificate(X509Certificate2 certificate2, string password, string path)
        {
            byte[] certBytes = certificate2.Export(X509ContentType.Pfx, password);
            File.WriteAllBytes(path, certBytes);
        }
    }
}
