using SQLite;

namespace FedPet.Data;

public class FeedingRecord
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    public int PetId { get; init; }
    public DateTime FeedingTime { get; init; }
}