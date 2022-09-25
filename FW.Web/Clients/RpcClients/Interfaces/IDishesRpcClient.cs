﻿using FW.BusinessLogic.Contracts;
using FW.Domain.Models;
using FW.Web.Clients.Interfaces;
namespace FW.Web.RpcClients.Interfaces
{
    public interface IDishesRpcClient : IBaseClient<DishVM, DishResponseVM>
    {
    }
}