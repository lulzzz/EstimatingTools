﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EstimatingLibrary
{
    public class TECSubScope : TECLocated, INotifyPointChanged, IDragDropable, ITypicalable, IConnectable, IInterlockable
    {
        #region Properties
        private ObservableCollection<IEndDevice> _devices;
        private ObservableCollection<TECPoint> _points;
        private ObservableCollection<TECInterlockConnection> _interlocks;

        public ObservableCollection<IEndDevice> Devices
        {
            get { return _devices; }
            set
            {
                if (Devices != null)
                {
                    Devices.CollectionChanged -= Devices_CollectionChanged;
                }
                var old = Devices;
                _devices = value;
                notifyCombinedChanged(Change.Edit, "Devices", this, value, old);
                Devices.CollectionChanged += Devices_CollectionChanged;
            }
        }
        public ObservableCollection<TECPoint> Points
        {
            get { return _points; }
            set
            {
                if (Points != null)
                {
                    Points.CollectionChanged -= PointsCollectionChanged;
                }
                var old = Points;
                _points = value;
                Points.CollectionChanged += PointsCollectionChanged;
                notifyCombinedChanged(Change.Edit, "Points", this, value, old);
            }
        }
        public ObservableCollection<TECInterlockConnection> Interlocks
        {
            get { return _interlocks; }
            set
            {
                if (Points != null)
                {
                    Interlocks.CollectionChanged -= InterlocksCollectionChanged;
                }
                var old = Interlocks;
                _interlocks = value;
                Points.CollectionChanged += InterlocksCollectionChanged;
                notifyCombinedChanged(Change.Edit, "Interlocks", this, value, old);
            }
        }


        public IControllerConnection Connection { get; private set; }
        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }
        
        public bool IsTypical { get; private set; }

        //Derived
        public bool IsConnected
        {
            get
            {
                return Connection != null;
            }
        }
        
        #endregion //Properties

        #region Constructors
        public TECSubScope(Guid guid, bool isTypical) : base(guid)
        {
            IsTypical = isTypical;
            _devices = new ObservableCollection<IEndDevice>();
            _points = new ObservableCollection<TECPoint>();
            Devices.CollectionChanged += Devices_CollectionChanged;
            Points.CollectionChanged += PointsCollectionChanged;
        }
        public TECSubScope(bool isTypical) : this(Guid.NewGuid(), isTypical) { }

        //Copy Constructor
        public TECSubScope(TECSubScope sourceSubScope, bool isTypical, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableListDictionary<ITECObject> characteristicReference = null) : this(isTypical)
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = sourceSubScope.Guid; }
            foreach (IEndDevice device in sourceSubScope.Devices)
            { _devices.Add(device); }
            foreach (TECPoint point in sourceSubScope.Points)
            {
                var toAdd = new TECPoint(point, isTypical);
                characteristicReference?.AddItem(point,toAdd);
                Points.Add(toAdd);
            }
            this.copyPropertiesFromScope(sourceSubScope);
        }
        #endregion //Constructors

        #region Events
        public event Action<int> PointChanged;
        #endregion
        
        #region Event Handlers
        private void PointsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            collectionChanged(sender, e, "Points");
        }
        private void Devices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            collectionChanged(sender, e, "Devices");
        }
        private void InterlocksCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            collectionChanged(sender, e, "Interlocks");
        }

        private void collectionChanged(object sender, NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                CostBatch costs = new CostBatch();
                int pointNumber = 0;

                foreach (TECObject item in e.NewItems)
                {
                    if(item is INotifyCostChanged cost)
                    {
                        costs += cost.CostBatch;
                    }
                    if(item is INotifyPointChanged pointed)
                    {
                        pointNumber += pointed.PointNumber;
                    }
                    
                    notifyCombinedChanged(Change.Add, propertyName, this, item);
                }
                notifyCostChanged(costs);
                notifyPointChanged(pointNumber);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                CostBatch costs = new CostBatch();
                int pointNumber = 0;

                foreach (TECObject item in e.NewItems)
                {
                    if (item is INotifyCostChanged cost)
                    {
                        costs += cost.CostBatch;
                    }
                    if (item is INotifyPointChanged pointed)
                    {
                        pointNumber += pointed.PointNumber;
                    }

                    notifyCombinedChanged(Change.Remove, propertyName, this, item);
                }
                notifyCostChanged(costs * -1);
                notifyPointChanged(pointNumber * -1);
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                notifyCombinedChanged(Change.Edit, propertyName, this, sender, sender);
            }
        }

        #endregion

        #region Methods

        public object DragDropCopy(TECScopeManager scopeManager)
        {
            TECSubScope outScope = new TECSubScope(this, this.IsTypical);
            ModelLinkingHelper.LinkScopeItem(outScope, scopeManager);
            return outScope;
        }
        
        public void AddPoint(TECPoint point)
        {
            Points.Add(point);
        }
        public void RemovePoint(TECPoint point)
        {
            Points.Remove(point);
        }

        public bool CanConnectToNetwork(TECNetworkConnection netConnect)
        {
            throw new NotImplementedException();
        }
        
        private int getPointNumber()
        {
            var totalPoints = 0;
            foreach (TECPoint point in Points)
            {
                totalPoints += point.Quantity;
            }
            return totalPoints;
        }

        protected override CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            foreach (IEndDevice dev in Devices)
            {
                if(dev is INotifyCostChanged costDev)
                {
                    costs += costDev.CostBatch;
                }
                else
                {
                    throw new Exception("This contains an unsupported end device.");
                }
            }
            return costs;
        }
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            List<TECObject> deviceList = new List<TECObject>();
            foreach (IEndDevice item in this.Devices)
            {
                deviceList.Add(item as TECObject);
            }
            saveList.AddRange(deviceList, "Devices");
            saveList.AddRange(this.Points, "Points");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            List<TECObject> deviceList = new List<TECObject>();
            foreach (IEndDevice item in this.Devices)
            {
                deviceList.Add(item as TECObject);
            }
            saveList.AddRange(deviceList.Distinct(), "Devices");
            return saveList;
        }
        protected override void notifyCostChanged(CostBatch costs)
        {
            if (!IsTypical)
            {
                base.notifyCostChanged(costs);
            }
        }
        private void notifyPointChanged(int numPoints)
        {
            if (!IsTypical)
            {
                PointChanged?.Invoke(numPoints);
            }
        }

        public IConnectable Copy(bool isTypical, Dictionary<Guid, Guid> guidDictionary)
        {
            return new TECSubScope(this, isTypical, guidDictionary);
        }
        #endregion

        #region IConnectable
        /// <summary>
        /// Returns the intersect of connection methods in this TECSubScope's devices.
        /// </summary>
        List<IProtocol> IConnectable.AvailableProtocols
        {
            get
            {
                List<IProtocol> protocols = new List<IProtocol>();
                foreach(IEndDevice endDev in this.Devices)
                {
                    if (protocols.Count <= 0)
                    {
                        protocols.AddRange(endDev.ConnectionMethods);
                    }
                    else
                    {
                        List<IProtocol> toRemove = new List<IProtocol>();
                        foreach(IProtocol protocol in protocols)
                        {
                            if (!endDev.ConnectionMethods.Contains(protocol))
                            {
                                toRemove.Add(protocol);
                            }
                        }
                        foreach(IProtocol protocol in toRemove)
                        {
                            protocols.Remove(protocol);
                        }
                    }
                }
                return protocols;
            }
        }
        IOCollection IConnectable.HardwiredIO
        {
            get
            {
                return this.Points.ToIOCollection();
            }
        }
        bool IConnectable.CanSetParentConnection(IControllerConnection connection)
        {
            return ((IConnectable)this).AvailableProtocols.Contains(connection.Protocol);            
        }
        void IConnectable.SetParentConnection(IControllerConnection connection)
        {
            Connection = connection;
            raisePropertyChanged("Connection");
        }
        IControllerConnection IConnectable.GetParentConnection()
        {
            return this.Connection;
        }

        #endregion
    }
}
