namespace Mistake3.TransferServices;

public class MessagingTransferService
{
    public int Counter { get; set; }
    public event EventHandler MessageReceived = (sender, args) => { };
}
