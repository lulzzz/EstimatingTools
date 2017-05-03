﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModelExtensions
{
    public class ScopeDataGridExtension : ViewModelBase, IDropTarget
    {
        #region Properties
        private VisibilityModel _dataGridVisibilty;
        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibilty; }
            set
            {
                _dataGridVisibilty = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            set
            {
                Bid.Locations.CollectionChanged -= Locations_CollectionChanged;
                _bid = value;
                RaisePropertyChanged("Bid");
                Bid.Locations.CollectionChanged += Locations_CollectionChanged;
                populateLocationSelections();
            }
        }

        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        private TECTemplates _templates;

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #region Selected Scope Properties
        private TECSystem _selectedSystem;
        public TECSystem SelectedSystem
        {
            get { return _selectedSystem; }
            set
            {
                _selectedSystem = value;
                RaisePropertyChanged("SelectedSystem");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECEquipment _selectedEquipment;
        public TECEquipment SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                _selectedEquipment = value;
                RaisePropertyChanged("SelectedEquipment");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECSubScope _selectedSubScope;
        public TECSubScope SelectedSubScope
        {
            get { return _selectedSubScope; }
            set
            {
                _selectedSubScope = value;
                RaisePropertyChanged("SelectedSubScope");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECDevice _selectedDevice;
        public TECDevice SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;
                RaisePropertyChanged("SelectedDevice");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECPoint _selectedPoint;
        public TECPoint SelectedPoint
        {
            get { return _selectedPoint; }
            set
            {
                _selectedPoint = value;
                RaisePropertyChanged("SelectedPoint");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECController _selectedController;
        public TECController SelectedController
        {
            get { return _selectedController; }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private TECPanel _selectedPanel;
        public TECPanel SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                _selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                NullifySelections(value);
                SelectionChanged?.Invoke(value);
            }
        }
        private ObservableCollection<TECLocation> _locationSelections;
        public ObservableCollection<TECLocation> LocationSelections
        {
            get { return _locationSelections; }
            set
            {
                _locationSelections = value;
                RaisePropertyChanged("LocationSelections");
            }
        }
        #endregion

        #region Point Interface Properties
        public string PointName
        {
            get { return _pointName; }
            set
            {
                _pointName = value;
                RaisePropertyChanged("PointName");
            }
        }
        private string _pointName;

        public string PointDescription
        {
            get { return _pointDescription; }
            set
            {
                _pointDescription = value;
                RaisePropertyChanged("PointDescription");
            }
        }
        private string _pointDescription;

        public PointTypes PointType
        {
            get { return _pointType; }
            set
            {
                _pointType = value;
                RaisePropertyChanged("PointType");
            }
        }
        private PointTypes _pointType;

        public int PointQuantity
        {
            get { return _pointQuantity; }
            set
            {
                _pointQuantity = value;
                RaisePropertyChanged("PointQuantity");
            }
        }
        private int _pointQuantity;
        #endregion //Point Interface Properties

        #region Command Properties
        public ICommand AddPointCommand { get; private set; }

        public RelayCommand<AddingNewItemEventArgs> AddNewEquipment { get; private set; }

        #endregion

        #endregion

        #region Intializers
        public ScopeDataGridExtension(TECBid bid)
        {
            _bid = bid;
            populateLocationSelections();
            DataGridVisibilty = new VisibilityModel();
            setupCommands();

            PointName = "";
            PointDescription = "";
        }
        public ScopeDataGridExtension(TECTemplates templates)
        {
            Templates = templates;
            DataGridVisibilty = new VisibilityModel();
            setupCommands();

            PointName = "";
            PointDescription = "";
        }
        #endregion

        #region Methods

        public void Refresh(TECBid bid)
        {
            refresh(bid);
        }

        public void Refresh(TECTemplates templates)
        {
            refresh(templates);
        }

        private void refresh(object bidOrTemplates)
        {
            if (bidOrTemplates is TECBid)
            {
                var bid = bidOrTemplates as TECBid;
                Bid = bid;
            }
            else if (bidOrTemplates is TECTemplates)
            {
                var templates = bidOrTemplates as TECTemplates;
                Templates = templates;
            }
        }

        #region Commands
        private void setupCommands()
        {
            AddPointCommand = new RelayCommand(AddPointExecute, AddPointCanExecute);
            AddNewEquipment = new RelayCommand<AddingNewItemEventArgs>(e => AddNewEquipmentExecute(e));
        }

        private void AddPointExecute()
        {
            TECPoint newPoint = new TECPoint();
            newPoint.Name = PointName;
            newPoint.Description = PointDescription;
            newPoint.Type = PointType;
            newPoint.Quantity = PointQuantity;
            if (PointType != 0)
            {
                SelectedSubScope.Points.Add(newPoint);
            }
        }
        private bool AddPointCanExecute()
        {
            if ((PointType != 0) && (PointName != ""))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void AddNewEquipmentExecute(AddingNewItemEventArgs e)
        {
            //e.NewItem = new TECEquipment("here","this", 12, new ObservableCollection<TECSubScope>());
            //((TECEquipment)e.NewItem).Location = SelectedSystem.Location;
        }
        #endregion //Commands

        private void NullifySelections(object obj)
        {
            //if(obj != null)
            //{
            //    if (!(obj is TECSystem))
            //    {
            //        SelectedSystem = null;
            //    }
            //    if (!(obj is TECEquipment))
            //    {
            //        SelectedEquipment = null;
            //    }
            //    if (!(obj is TECSubScope))
            //    {
            //        SelectedSubScope = null;
            //    }
            //    if (!(obj is TECDevice))
            //    {
            //        SelectedDevice = null;
            //    }
            //    if (!(obj is TECPoint))
            //    {
            //        SelectedPoint = null;
            //    }
            //    if (!(obj is TECController))
            //    {
            //        SelectedController = null;
            //    }
            //    if (!(obj is TECPanel))
            //    {
            //        SelectedPanel = null;
            //    }
            //}

        }

        private void populateLocationSelections()
        {
            LocationSelections = new ObservableCollection<TECLocation>();
            var noneLocation = new TECLocation();
            noneLocation.Name = "None";
            LocationSelections.Add(noneLocation);
            foreach (TECLocation location in Bid.Locations)
            {
                LocationSelections.Add(location);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }
        #endregion

        private void Locations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object location in e.NewItems)
                { LocationSelections.Add(location as TECLocation); }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object location in e.OldItems)
                { LocationSelections.Remove(location as TECLocation); }
            }
        }
    }
}
