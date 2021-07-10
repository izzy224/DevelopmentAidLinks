using System.Threading.Tasks;

namespace LinkExtractor.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges { get; }
    }
}