using SQLite;

namespace FedPet.Data;

public class FeedingRecord
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int PetId { get; set; }
    public DateTime FeedingTime { get; set; }
}