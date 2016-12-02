using System.Threading.Tasks;

namespace LaunchPal.Interface
{
    public interface ISendMail
    {
        Task SendMail(string title, string message);
    }
}
