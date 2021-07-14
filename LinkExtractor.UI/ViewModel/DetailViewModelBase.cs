using LinkExtractor.UI.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LinkExtractor.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        protected readonly IEventAggregator EventAggregator;
        

        public DetailViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        public abstract Task LoadAsync(int? id, string data);

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        
        public bool HasChanges 
        {
            get { return _hasChanges; }
            set
            {
                if(_hasChanges !=value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        protected abstract void OnDeleteExecute();
        protected abstract void OnSaveExecute();
        protected abstract bool OnSaveCanExecute();

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.GetEvent<DetailDeletedEvent>()
                .Publish(new DetailDeletedEventArgs
                {
                    Id = modelId,
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.GetEvent<DetailSavedEvent>()
                .Publish(new DetailSavedEventArgs
                {
                    Id = modelId,
                    ViewModelName = this.GetType().Name,
                    DisplayMember = displayMember
                });
        }


    }
}
