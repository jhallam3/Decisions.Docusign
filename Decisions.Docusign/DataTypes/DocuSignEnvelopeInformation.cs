using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionsFramework;
using Decisions.Docusign.DSServiceReference;
using System.Runtime.Serialization;
using DecisionsFramework.Utilities.validation.Rules;

namespace Decisions.Docusign.DataTypes
{

    [System.Xml.Serialization.XmlRoot(ElementName = "DocuSignEnvelopeInformation")]
    public class DocuSignEnvelopeInformation
    {



        /// <remarks/>
        public EnvelopeStatus EnvelopeStatus { get; set; }

        public DocumentPDF[] DocumentPDFs { get; set; }

        /// <remarks/>
        public string TimeZone { get; set; }


        /// <remarks/>
        public sbyte TimeZoneOffset { get; set; }

    }


}
