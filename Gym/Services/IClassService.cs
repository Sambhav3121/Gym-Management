using System.Threading.Tasks;

namespace Gym.Services
{
    public interface IClassService
    {
        Task<int> CountAvailableAsync();
    }
}
