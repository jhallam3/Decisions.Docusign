using DecisionsFramework.Design.ConfigurationStorage.Attributes;
using DecisionsFramework.Design.Flow.Service.Debugging.DebugData;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.Design.Properties.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Docusign
{
    public interface IDocusignCreds
    {
        string UserName { get; }
        string LoginEmail { get; }
        string AccountId { get; }
        string Password { get; }
        string IntegratorKey { get; }
        bool UseDemoEnvironment { get; }
    }

    [Writable]
    [DataContract]
    public class DocusignCredentials : IDocusignCreds, IDebuggerJsonProvider
    {
        [WritableValue]
        private string userName;

        [WritableValue]
        private string accountId;

        [WritableValue]
        private string password;

        [WritableValue]
        private string loginEmail;

        [WritableValue]
        private string integratorKey;

        [WritableValue]
        private bool useDemoEnvironment;

        [PropertyClassification(new[] { "Docusign Credentials" }, 1)]
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 1)]
        [DataMember]
        public string LoginEmail
        {
            get { return loginEmail; }
            set { loginEmail = value; }
        }


        [PropertyClassification(new[] { "Docusign Credentials" }, 1)]
        [DataMember]
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 2)]
        [PasswordText]
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 2)]
        [PasswordText]
        [DataMember]
        public string IntegratorKey
        {
            get { return integratorKey; }
            set { integratorKey = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 3)]
        [DataMember]
        public bool UseDemoEnvironment
        {
            get { return useDemoEnvironment; }
            set { useDemoEnvironment = value; }
        }

        public object GetJsonDebugView()
        {
            return new
            {
                UserName = this.UserName,
                LoginEmail = this.LoginEmail,
                AccountId = this.AccountId,
                Password = "********",
                IntegratorKey = "********",
                UseDemoEnvironment = this.UseDemoEnvironment
            };
        }
    }
}
