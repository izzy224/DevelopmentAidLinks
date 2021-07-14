using System.Threading.Tasks;

namespace LinkExtractor.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id, string data);
        bool HasChanges { get; }
    }
}