using Plugin.LocalNotification;

namespace FedPet.Data;

public static class NotificationHandler
{
    // NOTIFICATION METHODS
    public static async Task CreateNotification(DbService db, Pet pet, DateTime timeFrom)
    {
        // CANCEL EXISTING NOTIFICATION, SET UP NEW ONE
        LocalNotificationCenter.Current.Cancel(pet.Id);

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
            var request = new NotificationRequest
            {
                NotificationId = pet.Id,
                Title = "Feeding reminder",
                Description = $"Don't forget to feed {pet.Name}.",
                Schedule = new NotificationRequestSchedule { NotifyTime = target }
            };
                
            pet.NextFeeding = target;
            await LocalNotificationCenter.Current.Show(request);
        }
        else
            pet.NextFeeding = null;
        
        await db.UpdatePetAsync(pet);

        await Task.CompletedTask;
    }

    public static async Task CancelNotification(DbService db, Pet pet)
    {
        LocalNotificationCenter.Current.Cancel(pet.Id);
        pet.NextFeeding = null;
        await db.UpdatePetAsync(pet);
    }
}