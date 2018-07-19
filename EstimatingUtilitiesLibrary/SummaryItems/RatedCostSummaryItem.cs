﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;

namespace EstimatingUtilitiesLibrary.SummaryItems
{
    public class RatedCostSummaryItem : TECObject
    {
        #region Fields
        private ICost _ratedCost;

        private double _length;

        private double _totalCost;
        private double _totalLabor;
        #endregion

        //Constructor
        public RatedCostSummaryItem(ICost ratedCost, double length) : base(Guid.NewGuid())
        {
            _ratedCost = ratedCost;
            _length = length;
            updateTotals();
        }

        #region Properties
        public ICost RatedCost
        {
            get { return _ratedCost; }
        }

        public double Length
        {
            get { return _length; }
            private set
            {
                _length = value;
                raisePropertyChanged("Length");
            }
        }

        public double TotalCost
        {
            get { return _totalCost; }
            private set
            {
                _totalCost = value;
                raisePropertyChanged("TotalCost");
            }
        }
        public double TotalLabor
        {
            get { return _totalLabor; }
            private set
            {
                _totalLabor = value;
                raisePropertyChanged("TotalLabor");
            }
        }
        #endregion

        #region Methods
        public CostBatch AddLength(double length)
        {
            Length += length;
            return updateTotals();
        }
        public CostBatch RemoveLength(double length)
        {
            Length -= length;
            return updateTotals();
        }

        private CostBatch updateTotals()
        {
            double newCost = (RatedCost.Cost * Length);
            double newLabor = (RatedCost.Labor * Length);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostBatch(deltaCost, deltaLabor, RatedCost.Type);
        }
        #endregion
    }
}
