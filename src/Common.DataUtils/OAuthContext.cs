﻿using Common.DataUtils.Http;
using Microsoft.Extensions.Logging;

namespace Common.DataUtils
{
    /// <summary>
    /// Context for Graph calls
    /// </summary>
    public class GraphAppIndentityOAuthContext : ImportAppIndentityOAuthContext
    {
        public GraphAppIndentityOAuthContext(ILogger telemetry, string clientId, string tenantId, string clientSecret, string keyVaultUrl, bool useClientCertificate) :
            base(telemetry, clientId, tenantId, clientSecret, keyVaultUrl, useClientCertificate)
        { }

        public override string ResourceURL => "https://graph.microsoft.com/.default";
    }

    /// <summary>
    /// Context for Activity API calls
    /// </summary>
    public class ActivityAPIAppIndentityOAuthContext : ImportAppIndentityOAuthContext
    {
        public ActivityAPIAppIndentityOAuthContext(ILogger telemetry, string clientId, string tenantId, string clientSecret, string keyVaultUrl, bool useClientCertificate) :
            base(telemetry, clientId, tenantId, clientSecret, keyVaultUrl, useClientCertificate)
        { }

        public override string ResourceURL => "https://manage.office.com/.default";
    }
}
