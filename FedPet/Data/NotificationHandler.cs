namespace FedPet.Data;

public static class NotificationHandler
{
    public static bool PermissionsGranted = false;
    private static readonly NotificationManagerService NotificationManager = new();
    public static bool NavigateAfterNotification { get; set; }
    public static string NavigateAfterNotificationRoute { get; set; } = string.Empty;
    
    // NOTIFICATION METHODS
    public static async Task CreateNotification(DbService db, Pet pet, DateTime timeFrom)
    {
        // SKIP IF NOTIFICATION PERMISSIONS ARE NOT GRANTED
        if (!PermissionsGranted)
        {
            await Task.CompletedTask;
            return;
        }
        
        // CANCEL EXISTING NOTIFICATION, SET UP NEW ONE
        NotificationManager.DeleteNotification(pet.Id);

        DateTime target;
        
        // IF DAY INTERVAL SETUP DAILY NOTIF, DATETIME.NOW.DATE + DAYS + 12 HOURS IF PM 
        if (pet.NotificationIntervalHours == 0 && pet.NotificationIntervalMinutes == 0)
        {
            target = timeFrom.Date.AddDays(pet.NotificationIntervalDays)
                .AddMinutes(pet.NotifyAtMinutes);
            if (!pet.NotifyAtAm)
                target = target.AddHours(12);
        }
        
        // ELSE DATETIME.NOW + HOURS & MINUTES IN MINUTE FORMAT
        else
        {
            int minutes = pet.NotificationIntervalHours * 60 + pet.NotificationIntervalMinutes;
            target = timeFrom.AddMinutes(minutes);
        }

        if (target > DateTime.Now)
        {
            pet.NextFeeding = target;
            NotificationManager.SendNotification(pet, target);
        }
        else
            pet.NextFeeding = null;
        
        await db.UpdatePetAsync(pet);

        await Task.CompletedTask;
    }

    public static async Task CancelNotification(DbService db, Pet pet)
    {
        NotificationManager.DeleteNotification(pet.Id);
        pet.NextFeeding = null;
        await db.UpdatePetAsync(pet);
    }
}