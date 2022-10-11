using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net.Http;
using FW.WPF.Models;
using FW.WPF.Identity.Clients.Base;
namespace FW.WPF.Identity.Clients;

public class ClientIdentity : ClientIdentityBase<LoginModel>
{  
    public ClientIdentity(HttpClient Client):base (Client,"")
    { }
}
