using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace LinkExtractor.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        public MainViewModel(INavigationViewModel navigationViewModel, IEmployeeDetailViewModel employeeDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            EmployeeDetailViewModel = employeeDetailViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }
        public IEmployeeDetailViewModel EmployeeDetailViewModel { get; }
    }
}


