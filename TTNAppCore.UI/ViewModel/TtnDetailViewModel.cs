using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TTNAppCore.Model;
using TTNAppCore.UI.Data;
using TTNAppCore.UI.Data.Lookups;
using TTNAppCore.UI.Data.Repositories;
using TTNAppCore.UI.Event;
using TTNAppCore.UI.Export;
using TTNAppCore.UI.View.Services;
using TTNAppCore.UI.Wrapper;

namespace TTNAppCore.UI.ViewModel
{
    public class TtnDetailViewModel : DetailViewModelBase, ITtnDetailViewModel
    {
        private ITtnRepository _ttnRepository;
        private ITtnXlsxExporter _ttnXlsxExporter;
        private IMessageDialogService _messageDialogService;
        private IDriverLookupDataService _driverLookupDataService;
        private TtnWrapper _ttn;
        private bool _hasChanges;

        public TtnDetailViewModel(ITtnRepository ttnRepository,
            IEventAggregator eventAggregator, ITtnXlsxExporter ttnXlsxExporter,
            IMessageDialogService messageDialogService,
            IDriverLookupDataService driverLookupDataService)
            : base(eventAggregator)
        {
            _ttnRepository = ttnRepository;

            _ttnXlsxExporter = ttnXlsxExporter;
            _messageDialogService = messageDialogService;
            _driverLookupDataService = driverLookupDataService;

            Drivers = new ObservableCollection<LookupItem>();

            PrintCommand = new DelegateCommand(OnPrintExecute);
        }



        private void OnPrintExecute()
        {
            _ttnXlsxExporter.Export(Ttn.Model);
        }

        protected override async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Вы хотите удалить ТТН №{Ttn.Num} от {Ttn.Date.ToShortDateString()}",
                "Вопрос");
            if (result == MessageDialogResult.OK)
            {
                _ttnRepository.Remove(Ttn.Model);
                await _ttnRepository.SaveAsync();
                RaiseDetailDeletedEvent(Ttn.Id);
                //_eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                //    new AfterDetailDeletedEventArgs
                //    {
                //        Id = Ttn.Id,
                //        ViewModelName = nameof(TtnDetailViewModel)
                //    });
            }
        }

        public override async Task LoadAsync(int? ttnId)
        {
            var ttn = ttnId.HasValue
                ? await _ttnRepository.GetByIdAsync(ttnId.Value)
                : CreateNewTtn();

            InitializeTtn(ttn);

            await LoadDriversLookupAsync();
        }

        private void InitializeTtn(Ttn ttn)
        {
            Ttn = new TtnWrapper(ttn);
            Ttn.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _ttnRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Ttn.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Ttn.Id == 0)
            {
                //Ttn.DrivingLicense = "";
            }
        }

        private async Task LoadDriversLookupAsync()
        {
            if (Drivers == null) return;

            Drivers.Clear();
            Drivers.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await _driverLookupDataService.GetDriverLookupAsync();
            foreach (var lookupItem in lookup)
            {
                Drivers.Add(lookupItem);
            }
        }

        private Ttn CreateNewTtn()
        {
            var ttn = new Ttn()
            {
                Date = DateTime.Today,
                DateCreate = DateTime.Today
            };
            _ttnRepository.Add(ttn);
            return ttn;
        }

        public TtnWrapper Ttn
        {
            get => _ttn;
            private set
            {
                _ttn = value;
                OnPropertyChanged();
            }
        }

        //public bool HasChanges
        //{
        //    get { return _hasChanges; }
        //    set
        //    {
        //        if (_hasChanges != value)
        //        {
        //            _hasChanges = value;
        //            OnPropertyChanged();
        //            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        //        }
        //    }
        //}

        public ObservableCollection<LookupItem> Drivers { get; }

        //public ICommand SaveCommand { get; }
        //public ICommand DeleteCommand { get; }
        public ICommand PrintCommand { get; }

        protected override async void OnSaveExecute()
        {
            await _ttnRepository.SaveAsync();
            HasChanges = _ttnRepository.HasChanges();
            RaiseDetailSavedEvent(Ttn.Id, $"№ {Ttn.Num} от {Ttn.Model.Date.ToShortDateString()}");
            //_eventAggregator.GetEvent<AfterDetailSavedEvent>().Publish(
            //    new AfterDetailSavedEventArgs
            //    {
            //        Id = Ttn.Id,
            //        DisplayMember = $"№ {Ttn.Num} от {Ttn.Model.Date.ToShortDateString()}",
            //        ViewModelName = nameof(TtnDetailViewModel)

            //    });
        }

        protected override bool OnSaveCanExecute()
        {
            return Ttn != null && !Ttn.HasErrors && HasChanges;
        }
    }
}
