﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.SummaryItems;
using GalaSoft.MvvmLight;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels.Interfaces;

namespace TECUserControlLibrary.ViewModels
{
    public class LengthSummaryVM : ViewModelBase, IComponentSummaryVM
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Fields and Properties
        private readonly Dictionary<Guid, LengthSummaryItem> lengthDictionary = new Dictionary<Guid, LengthSummaryItem>();
        private readonly Dictionary<Guid, CostSummaryItem> assocCostDictionary = new Dictionary<Guid, CostSummaryItem>();
        private readonly Dictionary<Guid, RatedCostSummaryItem> ratedCostDictionary = new Dictionary<Guid, RatedCostSummaryItem>();

        protected readonly ObservableCollection<LengthSummaryItem> _lengthSummaryItems = new ObservableCollection<LengthSummaryItem>();
        private readonly ObservableCollection<CostSummaryItem> _assocTECItems = new ObservableCollection<CostSummaryItem>();
        private readonly ObservableCollection<CostSummaryItem> _assocElecItems = new ObservableCollection<CostSummaryItem>();
        private readonly ObservableCollection<RatedCostSummaryItem> _ratedTECItems = new ObservableCollection<RatedCostSummaryItem>();
        private readonly ObservableCollection<RatedCostSummaryItem> _ratedElecItems = new ObservableCollection<RatedCostSummaryItem>();

        private double _lengthCostTotal = 0;
        private double _lengthLaborTotal = 0;
        private double _assocTECCostTotal = 0;
        private double _assocTECLaborTotal = 0;
        private double _assocElecCostTotal = 0;
        private double _assocElecLaborTotal = 0;
        private double _ratedTECCostTotal = 0;
        private double _ratedTECLaborTotal = 0;
        private double _ratedElecCostTotal = 0;
        private double _ratedElecLaborTotal = 0;

        public ReadOnlyObservableCollection<LengthSummaryItem> LengthSummaryItems { get; }
        public ReadOnlyObservableCollection<CostSummaryItem> AssocTECItems { get; }
        public ReadOnlyObservableCollection<CostSummaryItem> AssocElecItems { get; }
        public ReadOnlyObservableCollection<RatedCostSummaryItem> RatedTECItems { get; }
        public ReadOnlyObservableCollection<RatedCostSummaryItem> RatedElecItems { get; }

        public double LengthCostTotal
        {
            get { return _lengthCostTotal; }
            protected set
            {
                _lengthCostTotal = value;
                RaisePropertyChanged("LengthCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double LengthLaborTotal
        {
            get { return _lengthLaborTotal; }
            protected set
            {
                _lengthLaborTotal = value;
                RaisePropertyChanged("LengthLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }
        public double AssocTECCostTotal
        {
            get { return _assocTECCostTotal; }
            private set
            {
                _assocTECCostTotal = value;
                RaisePropertyChanged("AssocTECCostTotal");
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double AssocTECLaborTotal
        {
            get { return _assocTECLaborTotal; }
            private set
            {
                _assocTECLaborTotal = value;
                RaisePropertyChanged("AssocTECLaborTotal");
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double AssocElecCostTotal
        {
            get { return _assocElecCostTotal; }
            private set
            {
                _assocElecCostTotal = value;
                RaisePropertyChanged("AssocElecCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double AssocElecLaborTotal
        {
            get { return _assocElecLaborTotal; }
            private set
            {
                _assocElecLaborTotal = value;
                RaisePropertyChanged("AssocElecLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }
        public double RatedTECCostTotal
        {
            get { return _ratedTECCostTotal; }
            private set
            {
                _ratedTECCostTotal = value;
                RaisePropertyChanged("RatedTECCostTotal");
                RaisePropertyChanged("TotalTECCost");
            }
        }
        public double RatedTECLaborTotal
        {
            get { return _ratedTECLaborTotal; }
            private set
            {
                _ratedTECLaborTotal = value;
                RaisePropertyChanged("RatedTECLaborTotal");
                RaisePropertyChanged("TotalTECLabor");
            }
        }
        public double RatedElecCostTotal
        {
            get { return _ratedElecCostTotal; }
            private set
            {
                _ratedElecCostTotal = value;
                RaisePropertyChanged("RatedElecCostTotal");
                RaisePropertyChanged("TotalElecCost");
            }
        }
        public double RatedElecLaborTotal
        {
            get { return _ratedElecLaborTotal; }
            private set
            {
                _ratedElecLaborTotal = value;
                RaisePropertyChanged("RatedElecLaborTotal");
                RaisePropertyChanged("TotalElecLabor");
            }
        }

        public double TotalTECCost
        {
            get
            {
                return (AssocTECCostTotal + RatedTECCostTotal);
            }
        }
        public double TotalTECLabor
        {
            get
            {
                return (AssocTECLaborTotal + RatedTECLaborTotal);
            }
        }
        public double TotalElecCost
        {
            get
            {
                return (LengthCostTotal + AssocElecCostTotal + RatedElecCostTotal);
            }
        }
        public double TotalElecLabor
        {
            get
            {
                return (LengthLaborTotal + AssocElecLaborTotal + RatedElecLaborTotal);
            }
        }
        #endregion
        
        public LengthSummaryVM()
        {
            LengthSummaryItems = new ReadOnlyObservableCollection<LengthSummaryItem>(_lengthSummaryItems);
            AssocTECItems = new ReadOnlyObservableCollection<CostSummaryItem>(_assocTECItems);
            AssocElecItems = new ReadOnlyObservableCollection<CostSummaryItem>(_assocElecItems);
            RatedTECItems = new ReadOnlyObservableCollection<RatedCostSummaryItem>(_ratedTECItems);
            RatedElecItems = new ReadOnlyObservableCollection<RatedCostSummaryItem>(_ratedElecItems);
        }

        #region Methods
        public CostBatch AddRun(TECElectricalMaterial material, double length)
        {
            CostBatch deltas = AddLength(material, length);
            foreach (ICost cost in material.AssociatedCosts)
            {
                deltas += addAssocCost(cost);
            }
            return deltas;
        }
        public CostBatch RemoveRun(TECElectricalMaterial material, double length)
        {
            CostBatch deltas = RemoveLength(material, length);
            foreach (ICost cost in material.AssociatedCosts)
            {
                deltas += removeAssocCost(cost);
            }
            return deltas;
        }
        public CostBatch AddLength(TECElectricalMaterial material, double length)
        {
            if (length < 0)
            {
                logger.Error("Length needs to be greater than 0 when adding to length summary. " +
                    "Failed to add length. Obj: {0}", material.Name);
                return new CostBatch();
            }

            CostBatch deltas = new CostBatch();
            bool containsItem = lengthDictionary.ContainsKey(material.Guid);
            if (containsItem)
            {
                LengthSummaryItem item = lengthDictionary[material.Guid];
                CostBatch delta = item.AddLength(length);
                LengthCostTotal += delta.GetCost(CostType.Electrical);
                LengthLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;
            }
            else
            {
                LengthSummaryItem item = new LengthSummaryItem(material, length);
                lengthDictionary.Add(material.Guid, item);
                _lengthSummaryItems.Add(item);
                LengthCostTotal += item.TotalCost;
                LengthLaborTotal += item.TotalLabor;
                deltas += new CostBatch(item.TotalCost, item.TotalLabor, CostType.Electrical);
            }
            foreach(ICost cost in material.RatedCosts)
            {
                deltas += addRatedCost(cost, length);
            }
            return deltas;
        }
        public CostBatch RemoveLength(TECElectricalMaterial material, double length)
        {
            if (length < 0)
            {
                logger.Error("Length needs to be greater than 0 when removing from length summary. " +
                    "Failed to remove length. Obj: {0}", material.Name);
                return new CostBatch();
            }

            bool containsItem = lengthDictionary.ContainsKey(material.Guid);
            if (containsItem)
            {
                CostBatch deltas = new CostBatch();
                LengthSummaryItem item = lengthDictionary[material.Guid];
                CostBatch delta = item.RemoveLength(length);
                LengthCostTotal += delta.GetCost(CostType.Electrical);
                LengthLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;

                if (item.Length <= 0)
                {
                    _lengthSummaryItems.Remove(item);
                    lengthDictionary.Remove(material.Guid);
                }
                foreach (ICost cost in material.RatedCosts)
                {
                    deltas += removeRatedCost(cost, length);
                }
                return deltas;
            }
            else
            {
                logger.Error("Electrical Material not found. Cannot remove length. Material: {0}",
                    material.Name);
                return new CostBatch();
            }
        }

        protected CostBatch addAssocCost(ICost cost)
        {
            CostBatch deltas = new CostBatch();
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostBatch delta = item.AddQuantity(1);
                AssocTECCostTotal += delta.GetCost(CostType.TEC);
                AssocTECLaborTotal += delta.GetLabor(CostType.TEC);
                AssocElecCostTotal += delta.GetCost(CostType.Electrical);
                AssocElecLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;
            }
            else
            {
                CostSummaryItem item = new CostSummaryItem(cost);
                assocCostDictionary.Add(cost.Guid, item);
                if (cost.Type == CostType.TEC)
                {
                    _assocTECItems.Add(item);
                    AssocTECCostTotal += item.TotalCost;
                    AssocTECLaborTotal += item.TotalLabor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    _assocElecItems.Add(item);
                    AssocElecCostTotal += item.TotalCost;
                    AssocElecLaborTotal += item.TotalLabor;
                }
                deltas += new CostBatch(item.TotalCost, item.TotalLabor, cost.Type);
            }
            return deltas;
        }
        protected CostBatch removeAssocCost(ICost cost)
        {
            bool containsItem = assocCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                CostBatch deltas = new CostBatch();
                CostSummaryItem item = assocCostDictionary[cost.Guid];
                CostBatch delta = item.RemoveQuantity(1);
                AssocTECCostTotal += delta.GetCost(CostType.TEC);
                AssocTECLaborTotal += delta.GetLabor(CostType.TEC);
                AssocElecCostTotal += delta.GetCost(CostType.Electrical);
                AssocElecLaborTotal += delta.GetLabor(CostType.Electrical);
                deltas += delta;

                if (item.Quantity < 1)
                {
                    assocCostDictionary.Remove(cost.Guid);
                    if (cost.Type == CostType.TEC)
                    {
                        _assocTECItems.Remove(item);
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        _assocElecItems.Remove(item);
                    }
                }
                return deltas;
            }
            else
            {
                logger.Error("Associated cost not found. Cannot remove associated cost. Cost: {0}",
                    cost.Name);
                return new CostBatch();
            }
        }
        protected CostBatch addRatedCost(ICost cost, double length)
        {
            bool containsItem = ratedCostDictionary.ContainsKey(cost.Guid);
            if (containsItem)
            {
                RatedCostSummaryItem item = ratedCostDictionary[cost.Guid];
                CostBatch delta = item.AddLength(length);
                if (cost.Type == CostType.TEC)
                {
                    RatedTECCostTotal += delta.GetCost(CostType.TEC);
                    RatedTECLaborTotal += delta.GetLabor(CostType.TEC);
                }
                else if (cost.Type == CostType.Electrical)
                {
                    RatedElecCostTotal += delta.GetCost(CostType.Electrical);
                    RatedElecLaborTotal += delta.GetLabor(CostType.Electrical);
                }
                return delta;
            }
            else
            {
                RatedCostSummaryItem item = new RatedCostSummaryItem(cost, length);
                ratedCostDictionary.Add(cost.Guid, item);
                if (cost.Type == CostType.TEC)
                {
                    _ratedTECItems.Add(item);
                    RatedTECCostTotal += item.TotalCost;
                    RatedTECLaborTotal += item.TotalLabor;
                }
                else if (cost.Type == CostType.Electrical)
                {
                    _ratedElecItems.Add(item);
                    RatedElecCostTotal += item.TotalCost;
                    RatedElecLaborTotal += item.TotalLabor;
                }
                return new CostBatch(item.TotalCost, item.TotalLabor, cost.Type);
            }
        }
        protected CostBatch removeRatedCost(ICost cost, double length)
        {
            if (length > 0)
            {
                bool containsItem = ratedCostDictionary.ContainsKey(cost.Guid);
                if (containsItem)
                {
                    RatedCostSummaryItem item = ratedCostDictionary[cost.Guid];
                    CostBatch delta = item.RemoveLength(length);
                    if (cost.Type == CostType.TEC)
                    {
                        RatedTECCostTotal += delta.GetCost(CostType.TEC);
                        RatedTECLaborTotal += delta.GetLabor(CostType.TEC);
                    }
                    else if (cost.Type == CostType.Electrical)
                    {
                        RatedElecCostTotal += delta.GetCost(CostType.Electrical);
                        RatedElecLaborTotal += delta.GetLabor(CostType.Electrical);
                    }
                    if (item.Length <= 0)
                    {
                        ratedCostDictionary.Remove(cost.Guid);
                        if (cost.Type == CostType.TEC)
                        {
                            _ratedTECItems.Remove(item);
                        }
                        else if (cost.Type == CostType.Electrical)
                        {
                            _ratedElecItems.Remove(item);
                        }
                    }
                    return delta;
                }
                else
                {
                    logger.Error("Rated cost not found. Cannot remove rated cost length. Cost: {0}",
                        cost.Name);
                    return new CostBatch();
                }
            }
            else
            {
                return new CostBatch();
            }
        }
        #endregion
    }
}
