﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class ConnectOnAddVM : ViewModelBase
    {
        private List<TECSubScope> toConnect;
        private TECSystem parent;
        private double _length;
        private double _conduitLength;
        private TECElectricalMaterial _conduitType;
        private bool _isPlenum;
        private bool _connect = false;
        private TECController _selectedController;

        public List<TECController> ParentControllers { get; private set; }
        public List<TECElectricalMaterial> ConduitTypes { get; private set; }
        public double Length
        {
            get { return _length; }
            set
            {
                _length = value;
                RaisePropertyChanged("Length");
            }
        }
        public double ConduitLength
        {
            get { return _conduitLength; }
            set
            {
                _conduitLength = value;
                RaisePropertyChanged("ConduitLength");
            }
        }
        public TECElectricalMaterial ConduitType
        {
            get { return _conduitType; }
            set
            {
                _conduitType = value;
                RaisePropertyChanged("ConduitType");
            }
        }
        public bool IsPlenum
        {
            get { return _isPlenum; }
            set
            {
                _isPlenum = value;
                RaisePropertyChanged("IsPlenum");
            }
        }
        public bool Connect
        {
            get { return _connect; }
            set
            {
                _connect = value;
                RaisePropertyChanged("Connect");
            }
        }
        public TECController SelectedController
        {
            get { return _selectedController; }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
            }
        }
        
        public ConnectOnAddVM(IEnumerable<TECSubScope> toConnect, TECSystem parent, IEnumerable<TECElectricalMaterial> conduitTypes, IEnumerable<TECConnectionType> connectionTypes)
        {
            this.toConnect = new List<TECSubScope>(toConnect);
            this.parent = parent;
            this.ConduitTypes =  new List<TECElectricalMaterial>(conduitTypes);
            ParentControllers = getCompatibleControllers(parent);
        }

        private List<TECController> getCompatibleControllers(TECSystem parent)
        {
            List<TECController> result = new List<TECController>();
            foreach(TECController controller in parent.Controllers)
            {
                bool allCompatible = true;

                foreach(IConnectable connectable in toConnect)
                {
                    if (!controller.CanConnect(connectable))
                    {
                        allCompatible = false;
                        break;
                    }
                }

                if (allCompatible)
                {
                    result.Add(controller);
                }
            }
            return result;
        }

        public void Update(IEnumerable<TECSubScope> toConnect)
        {
            this.toConnect = new List<TECSubScope>(toConnect);
            ParentControllers = getCompatibleControllers(parent);
            if (!ParentControllers.Contains(SelectedController))
            {
                SelectedController = null;
            }
            RaisePropertyChanged("ParentControllers");
        }
        public void ExecuteConnection(IEnumerable<TECSubScope> finalToConnect)
        {
            foreach(TECSubScope subScope in finalToConnect)
            {
                ExecuteConnection(subScope);
            }
        }
        public void ExecuteConnection(TECSubScope finalToConnect)
        {
            if (Connect && SelectedController != null)
            {
                IProtocol protocol = (finalToConnect as IConnectable).AvailableProtocols.First();
                if (parent is TECTypical typical)
                {
                    List<IControllerConnection> connections = typical.CreateTypicalAndInstanceConnections(SelectedController, finalToConnect, protocol);
                    foreach (IControllerConnection conn in connections)
                    {
                        setConnectionProperties(conn);
                    }
                }
                else
                {
                    connectControllerToSubScope(SelectedController, finalToConnect);
                }
            }
        }
        
        private void connectControllerToSubScope(TECController controller, TECSubScope finalToConnect)
        {
            IProtocol protocol = (finalToConnect as IConnectable).AvailableProtocols.First();
            IControllerConnection connection = controller.Connect(finalToConnect, protocol);
            setConnectionProperties(connection);
        }
        public bool CanConnect()
        {
            if (Connect)
            {
                return (SelectedController != null);
            }
            else
            {
                return true;
            }
        }

        private void setConnectionProperties(IControllerConnection connection)
        {
            connection.Length += Length;
            connection.ConduitLength += ConduitLength;
            if (connection.ConduitType == null)
            {
                connection.ConduitType = ConduitType;
            }
            if (!connection.IsPlenum)
            {
                connection.IsPlenum = IsPlenum;
            }
        }
    }
}
