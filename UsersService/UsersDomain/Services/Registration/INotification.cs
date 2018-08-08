using System.Threading.Tasks;

namespace UsersDomain.Services.Registration
{
    public interface INotification
    {
        Task NotifyAsync(UserDto data);
    }
}
