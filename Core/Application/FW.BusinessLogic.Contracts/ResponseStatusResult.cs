namespace FW.BusinessLogic.Contracts
{
    public class ResponseStatusResult
    {
        public Guid Id { get; set; }
        public StatusResult Status { get; set; }
        public string Title { get; set; }
    }
}
