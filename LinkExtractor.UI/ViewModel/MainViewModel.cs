using LinkExtractor.UI.Events;
using LinkExtractor.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace LinkExtractor.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private IEmployeeDetailViewModel _employeeDetailViewModel;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IEmployeeDetailViewModel> employeeDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _employeeDetailViewModelCreator = employeeDetailViewModelCreator;
            _eventAggregator.GetEvent<OpenEmployeeDetailViewEvent>()
                .Subscribe(OnOpenFriendDetailView);

            NavigationViewModel = navigationViewModel;
        }



        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        

        public INavigationViewModel NavigationViewModel { get; }
        

        public IEmployeeDetailViewModel EmployeeDetailViewModel
        {
            get { return _employeeDetailViewModel; }
            private set { _employeeDetailViewModel = value; OnPropertyChanged(); }
        }


        private Func<IEmployeeDetailViewModel> _employeeDetailViewModelCreator;
        private async void OnOpenFriendDetailView(int employeeId)
        {
            if(EmployeeDetailViewModel!=null && EmployeeDetailViewModel.HasChanges)
            {
                var res = _messageDialogService.ShowOkCancelDialog("Changes have been made. Navigate away?", "Question");

                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            EmployeeDetailViewModel = _employeeDetailViewModelCreator();
            await EmployeeDetailViewModel.LoadAsync(employeeId);
        }


    }
}


