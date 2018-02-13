﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class ReplaceActuatorVM : ViewModelBase
    {
        public TECValve Valve { get; }
        public List<TECDevice> ViableReplacements { get; }
        public TECDevice SelectedReplacement { get; set; }

        public ICommand ReplaceCommand { get; }

        public ReplaceActuatorVM(TECValve valve, IEnumerable<TECDevice> devices)
        {
            this.Valve = valve;
            ViableReplacements = new List<TECDevice>();
            foreach(TECDevice dev in devices)
            {
                if (valve.Actuator.ConnectionTypes.Matches(dev.ConnectionTypes) && valve.Actuator != dev)
                {
                    ViableReplacements.Add(dev);
                }
            }
            ReplaceCommand = new RelayCommand(replaceExecute, replaceCanExecute);
        }

        private void replaceExecute()
        {
            Valve.Actuator = SelectedReplacement;
        }
        private bool replaceCanExecute()
        {
            return SelectedReplacement != null;
        }
    }
}
