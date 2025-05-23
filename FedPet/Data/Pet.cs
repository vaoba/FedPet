using SQLite;

namespace FedPet.Data;

public class Pet
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public string? Name { get; set; }
    public int NotificationIntervalDays { get; set; }
    public int NotificationIntervalHours { get; set; }
    public int NotificationIntervalMinutes { get; set; }
    public int NotifyAtMinutes { get; set; } = 60;
    public bool NotifyAtAm { get; set; }
    public bool NotificationEnabled { get; set; }
    public int Order { get; set; }
    public DateTime? NextFeeding { get; set; }
}