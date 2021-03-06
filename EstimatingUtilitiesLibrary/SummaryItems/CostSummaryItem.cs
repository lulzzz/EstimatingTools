﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;

namespace EstimatingUtilitiesLibrary.SummaryItems
{
    public class CostSummaryItem : TECObject
    {
        private ICost _cost;
        public ICost Cost
        {
            get { return _cost; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            private set
            {
                _quantity = value;
                raisePropertyChanged("Quantity");
            }
        }

        private double _totalCost;
        public double TotalCost
        {
            get
            {
                return _totalCost;
            }
            private set
            {
                _totalCost = value;
                raisePropertyChanged("TotalCost");
            }
        }

        private double _totalLabor;
        public double TotalLabor
        {
            get
            {
                return _totalLabor;
            }
            private set
            {
                _totalLabor = value;
                raisePropertyChanged("TotalLabor");
            }
        }

        public CostSummaryItem(ICost cost) : base(Guid.NewGuid())
        {
            _cost = cost;
            if (cost is TECMisc misc)
            {
                _quantity = misc.Quantity;
            }
            else
            {
                _quantity = 1;
            }
            updateTotals();
        }

        public CostBatch AddQuantity(int quantity)
        {
            Quantity += quantity;
            return updateTotals();
        }
        public CostBatch RemoveQuantity(int quantity)
        {
            Quantity -= quantity;
            return updateTotals();
        }
        public CostBatch Refresh()
        {
            return updateTotals();
        }

        private CostBatch updateTotals()
        {
            double newCost = (Cost.Cost * Quantity);
            double newLabor = (Cost.Labor * Quantity);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostBatch(deltaCost, deltaLabor, Cost.Type);
        }
    }
}
