﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class ConnectableFilterVM<T> : ViewModelBase, IConnectableFilterVM 
        where T : ITECObject, INetworkConnectable
    {
        private readonly ReadOnlyObservableCollection<T> allConnectables;

        public ObservableCollection<T> FilteredConnectables { get; }

        #region Filter Fields and Properties
        public ReadOnlyCollection<IOType> NetworkIOTypes { get; }

        private string _searchQuery;
        private bool _includeConnected;
        private IOType _selectedIOType;
        private bool _filterByIO;
        private readonly List<T> _exclusions;

        public string SearchQuery
        {
            get { return _searchQuery; }
            set
            {
                if (SearchQuery != value)
                {
                    _searchQuery = value;
                    RaisePropertyChanged("SearchQuery");
                    refilter();
                }
            }
        }
        public bool IncludeConnected
        {
            get { return _includeConnected; }
            set
            {
                if (IncludeConnected != value)
                {
                    _includeConnected = value;
                    RaisePropertyChanged("IncludeConnected");
                    refilter();
                }
            }
        }
        public IOType SelectedIOType
        {
            get { return _selectedIOType; }
            set 
            {
                if (SelectedIOType != value)
                {
                    _selectedIOType = value;
                    RaisePropertyChanged("IOType");
                    refilter();
                }
            }
        }
        public bool FilterByIO
        {
            get { return _filterByIO; }
            set
            {
                if (FilterByIO != value)
                {
                    _filterByIO = value;
                    RaisePropertyChanged("FilterByIO");
                    refilter();
                }
            }
        }
        public ReadOnlyCollection<T> Exclusions
        {
            get { return new ReadOnlyCollection<T>(_exclusions); }
        }
        #endregion

        public ConnectableFilterVM(ObservableCollection<T> connectables)
        {
            allConnectables = new ReadOnlyObservableCollection<T>(connectables);
            (allConnectables as INotifyCollectionChanged).CollectionChanged += allConnectablesCollectionChanged;
            FilteredConnectables = new ObservableCollection<T>();
            NetworkIOTypes = new ReadOnlyCollection<IOType>(TECIO.NetworkIO);
            _exclusions = new List<T>();

            refilter();
        }

        public void AddExclusion(T exclusion)
        {
            if (exclusion != null)
            {
                _exclusions.Add(exclusion);
                refilter();
            }
        }
        public void RemoveExclusion(T exclusion)
        {
            if (exclusion != null && Exclusions.Contains(exclusion))
            {
                _exclusions.Remove(exclusion);
                refilter();
            }
        }

        private void allConnectablesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //Add any connectables that pass
                foreach(object value in e.NewItems)
                {
                    if (value is T connectable && passesFilter(connectable))
                    {
                        FilteredConnectables.Add(connectable);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                //Remove any connectables that exist
                foreach(object value in e.OldItems)
                {
                    if (value is T connectable && FilteredConnectables.Contains(connectable))
                    {
                        FilteredConnectables.Remove(connectable);
                    }
                }
            }
        }

        private void refilter()
        {
            //Remove connectables that don't pass
            List<T> toRemove = new List<T>();
            foreach(T connectable in FilteredConnectables)
            {
                if (!passesFilter(connectable))
                {
                    toRemove.Add(connectable);
                }
            }
            foreach(T connectable in toRemove)
            {
                FilteredConnectables.Remove(connectable);
            }

            //Add connectables that do pass
            foreach(T connectable in allConnectables)
            {
                if (!FilteredConnectables.Contains(connectable) && passesFilter(connectable))
                {
                    FilteredConnectables.Add(connectable);
                }
            }
        }

        private bool passesFilter(T connectable)
        {
            //Search filter
            List<T> results = allConnectables.GetSearchResult(SearchQuery);
            if (!results.Contains(connectable))
            {
                return false;
            }

            //Connected filter
            if (!IncludeConnected && connectable.ParentConnection != null)
            {
                return false;
            }

            //IOType filter
            if (FilterByIO && !(connectable.AvailableNetworkIO.Contains(SelectedIOType)))
            {
                return false;
            }

            //Exclusions filter
            if (Exclusions.Contains(connectable))
            {
                return false;
            }

            //All filters passed
            return true;
        }
    }
}
