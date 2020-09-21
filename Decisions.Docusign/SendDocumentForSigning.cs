using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.ServiceModel;
using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using DecisionsFramework.Design.Flow;
using DecisionsFramework.Design.Flow.Mapping;
using Decisions.Docusign.DSServiceReference;
using Decisions.Docusign.DataTypes;
using DecisionsFramework.Design.Flow.Mapping.InputImpl;

namespace Decisions.Docusign
{
    [AutoRegisterStep("Send Document For Signing", "Integration", "Docusign")]
    [Writable]
    public sealed class SendDocumentForSigning : ISyncStep, IDataConsumer, IDataProducer, IDefaultInputMappingStep
    {
        private const string OUTCOME_SENT = "Sent";
        private const string OUTCOME_ERROR = "Error";

        private const string OUTPUT_ENVELOPEID = "EnvelopeID";
        private const string OUTPUT_ERRORMESSAGE = "ErrorMessage";

        private const string INPUT_DOCUMENT = "Document";
        private const string INPUT_DOCUMENTTITLE = "DocumentTitle";
        private const string INPUT_RECIPIENTS = "Recipients";
        private const string INPUT_SUBJECT = "Subject";
        private const string INPUT_EMAILBLURB = "EmailBlurb";
        private const string INPUT_CREDS = "OverrideCredentials";

        #region Outcomes

        public OutcomeScenarioData[] OutcomeScenarios
        {
            get
            {
                return new[] {
                    new OutcomeScenarioData(OUTCOME_SENT, new DataDescription(typeof(string), OUTPUT_ENVELOPEID)),
                    new OutcomeScenarioData(OUTCOME_ERROR, new DataDescription(typeof(string), OUTPUT_ERRORMESSAGE))
                };
            }
        }

        #endregion

        #region Input Data
        public DataDescription[] InputData
        {
            get
            {
                return new DataDescription[]
                {
                    new DataDescription(typeof (Byte), INPUT_DOCUMENT, true),
                    new DataDescription(typeof(string), INPUT_DOCUMENTTITLE),
                    new DataDescription(typeof (RecipientTabMapping), INPUT_RECIPIENTS, true),
                    new DataDescription(typeof (string), INPUT_SUBJECT),
                    new DataDescription(typeof (string), INPUT_EMAILBLURB),
                    new DataDescription(typeof (DocusignCredentials), INPUT_CREDS)
                };
            }
        }

        public IInputMapping[] DefaultInputs
        {
            get
            {
                return new IInputMapping[]
                {
                    new SelectValueInputMapping { InputDataName = INPUT_DOCUMENT },
                    new SelectValueInputMapping { InputDataName = INPUT_DOCUMENTTITLE },
                    new SelectValueInputMapping { InputDataName = INPUT_RECIPIENTS },
                    new SelectValueInputMapping { InputDataName = INPUT_SUBJECT },
                    new SelectValueInputMapping { InputDataName = INPUT_EMAILBLURB },
                    new NullInputMapping { InputDataName = INPUT_CREDS },
                };
            }
        }
        #endregion

        #region Run

        public ResultData Run(StepStartData data)
        {
            IDocusignCreds creds;
            if (data.Data.ContainsKey(INPUT_CREDS) && data.Data[INPUT_CREDS] != null)
                creds = data.Data[INPUT_CREDS] as IDocusignCreds;
            else
                creds = DSServiceClientFactory.DsSettings;

            var dsClient = DSServiceClientFactory.GetDsClient(creds);

            var document = (byte[])data.Data[INPUT_DOCUMENT];
            var documentTitle = (string)data.Data[INPUT_DOCUMENTTITLE];
            var recipients = (RecipientTabMapping[])data.Data[INPUT_RECIPIENTS];
            var subject = (string)data.Data[INPUT_SUBJECT];
            var emailBlurb = (string)data.Data[INPUT_EMAILBLURB];

            Dictionary<string, object> resultData = new Dictionary<string, object>();

            try
            {
                using (var scope = new System.ServiceModel.OperationContextScope(dsClient.InnerChannel))
                {

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = DSServiceClientFactory.GetAuthHeaderRequestProperty(creds);

                    var dsRecipients = new List<Recipient>();
                    var tabs = new List<Tab>();
                    var recipientsList = recipients.ToList();

                    //only supporting a single document for now
                    var documents = new Document[]{
                        new Document {
                            PDFBytes = document,
                            ID = "1",
                            Name = documentTitle
                        }
                    };

                    //each recipient has a list of tabs; this is represented by the RecipientTabMapping type
                    var recipientIndex = 1; //RecipientID must be a non-negative integer; using the index of each Recipient in the list for simplicity
                    foreach (var rtm in recipientsList)
                    {
                        dsRecipients.Add(new Recipient
                        {
                            Email = rtm.EmailAddress,
                            UserName = rtm.EmailAddress,
                            Type = RecipientTypeCode.Signer,
                            RoutingOrder = (ushort)rtm.RoutingOrder,
                            RoutingOrderSpecified = rtm.RoutingOrder > 0,
                            ID = recipientIndex.ToString()
                        });

                        //add a Tab to the list for each of the RTM's simplified tab objects, setting the RecipientID and DocumentID to match current recipient and document
                        //first do absolutely positioned tabs (normal Tab)
                        if (rtm.AbsolutePositionTabs != null)
                        {
                            foreach (var apt in rtm.AbsolutePositionTabs)
                            {
                                tabs.Add(new Tab
                                {
                                    PageNumber = apt.PageNumber.ToString(),
                                    XPosition = apt.XPosition.ToString(),
                                    YPosition = apt.YPosition.ToString(),
                                    Type = apt.TabType,
                                    RecipientID = recipientIndex.ToString(),
                                    DocumentID = "1"
                                });
                            }
                        };
                        //then do AnchorTabs
                        if (rtm.AnchorStringTabs != null)
                        {
                            foreach (var ast in rtm.AnchorStringTabs)
                            {
                                tabs.Add(new Tab
                                {
                                    PageNumber = ast.PageNumber.ToString(),
                                    AnchorTabItem = new AnchorTab { AnchorTabString = ast.AnchorTabString, XOffset = ast.XOffset, YOffset = ast.YOffset },
                                    Type = ast.TabType,
                                    RecipientID = recipientIndex.ToString(),
                                    DocumentID = "1"
                                });
                            }
                        };
                        recipientIndex++;
                    };

                    //construct the envelope and send; return EnvelopeID
                    var envelopeID = dsClient.CreateAndSendEnvelope(new Envelope
                    {
                        Subject = subject,
                        EmailBlurb = emailBlurb,
                        Recipients = dsRecipients.ToArray<Recipient>(),
                        AccountId = creds.AccountId,
                        Documents = documents,
                        Tabs = tabs.ToArray()
                    }).EnvelopeID;

                   
                    resultData.Add(OUTPUT_ENVELOPEID, envelopeID);
                    return new ResultData(OUTCOME_SENT, resultData);
                }
            }
            catch (Exception ex)
            {
                resultData.Add(OUTPUT_ERRORMESSAGE, ex.Message);
                return new ResultData(OUTCOME_ERROR, resultData);
            }
        }

        #endregion
    }
}
