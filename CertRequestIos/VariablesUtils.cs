using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Moldes.CertRequest.IosVariables
{
    class VariablesUtils
    {
        public class PerfilConfiguracion
        {
            public PerfilConfiguracion()
            {
                String XML_PROFILE =
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

            }
        }

          
    }
}