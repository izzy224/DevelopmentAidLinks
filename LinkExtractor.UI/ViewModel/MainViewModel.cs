using Autofac.Features.Indexed;
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
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string,IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _detailViewModelCreator = detailViewModelCreator;
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<DetailDeletedEvent>()
                .Subscribe(DetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            NavigationViewModel = navigationViewModel;
        }



        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewDetailCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set { _detailViewModel = value; OnPropertyChanged(); }
        }



        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if(DetailViewModel!=null && DetailViewModel.HasChanges)
            {
                var res = await _messageDialogService.ShowOkCancelDialogAsync("Changes have been made. Navigate away?", "Question");

                if (res == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            //switch(args.ViewModelName)
            //{
            //    case nameof(EmployeeDetailViewModel):
            //        DetailViewModel = _employeeDetailViewModelCreator();
            //        break;
            //    case nameof(WorkshiftDetailViewModel):
            //        DetailViewModel = _workshiftDetailViewModelCreator();
            //        break;
            //    default:
            //        throw new Exception($"ViewModel {args.ViewModelName} not mapped");
            //}

            DetailViewModel = _detailViewModelCreator[args.ViewModelName];
            await DetailViewModel.LoadAsync(args.Id, args.Data);
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


