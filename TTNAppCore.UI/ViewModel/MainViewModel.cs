using Autofac.Features.Indexed;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TTNAppCore.Model;
using TTNAppCore.UI.Data;
using TTNAppCore.UI.Data.Repositories;
using TTNAppCore.UI.Event;
using TTNAppCore.UI.View.Services;

namespace TTNAppCore.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private IIndex<string, IDetailViewModel> _detailViewModelCreator;
        //private Func<ITtnDetailViewModel> _ttnDetailViewModelCreator;
        //private Func<IDriverDetailViewModel> _driverDetailViewModelCreator;
        private IDetailViewModel _detailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            //Func<ITtnDetailViewModel> ttnDetailViewModelCreator,
            //Func<IDriverDetailViewModel> driverDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService
            )
        {
            _eventAggregator = eventAggregator;
            _detailViewModelCreator = detailViewModelCreator;
            _messageDialogService = messageDialogService;
            
            //_ttnDetailViewModelCreator = ttnDetailViewModelCreator;
            //_driverDetailViewModelCreator = driverDetailViewModelCreator;

            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);

            NavigationViewModel = navigationViewModel;
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }

        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            {
                ViewModelName=viewModelType.Name
            });
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
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("Есть не сохраненные данные! Выйти из формы?", "Вопрос");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            //switch (args.ViewModelName)
            //{
            //    case nameof(TtnDetailViewModel):
            //        DetailViewModel = _ttnDetailViewModelCreator();
            //        break;
            //    case nameof(DriverDetailViewModel):
            //        DetailViewModel = _driverDetailViewModelCreator();
            //        break;
            //    default:
            //        throw new Exception($"viewmodel {args.ViewModelName} not mapped");
            //}

            DetailViewModel = _detailViewModelCreator[args.ViewModelName];

            await DetailViewModel.LoadAsync(args.Id);
        }
    }
}
