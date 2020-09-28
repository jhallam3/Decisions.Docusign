using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionsFramework.Data.DataTypes;
using DecisionsFramework.Design.Flow;
using System.ServiceModel;
using System.ServiceModel.Channels;
using DecisionsFramework.Design.Flow.StepImplementations;
using System.Xml.Serialization;
using System.IO;

namespace Decisions.Docusign
{


    [AutoRegisterMethodsOnClass(true, "Integration", "Docusign")]
    public static class DocusignSteps
    {        
        //Docusign API dev guide says this method is subject to call limit and should not be used more than once every 15 min per unique envelope ID
        public static string GetDocumentStatus(string envelopeId, [IgnoreMappingDefault] DocusignCredentials overrideCredentials = null)
        {
            IDocusignCreds creds = overrideCredentials as IDocusignCreds ?? DSServiceClientFactory.DsSettings;

            var dsClient = DSServiceClientFactory.GetDsClient(creds);

            using (var scope = new System.ServiceModel.OperationContextScope(dsClient.InnerChannel))
            {                
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = DSServiceClientFactory.GetAuthHeaderRequestProperty(creds);
                
                return dsClient.RequestStatus(envelopeId).Status.ToString();
            }

        }

        public static Docusign.DataTypes.DocuSignEnvelopeInformation DeserialiseDocusignEnvelopeInformation(string XML)
        {
            try
            {


                var result = (Decisions.Docusign.DataTypes.DocuSignEnvelopeInformation)new XmlSerializer(typeof(Decisions.Docusign.DataTypes.DocuSignEnvelopeInformation), "http://www.docusign.net/API/3.0").Deserialize(new StringReader(XML));
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static FileData GetSignedDocument(string envelopeId, [IgnoreMappingDefault] DocusignCredentials overrideCredentials = null)
        {
            IDocusignCreds creds = overrideCredentials as IDocusignCreds ?? DSServiceClientFactory.DsSettings;

            var dsClient = DSServiceClientFactory.GetDsClient(creds);

            using (var scope = new System.ServiceModel.OperationContextScope(dsClient.InnerChannel))
            {
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = DSServiceClientFactory.GetAuthHeaderRequestProperty(creds);

                var documentsPDFs = dsClient.RequestDocumentPDFs(envelopeId);

                if (documentsPDFs == null || documentsPDFs.DocumentPDF == null || documentsPDFs.DocumentPDF.Length == 0)
                {
                    return null;
                }

                return new FileData(string.Format("{0}.pdf", documentsPDFs.DocumentPDF[0].Name), documentsPDFs.DocumentPDF[0].PDFBytes);
              
            }

        }
                       
    }
}
