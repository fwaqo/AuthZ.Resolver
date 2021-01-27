using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using IdentityModel;
using IdentityModel.Client;

namespace Authorization.Models
{
    public class PrincipalAuthorizationsRequest : HttpRequestMessage
    {
        public PrincipalAuthorizationsRequest()
        {
            Headers.Accept.Clear();
            Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Address 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the client assertion.
        /// </summary>
        /// <value>
        /// The assertion.
        /// </value>
        public void Prepare()
        {
            if (!string.IsNullOrEmpty(Address))
            {
                RequestUri = new Uri(Address ?? "");
            }
        }
    }
}