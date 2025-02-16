using _Microsoft.Android.Resource.Designer;
using Android.App;
using Android.Content;
using Android.Graphics;

namespace FedPet;

[BroadcastReceiver(Enabled = true, Exported = false)]
public class AlarmHandler : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        if (intent?.Extras == null) return;
        var title = intent.GetStringExtra(NotificationManagerService.TitleKey);
        var message = intent.GetStringExtra(NotificationManagerService.MessageKey);
        var petId = intent.GetIntExtra(NotificationManagerService.PetIdKey, 0);

        if (context != null)
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.PutExtra(NotificationManagerService.PetIdKey, petId);

            var pendingIntent = PendingIntent.GetActivity(context, petId, notificationIntent,
                PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            var notificationBuilder = new Notification.Builder(context, NotificationManagerService.ChannelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(ResourceConstant.Drawable.mini)
                .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, ResourceConstant.Drawable.full))
                .SetContentIntent(pendingIntent)
                .SetAutoCancel(true);
        
            var manager = (NotificationManager?)context.GetSystemService(Context.NotificationService);
            manager?.Notify(petId, notificationBuilder.Build());
        }
    }
}