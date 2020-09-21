using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decisions.Docusign.DSServiceReference;
using System.Runtime.Serialization;

namespace Decisions.Docusign.DataTypes
{
    [DataContract]
    public class SimpleAnchorTab
    {
        [DataMember]
        public string AnchorTabString { get; set; }

        [DataMember]
        public int XOffset { get; set; }

        [DataMember]
        public int YOffset { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public TabTypeCode TabType { get; set; }

        public override string ToString()
        {
            return string.Format("{0} at {1} on page {2}, offset {3},{4}", TabType.ToString(), AnchorTabString, PageNumber, XOffset, YOffset);
        }
    }
}
