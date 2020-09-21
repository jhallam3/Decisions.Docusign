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
    public class SimpleTab
    {
        [DataMember]
        public int XPosition { get; set; }

        [DataMember]
        public int YPosition { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public TabTypeCode TabType { get; set; }

        public override string ToString()
        {
            return string.Format("{0} on page {1} at {2},{3}", TabType.ToString(), PageNumber, XPosition, YPosition);
        }

    }
}
