using LinkExtractor.Models;
using LinkExtractor.UI.DataServices.Repositories;
using LinkExtractor.UI.View.Services;
using LinkExtractor.UI.Wrapper;
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
    public class WorkshiftDetailViewModel : DetailViewModelBase, IWorkshiftDetailViewModel
    {
        private IWorkshiftRepository _workshiftRepository;
        private WorkshiftWrapper _workshift;
        private IMessageDialogService _messageDialogService;

        private Employee _selectedAvailableEmployee;
        private Employee _selectedAddedEmployee;
        private List<Employee> _allEmployees;

        public WorkshiftDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IWorkshiftRepository workshiftRepository) : base(eventAggregator)
        {
            _workshiftRepository = workshiftRepository;
            _messageDialogService = messageDialogService;
            AddedEmployees = new ObservableCollection<Employee>();
            AvailableEmployees = new ObservableCollection<Employee>();
            AddEmployeeCommand = new DelegateCommand(OnAddEmployeeExecute, OnAddEmployeeCanExecute);
            RemoveEmployeeCommand = new DelegateCommand(OnRemoveEmployeeExecute, OnRemoveEmployeeCanExecute);
        }



        public WorkshiftWrapper Workshift 
        {
            get { return _workshift; }
            private set 
            {
                _workshift = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Employee> AddedEmployees { get; }
        public ObservableCollection<Employee> AvailableEmployees { get; }
        public ICommand AddEmployeeCommand { get; }
        public ICommand RemoveEmployeeCommand { get; }

        public Employee SelectedAvailableEmployee
        {
            get { return _selectedAvailableEmployee; }
            set 
            { 
                _selectedAvailableEmployee = value;
                OnPropertyChanged();
                ((DelegateCommand)AddEmployeeCommand).RaiseCanExecuteChanged();
            }
        }
        public Employee SelectedAddedEmployee
        {
            get { return _selectedAddedEmployee; }
            set 
            {
                _selectedAddedEmployee = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveEmployeeCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? workshiftId)
        {
            var workshift = workshiftId.HasValue
                ? await _workshiftRepository.GetByIdAsync(workshiftId.Value)
                : CreateNewWorkshift();

            InitializeWorkshift(workshift);

           _allEmployees = await _workshiftRepository.GetAllEmployeesAsync();
            SetupPicklist();
        }

        private void SetupPicklist()
        {
            var workshiftEmployeeIds = Workshift.Model.Employees.Select(e => e.Id).ToList();
            var addedEmployees = _allEmployees.Where(e => workshiftEmployeeIds.Contains(e.Id)).OrderBy(e => e.Id);
            //Left off here
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you want to delete this workshift?","Confirm delete");
            if(result == MessageDialogResult.Ok)
            {
                _workshiftRepository.Remove(Workshift.Model);
                _workshiftRepository.SaveAsync();
                RaiseDetailDeletedEvent(Workshift.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Workshift != null && !Workshift.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _workshiftRepository.SaveAsync();
            HasChanges = _workshiftRepository.HasChanges();
            RaiseDetailSavedEvent(Workshift.Id, Workshift.Date.ToShortDateString());
        }

        private Workshift CreateNewWorkshift()
        {
            var workshift = new Workshift()
            {
                Date = DateTime.Now
            };
            _workshiftRepository.Add(workshift);
            return workshift;
        }

        private void InitializeWorkshift(Workshift workshift)
        {
            Workshift = new WorkshiftWrapper(workshift);
            Workshift.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _workshiftRepository.HasChanges();
                }
                if(e.PropertyName == nameof(Workshift.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();


            if (Workshift.Id == 0)
                Workshift.Date = DateTime.Now;
        }



        private void OnRemoveEmployeeExecute()
        {
            throw new NotImplementedException();
        }

        private bool OnRemoveEmployeeCanExecute()
        {
            return SelectedAddedEmployee != null;
        }

        private bool OnAddEmployeeCanExecute()
        {
            return SelectedAvailableEmployee != null;
        }

        private void OnAddEmployeeExecute()
        {
            throw new NotImplementedException();
        }
    }
}
