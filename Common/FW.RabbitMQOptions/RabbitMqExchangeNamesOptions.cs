namespace FW.RabbitMQOptions
{
    public class RabbitMqExchangeNamesOptions
    {
        public const string KeyValue = "RabbitMQ:ExchangeNames";

        public string ChangesProducts { get; set; }
        public string Dishes { get; set; }
        public string Ingredients { get; set; }
        public string Recipes { get; set; }
        public string Warehouses { get; set; }
    }
}
