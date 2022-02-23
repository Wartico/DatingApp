namespace API.Models
{
    public class CreateMessageRequest
    {
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
    }
}
