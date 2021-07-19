using Autofac;
using LinkExtractor.Models;
using LinkExtractor.UI.DataServices.Repositories;
using LinkExtractor.UI.Events;
using LinkExtractor.UI.Startup;
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
        private IEmployeeRepository _employeeRepository;
        private IEmployeeWorkshiftRepository _employeeWorkshiftRepository;
        private WorkshiftWrapper _workshift;
        private IMessageDialogService _messageDialogService;

        private Employee _selectedAvailableEmployee;
        private Employee _selectedAddedEmployee;
        private List<Employee> _allEmployees;

        public WorkshiftDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IWorkshiftRepository workshiftRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeWorkshiftRepository employeeWorkshiftRepository
            ) : base(eventAggregator)
        {
            _workshiftRepository = workshiftRepository;
            _employeeRepository = employeeRepository;
            _employeeWorkshiftRepository = employeeWorkshiftRepository;
            _messageDialogService = messageDialogService;
            AddedEmployees = new ObservableCollection<Employee>();
            AvailableEmployees = new ObservableCollection<Employee>();
            AddEmployeeCommand = new DelegateCommand(OnAddEmployeeExecute, OnAddEmployeeCanExecute);
            RemoveEmployeeCommand = new DelegateCommand(OnRemoveEmployeeExecute, OnRemoveEmployeeCanExecute);
            GetTenderSingleCommand = new DelegateCommand(OnGetTenderSingleExecute, OnGetTenderSingleCanExecute);
            GetTenderAllCommand = new DelegateCommand(OnGetTenderAllExecute, OnGetTenderAllCanExecute);
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
        public ICommand GetTenderSingleCommand { get; }
        public ICommand GetTenderAllCommand { get; }

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
                ((DelegateCommand)GetTenderSingleCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? workshiftId, string data = null)
        {
            var workshift = workshiftId.HasValue && workshiftId.Value != 0
                ? await _workshiftRepository.GetByIdAsync(workshiftId.Value)
                : CreateNewWorkshift(data);

            InitializeWorkshift(workshift);

           _allEmployees = await _employeeRepository.GetAllEmployeesAsync();
            SetupPicklist();
        }

        private async void SetupPicklist()
        {
            //var employeeIds = await _employeeRepository.GetAllIdAsync();
            var workshiftEmployeeIds = await _employeeWorkshiftRepository.GetEmployeesIdByWorkshiftAsync(Workshift.Id);
            //var workshiftEmployeeIds = Workshift.Model.Employees.Select(e => e.Id).ToList();
            //var addedEmployees = _allEmployees.Where(e => workshiftEmployeeIds.Contains(e.Id)).OrderBy(e => e.Name);
            //var availableEmployees = _allEmployees.Except(addedEmployees).OrderBy(e => e.Name);
            var addedEmployees = _allEmployees.Where(e => workshiftEmployeeIds.Contains(e.Id)).OrderBy(e => e.Name);
            var availableEmployees = _allEmployees.Except(addedEmployees).OrderBy(e => e.Name);


            AddedEmployees.Clear();
            AvailableEmployees.Clear();

            foreach (var e in addedEmployees)
            {
                AddedEmployees.Add(e);
            }
            foreach(var e in availableEmployees)
            {
                AvailableEmployees.Add(e);
            }
            ((DelegateCommand)GetTenderAllCommand).RaiseCanExecuteChanged();
        }

        protected async override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you want to delete this workshift?","Confirm delete");
            if(result == MessageDialogResult.Ok)
            {
                _workshiftRepository.Remove(Workshift.Model);
                await _workshiftRepository.SaveAsync();
                await _employeeWorkshiftRepository.SaveAsync();
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
            await _employeeWorkshiftRepository.SaveAsync();
            HasChanges = _workshiftRepository.HasChanges() || _employeeWorkshiftRepository.HasChanges();
            RaiseDetailSavedEvent(Workshift.Id, Workshift.Date.ToShortDateString());
        }

        private Workshift CreateNewWorkshift(string data)
        {
            var workshift = new Workshift()
            {
                Date = Convert.ToDateTime(data)
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
            ((DelegateCommand)GetTenderAllCommand).RaiseCanExecuteChanged();

        }



        private async void OnRemoveEmployeeExecute()
        {
            var employeeToRemove = SelectedAddedEmployee;
            var employeeWorkshift = await _employeeWorkshiftRepository.GetByFk(Workshift.Id, employeeToRemove.Id);
            
            _employeeWorkshiftRepository.Remove(employeeWorkshift);
            AvailableEmployees.Add(employeeToRemove);
            AddedEmployees.Remove(employeeToRemove);
            HasChanges = _workshiftRepository.HasChanges() || _employeeWorkshiftRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GetTenderAllCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveEmployeeCanExecute()
        {
            //TODO : Check if employee has tenders
            return SelectedAddedEmployee != null;
        }

        private bool OnAddEmployeeCanExecute()
        {
            return SelectedAvailableEmployee != null;
        }

        private void OnAddEmployeeExecute()
        {
            var employeeToAdd = SelectedAvailableEmployee;

            _employeeWorkshiftRepository.Add(new EmployeeWorkshift() 
            { WorkshiftId = Workshift.Id, EmployeeId = SelectedAvailableEmployee.Id });//, Employee = SelectedAvailableEmployee, Workshift = this.Workshift.Model});
            AddedEmployees.Add(employeeToAdd);
            AvailableEmployees.Remove(employeeToAdd);
            HasChanges = _workshiftRepository.HasChanges() || _employeeWorkshiftRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GetTenderAllCommand).RaiseCanExecuteChanged();
        }


        private bool OnGetTenderAllCanExecute()
        {
            //TODO : Implement more logic!!
            return AddedEmployees.Count>0;
        }

        private void OnGetTenderAllExecute()
        {
            throw new NotImplementedException();
            //Publish an event with the data needed -- maybe an foreach for every selected employee
        }

        private async void OnGetTenderSingleExecute()
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();
            var tenderParser = container.Resolve<TenderParser>();
            var employeeWorkshift = await _employeeWorkshiftRepository.GetByFk(Workshift.Id, SelectedAddedEmployee.Id);
            
            if(!tenderParser.IsActive)
            {
                tenderParser.Show();
                tenderParser.Args = new TenderRequestEventArgs() { Id = employeeWorkshift.Id, Quantity = 30, 
                    FileName = SelectedAddedEmployee.Name+" "+ SelectedAddedEmployee.Surname +" "+ Workshift.Date.ToShortDateString().Replace('/','.')};
                //EventAggregator.GetEvent<TenderRequestEvent>().Publish(new TenderRequestEventArgs() { Id = employeeWorkshift.Id, Quantity = 30 });
            }


        }

        private bool OnGetTenderSingleCanExecute()
        {
            //TODO : Implement more logic!! Check if 
            return SelectedAddedEmployee!=null;
        }
    }
}
