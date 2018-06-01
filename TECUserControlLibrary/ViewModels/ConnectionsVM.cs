﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.Utilities.DropTargets;

namespace TECUserControlLibrary.ViewModels
{
    public class ConnectionsVM : ViewModelBase, IDropTarget, NetworkConnectionDropTargetDelegate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IRelatable root;
        private readonly ScopeGroup rootConnectableGroup;
        private readonly ScopeGroup rootControllersGroup;
        private readonly Func<ITECObject, bool> filterPredicate;

        private readonly List<TECController> allControllers;
        private readonly List<IConnectable> allConnectables;
        private ScopeGroup _selectedControllerGroup;
        private ScopeGroup _selectedConnectableGroup;
        private IControllerConnection _selectedConnection;
        private Double _defaultWireLength = 50.0;
        private Double _defaultConduitLength = 30.0;
        private TECElectricalMaterial _defaultConduitType;
        private bool _defaultPlenum = false;
        private bool _selectionNeeded = false;
        private IProtocol _selectedProtocol;
        private List<IProtocol> _compatibleProtocols;

        private bool _omitConnectedControllers = false;
        private TECProtocol _controllerFilterProtocol;
        private TECLocation _controllerFilterLocation;

        private bool _omitConnectedConnectables = false;
        private TECProtocol _connectableFilterProtocol;
        private TECLocation _connectableFilterLocation;

        public ObservableCollection<ScopeGroup> Connectables
        {
            get
            {
                return rootConnectableGroup.ChildrenGroups;
            }
        }
        public ObservableCollection<ScopeGroup> Controllers
        {
            get
            {
                return rootControllersGroup.ChildrenGroups;
            }
        }

        public ScopeGroup SelectedControllerGroup
        {
            get { return _selectedControllerGroup; }
            set
            {
                _selectedControllerGroup = value;
                RaisePropertyChanged("SelectedControllerGroup");
                RaisePropertyChanged("SelectedController");
                Selected?.Invoke(value?.Scope as TECObject);
            }
        }
        public ScopeGroup SelectedConnectableGroup
        {
            get { return _selectedConnectableGroup; }
            set
            {
                _selectedConnectableGroup = value;
                RaisePropertyChanged("SelectedConnectableGroup");
                RaisePropertyChanged("SelectedConnectable");
                Selected?.Invoke(value?.Scope as TECObject);
            }
        }

        public TECController SelectedController
        {
            get { return SelectedControllerGroup?.Scope as TECController; }
        }
        public IConnectable SelectedConnectable
        {
            get { return SelectedConnectableGroup?.Scope as IConnectable; }
        }
        public IControllerConnection SelectedConnection
        {
            get { return _selectedConnection; }
            set
            {
                _selectedConnection = value;
                RaisePropertyChanged("SelectedConnection");
                Selected?.Invoke(value as TECObject);
            }
        }

        public Double DefaultWireLength
        {
            get { return _defaultWireLength; }
            set
            {
                _defaultWireLength = value;
                RaisePropertyChanged("DefaultWireLength");
            }
        }
        public Double DefaultConduitLength
        {
            get { return _defaultConduitLength; }
            set
            {
                _defaultConduitLength = value;
                RaisePropertyChanged("DefaultConduitLength");
            }
        }
        public TECElectricalMaterial DefaultConduitType
        {
            get { return _defaultConduitType; }
            set
            {
                _defaultConduitType = value;
                RaisePropertyChanged("DefaultConduitType");
            }
        }
        public bool DefaultPlenum
        {
            get { return _defaultPlenum; }
            set
            {
                _defaultPlenum = value;
                RaisePropertyChanged("DefaultPlenum");
            }
        }
        public TECCatalogs Catalogs { get; }
        public ObservableCollection<TECLocation> Locations { get; }
        public bool SelectionNeeded
        {
            get { return _selectionNeeded; }
            set
            {
                _selectionNeeded = value;
                RaisePropertyChanged("SelectionNeeded");
            }
        }
        public IProtocol SelectedProtocol
        {
            get { return _selectedProtocol; }
            set
            {
                _selectedProtocol = value;
                RaisePropertyChanged("SelectedProtocol");
            }
        }
        
        public List<IProtocol> CompatibleProtocols
        {
            get { return _compatibleProtocols; }
            set
            {
                _compatibleProtocols = value;
                RaisePropertyChanged("CompatibleProtocols");
            }
        }
        
        public RelayCommand SelectProtocolCommand { get; private set; }
        public RelayCommand CancelProtocolSelectionCommand { get; private set; }

        public event Action<TECObject> Selected;

        public bool OmitConnectedControllers
        {
            get { return _omitConnectedControllers; }
            set
            {
                if (_omitConnectedControllers != value)
                {
                    _omitConnectedControllers = value;
                }
            }
        }
        public TECProtocol ControllerFilterProtocol
        {
            get { return _controllerFilterProtocol; }
            set
            {
                if (_controllerFilterProtocol != value)
                {
                    _controllerFilterProtocol = value;
                }
            }
        }
        public TECLocation ControllerFilterLocation
        {
            get { return _controllerFilterLocation; }
            set
            {
                if (_controllerFilterLocation != value)
                {
                    _controllerFilterLocation = value;
                }
            }
        }

        public bool OmitConnectedConnectables
        {
            get { return _omitConnectedConnectables; }
            set
            {
                if (_omitConnectedConnectables != value)
                {
                    _omitConnectedConnectables = value;
                }
            }
        }
        public TECProtocol ConnectableFilterProtocol
        {
            get { return _connectableFilterProtocol; }
            set
            {
                if (_connectableFilterProtocol != value)
                {
                    _connectableFilterProtocol = value;
                }
            }
        }
        public TECLocation ConnectableFilterLocation
        {
            get { return _connectableFilterLocation; }
            set
            {
                if (_connectableFilterLocation != value)
                {
                    _connectableFilterLocation = value;
                }
            }
        }

        public NetworkConnectionDropTarget ConnectionDropHandler { get; }
        TECNetworkConnection NetworkConnectionDropTargetDelegate.SelectedConnection => SelectedConnection as TECNetworkConnection;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="watcher"></param>
        /// <param name="includeFilter">Predicate for "where" clause of direct children of root.</param>
        public ConnectionsVM(IRelatable root, ChangeWatcher watcher, TECCatalogs catalogs, IEnumerable<TECLocation> locations = null, Func<ITECObject, bool> filterPredicate = null)
        {
            if (filterPredicate == null)
            {
                filterPredicate = item => true;
            }
            this.filterPredicate = filterPredicate;

            this.root = root;
            this.Catalogs = catalogs;
            this.Locations = new ObservableCollection<TECLocation>(locations);
            if(this.Catalogs.ConduitTypes.Count > 0)
            {
                this.DefaultConduitType = this.Catalogs.ConduitTypes[0];
            }

            watcher.ScopeChanged += parentChanged;

            this.rootConnectableGroup = new ScopeGroup("root");
            this.rootControllersGroup = new ScopeGroup("root");

            foreach(ITECObject child in root.GetDirectChildren())
            {
                repopulate(child, addConnectable);
            }

            SelectProtocolCommand = new RelayCommand(selectProtocolExecute, selectProtocolCanExecute);
            CancelProtocolSelectionCommand = new RelayCommand(cancelProtocolSelectionExecute);

            ConnectionDropHandler = new NetworkConnectionDropTarget(this);
        }

        private void cancelProtocolSelectionExecute()
        {
            SelectedProtocol = null;
            SelectionNeeded = false;
        }

        private void selectProtocolExecute()
        {
            SelectedController.Connect(SelectedConnectable, SelectedProtocol);
            cancelProtocolSelectionExecute();
        }

        private bool selectProtocolCanExecute()
        {
            return SelectedProtocol != null && SelectedConnectable != null && SelectedController != null;
        }

        private void repopulate(ITECObject child, Action<ScopeGroup, IConnectable> action)
        {
            if(child is IConnectable connectable)
            {
                action(this.rootConnectableGroup, connectable);
                if (connectable is TECController)
                {
                    action(this.rootControllersGroup, connectable);
                }
            }
            else if (child is IRelatable relatable)
            {
                foreach (ITECObject nextChild in relatable.GetDirectChildren().Where(filterPredicate))
                {
                    repopulate(nextChild, action);
                }
            }
        }

        private static bool containsConnectable(ScopeGroup group)
        {
            if (group.Scope is IConnectable)
            {
                return true;
            }
            
            foreach(ScopeGroup childGroup in group.ChildrenGroups)
            {
                if (containsConnectable(childGroup))
                {
                    return true;
                }
            }
            return false;
        }
        
        private void parentChanged(TECChangedEventArgs obj)
        {
            if (obj.Value is ITECObject tecObj)
            {
                if (obj.Change == Change.Add)
                {
                    repopulate(tecObj, addConnectable);
                }
                else if (obj.Change == Change.Remove)
                {
                    repopulate(tecObj, removeConnectable);
                }
            }
            else if (obj.Value is TECLocation location)
            {
                if (obj.Change == Change.Add)
                {
                    this.Locations.Add(location);
                }
                else if (obj.Change == Change.Remove)
                {
                    this.Locations.Remove(location);
                }
            }
        }
        
        private void addConnectable(ScopeGroup rootGroup, IConnectable connectable)
        {
            if (!filterPredicate(connectable)) return;
            bool isDescendant = root.IsDirectDescendant(connectable);
            if (!root.IsDirectDescendant(connectable))
            {
                logger.Error("New connectable doesn't exist in root object.");
                return;
            }

            List<ITECObject> path = this.root.GetObjectPath(connectable);

            ScopeGroup lastGroup = rootGroup;
            int lastIndex = 0;

            for(int i = path.Count - 1; i > 0; i--)
            {
                if (path[i] is ITECScope scope)
                {
                    ScopeGroup group = rootGroup.GetGroup(scope);

                    if (group != null)
                    {
                        lastGroup = group;
                        lastIndex = i;
                        break;
                    }
                }
                else
                {
                    logger.Error("Object in path to connectable isn't ITECScope, cannot build group hierarchy.");
                    return;
                }
            }
            
            ScopeGroup currentGroup = lastGroup;
            for(int i = lastIndex + 1; i < path.Count; i++)
            {
                if (path[i] is ITECScope scope)
                {
                    ScopeGroup newGroup = new ScopeGroup(scope);
                    currentGroup.Add(newGroup);
                    currentGroup = newGroup;
                }
                else
                {
                    logger.Error("Object in path to connectable isn't ITECScope, cannot build group hierarchy.");
                    return;
                }
            }
        }
        private void removeConnectable(ScopeGroup rootGroup, IConnectable connectable)
        {
            if (!filterPredicate(connectable)) return;
            List<ScopeGroup> path = rootGroup.GetPath(connectable);

            path[path.Count - 2].Remove(path.Last());

            ScopeGroup parentGroup = rootGroup;
            for(int i = 1; i < path.Count; i++)
            {
                if (!containsConnectable(path[i]))
                {
                    parentGroup.Remove(path[i]);
                    return;
                }
                else
                {
                    parentGroup = path[i];
                }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if(dropInfo.Data is ScopeGroup group && SelectedController != null)
            {
                if(SelectedController.CanConnect(group.Scope as IConnectable))
                {
                    UIHelpers.SetDragAdorners(dropInfo);
                }
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            IConnectable connectable = ((ScopeGroup)dropInfo.Data).Scope as IConnectable;
            var compatibleProtocols = SelectedController.CompatibleProtocols(connectable);
            if(compatibleProtocols.Count == 1)
            {
                var connection = SelectedController.Connect(connectable, compatibleProtocols.First());
                connection.Length = this.DefaultWireLength;
                connection.ConduitType = this.DefaultConduitType;
                connection.ConduitLength = this.DefaultConduitLength;
                connection.IsPlenum = this.DefaultPlenum;
            } else
            {
                SelectionNeeded = true;
                CompatibleProtocols = compatibleProtocols;
            }
            
        }


    }
}
