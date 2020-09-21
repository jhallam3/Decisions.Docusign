using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionsFramework.ServiceLayer;
using Decisions.Docusign.DSServiceReference;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Decisions.Docusign
{
    public class DSServiceClientFactory
    {

        private const string DEMO_ENDPOINT = "https://demo.docusign.net/api/3.0/dsapi.asmx";
        private const string PROD_ENDPOINT = "https://www.docusign.net/api/3.0/dsapi.asmx";

        public static DocusignSettings DsSettings
        {
            get
            {
                return ModuleSettingsAccessor<DocusignSettings>.GetSettings();
            }
        }

        private static string GetDsAuth(IDocusignCreds creds)
        {
            return "<DocuSignCredentials><Username>" + creds.UserName
                + "</Username><Password>" + creds.Password
                + "</Password><IntegratorKey>" + creds.IntegratorKey
                + "</IntegratorKey></DocuSignCredentials>";
        }

        public static HttpRequestMessageProperty GetAuthHeaderRequestProperty(IDocusignCreds creds)
        {
            var httpRequestProperty = new HttpRequestMessageProperty();
            httpRequestProperty.Headers.Add("X-DocuSign-Authentication", GetDsAuth(creds));
            return httpRequestProperty;
        }

        public static DSAPIServiceSoapClient GetDsClient(IDocusignCreds creds)
        {
            return new DSAPIServiceSoapClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport) { MaxReceivedMessageSize = 167772160 }, new EndpointAddress(GetEndpoint(creds)));
        }

        private static string GetEndpoint(IDocusignCreds creds)
        {
            if (creds.UseDemoEnvironment) return DEMO_ENDPOINT;
            else return PROD_ENDPOINT;
        }
    }
}
