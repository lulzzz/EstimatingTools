﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECController : TECScope
    {
        private double _cost;
        private ObservableCollection<TECConnection> _connections;
        private ObservableCollection<ConnectionType> _types;

        public double Cost
        {
            get { return _cost; }
            set
            {
                var temp = this.Copy();
                _cost = value;
                NotifyPropertyChanged("Cost", temp, this);
            }
        }
        public ObservableCollection<TECConnection> Connections
        {
            get { return _connections; }
            set
            {
                var temp = this.Copy();
                _connections = value;
                NotifyPropertyChanged("Connections", temp, this);
            }
        }
        public ObservableCollection<ConnectionType> Types
        {
            get { return _types; }
            set
            {
                var temp = this.Copy();
                _types = value;
                NotifyPropertyChanged("Types", temp, this);
            }
        }

        public TECController(string name, string desciption, Guid guid, double cost) : base(name, desciption, guid)
        {
            _cost = cost;
            _types = new ObservableCollection<ConnectionType>();
            _types.Add(ConnectionType.TwoC18);
            _connections = new ObservableCollection<TECConnection>();
        }

        public TECController() : this("", "", Guid.NewGuid(), 0)
        {
        }

        #region Methods
        public override Object Copy()
        {
            TECController outController = new TECController(this.Name,
                this.Description,
                this.Guid,
                this.Cost);

            foreach (ConnectionType type in this.Types)
            {
                outController.Types.Add(type);
            }

            foreach (TECConnection conn in this.Connections)
            {
                outController.Connections.Add(conn);
            }
            
            return outController;
        }

        public override Object DragDropCopy()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
