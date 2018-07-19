﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECInterlockConnection : TECScope, IConnection, INotifyCostChanged, IRelatable, ITypicalable
    {
        private readonly ConnectionWrapper connection;
        public bool IsTypical { get; private set; } = false;
        
        public List<TECConnectionType> ConnectionTypes { get; }
        
        public TECInterlockConnection(Guid guid, IEnumerable<TECConnectionType> connectionTypes) : base(guid)
        {
            this.connection = new ConnectionWrapper(guid, new TECHardwiredProtocol(connectionTypes));
            this.ConnectionTypes = new List<TECConnectionType>(connectionTypes);
            subscribeToConnection();
        }

        public TECInterlockConnection(IEnumerable<TECConnectionType> connectionTypes) : this(Guid.NewGuid(), connectionTypes) { }
        public TECInterlockConnection(TECInterlockConnection source, Dictionary<Guid, Guid> guidDictionary = null) : base(source.Guid)
        {
            this.connection = new ConnectionWrapper(source.connection, guidDictionary);
            subscribeToConnection();
        }
        
        private void subscribeToConnection()
        {
            this.connection.CostChanged += notifyCostChanged;
            this.connection.TECChanged += (args) => notifyTECChanged(args.Change, args.PropertyName, args.Sender, args.Value, args.OldValue);
            this.connection.PropertyChanged += (sender, args) => raisePropertyChanged(args.PropertyName);
        }

        #region IConnection
        public double ConduitLength { get => ((IConnection)connection).ConduitLength; set => ((IConnection)connection).ConduitLength = value; }
        public TECElectricalMaterial ConduitType { get => ((IConnection)connection).ConduitType; set => ((IConnection)connection).ConduitType = value; }
        public bool IsPlenum { get => ((IConnection)connection).IsPlenum; set => ((IConnection)connection).IsPlenum = value; }
        public double Length { get => ((IConnection)connection).Length; set => ((IConnection)connection).Length = value; }

        public IProtocol Protocol => ((IConnection)connection).Protocol;

        #endregion

        #region INotifyCostChanged
        protected override CostBatch getCosts()
        {
            CostBatch costs = base.getCosts();
            costs += connection.CostBatch;
            return costs;
        }
        #endregion

        #region IRelatable
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.AddRange(connection.PropertyObjects);
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.AddRange(connection.PropertyObjects);
            return saveList;
        }


        #endregion

        #region ITypicalable

        ITECObject ITypicalable.CreateInstance(ObservableListDictionary<ITECObject> typicalDictionary)
        {
            return new TECInterlockConnection(this);
        }

        void ITypicalable.AddChildForProperty(string property, ITECObject item)
        {
            this.AddChildForScopeProperty(property, item);
        }

        bool ITypicalable.RemoveChildForProperty(string property, ITECObject item)
        {
            return this.RemoveChildForScopeProperty(property, item);
        }

        bool ITypicalable.ContainsChildForProperty(string property, ITECObject item)
        {
            return this.ContainsChildForScopeProperty(property, item);
        }

        void ITypicalable.MakeTypical()
        {
            this.IsTypical = true;
        }
        #endregion

        private class ConnectionWrapper : TECConnection
        {
            public override IProtocol Protocol { get; }

            public ConnectionWrapper(IProtocol protocol) : base()
            {
                this.Protocol = protocol;
            }

            public ConnectionWrapper(Guid guid, IProtocol protocol) : base(guid)
            {
                this.Protocol = protocol;
            }

            public ConnectionWrapper(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : base(connectionSource, guidDictionary)
            {
                this.Protocol = connectionSource.Protocol;
            }
        }
    }
}