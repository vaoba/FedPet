namespace FedPet;

public class NotificationEventArgs : EventArgs
{
    public string Title { get; set; }
    public string Message { get; set; }
    public int PetId { get; set; }
}