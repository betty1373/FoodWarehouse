namespace FW.RabbitMQOptions;

public class RabbitMqQueueNamesOptions
{
    public const string KeyValue = "RabbitMQ:QueueNames";

    public QueueNamesWithGetByParentId ChangesProducts { get; set; }
    public QueueNamesDishes Dishes { get; set; }
    public QueueNames Ingredients { get; set; }
    public QueueNamesWithGetByParentId Products { get; set; }
    public QueueNamesWithGetByParentId Recipes { get; set; }
    public QueueNamesWithGetByParentId Warehouses { get; set; }
}
public class QueueNames
{
    public string Get { get; set; }
    public string GetPage { get; set; }
    public string GetAll { get; set; }
    public string Count { get; set; }
    public string Create { get; set; }
    public string Update { get; set; }
    public string Delete { get; set; }

    public virtual string[] AllNames
    { 
        get 
        {
            return new string[] { Get, GetPage, GetAll, Count, Create, Update, Delete }; 
        }
    }
}
public class QueueNamesWithGetByParentId : QueueNames
{
    public string GetByParentId { get; set; }
    public override string[] AllNames
    {
        get
        {
            return new string[] { Get, GetByParentId, GetPage, GetAll, Count, Create, Update, Delete };
        }
    }
}
public class QueueNamesDishes : QueueNamesWithGetByParentId
{
    public string Cook { get; set; }
    public override string[] AllNames
    {
        get
        {
            return new string[] { Get, GetByParentId, GetPage, GetAll, Count, Create, Update, Delete, Cook };
        }
    }
}
