﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{

    public class TECConnection : TECObject
    {
        #region Properties
        private Guid _guid;
        private double _length;
        private TECController _controller;
        private ObservableCollection<TECScope> _scope;
        private ObservableCollection<IOType> _ioTypes;

        public Guid Guid
        {
            get { return _guid; }
        }
        public double Length
        {
            get { return _length; }
            set
            {
                var temp = this.Copy();
                _length = value;
                NotifyPropertyChanged("Length", temp, this);
            }
        }
        
        public TECController Controller
        {
            get { return _controller; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_controller, value);
                var temp = Copy();
                _controller = value;
                NotifyPropertyChanged("Controller", temp, this);
                temp = Copy();
                NotifyPropertyChanged("RelationshipPropertyChanged", temp, oldNew);
            }
        }
        public ObservableCollection<TECScope> Scope
        {
            get { return _scope; }
            set
            {
                var temp = this.Copy();
                Scope.CollectionChanged -= collectionChanged;
                _scope = value;
                NotifyPropertyChanged("Scope", temp, this);
                Scope.CollectionChanged += collectionChanged;
            }
        }
        public ObservableCollection<TECConnectionType> ConnectionTypes
        {
            get { return getConnectionTypes(); }
            
        }
        public ObservableCollection<IOType> IOTypes
        {
            get { return _ioTypes; }
            set
            {
                var temp = this.Copy();
                _ioTypes = value;
                NotifyPropertyChanged("IOTypes", temp, this);
            }
        }
        
        private TECConduitType _conduitType;
        public TECConduitType ConduitType
        {
            get { return _conduitType; }
            set
            {
                var oldNew = Tuple.Create<Object, Object>(_conduitType, value);
                var temp = Copy();
                _conduitType = value;
                NotifyPropertyChanged("ConduitType", temp, this);
                temp = Copy();
                NotifyPropertyChanged("ObjectPropertyChanged", temp, oldNew);
            }
        }
        #endregion //Properties

        #region Constructors 
        public TECConnection(Guid guid)
        {
            _guid = guid;
            _length = 0;
            _scope = new ObservableCollection<TECScope>();
            _ioTypes = new ObservableCollection<IOType>();
            _controller = new TECController();
            Scope.CollectionChanged += collectionChanged;
            IOTypes.CollectionChanged += collectionChanged;
        }

        public TECConnection() : this(Guid.NewGuid()) { }

        public TECConnection(TECConnection connectionSource, Dictionary<Guid, Guid> guidDictionary = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[connectionSource.Guid] = _guid; }

            _length = connectionSource.Length;
            _scope = connectionSource.Scope;
            _ioTypes = connectionSource.IOTypes;
            _controller = connectionSource.Controller;
            if (connectionSource.ConduitType != null)
            { _conduitType = connectionSource.ConduitType.Copy() as TECConduitType; }
        }
        #endregion //Constructors

        #region Methods

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(object item in e.NewItems)
                {
                    NotifyPropertyChanged("AddRelationship", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("RemoveRelationship", this, item);
                }
            }
        }

        public override Object Copy()
        {
            TECConnection connection = new TECConnection(this);
            connection._guid = this._guid;
            return connection;
        }

        private ObservableCollection<TECConnectionType> getConnectionTypes()
        {
            var outConnectionTypes = new ObservableCollection<TECConnectionType>();

            foreach(TECScope scope in Scope)
            {
                if(scope is TECSubScope)
                {
                    var sub = scope as TECSubScope;
                    foreach(TECDevice dev in sub.Devices)
                    {
                        outConnectionTypes.Add(dev.ConnectionType);
                    }
                }
            }

            return outConnectionTypes;
        }

        #endregion

    }
}
