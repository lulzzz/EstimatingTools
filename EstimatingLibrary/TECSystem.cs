﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSystem : TECScope
    {//TECSystem is the largest encapsulating object in the System-Equipment hierarchy, offering a specific structure for the needs of the estimating tool. A seperate hierarchy exists for TECScopeBranch as a more generic object.
        #region Properties
        private ObservableCollection<TECEquipment> _equipment;
        public ObservableCollection<TECEquipment> Equipment
        {
            get { return _equipment; }
            set
            {
                    var temp = this.Copy();
                    _equipment = value;
                    NotifyPropertyChanged("Equipment", temp, this);
                    subscribeToEquipment();
            }
        }

        public double BudgetPriceModifier
        {
            get { return _budgetPriceModifier; }
            set
            {
                var temp = this.Copy();
                if(_budgetPriceModifier != value)
                {
                    if (value < 0)
                    {
                        _budgetPriceModifier = -1;
                    }
                    else
                    {
                        _budgetPriceModifier = value;
                    }
                    NotifyPropertyChanged("BudgetPriceModifier", temp, this);
                    RaisePropertyChanged("TotalBudgetPrice");
                    RaisePropertyChanged("BudgetUnitPrice");
                }
            }
        }
        private double _budgetPriceModifier;
        public double BudgetUnitPrice
        {
            get
            {
                double price = 0;
                bool priceExists = false;
                if (BudgetPriceModifier >= 0 )
                {
                    price += BudgetPriceModifier;
                    priceExists = true;
                }
                foreach (TECEquipment equip in Equipment)
                {
                    if (equip.TotalBudgetPrice >= 0)
                    {
                        price += (equip.TotalBudgetPrice);
                        priceExists = true;
                    }
                }
                if (priceExists)
                { return price; }
                else
                { return -1; }
            }
        }
        new public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
                RaisePropertyChanged("TotalBudgetPrice");             
            }
        }
        public int EquipmentQuantity
        {
            get
            {
                int equipQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                { equipQuantity += equip.Quantity; }
                return equipQuantity;
            }
        }
        public int SubScopeQuantity
        {
            get
            {
                int ssQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                { ssQuantity += (equip.SubScopeQuantity * equip.Quantity); }
                return ssQuantity;
            }
        }

        public double TotalBudgetPrice
        {
            get { return (Quantity * BudgetUnitPrice); }
        }
        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        public ObservableCollection<TECSubScope> SubScope
        {
            get
            {
                var outSubScope = new ObservableCollection<TECSubScope>();
                foreach(TECEquipment equip in Equipment)
                {
                    foreach(TECSubScope sub in equip.SubScope)
                    {
                        outSubScope.Add(sub);
                    }
                }
                return outSubScope;
            }
        }
        #endregion //Properties

        #region Constructors
        public TECSystem(Guid guid) : base(guid)
        {
            _budgetPriceModifier = -1;
            _equipment = new ObservableCollection<TECEquipment>();

            Equipment.CollectionChanged += Equipment_CollectionChanged;
        }
        
        public TECSystem() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECSystem(TECSystem sourceSystem) : this()
        {
            foreach (TECEquipment equipment in sourceSystem.Equipment)
            { Equipment.Add(new TECEquipment(equipment)); }
            _budgetPriceModifier = sourceSystem.BudgetPriceModifier;
            this.copyPropertiesFromScope(sourceSystem);
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECSystem outSystem = new TECSystem();
            outSystem._guid = Guid;
            foreach (TECEquipment equipment in this.Equipment)
            { outSystem.Equipment.Add(equipment.Copy() as TECEquipment); }
            outSystem._budgetPriceModifier = this.BudgetPriceModifier;
            outSystem.copyPropertiesFromScope(this);
            return outSystem;
        }

        public override object DragDropCopy()
        {
            TECSystem outSystem = new TECSystem(this);
            return outSystem;
        }

        private double getMaterialCost()
        {
            double cost = 0;
            foreach(TECEquipment equipment in this.Equipment)
            {
                cost += equipment.MaterialCost;
            }
            return cost;
        }
        private double getLaborCost()
        {
            double cost = 0;
            foreach (TECEquipment equipment in this.Equipment)
            {
                cost += equipment.LaborCost;
            }
            return cost;
        }

        private void EquipmentChanged(string name)
        {
            if(name == "Quantity")
            {
                RaisePropertyChanged("SubScopeQuantity");
                RaisePropertyChanged("TotalBudgetPrice");
                RaisePropertyChanged("BudgetUnitPrice");
                RaisePropertyChanged("EquipmentQuantity");
            } else if (name == "SubScopeQuantity")
            {
                RaisePropertyChanged("SubScopeQuantity");
            } else if (name == "BudgetUnitPrice")
            {
                RaisePropertyChanged("TotalBudgetPrice");
                RaisePropertyChanged("BudgetUnitPrice");
            } else if (name == "TotalPoints")
            {
                RaisePropertyChanged("TotalPoints");
            } else if (name == "TotalDevices")
            {
                RaisePropertyChanged("TotalDevices");
            }
            else if (name == "SubLength")
            {
                RaisePropertyChanged("SubLength");
            }
        }

        private void Equipment_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("EquipmentQuantity");
            RaisePropertyChanged("BudgetUnitPrice");
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                    checkForTotalsInEquipment(item as TECEquipment);
                }

            } else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender);
            }

            subscribeToEquipment();
        }

        private void subscribeToEquipment()
        {
            foreach (TECEquipment scope in this.Equipment)
            {
                scope.PropertyChanged += (equipSender, args) => this.EquipmentChanged(args.PropertyName);
            }
        }

        private void checkForTotalsInEquipment(TECEquipment equipment)
        {
            foreach (TECSubScope sub in equipment.SubScope)
            {
                if (sub.Points.Count > 0)
                {
                    RaisePropertyChanged("TotalPoints");
                }
                if (sub.Devices.Count > 0)
                {
                    RaisePropertyChanged("TotalDevices");
                }
            }
        }

        #endregion

    }
}
