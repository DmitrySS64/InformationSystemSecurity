namespace InformationSystemSecurity.domain.Models;

public record AssociatedData
{
    public string SecurityLevel { get; init; }
    public string Sender { get; init; }
    public string Receiver { get; init; }
    public string SessionId { get; init; }

    public AssociatedData(string securityLevel, string sender, string receiver, string sessionId)
    {
        if (securityLevel.Length != 2)
            throw new ArgumentException("SecurityLevel must be exactly 2 characters (10 bits: 5 bits type + 5 bits subtype)", nameof(securityLevel));
        
        if (sender.Length != 8)
            throw new ArgumentException("Sender must be exactly 8 characters (40 bits)", nameof(sender));
        
        if (receiver.Length != 8)
            throw new ArgumentException("Receiver must be exactly 8 characters (40 bits)", nameof(receiver));
        
        if (sessionId.Length != 9)
            throw new ArgumentException("SessionId must be exactly 9 characters (45 bits)", nameof(sessionId));
        
        SecurityLevel = securityLevel;
        Sender = sender;
        Receiver = receiver;
        SessionId = sessionId;
    }
}