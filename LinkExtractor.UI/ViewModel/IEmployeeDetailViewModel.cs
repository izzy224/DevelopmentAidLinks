using System.Threading.Tasks;

namespace LinkExtractor.UI.ViewModel
{
    public interface IEmployeeDetailViewModel
    {
        Task LoadAsync(int friendId);
    }
}