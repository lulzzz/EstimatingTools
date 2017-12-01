﻿using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class ScopeCollectionsTabVM : ViewModelBase, IDropTarget
    {
        #region Properties
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        public ObservableCollection<TECObject> ResultCollection
        {
            get { return _resultCollection; }
            set
            {
                _resultCollection = value;
                RaisePropertyChanged("ResultCollection");
            }

        }
        public AllSearchableObjects ChosenType
        {
            get { return _chosenType; }
            set
            {
                _chosenType = value;
                RaisePropertyChanged("ChosenType");
            }
        }
        
        private ObservableCollection<TECObject> _resultCollection;
        private TECTemplates _templates;
        private AllSearchableObjects _chosenType;
        private TECCatalogs catalogs;

        public ICommand SearchCollectionCommand { get; private set; }
        public ICommand EndSearchCommand { get; private set; }

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;
        #endregion
        
        #region Search
        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                RaisePropertyChanged("SearchString");
            }
        }
        #endregion
        #endregion

        #region Intializers
        public ScopeCollectionsTabVM(TECTemplates templates, TECCatalogs catalogs)
        {
            Templates = templates;
            this.catalogs = catalogs; 
            SearchCollectionCommand = new RelayCommand(SearchCollectionExecute, SearchCanExecute);
            EndSearchCommand = new RelayCommand(EndSearchExecute);
            populateItemsCollections();
            SearchString = "";
        }
        #endregion

        #region Methods

        public void Refresh(TECTemplates templates, TECCatalogs catalogs)
        {
            Templates = templates;
            this.catalogs = catalogs;
            populateItemsCollections();
        }

        #region Commands
        private void SearchCollectionExecute()
        {
            bool isOr = false;
            string searchString = SearchString;
            if (searchString.Length > 0 && searchString[0] == '*')
            {
                isOr = true;
                searchString = searchString.Remove(0,1);
            }
            char[] dilemeters = { ',', ' ' };
            string[] searchCriteria = searchString.ToUpper().Split(dilemeters, StringSplitOptions.RemoveEmptyEntries);
            switch (ChosenType)
            {
                case AllSearchableObjects.System:
                    ResultCollection = getResultCollection(Templates.SystemTemplates, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Equipment:
                    ResultCollection = getResultCollection(Templates.EquipmentTemplates, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.SubScope:
                    ResultCollection = getResultCollection(Templates.SubScopeTemplates, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Devices:
                    ResultCollection = getResultCollection(catalogs.Devices, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Controllers:
                    ResultCollection = getResultCollection(Templates.ControllerTemplates, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.AssociatedCosts:
                    ResultCollection = getResultCollection(catalogs.AssociatedCosts, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Panels:
                    ResultCollection = getResultCollection(Templates.PanelTemplates, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.MiscCosts:
                    ResultCollection = getResultCollection(Templates.MiscCostTemplates.Where(x => x.Type == CostType.TEC), searchCriteria, isOr);
                    break;
                case AllSearchableObjects.MiscWiring:
                    ResultCollection = getResultCollection(Templates.MiscCostTemplates.Where(x => x.Type == CostType.Electrical), searchCriteria, isOr);
                    break;
                case AllSearchableObjects.ControllerTypes:
                    ResultCollection = getResultCollection(catalogs.ControllerTypes, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.PanelTypes:
                    ResultCollection = getResultCollection(catalogs.PanelTypes, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Tags:
                    ResultCollection = getResultCollection(catalogs.Tags, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Wires:
                    ResultCollection = getResultCollection(catalogs.ConnectionTypes, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Conduits:
                    ResultCollection = getResultCollection(catalogs.ConduitTypes, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.Valves:
                    ResultCollection = getResultCollection(catalogs.Valves, searchCriteria, isOr);
                    break;
                case AllSearchableObjects.IOModules:
                    ResultCollection = getResultCollection(catalogs.IOModules, searchCriteria, isOr);
                    break;
                default:
                    break;

            }
        }
        private bool SearchCanExecute()
        {
            return true;
        }
        private void EndSearchExecute()
        {
            SearchString = "";
            populateItemsCollections();
        }
        #endregion

        public void populateItemsCollections()
        {
            ResultCollection = new ObservableCollection<TECObject>();
            switch (ChosenType)
            {
                case AllSearchableObjects.System:
                    foreach (TECSystem sys in Templates.SystemTemplates)
                    {
                        ResultCollection.Add(sys);
                    }
                    break;
                case AllSearchableObjects.Equipment:
                    foreach (TECEquipment equip in Templates.EquipmentTemplates)
                    {
                        ResultCollection.Add(equip);
                    }
                    break;
                case AllSearchableObjects.SubScope:
                    foreach (TECSubScope ss in Templates.SubScopeTemplates)
                    {
                        ResultCollection.Add(ss);
                    }
                    break;
                case AllSearchableObjects.Devices:
                    foreach (TECDevice dev in catalogs.Devices)
                    {
                        ResultCollection.Add(dev);
                    }
                    break;
                case AllSearchableObjects.Controllers:
                    foreach (TECController control in Templates.ControllerTemplates)
                    {
                        ResultCollection.Add(control);
                    }
                    break;
                case AllSearchableObjects.Panels:
                    foreach (TECPanel panel in Templates.PanelTemplates)
                    {
                        ResultCollection.Add(panel);
                    }
                    break;
                case AllSearchableObjects.MiscCosts:
                    foreach (TECMisc cost in Templates.MiscCostTemplates)
                    {
                        if (cost.Type == CostType.TEC)
                        {
                            ResultCollection.Add(cost);
                        }
                    }
                    break;
                case AllSearchableObjects.MiscWiring:
                    foreach (TECMisc cost in Templates.MiscCostTemplates)
                    {
                        if (cost.Type == CostType.Electrical)
                        {
                            ResultCollection.Add(cost);
                        }
                    }
                    break;
                case AllSearchableObjects.Wires:
                    foreach(TECElectricalMaterial wire in catalogs.ConnectionTypes)
                    {
                        ResultCollection.Add(wire);
                    }
                    break;
                case AllSearchableObjects.Conduits:
                    foreach (TECElectricalMaterial conduit in catalogs.ConduitTypes)
                    {
                        ResultCollection.Add(conduit);
                    }
                    break;
                case AllSearchableObjects.IOModules:
                    foreach(TECIOModule module in catalogs.IOModules)
                    {
                        ResultCollection.Add(module);
                    }
                    break;
                case AllSearchableObjects.PanelTypes:
                    foreach (TECPanelType type in catalogs.PanelTypes)
                    {
                        ResultCollection.Add(type);
                    }
                    break;
                case AllSearchableObjects.ControllerTypes:
                    foreach (TECPanelType type in catalogs.PanelTypes)
                    {
                        ResultCollection.Add(type);
                    }
                    break;
                case AllSearchableObjects.Tags:
                    foreach (TECLabeled tag in catalogs.Tags)
                    {
                        ResultCollection.Add(tag);
                    }
                    break;
                default:
                    break;
            }
        }
        
        public void DragOver(IDropInfo dropInfo)
        {
            DragHandler(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            DropHandler(dropInfo);
        }
        
        private ObservableCollection<TECObject> getResultCollection(IEnumerable<TECObject> source, string[] searchCriteria, bool isOr)
        {
            var outCollection = new ObservableCollection<TECObject>();
            foreach (TECObject item in source)
            {
                if(item is TECScope scope)
                {
                    string[] references = { scope.Name.ToUpper(), scope.Description.ToUpper() };
                    foreach(TECLabeled tag in scope.Tags)
                    {
                        references.Append(tag.Label);
                    }
                    if (scope is TECHardware hardware)
                    {
                        references.Append(hardware.Manufacturer.Label.ToUpper());
                    }
                    if (isOr)
                    {
                        if(UtilitiesMethods.StringsContainsAnyStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                    else
                    {
                        if (UtilitiesMethods.StringsContainStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                }
                else if (item is TECLabeled labeled)
                {
                    string[] references = { labeled.Label.ToUpper() };
                    if (isOr)
                    {
                        if (UtilitiesMethods.StringsContainsAnyStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                    else
                    {
                        if (UtilitiesMethods.StringsContainStrings(references, searchCriteria))
                        {
                            outCollection.Add(item);
                        }
                    }
                }
            }
            return outCollection;
        }
        #endregion
        
    }
}
