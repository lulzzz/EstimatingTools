﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.Utilities.DropTargets;

namespace TECUserControlLibrary.Models
{
    public class ControllerTypePropertiesItem : ViewModelBase, IDropTarget
    {
        private IOType _selectedIO;

        public TECControllerType ControllerType { get; }
        public QuantityCollection<TECIOModule> IOModules { get; }
        public IOType SelectedIO
        {
            get { return _selectedIO; }
            set
            {
                _selectedIO = value;
                RaisePropertyChanged("SelectedIO");
            }
        }
        public ICommand AddIOCommand { get; private set; }
        public IDropTarget ProtocolToIODropTarget { get; }

        public ControllerTypePropertiesItem(TECControllerType controllerType)
        {
            ControllerType = controllerType;
            IOModules = new QuantityCollection<TECIOModule>(controllerType.IOModules);
            IOModules.QuantityChanged += ioModules_QuantityChanged;
            AddIOCommand = new RelayCommand(addIOExecute, canAddIO);
            this.ProtocolToIODropTarget = new ProtocolToIODropTarget();
        }

        private void ioModules_QuantityChanged(TECIOModule arg1, int arg2, int arg3)
        {
            int change = arg3 - arg2;
            if(change > 0)
            {
                for(int x = 0; x < change; x++)
                {
                    ControllerType.IOModules.Add(arg1);
                }
            } else if (change < 0)
            {
                for(int x = 0; x > change; x--)
                {
                    ControllerType.IOModules.Remove(arg1);
                }
            }
        }

        private void addIOExecute()
        {
            bool added = false;

            foreach (TECIO io in ControllerType.IO)
            {
                if (io.Type == SelectedIO)
                {
                    io.Quantity++;
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                ControllerType.IO.Add(new TECIO(SelectedIO));
            }
        }

        private bool canAddIO()
        {
            return true;
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            UIHelpers.DragOver(dropInfo, (item, sourceType, targetType) =>
            {
                return sourceType == typeof(TECIOModule);
            });
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            if(dropInfo.Data is TECIOModule module)
            {
                bool foundModule = false;
                foreach(var item in IOModules)
                {
                    if(item.Item == module)
                    {
                        item.Quantity++;
                        foundModule = true;
                        break;
                    }
                }
                if (!foundModule)
                {
                    ControllerType.IOModules.Add(module);
                    IOModules.Add(module);
                }
            }
        }
    }

}
