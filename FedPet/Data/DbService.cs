using SQLite;

namespace FedPet.Data;

public class DbService
{
    private static string DbPath => Path.Combine(FileSystem.AppDataDirectory, "data.db");
    private readonly SQLiteAsyncConnection _db;

    public DbService()
    {
        _db = new SQLiteAsyncConnection(DbPath);
        _db.CreateTableAsync<Pet>().Wait();
        _db.CreateTableAsync<FeedingRecord>().Wait();
    }

    //PET ENDPOINTS
    public async Task<int> PostPetAsync(Pet pet)
    {
        List<Pet> pets = await GetPetsAsync();
        // pet.NotifyAtMinutes = 60;
        pet.Order = pets.Count;
        return await _db.InsertAsync(pet);
    }
    public async Task<Pet> GetPetAsync(int id)
    {
        return await _db.Table<Pet>().Where(p => p.Id == id).FirstOrDefaultAsync();
    }
    public async Task<int> UpdatePetAsync(Pet pet)
    {
        return await _db.UpdateAsync(pet);
    }

    public async Task<int> UpdatePetListAsync(List<Pet> pets)
    {
        return await _db.UpdateAsync(pets);
    }
    public async Task<int> DeletePetAsync(Pet pet)
    {
        //delete all feeding records tied to pet being deleted
        List<FeedingRecord> records = await GetFeedingRecordsAsync(pet.Id);
        foreach (FeedingRecord record in records)
        {
            await DeleteFeedingRecordAsync(record);
        }
        //reorder down all pets with higher order number than the one being deleted
        List<Pet> pets = await GetPetsAsync();
        int index = pets.IndexOf(pet);
        if (index + 1 < pets.Count)
        {
            for (int i = index + 1; i < pets.Count; i++)
            {
                pets[i].Order -= 1;
                await UpdatePetAsync(pets[i]);
            }
        }
        
        return await _db.DeleteAsync(pet);
    }
    public async Task<List<Pet>> GetPetsAsync()
    {
        List<Pet> pets = await _db.Table<Pet>().ToListAsync();
        pets = pets.OrderBy(p => p.Order).ToList();
        return pets;
    }
    
    //FEEDINGRECORD ENDPOINTS
    public async Task<List<FeedingRecord>> GetFeedingRecordsAsync(int petId)
    {
        return await _db.Table<FeedingRecord>().Where(f => f.PetId == petId).ToListAsync();
    }

    public async Task<int> PostFeedingRecordAsync(FeedingRecord record)
    {
        return await _db.InsertAsync(record);
    }

    public async Task<int> DeleteFeedingRecordAsync(FeedingRecord record)
    {
        return await _db.DeleteAsync(record);
    }
}