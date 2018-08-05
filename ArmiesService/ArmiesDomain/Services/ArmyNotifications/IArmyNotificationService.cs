using System.Threading.Tasks;

namespace ArmiesDomain.Services.ArmyNotifications
{
    public interface IArmyNotificationService
    {
        Task NotifyThatCreatedAsync(ArmyNotificationDto data);
    }
}
