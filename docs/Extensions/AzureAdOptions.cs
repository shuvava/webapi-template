namespace WebAppBase.Extensions
{
    /// <summary>
    ///     Settings relative to the AzureAD applications involved in this Web Application
    ///     These are deserialized from the AzureAD section of the appsettings.json file
    /// </summary>
    public class AzureAdOptions
    {
        /// <summary>
        ///     ClientId (Application Id) of this Web Application
        /// </summary>
        public string ClientId { get; set; }


        /// <summary>
        ///     Azure AD Cloud instance
        /// </summary>
        public string Instance { get; set; }


        /// <summary>
        ///     Tenant Id, as obtained from the Azure portal:
        ///     (Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs)
        /// </summary>
        public string TenantId { get; set; }


        public string Audiences { get; set; }


        /// <summary>
        ///     IssuerUrl delivering the token for your tenant
        /// </summary>
        public string IssuerUrl => $"{Instance}{TenantId}";
    }
}
