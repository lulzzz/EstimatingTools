﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddMiscVM : AddVM
    {
        private TECSystem parent;
        private TECMisc toAdd;
        private TECScopeManager scopeManager;

        public TECMisc ToAdd
        {
            get { return toAdd; }
            private set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }

        public AddMiscVM(TECSystem parentSystem, TECScopeManager scopeManager) : base(scopeManager)
        {
            this.scopeManager = scopeManager;
            parent = parentSystem;
            ToAdd = new TECMisc(CostType.TEC);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
        }

        private bool addCanExecute()
        {
            return true;
        }
        private void addExecute()
        {
            parent.MiscCosts.Add(ToAdd);
            Added?.Invoke(ToAdd);
        }

        internal void SetTemplate(TECMisc misc)
        {
            ToAdd = new TECMisc(misc);
        }
    }
}
