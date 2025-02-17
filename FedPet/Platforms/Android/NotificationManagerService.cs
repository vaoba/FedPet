using Android.App;
using Android.Content;
using Android.OS;
using FedPet.Data;

namespace FedPet;

public class NotificationManagerService
{
    public const string ChannelId = "fedpet";
    private const string ChannelName = "FedPet";
    private const string ChannelDescription = "FedPet notification channel.";
    
    public const string TitleKey = "title";
    public const string MessageKey = "message";
    public const string PetIdKey = "petId";

    private bool _channelInitialized;

    public NotificationManagerService()
    {
        CreateNotificationChannel();
    }
    
    public void SendNotification(Pet pet, DateTime notificationTime)
    {
        if (!_channelInitialized) CreateNotificationChannel();

        Intent intent = new Intent(Platform.AppContext, typeof(AlarmHandler));
        intent.PutExtra(TitleKey, "Feeding reminder");
        intent.PutExtra(MessageKey, $"Don't forget to feed {pet.Name}.");
        intent.PutExtra(PetIdKey, pet.Id);
        intent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);
        
        var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.CancelCurrent;
        
        PendingIntent? pendingIntent = PendingIntent.GetBroadcast(Platform.AppContext, pet.Id, intent, pendingIntentFlags);
        long triggerTime = GetNotifyTime(notificationTime);
        AlarmManager? alarmManager = Platform.AppContext.GetSystemService(Context.AlarmService) as AlarmManager;
        if (pendingIntent != null) alarmManager?.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerTime, pendingIntent);
    }

    public void DeleteNotification(int id)
    {
        Intent intent = new Intent(Platform.AppContext, typeof(AlarmHandler));
        
        var pendingIntentFlags = (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            ? PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.CancelCurrent;
        
        PendingIntent? pendingIntent = PendingIntent.GetBroadcast(Platform.AppContext, id, intent, pendingIntentFlags);
        AlarmManager? alarmManager = Platform.AppContext.GetSystemService(Context.AlarmService) as AlarmManager;
        if (pendingIntent != null) alarmManager?.Cancel(pendingIntent);
    }
    
    void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channelNameJava = new Java.Lang.String(ChannelName);
            var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
            {
                Description = ChannelDescription
            };

            var manager = (NotificationManager?)Platform.AppContext.GetSystemService(Context.NotificationService);
            manager?.CreateNotificationChannel(channel);
            
            if (manager is not null ) _channelInitialized = true;
        }
    }
    
    // UTC time starts at 1970 1 1, DateTime starts at 1 1 0, converts datetime to UTC because that's what android understands
    long GetNotifyTime(DateTime notifyTime)
    {
        DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
        double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
        long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
        return utcAlarmTime; // milliseconds
    }
    
}