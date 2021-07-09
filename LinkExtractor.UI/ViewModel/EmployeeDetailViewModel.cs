using LinkExtractor.Models;
using LinkExtractor.UI.DataServices;
using LinkExtractor.UI.DataServices.Lookups;
using LinkExtractor.UI.DataServices.Repositories;
using LinkExtractor.UI.Events;
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
    public class EmployeeDetailViewModel : ViewModelBase, IEmployeeDetailViewModel
    {
        private IEmployeeRepository _employeeRepository;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;
        private ITeamsLookupDataService _teamsLookupDataService;
        private EmployeeWrapper _employee;

        public EmployeeDetailViewModel(IEmployeeRepository employeeRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ITeamsLookupDataService teamsLookupDataService)
        {
            _employeeRepository = employeeRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _teamsLookupDataService = teamsLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            Teams = new ObservableCollection<LookupItem>();
        }



        public async Task LoadAsync(int? employeeId)
        {
            var employee = employeeId.HasValue ? await _employeeRepository.GetByIdAsync(employeeId.Value) : CreateNewEmployee();
            InitializeEmployee(employee);

            await LoadTeamsLookupAsync();
        }

        private void InitializeEmployee(Employee employee)
        {
            Employee = new EmployeeWrapper(employee);
            Employee.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _employeeRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Employee.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Employee.Id == 0)
            {
                //For triggering the validation
                Employee.Name = "";
                Employee.Surname = "";
                Employee.Email = "";
            }
        }

        private async Task LoadTeamsLookupAsync()
        {
            Teams.Clear();
            Teams.Add(new NullLookupItem());
            var lookup = await _teamsLookupDataService.GetTeamsLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Teams.Add(lookupItem);
            }
        }

        public EmployeeWrapper Employee
        {
            get { return _employee; }
            private set
            {
                _employee = value;
                OnPropertyChanged();

            }
        }

        private bool _hasChanges;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<LookupItem> Teams { get; }

        private async void OnSaveExecute()
        {
            await _employeeRepository.SaveAsync();
            HasChanges = _employeeRepository.HasChanges();
            _eventAggregator.GetEvent<EmployeeSavedEvent>()
                .Publish(new EmployeeSavedEventArgs
                {
                    Id = Employee.Id,
                    DisplayMember = Employee.Name + ' ' + Employee.Surname
                }
                );
        }

        private bool OnSaveCanExecute()
        {
            return Employee != null && !Employee.HasErrors && HasChanges;
        }

        private async void OnDeleteExecute()
        {
            var res = _messageDialogService.ShowOkCancelDialog($"Are you sure you want to delete the employee {Employee.Name} {Employee.Surname}", "Confirmation dialog");
            if (res == MessageDialogResult.Ok)
            {

                _employeeRepository.Remove(Employee.Model);
                await _employeeRepository.SaveAsync();
                _eventAggregator.GetEvent<EmployeeDeletedEvent>().Publish(Employee.Id);
            }
        }

        private Employee CreateNewEmployee()
        {
            var employee = new Employee();
            _employeeRepository.Add(employee);
            return employee;
        }


    }
}
