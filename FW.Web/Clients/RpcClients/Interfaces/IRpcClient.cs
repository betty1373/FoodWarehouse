namespace FW.Web.RpcClients.Interfaces
{
    public interface IRpcClient : IDisposable
    {
        public Task<string> CallAsync(string exchangeName, string queueName, string message);
    }
}
