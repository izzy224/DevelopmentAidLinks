using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.DataServices.Lookups;
using LinkExtractor.UI.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkExtractor.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IEmployeeLookupDataService _employeeLookupService;
        private IEventAggregator _eventAggregator;
        private IWorkshiftLookupDataService _workshiftLookupService;
        private DateTime _selectedDate;

        public NavigationViewModel(IEmployeeLookupDataService employeeLookupService, IWorkshiftLookupDataService workshiftLookupService,
            IEventAggregator eventAggregator)
        {
            _employeeLookupService = employeeLookupService;
            _eventAggregator = eventAggregator;
            _workshiftLookupService = workshiftLookupService;
            Employees = new ObservableCollection<NavigationItemViewModel>();
            Workshift = new NavigationItemViewModel(0, "", _eventAggregator, nameof(WorkshiftDetailViewModel));
            _selectedDate = DateTime.Today;
            _eventAggregator.GetEvent<DetailSavedEvent>().Subscribe(DetailSaved);
            _eventAggregator.GetEvent<DetailDeletedEvent>().Subscribe(DetailDeleted);
            ChangeWorkshiftCommand = new DelegateCommand(OnChangeWorkshiftExecute);
        }

        private void OnChangeWorkshiftExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Publish(new OpenDetailViewEventArgs() { Id = Workshift.Id, ViewModelName = nameof(WorkshiftDetailViewModel),Data = Workshift.DisplayMember});
        }

        public async Task LoadAsync()
        {
            var lookup = await _employeeLookupService.GetEmployeeLookupAsync();
            Employees.Clear();
            foreach (var item in lookup)
            {
                Employees.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(EmployeeDetailViewModel)));
            }

            UpdateWorkshift();


            //lookup = await _workshiftLookupService.GetWorkshiftLookupAsync();
            //Workshifts.Clear();
            //foreach (var item in lookup)
            //{
            //    Workshifts.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator, nameof(WorkshiftDetailViewModel)));
            //}
        }

        private async void UpdateWorkshift()
        {
            var workshiftLookup = await _workshiftLookupService.GetWorkshiftLookupByDateAsync(SelectedDate);

            if (workshiftLookup != null)
                Workshift = new NavigationItemViewModel(workshiftLookup.Id, workshiftLookup.DisplayMember, _eventAggregator, nameof(WorkshiftDetailViewModel));
            else
                Workshift = new NavigationItemViewModel(0, SelectedDate.ToShortDateString(), _eventAggregator, nameof(WorkshiftDetailViewModel));
        }

        public ObservableCollection<NavigationItemViewModel> Employees { get; }
        public NavigationItemViewModel Workshift { get; set; }
        public ICommand ChangeWorkshiftCommand { get; } 
        
        public DateTime SelectedDate 
        {
            get { return _selectedDate; }
            set 
            { 
                _selectedDate = value;
                UpdateWorkshift();
            }
        }

        private void DetailDeleted(DetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    DetailDeleted(Employees, args);
                    break;

            }

        }

        private void DetailDeleted(ObservableCollection<NavigationItemViewModel> items, DetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(e => e.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void DetailSaved(DetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(EmployeeDetailViewModel):
                    DetailSaved(Employees, args);
                    break;
            }

        }

        private void DetailSaved(ObservableCollection<NavigationItemViewModel> items, DetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(e => e.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }

}
