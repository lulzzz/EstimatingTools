﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{

    abstract public class TECConnection : TECObject, INotifyCostChanged, IRelatable, IConnection, ICatalogContainer
    {
        #region Properties
        protected double _length = 0;
        protected double _conduitLength = 0;
        protected TECElectricalMaterial _conduitType;
        protected bool _isPlenum = true;

        public double Length
        {
            get { return _length; }
            set
            {
                if(_length != value)
                {
                    var old = Length;
                    var originalCost = this.CostBatch;
                    _length = value;
                    notifyCombinedChanged(Change.Edit, "Length", this, value, old);
                    notifyCostChanged(CostBatch - originalCost);
                }
            }
        }
        public double ConduitLength
        {
            get { return _conduitLength; }
            set
            {
                if(_conduitLength != value)
                {
                    var old = ConduitLength;
                    _conduitLength = value;
                    notifyCombinedChanged(Change.Edit, "ConduitLength", this, value, old);
                    CostBatch previous = ConduitType != null ? ConduitType.GetCosts(old) : new CostBatch();
                    CostBatch current = ConduitType != null ? ConduitType.GetCosts(value) : new CostBatch();
                    notifyCostChanged(current - previous);
                }
            }
        }
        public TECElectricalMaterial ConduitType
        {
            get { return _conduitType; }
            set
            {
                if(_conduitType != value)
                {
                    var old = ConduitType;
                    _conduitType = value;
                    notifyCombinedChanged(Change.Edit, "ConduitType", this, value, old);
                    CostBatch previous = old != null ? old.GetCosts(ConduitLength) : new CostBatch();
                    CostBatch current = value != null ? value.GetCosts(ConduitLength) : new CostBatch();
                    notifyCostChanged(current - previous);
                }
            }
        }
        public bool IsPlenum
        {
            get { return _isPlenum; }
            set
            {
                if(_isPlenum != value)
                {
                    var old = IsPlenum;
                    CostBatch oldCost = this.CostBatch;
                    _isPlenum = value;
                    notifyCombinedChanged(Change.Edit, "IsPlenum", this, value, old);
                    notifyCostChanged(this.CostBatch - oldCost);
                }
            }
        }

        public CostBatch CostBatch
        {
            get { return getCosts(); }
        }

        public RelatableMap PropertyObjects
        {
            get { return propertyObjects(); }
        }
        public RelatableMap LinkedObjects
        {
            get { return linkedObjects(); }
        }
        abstract public IProtocol Protocol { get; }
        #endregion //Properties

        public event Action<CostBatch> CostChanged;

        #region Constructors 
        public TECConnection(Guid guid) : base(guid) { }
        public TECConnection() : this(Guid.NewGuid()) { }
        public TECConnection(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = connectionSource.Guid; }

            _length = connectionSource.Length;
            _conduitLength = connectionSource.ConduitLength;
            _isPlenum = connectionSource.IsPlenum;
            if (connectionSource.ConduitType != null)
            { _conduitType = connectionSource.ConduitType; }
        }
        #endregion //Constructors

        protected virtual void notifyCostChanged(CostBatch costs)
        {
            CostChanged?.Invoke(costs);
        }

        protected virtual CostBatch getCosts()
        {
            CostBatch costs = new CostBatch();
            foreach (TECConnectionType connectionType in this.Protocol.ConnectionTypes)
            {
                costs += connectionType.GetCosts(Length, IsPlenum);
            }
            if (ConduitType != null)
            {
                costs += ConduitType.GetCosts(ConduitLength);
            }
            return costs;
        }
        protected virtual RelatableMap propertyObjects()
        {
            RelatableMap saveList = new RelatableMap();
            if(this.ConduitType != null)
            {
                saveList.Add(this.ConduitType, "ConduitType");
            }
            return saveList;
        }
        protected virtual RelatableMap linkedObjects()
        {
            RelatableMap relatedList = new RelatableMap();
            if (this.ConduitType != null)
            {
                relatedList.Add(this.ConduitType, "ConduitType");
            }
            return relatedList;
        }

        #region ICatalogContainer
        public virtual bool RemoveCatalogItem<T>(T item, T replacement) where T : class, ICatalog
        {
            bool replacedConduit = false;
            if (item == this.ConduitType)
            {
                if (replacement is TECElectricalMaterial newConduit)
                {
                    this.ConduitType = newConduit;
                }
                else
                {
                    this.ConduitType = null;
                }
                replacedConduit = true;
            }
            return replacedConduit;
        }
        #endregion
    }
}