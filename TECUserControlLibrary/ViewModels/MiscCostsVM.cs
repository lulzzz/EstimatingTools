﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MiscCostsVM : TECVMBase, IDropTarget
    {
        private ObservableCollection<TECMisc> _tecCostCollection;
        public ObservableCollection<TECMisc> TECCostCollection
        {
            get { return _tecCostCollection; }
            set
            {
                _tecCostCollection = value;
                RaisePropertyChanged("TECCostCollection");
            }
        }

        private ObservableCollection<TECMisc> _electricalCostCollection;
        public ObservableCollection<TECMisc> ElectricalCostCollection
        {
            get { return _electricalCostCollection; }
            set
            {
                _electricalCostCollection = value;
                RaisePropertyChanged("ElectricalCostCollection");
            }
        }

        private ObservableCollection<TECMisc> sourceCollection;

        public Visibility QuantityVisibility { get; set; }

        private TECMisc _selected;
        public TECMisc Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged("Selected");
                SelectionChanged?.Invoke(value);
            }
        }

        private string _miscName;
        public string MiscName
        {
            get { return _miscName; }
            set
            {
                _miscName = value;
                RaisePropertyChanged("MiscName");
            }
        }

        private double _cost;
        public double Cost
        {
            get { return _cost; }
            set
            {
                _cost = value;
                RaisePropertyChanged("Cost");
            }
        }

        private double _labor;
        public double Labor
        {
            get { return _labor; }
            set
            {
                _labor = value;
                RaisePropertyChanged("Labor");
            }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }

        private CostType _miscType;
        public CostType MiscType
        {
            get { return _miscType; }
            set
            {
                _miscType = value;
                RaisePropertyChanged("MiscType");
            }
        }

        private TECBid _bid;
        private TECTemplates _templates;
        private TECSystem _system;

        public ICommand AddNewCommand { get; private set; }
        public RelayCommand<TECMisc> DeleteCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MiscCostsVM class.
        /// </summary>
        public MiscCostsVM(TECScopeManager scopeManager)
        {
            Refresh(scopeManager);
            setup();
        }
        
        public MiscCostsVM(TECSystem system)
        {
            Refresh(system);
            setup();
        }

        public event Action<TECObject> SelectionChanged;

        public void Refresh(TECScopeManager scopeManager)
        {
            var bid = scopeManager as TECBid;
            var templates = scopeManager as TECTemplates;
            if (bid != null)
            {
                QuantityVisibility = Visibility.Visible;
                if (_bid != null)
                {
                    _bid.MiscCosts.CollectionChanged -= MiscCosts_CollectionChanged;
                }

                bid.MiscCosts.CollectionChanged += MiscCosts_CollectionChanged;
                _bid = bid;
                sourceCollection = bid.MiscCosts;
                populateCollections();
            }
            else if (templates != null)
            {
                QuantityVisibility = Visibility.Collapsed;
                if (templates != null)
                {
                    templates.MiscCostTemplates.CollectionChanged -= MiscCosts_CollectionChanged;
                }

                templates.MiscCostTemplates.CollectionChanged += MiscCosts_CollectionChanged;
                _templates = templates;
                sourceCollection = templates.MiscCostTemplates;
                populateCollections();
            }


        }
        public void Refresh(TECSystem system)
        {
            QuantityVisibility = Visibility.Visible;
            if (_bid != null)
            {
                system.MiscCosts.CollectionChanged -= MiscCosts_CollectionChanged;
            }

            system.MiscCosts.CollectionChanged += MiscCosts_CollectionChanged;
            _system = system;
            sourceCollection = system.MiscCosts;
            populateCollections();
        }

        private void setup()
        {
            AddNewCommand = new RelayCommand(addNewExecute, addNewCanExecute);
            DeleteCommand = new RelayCommand<TECMisc>(deleteExecute, canDelete);
            MiscType = CostType.TEC;
            MiscName = "";
            Cost = 0;
            Labor = 0;
            Quantity = 1;
        }

        private bool addNewCanExecute()
        {
            return (MiscName != "");
        }
        private void addNewExecute()
        {
            bool isTypical = (_system != null && _system is TECTypical);
            TECMisc newMisc = new TECMisc(MiscType, isTypical);
            newMisc.Name = MiscName;
            newMisc.Cost = Cost;
            newMisc.Labor = Labor;
            newMisc.Quantity = Quantity;

            sourceCollection.Add(newMisc);
        }

        private void deleteExecute(TECMisc obj)
        {
            sourceCollection.Remove(obj);
        }
        private bool canDelete(TECMisc arg)
        {
            return true;
        }

        private void MiscCosts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(TECMisc misc in e.NewItems)
                {
                    handleAddMisc(misc);
                }
            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECMisc misc in e.OldItems)
                { 
                    handleRemoveMisc(misc);
                }
            }
        }

        private void Misc_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var args = e as TECChangedEventArgs;
            if(e.PropertyName == "Type")
            {
                TECMisc old = args.OldValue as TECMisc;
                TECMisc current = sender as TECMisc;
                if (old.Type == CostType.TEC){
                    TECCostCollection.Remove(sender as TECMisc);
                } else if (old.Type == CostType.Electrical)
                {
                    ElectricalCostCollection.Remove(sender as TECMisc);
                }

                if(current.Type == CostType.TEC)
                {
                    TECCostCollection.Add(current);
                }
                else if (current.Type == CostType.Electrical)
                {
                    ElectricalCostCollection.Add(current);
                }
            }
        }

        private void populateCollections()
        {
            TECCostCollection = new ObservableCollection<TECMisc>();
            ElectricalCostCollection = new ObservableCollection<TECMisc>();
            foreach(TECMisc misc in sourceCollection)
            {
                handleAddMisc(misc);
            }
        }

        private void handleAddMisc(TECMisc misc)
        {
            misc.PropertyChanged += Misc_PropertyChanged;
            if (misc.Type == CostType.TEC)
            {
                TECCostCollection.Add(misc);
            }
            else if (misc.Type == CostType.Electrical)
            {
                ElectricalCostCollection.Add(misc);
            }
        }
        private void handleRemoveMisc(TECMisc misc)
        {
            misc.PropertyChanged -= Misc_PropertyChanged;
            if (misc.Type == CostType.TEC)
            {
                TECCostCollection.Remove(misc);
            }
            else if (misc.Type == CostType.Electrical)
            {
                ElectricalCostCollection.Remove(misc);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.StandardDragOver(dropInfo,
                type =>
                {
                    if (type == typeof(TECMisc) && dropInfo.Data is TECCost)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
            );
        }
        public void Drop(IDropInfo dropInfo)
        {
            TECScopeManager scopeManager;
            if (_templates != null)
            {
                scopeManager = _templates;
            }
            else
            {
                scopeManager = _bid;
            }
            bool isTypical = (_system != null && _system is TECTypical);
            TECMisc newMisc = null;
            if(dropInfo.Data is TECMisc misc)
            {
                newMisc = new TECMisc(misc, isTypical);
            } else if (dropInfo.Data is TECCost cost)
            {
                newMisc = new TECMisc(cost, isTypical);
            } else
            {
                throw new NotImplementedException();
            }
            sourceCollection.Add(newMisc);
        }
    }
}