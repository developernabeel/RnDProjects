using SBIReportUtility.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SBIReportUtility.Web.AuthenticationExt
{
    public class Authentication : IAuthentication
    {
        public Authentication()
        {

        }

        public IIdentity Identity { get; set; }

        public Authentication(string email)
        {
            this.Identity = new GenericIdentity(email);
        }

        public UserModel UserContext
        {
            get;
            set;
        }

        public bool IsInRole(string Role)
        {
            throw new NotImplementedException();
        }
    }
}