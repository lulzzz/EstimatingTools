﻿using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TECUserControlLibrary.Models
{
    public class SubScopeConnection : TECObject
    {
        private TECSubScope _subScope;
        public TECSubScope SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
                RaisePropertyChanged("Subscope");
            }
        }

        private TECController _controller;
        public TECController Controller
        {
            get { return _controller; }
            set
            {
                handleControllerSelection(value);
                _controller = value;
                RaisePropertyChanged("Controller");
            }
        }

        private TECSystem _parentSystem;
        public TECSystem ParentSystem
        {
            get { return _parentSystem; }
            set
            {
                _parentSystem = value;
                RaisePropertyChanged("ParentSystem");
            }
        }

        private TECEquipment _parentEquipment;
        public TECEquipment ParentEquipment
        {
            get { return _parentEquipment; }
            set
            {
                _parentEquipment = value;
                RaisePropertyChanged("ParentEquipment");
            }
        }

        public TECConduitType ConduitType
        {
            get
            {
                if (SubScope.Connection == null)
                {
                    return null;
                }
                else
                {
                    return SubScope.Connection.ConduitType;
                }
            }
            set
            {
                handleConduitSelection(value);
                RaisePropertyChanged("ConduitType");

            }
        }

        public SubScopeConnection(TECSubScope subscope)
        {
            _subScope = subscope;
            _controller = null;
            if (subscope.Connection != null)
            {
                _controller = SubScope.Connection.ParentController;
            }
        }

        private void handleControllerSelection(TECController controller)
        {
            if (controller != null)
            {
                controller.AddSubScope(SubScope);
            }
            else
            {
                if (SubScope.Connection != null)
                {
                    Controller.RemoveSubScope(SubScope);
                }
            }
        }

        private void handleConduitSelection(TECConduitType conduitType)
        {
            if (SubScope.Connection != null)
            {
                SubScope.Connection.ConduitType = conduitType;
            }
        }

        public override object Copy()
        {
            throw new NotImplementedException();
        }
    }
}
