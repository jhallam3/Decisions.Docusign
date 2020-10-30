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
    [DataContract]
    [ValidationRules]
    public class RecipientTabMapping: IValidationSource
    {
        [DataMember]
        [EmptyStringRule]
        public string EmailAddress { get; set; }

        [DataMember]
        public string SignerNameField { get; set; }

        [DataMember]
        public string UserNameField { get; set; }

        [DataMember]
        public int RoutingOrder { get; set; }

        [DataMember]
        public SimpleTab[] AbsolutePositionTabs { get; set; }       

        [DataMember]
        public SimpleAnchorTab[] AnchorStringTabs { get; set; }

        public override string ToString()
        {
            return EmailAddress;
        }

        public ValidationIssue[] GetValidationIssues()
        {
            return null;
        }
    }
}
