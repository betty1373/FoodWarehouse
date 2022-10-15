using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Net.Http;
using FW.Domain;
using IdentityModel;
using IdentityModel.Client;
using FW.WPF.ViewModels;

namespace FW.WPF.Identity.Interfaces;

public interface IClientIdentity<T> where T : class,IUserIdentity
{ 
    Task<string?> RequestPasswordTokenAsync(T model, CancellationToken Cancel = default);
}
