namespace FW.RabbitMQOptions
{
	public class RabbitMqConnectionOptions
	{
		public const string KeyValue = "RabbitMQ:Connection";

		public string ClientName { get; set; }
		public string HostName { get; set; }
		public string VirtualHost { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}
