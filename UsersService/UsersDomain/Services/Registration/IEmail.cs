using System.Threading.Tasks;

namespace UsersDomain.Services.Registration
{
    public interface IEmail
    {
        Task SendAsync(EmailDto data);
    }
}
