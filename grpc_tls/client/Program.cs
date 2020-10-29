﻿using GameService;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        static string ca = @"-----BEGIN CERTIFICATE-----
MIIFkzCCA3ugAwIBAgIUTratzzbxHY32KG+GyTdktcXwXxwwDQYJKoZIhvcNAQEL
BQAwWTELMAkGA1UEBhMCQVUxEzARBgNVBAgMClNvbWUtU3RhdGUxITAfBgNVBAoM
GEludGVybmV0IFdpZGdpdHMgUHR5IEx0ZDESMBAGA1UEAwwJbG9jYWxob3N0MB4X
DTIwMTAyODEyMTczOFoXDTMwMTAyNjEyMTczOFowWTELMAkGA1UEBhMCQVUxEzAR
BgNVBAgMClNvbWUtU3RhdGUxITAfBgNVBAoMGEludGVybmV0IFdpZGdpdHMgUHR5
IEx0ZDESMBAGA1UEAwwJbG9jYWxob3N0MIICIjANBgkqhkiG9w0BAQEFAAOCAg8A
MIICCgKCAgEA3DuzpZqO3KUpY1zU7zH50kFTj6wEfmehutIYFylqrLvDt+QeqfIo
ZKvQ/rI0F7jvIFBF5+uaX42MWZmJ6HBaerEoeb/Hw0i5qMd3pnKkmTu63/zhFIWB
en+7niGNGsNLGSXQH6cYMOpsFhcjZW+3F6q6/rosjS7+fD87o8V/4b3LmxraCkyd
PLB0pX0LM8brnxFdUYAWFQLMZBFkEryh/eD3IL2oqVzSo+kYhXs14wty7+XJEcyW
shXNe2r+s+GHSd1TIeoc+SZSXkG4OHpilUoTW1uHFsbipZxcQxMpEKk0OIRRKOb4
0FWQzjQppIuGk+VeqKhRU8SWzWd9W0DfL2gGeMWglqwW52+mnSEE+gkaK+AwSz9y
3kWzyhvGoEpWFV0BaSNRj+miW8HHbT55x/z/X4nwVcL+ch1KaaZTagtKVm0D4Qqv
MeY5ZFfm1DQmsLRUA8h9IDcmTwuIfrnm5a+AOPk+MnQy40GamV2PGgnJymzBZIYd
WbyJw0GQDnaMxxKRUC3AqHj4KE+Y5lO+qzJdTvsTwMnopfP++hjmh3OuWIxjXCAh
qoB3m4dB74Jyf+b+xHlXHcj1u+RdhV9HoG/5N2HTa9bxROmeT3O0H8B8we+U+gAS
OZEuh8tD9dh84eK6Yk8dZIvhW7XMSUny8U3CqiJTOdIiQVNTBEXRgHECAwEAAaNT
MFEwHQYDVR0OBBYEFDsAE7b374I8AscjBWVkgO2Y7PK5MB8GA1UdIwQYMBaAFDsA
E7b374I8AscjBWVkgO2Y7PK5MA8GA1UdEwEB/wQFMAMBAf8wDQYJKoZIhvcNAQEL
BQADggIBAL9MtGSDU22yVn/nLmeWb4CIIvXRFiVd8u9xDpwpzLvO5Rdh6fHgvNDp
SCAuGzXQrCCLmzVu17QrvCrEnnjg8fBnuuWa1PPsqtgVo2DUEyqu3auIdhpvDmXQ
qXFbxnAuQDRX5yeJjuX5Q2GeyS6yhxioX0AzKm1lb9fdlu/ARmp8Bg3d9tb3ovTA
mPSBbvG2inAf/ooWIa/FhQzWMdtPSAy68vD7klvR+7NWnKze0Cyh3QbbAzF6aDa/
uL/OjLdYuMXJd7KGjOeBZeZgcAjx49Q3acrvvdyvE/reVdZFf0j1e5YwvNUCF04D
sSvTqPDvzj9XY0yOvf7OGhPc5fOLLrf47jI7YjxrkuVL2OyNZ51icPnCMwL3YwfW
jYedW0wlIvQGWyREpbWR8a4KbIEdwwYudIHsVYDbJgUiqJmZF//GgKnGy9iNJifF
6XS46i/K61QkyFM5DKlFBBchsRBy7ZOSSaVZ6QXsGAFz+tECMGysHIWc08ohHttT
AflwJ+53ziZrRYdAosBlTCI3UQEWkqT6MdOUuHK8J6gQYlt0Tb6E3r4DzMkrvyph
010nsqVThIo+dlClvHo+FpGyxXgtfR6no8z/TGMs1HOGOwjV103zxW3mwSzkofvc
qZAfZKury8I2iWTF25cMlDo1R1sNmNXQFLUfB7f5LrZgz40w5Uzm
-----END CERTIFICATE-----";



        public static string ExportToPEM(X509Certificate cert)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");

            return builder.ToString();
        }

        public static string GetRootCertificates()
        {
            StringBuilder builder = new StringBuilder();
            X509Store store = new X509Store(StoreName.Root);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 mCert in store.Certificates)
            {
                builder.AppendLine(
                    "# Issuer: " + mCert.Issuer.ToString() + "\n" +
                    "# Subject: " + mCert.Subject.ToString() + "\n" +
                    "# Label: " + mCert.FriendlyName.ToString() + "\n" +
                    "# Serial: " + mCert.SerialNumber.ToString() + "\n" +
                    "# SHA1 Fingerprint: " + mCert.GetCertHashString().ToString() + "\n" +
                    ExportToPEM(mCert) + "\n");
            }
            return builder.ToString();
        }

        static async Task Main(string[] args)
        {
            try
            {
                Environment.SetEnvironmentVariable("GRPC_TRACE", "api,http,cares_resolver,cares_address_sorting,transport_security,tsi");
                Environment.SetEnvironmentVariable("GRPC_VERBOSITY", "debug");
                Grpc.Core.GrpcEnvironment.SetLogger(new Grpc.Core.Logging.ConsoleLogger());
                //GetRootCertificates();



                var clientCredentials = new SslCredentials(ca);


                //var ssl = new SslCredentials(ca, new KeyCertificatePair(clientcert, clientkey));

                var options = new List<ChannelOption>
                {
                    new ChannelOption(ChannelOptions.SslTargetNameOverride, "localhost")
                };

                var channel = new Channel("localhost:50051", clientCredentials, options);
                var client = new GameService.Lobby.LobbyClient(channel);



                using (var call = client.Login(new LoginRequest { PlayerId = "test", DeviceUniqueIdentifier = "tttt" }))
                {
                    var responseStream = call.ResponseStream;
                    while (await responseStream.MoveNext())
                    {
                    }
                }

                await channel.ShutdownAsync();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
