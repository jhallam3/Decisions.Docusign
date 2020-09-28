using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecisionsFramework.Data.ORMapper;
using DecisionsFramework.Design.Properties;
using DecisionsFramework.Design.Properties.Attributes;
using DecisionsFramework.ServiceLayer;
using DecisionsFramework.ServiceLayer.Actions;
using DecisionsFramework.ServiceLayer.Actions.Common;
using DecisionsFramework.ServiceLayer.Utilities;

namespace Decisions.Docusign
{
    
    public class DocusignSettings : AbstractModuleSettings, IInitializable, IDocusignCreds
    {
        [ORMField]
        private string userName;

        [ORMField]
        private string accountId;

        [ORMField]
        private string password;

        [ORMField]
        private string loginEmail;

        [ORMField]
        private string integratorKey;

        [ORMField]
        private bool useDemoEnvironment;        
       
        public DocusignSettings()
        {
            this.EntityName = "Docusign Settings";
        }

        [PropertyClassification(new [] {"Docusign Credentials"}, 1)]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 1)]
        public string LoginEmail
        {
            get { return loginEmail; }
            set { loginEmail = value; }
        }


        [PropertyClassification(new[] { "Docusign Credentials" }, 1)]
        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 2)]
        [PasswordText]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 2)]
        [PasswordText]
        public string IntegratorKey
        {
            get { return integratorKey; }
            set { integratorKey = value; }
        }

        [PropertyClassification(new[] { "Docusign Credentials" }, 3)]
        public bool UseDemoEnvironment
        {
            get { return useDemoEnvironment; }
            set { useDemoEnvironment = value; }
        }

        public override BaseActionType[] GetActions(AbstractUserContext userContext, EntityActionType[] types)
        {
            return new BaseActionType[] { new EditEntityAction(typeof(DocusignSettings), "Edit", "") { IsDefaultGridAction = true } };
        }

        public void Initialize()
        {
            // this will create it
            ModuleSettingsAccessor<DocusignSettings>.GetSettings();
        }

        public void SetDocusignSettings(DocusignSettings settings)
        {

            settings.Store();


        }


        public DocusignSettings GetDocusignSettings()
        {

            return DSServiceClientFactory.DsSettings;

        }



        //   public DocusignAPI.
    }
}
