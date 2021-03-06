﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    public class PropertiesVM : ViewModelBase, IDropTarget
    {
        public TECScopeManager ScopeManager { get; private set; }

        private TECCatalogs _catalogs;
        private TECParameters _parameters;
        private bool _readOnly;
        private TECObject _selected;
        private string _templateText;
        private bool _displayReferenceProperty = false;
        private ObservableCollection<TECLocation> _locations;

        public TECCatalogs Catalogs
        {
            get { return _catalogs; }
            set
            {
                _catalogs = value;
                RaisePropertyChanged("Catalogs");
            }
        }
        public TECParameters Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                RaisePropertyChanged("Parameters");
            }
        }
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                RaisePropertyChanged("ReadOnly");
            }

        }
        public TECObject Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                setReadOnly(value);
                RaisePropertyChanged("Selected");
                if (IsTemplates)
                {
                    DisplayReferenceProperty = value is TECEquipment || value is TECSubScope ? true : false;
                    TemplateText = getTemplateText(value);
                }
            }
        }
        public bool DisplayReferenceProperty
        {
            get { return _displayReferenceProperty; }
            set
            {
                _displayReferenceProperty = value;
                RaisePropertyChanged("DisplayReferenceProperty");
            }
        }
        public RelayCommand<TECObject> DeleteConnectionTypeCommand { get; private set; }
        public RelayCommand<TECConnectionType> DeleteProtocolConnectionTypeCommand { get; private set; }
        public ObservableCollection<TECLocation> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                RaisePropertyChanged("Locations");
            }
        }

        private string getTemplateText(TECObject item)
        {
            TECTemplates templates = ScopeManager as TECTemplates;
            if(templates.IsTemplateObject(item))
            {
                string parentString = "";
                if(item is TECSubScope subScope)
                {
                    TECSubScope parent = templates.SubScopeSynchronizer.GetParent(subScope);
                    if (item != parent && parent!= null)
                    {
                        parentString = String.Format(" of {0}",
                            parent.Name);
                    }
                } else if(item is TECEquipment equipment)
                {
                    TECEquipment parent = templates.EquipmentSynchronizer.GetParent(equipment);
                    if (item != parent && parent != null)
                    {
                        parentString = String.Format(" of {0}",
                            parent.Name);
                    }
                }

                return String.Format("Reference{0}", 
                    parentString);
            }
            return "Instance Template";
        }

        public bool IsTemplates { get; }
        public string TemplateText
        {
            get { return _templateText; }
            set
            {
                _templateText = value;
                RaisePropertyChanged("TemplateText");
            }
        }

        private void setReadOnly(TECObject value)
        {
            ReadOnly = false;
            if(ScopeManager is TECBid)
            {
                if(value is TECHardware)
                {
                    ReadOnly = true;
                }
            }
            
        }

        public string TestString
        {
            get { return "test"; }
        }

        public PropertiesVM(TECCatalogs catalogs, TECScopeManager scopeManager)
        {
            IsTemplates = scopeManager is TECTemplates;
            TemplateText = "Instance Template";
            DeleteConnectionTypeCommand = new RelayCommand<TECObject>(deleteConnectionTypeExecute, canDeleteConnectionType);
            DeleteProtocolConnectionTypeCommand = new RelayCommand<TECConnectionType>(deleteProtocolConnectionTypeExecute, canDeleteProtocolConnectionType);
            Refresh(catalogs, scopeManager);
        }

        private void deleteProtocolConnectionTypeExecute(TECConnectionType obj)
        {
            (Selected as TECProtocol).ConnectionTypes.Remove(obj);
        }

        private bool canDeleteProtocolConnectionType(TECConnectionType arg)
        {
            return Selected is TECProtocol;
        }

        private void deleteConnectionTypeExecute(TECObject obj)
        {
            if(obj is TECProtocol prot)
            {
                (Selected as TECDevice).PossibleProtocols.Remove(prot);

            }
            else if (obj is TECConnectionType connType)
            {
                (Selected as TECDevice).HardwiredConnectionTypes.Remove(connType);

            }
        }

        private bool canDeleteConnectionType(TECObject arg)
        {
            return Selected is TECDevice && (arg is TECProtocol || arg is TECConnectionType);
        }

        public void Refresh(TECCatalogs catalogs, TECScopeManager scopeManager)
        {
            Catalogs = catalogs;
            if(scopeManager is TECBid bid)
            {
                Locations = bid.Locations;
                Parameters = bid.Parameters;
            }
            this.ScopeManager = scopeManager;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            DragDropHelpers.StandardDragOver(dropInfo);
        }

        public void Drop(IDropInfo dropInfo)
        {
            DragDropHelpers.StandardDrop(dropInfo, ScopeManager);
        }
    }
}
