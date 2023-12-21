using MimeKit;

namespace API.Service.Email;

public class Message
{
    public Message(IEnumerable<string> to, string email, string subject, string content)
    {
        To = new List<MailboxAddress>();
        To.AddRange(to.Select(x => new MailboxAddress(x, email)));
        Subject = subject;
        Content = content;        
    }
    
    public List<MailboxAddress> To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}