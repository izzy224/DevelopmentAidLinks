using LinkExtractor.UI.Events;
using LinkExtractor.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkExtractor.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private IDetailViewModel _detailViewModel;
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
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<DetailDeletedEvent>()
                .Subscribe(DetailDeleted);

            AddNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            NavigationViewModel = navigationViewModel;
        }



        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand AddNewDetailCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set { _detailViewModel = value; OnPropertyChanged(); }
        }


        private Func<IEmployeeDetailViewModel> _employeeDetailViewModelCreator;
        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if(DetailViewModel!=null && DetailViewModel.HasChanges)
            {
                var res = _messageDialogService.ShowOkCancelDialog("Changes have been made. Navigate away?", "Question");

                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            switch(args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    DetailViewModel = _employeeDetailViewModelCreator();
                    break;
            }
            
            await DetailViewModel.LoadAsync(args.Id);
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs() { ViewModelName = viewModelType.Name});
        }

        private void DetailDeleted(DetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }
    }
}


