using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FW.WPF.Domain.Exceptions;

public class IdentityServerNotFoundException : Exception
{
    public string IdentityServer { get; set; }

    public IdentityServerNotFoundException(string server)
    {
        IdentityServer = server;
    }

    public IdentityServerNotFoundException(string message, string server) : base(message)
    {
        IdentityServer = server;
    }

    public IdentityServerNotFoundException(string message, Exception innerException, string server) : base(message, innerException)
    {
        IdentityServer = server;
    }
}
