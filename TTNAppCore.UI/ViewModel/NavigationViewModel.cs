using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTNAppCore.Model;
using TTNAppCore.UI.Data;
using TTNAppCore.UI.Data.Lookups;
using TTNAppCore.UI.Event;

namespace TTNAppCore.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IEventAggregator _eventAggregator;
        private ITTNLookupDataService _tTNLookupDataService { get; }
        private IDriverLookupDataService _driverLookupDataService;

        public NavigationViewModel(ITTNLookupDataService tTNLookupDataService,
            IDriverLookupDataService driverLookupDataService,
            IEventAggregator eventAggregator)
        {
            _tTNLookupDataService = tTNLookupDataService;
            _driverLookupDataService = driverLookupDataService;
            _eventAggregator = eventAggregator;
            TTNs = new ObservableCollection<NavigationItemViewModel>();
            Drivers = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(TtnDetailViewModel):
                    AfterDetailDeleted(TTNs, args);
                    break;
                case nameof(DriverDetailViewModel):
                    AfterDetailDeleted(Drivers, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(TtnDetailViewModel):
                    AfterDetailSaved(TTNs, args);
                    break;
                case nameof(DriverDetailViewModel):
                    AfterDetailSaved(Drivers, args);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName,
                    _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _tTNLookupDataService.GetTTNLookupAsync();

            TTNs.Clear();
            foreach (var item in lookup)
            {
                TTNs.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(TtnDetailViewModel),
                    _eventAggregator));
            }

            lookup = await _driverLookupDataService.GetDriverLookupAsync();

            Drivers.Clear();
            foreach (var item in lookup)
            {
                Drivers.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(DriverDetailViewModel),
                    _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> TTNs { get; }
        public ObservableCollection<NavigationItemViewModel> Drivers { get; }
    }
}
