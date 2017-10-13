﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class TypicalConnectionsVM : ViewModelBase
    {
        #region Fields
        private ObservableCollection<TECController> _controllers;
        private ObservableCollection<TypicalSubScope> _subScope;
        private TECController _selectedController;
        #endregion

        #region Properties
        public ObservableCollection<TECController> Controllers
        {
            get
            {
                return _controllers;
            }
            set
            {
                _controllers = value;
                RaisePropertyChanged("Controllers");
            }
        }
        public ObservableCollection<TypicalSubScope> SubScope
        {
            get { return _subScope; }
            set
            {
                _subScope = value;
                RaisePropertyChanged("SubScope");
            }
        }
        public TECController SelectedController
        {
            get
            {
                return _selectedController;
            }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
            }
        }
        #endregion

        public TypicalConnectionsVM(TECTypical typical, IEnumerable<TECController> bidControllers)
        {
            initializeCollections();
            foreach (TECSubScope ss in typical.GetAllSubScope())
            {
                SubScope.Add(new TypicalSubScope(ss, typical.TypicalInstanceDictionary.GetInstances(ss).ConvertAll((x) => (TECSubScope)x )));
            }
            foreach (TECController controller in typical.Controllers)
            {
                Controllers.Add(controller);
            }
            foreach(TECController controller in bidControllers)
            {
                Controllers.Add(controller);
            }
        }

        private void initializeCollections()
        {
            _controllers = new ObservableCollection<TECController>();
            _subScope = new ObservableCollection<TypicalSubScope>();
        }
    }
}
