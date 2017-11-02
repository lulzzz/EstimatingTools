﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.CommandWpf;
using TECUserControlLibrary.ViewModels.AddVMs;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels
{
    public class TypicalHierarchyVM : ViewModelBase
    {
        private TECTypical _selectedTypical;
        private TECBid bid;

        public SystemHierarchyVM SystemHierarchyVM { get; }
        public ReadOnlyObservableCollection<TECTypical> TypicalSystems { get; }
        public TECTypical SelectedTypical
        {
            get { return _selectedTypical; }
            set
            {
                _selectedTypical = value;
                RaisePropertyChanged("SelectedTypical");
                Selected?.Invoke(value);
            }
        }
        public ICommand AddInstanceCommand { get; private set; }


        public TypicalHierarchyVM(TECBid bid)
        {
            this.bid = bid;
            SystemHierarchyVM = new SystemHierarchyVM(bid);
            SystemHierarchyVM.Selected += this.Selected;
            TypicalSystems = new ReadOnlyObservableCollection<TECTypical>(bid.Systems);
            AddInstanceCommand = new RelayCommand(addInstanceExecute, canAddInstance);

        }

        public event Action<TECObject> Selected;

        private void addInstanceExecute()
        {
            SystemHierarchyVM.SelectedVM = new AddInstanceVM(SelectedTypical, bid);
        }

        private bool canAddInstance()
        {
            return SelectedTypical != null;
        }
    }
}
