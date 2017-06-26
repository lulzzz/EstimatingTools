﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public class MiscCostsSummaryVM : ViewModelBase
    {
        #region Properties
        public TECBid Bid { get; private set; }

        public CostType CostType { get; private set; }

        private ObservableCollection<CostSummaryItem> _miscCostSummaryItems;
        public ObservableCollection<CostSummaryItem> MiscCostSummaryItems
        {
            get { return _miscCostSummaryItems; }
            set
            {
                _miscCostSummaryItems = value;
                RaisePropertyChanged("MiscCostSummaryItems");
            }
        }

        private Dictionary<Guid, CostSummaryItem> assCostDictionary;

        private ObservableCollection<CostSummaryItem> _assCostSummaryItems;
        public ObservableCollection<CostSummaryItem> AssCostSummaryItems
        {
            get { return _assCostSummaryItems; }
            set
            {
                _assCostSummaryItems = value;
                RaisePropertyChanged("AssCostSummaryItems");
            }
        }

        private double _miscCostSubTotalCost;
        public double MiscCostSubTotalCost
        {
            get { return _miscCostSubTotalCost; }
            set
            {
                _miscCostSubTotalCost = value;
                RaisePropertyChanged("MiscCostSubTotalCost");
            }
        }

        private double _miscCostSubTotalLabor;
        public double MiscCostSubTotalLabor
        {
            get { return _miscCostSubTotalLabor; }
            set
            {
                _miscCostSubTotalLabor = value;
                RaisePropertyChanged("MiscCostSubTotalLabor");
            }
        }

        private double _assCostSubTotalCost;
        public double AssCostSubTotalCost
        {
            get { return _assCostSubTotalCost; }
            set
            {
                _assCostSubTotalCost = value;
                RaisePropertyChanged("AssCostSubTotalCost");
            }
        }

        private double _assCostSubTotalLabor;
        public double AssCostSubTotalLabor
        {
            get { return _assCostSubTotalLabor; }
            set
            {
                _assCostSubTotalLabor = value;
                RaisePropertyChanged("AssCostSubTotalLabor");
            }
        }
        #endregion

        public MiscCostsSummaryVM(TECBid bid, CostType type)
        {
            CostType = type;
            reinitialize(bid);
        }

        public void Refresh(TECBid bid)
        {
            reinitialize(bid);
        }

        private void reinitialize(TECBid bid)
        {
            Bid = bid;
            
            MiscCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            assCostDictionary = new Dictionary<Guid, CostSummaryItem>();
            AssCostSummaryItems = new ObservableCollection<CostSummaryItem>();

            MiscCostSubTotalCost = 0;
            MiscCostSubTotalLabor = 0;
            AssCostSubTotalCost = 0;
            AssCostSubTotalLabor = 0;

            foreach(TECMisc misc in bid.MiscCosts)
            {
                AddMiscCost(misc);
            }
            foreach(TECSystem typical in bid.Systems)
            {
                AddTypicalSystem(typical);
                foreach(TECSystem instance in typical.SystemInstances)
                {
                    AddInstanceSystem(instance);
                }
            }
        }

        #region Add/Remove
        public void AddTypicalSystem(TECSystem system)
        {
            foreach(TECMisc misc in system.MiscCosts)
            {
                AddMiscCost(misc, system);
            }
        }
        public void RemoveTypicalSystem(TECSystem system)
        {
            foreach(TECMisc misc in system.MiscCosts)
            {
                RemoveMiscCost(misc);
            }
        }

        public void AddInstanceSystem(TECSystem system)
        {
            foreach (TECCost cost in system.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECEquipment equip in system.Equipment)
            {
                AddEquipment(equip);
            }
        }
        public void RemoveInstanceSystem(TECSystem system)
        {
            foreach (TECCost cost in system.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECEquipment equip in system.Equipment)
            {
                RemoveEquipment(equip);
            }
        }

        public void AddEquipment(TECEquipment equip)
        {
            foreach (TECCost cost in equip.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECSubScope ss in equip.SubScope)
            {
                AddSubScope(ss);
            }
        }
        public void RemoveEquipment(TECEquipment equip)
        {
            foreach (TECCost cost in equip.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECSubScope ss in equip.SubScope)
            {
                RemoveSubScope(ss);
            }
        }

        public void AddSubScope(TECSubScope subScope)
        {
            foreach (TECCost cost in subScope.AssociatedCosts)
            {
                AddAssCost(cost);
            }
            foreach (TECPoint point in subScope.Points)
            {
                AddPoint(point);
            }
        }
        public void RemoveSubScope(TECSubScope subScope)
        {
            foreach (TECCost cost in subScope.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
            foreach (TECPoint point in subScope.Points)
            {
                RemovePoint(point);
            }
        }

        public void AddDevice(TECDevice dev)
        {
            foreach(TECCost cost in dev.AssociatedCosts)
            {
                AddAssCost(cost);
            }
        }
        public void RemoveDevice(TECDevice dev)
        {
            foreach(TECCost cost in dev.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
        }

        public void AddPoint(TECPoint point)
        {
            foreach(TECCost cost in point.AssociatedCosts)
            {
                AddAssCost(cost);
            }
        }
        public void RemovePoint(TECPoint point)
        {
            foreach(TECCost cost in point.AssociatedCosts)
            {
                RemoveAssCost(cost);
            }
        }

        public void AddAssCost(TECCost cost)
        {
            if (cost.Type == this.CostType)
            {
                Tuple<double, double> delta = ElectricalMaterialSummaryVM.AddCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
        }
        public void RemoveAssCost(TECCost cost)
        {
            if (cost.Type == this.CostType)
            {
                Tuple<double, double> delta = ElectricalMaterialSummaryVM.RemoveCost(cost, assCostDictionary, AssCostSummaryItems);
                AssCostSubTotalCost += delta.Item1;
                AssCostSubTotalLabor += delta.Item2;
            }
        }

        public void AddMiscCost(TECMisc misc, TECSystem system = null)
        {
            if (misc.Type == this.CostType)
            {
                CostSummaryItem miscItem = null;
                if (system != null)
                {
                    miscItem = new CostSummaryItem(misc, system);
                }
                else
                {
                    miscItem = new CostSummaryItem(misc);
                }
                MiscCostSubTotalCost += miscItem.TotalCost;
                MiscCostSubTotalLabor += miscItem.TotalLabor;
                MiscCostSummaryItems.Add(miscItem);
            }
        }
        public void RemoveMiscCost(TECMisc misc)
        {
            if (misc.Type == this.CostType)
            {
                CostSummaryItem itemToRemove = null;
                foreach (CostSummaryItem miscItem in MiscCostSummaryItems)
                {
                    if (misc.Guid == miscItem.Cost.Guid)
                    {
                        MiscCostSubTotalCost -= miscItem.TotalCost;
                        MiscCostSubTotalLabor -= miscItem.TotalLabor;
                        itemToRemove = miscItem;
                        break;
                    }
                }
                if (itemToRemove != null)
                {
                    MiscCostSummaryItems.Remove(itemToRemove);
                }
                else
                {
                    throw new InvalidOperationException("Misc not found in summary items.");
                }
            }
        }
        #endregion
    }
}