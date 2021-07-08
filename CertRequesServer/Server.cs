using System;
using System.Threading;
using EmbedIO;



namespace CertRequestServer
{
    public class Server
    {
        private static String XML_PROFILE_TEMPLATE =
             "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n"
             + "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">\r\n"
             + "<plist version=\"1.0\">\r\n"
             + "<dict>\r\n"
             + "	<key>PayloadContent</key>\r\n"
             + "	<array>\r\n"
             + "		<dict>\r\n"
             + "			<key>Password</key>\r\n"
             + "			<string>12341234</string>\r\n"
             + "			<key>PayloadCertificateFileName</key>\r\n"
             + "			<string>CERTIFICADO.pfx</string>\r\n"
             + "			<key>PayloadContent</key>\r\n"
             + "			<data>\r\n"
             + "			    $PKCS12$\r\n"
             + "			</data>\r\n"
             + "			<key>PayloadDescription</key>\r\n"
              + "			<string>Añade un certificado con formato PKCS#12</string>\r\n"
              + "			<key>PayloadDisplayName</key>\r\n"
              + "			<string>CERTIFICADO_.pfx</string>\r\n"
              + "			<key>PayloadIdentifier</key>\r\n"
              + "			<string>com.apple.security.pkcs12.$UUID1$</string>\r\n"
              + "			<key>PayloadType</key>\r\n"
              + "			<string>com.apple.security.pkcs12</string>\r\n"
              + "			<key>PayloadUUID</key>\r\n"
              + "			<string>$UUID1$</string>\r\n"
              + "			<key>PayloadVersion</key>\r\n"
              + "			<integer>1</integer>\r\n"
              + "		</dict>\r\n"
              + "	</array>\r\n"
              + "	<key>PayloadDisplayName</key>\r\n"
              + "	<string>Certificado de SSL</string>\r\n"
              + "	<key>PayloadIdentifier</key>\r\n"
              + "	<string>Certificado.$UUID2$</string>\r\n"
              + "	<key>PayloadRemovalDisallowed</key>\r\n"
              + "	<false/>\r\n"
              + "	<key>PayloadType</key>\r\n"
              + "	<string>Configuration</string>\r\n"
              + "	<key>PayloadUUID</key>\r\n"
              + "	<string>$UUID2$</string>\r\n"
              + "	<key>PayloadVersion</key>\r\n"
              + "	<integer>1</integer>\r\n"
              + "</dict>\r\n"
              + "</plist>"
          ;
        public static void EmbedioServer(byte[] p12)
        {

            String b64P12 = Convert.ToBase64String(p12);
            String uuid1 = Guid.NewGuid().ToString();
            String uuid2 = Guid.NewGuid().ToString();

            String xmlProfile = XML_PROFILE_TEMPLATE
                .Replace("$PKCS12$", b64P12)
                .Replace("$UUID$1", uuid1)
                .Replace("$UUID$2", uuid2)
                ;

            WebServer server = new WebServer(o => o
                .WithUrlPrefix("http://localhost:9696/")
                .WithMode(HttpListenerMode.EmbedIO))
                .WithLocalSessionManager()
                .WithCustomMimeType(".mobileconfig", "application/x-apple-aspen-config")
                .WithAction("/certificado.mobileconfig", HttpVerbs.Get, ctx =>
                {
                    return ctx.SendStringAsync(
                        xmlProfile,
                        "application/x-apple-aspen-config",
                        System.Text.Encoding.UTF8
                    );
                }
            );

            var cts = new CancellationTokenSource();
            var task = server.RunAsync(cts.Token);

            bool stop = false;
            while (!stop)
            {
                Thread.Sleep(50000);
            }

            cts.Cancel();
            task.Wait();
            server.Dispose();

        }
    }
}
