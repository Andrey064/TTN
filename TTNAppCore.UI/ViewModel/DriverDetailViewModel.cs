using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.Model;
using TTNAppCore.UI.Data.Repositories;
using TTNAppCore.UI.View.Services;
using TTNAppCore.UI.Wrapper;

namespace TTNAppCore.UI.ViewModel
{
    public class DriverDetailViewModel : DetailViewModelBase, IDriverDetailViewModel
    {
        private IMessageDialogService _messageDialogService;
        private DriverWrapper _driver;
        private IDriverRepository _driverRepository;

        public DriverDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IDriverRepository driverRepository) : base(eventAggregator)
        {
            _messageDialogService = messageDialogService;
            _driverRepository = driverRepository;
        }

        public DriverWrapper Driver
        {
            get { return _driver; }
            private set
            {
                _driver = value;
                OnPropertyChanged();
            }
        }

        public override async Task LoadAsync(int? driverId)
        {
            var driver = driverId.HasValue
                ? await _driverRepository.GetByIdAsync(driverId.Value)
                : CreateNewDriver();

            InitializeDriver(driver);

        }

        protected override async void OnDeleteExecute()
        {
            if (await _driverRepository.HasTtnAsync(Driver.Id))
            {
                _messageDialogService.ShowInfoDialog($"Водитель {Driver.Name} не может быть удален так как он добавлен в накладную");
                return;
            }

            var result = _messageDialogService.ShowOkCancelDialog($"Вы хотите удалить водителя {Driver.Name}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _driverRepository.Remove(Driver.Model);
                _driverRepository.SaveAsync();
                RaiseDetailDeletedEvent(Driver.Id);
            }
        }

        private void InitializeDriver(Driver driver)
        {
            Driver=new DriverWrapper(driver);
            Driver.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _driverRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Driver.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Driver.Id == 0)
            {
                Driver.Name = "";
                Driver.DrivingLicense = "";
            }

        }

        private Driver CreateNewDriver()
        {
            var driver = new Driver();

            _driverRepository.Add(driver);

            return driver;
        }

        protected override bool OnSaveCanExecute()
        {
            return Driver != null && !Driver.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _driverRepository.SaveAsync();
            HasChanges = _driverRepository.HasChanges();
            RaiseDetailSavedEvent(Driver.Id, Driver.Name);
        }
    }
}
