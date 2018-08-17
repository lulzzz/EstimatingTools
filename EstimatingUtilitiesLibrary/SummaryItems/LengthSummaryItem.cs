﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using System;

namespace EstimatingUtilitiesLibrary.SummaryItems
{
    public class LengthSummaryItem : TECObject
    {
        #region Fields
        private TECElectricalMaterial _material;

        private double _length;

        private double _totalCost;
        private double _totalLabor;
        #endregion

        //Cosntructor
        public LengthSummaryItem(TECElectricalMaterial material, double length) : base(Guid.NewGuid())
        {
            _material = material;
            _length = length;
            updateTotals();
        }

        #region Properties
        public TECElectricalMaterial Material
        {
            get { return _material; }
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

        public virtual double UnitCost
        {
            get { return this.Material.Cost; }
        }
        public virtual double UnitLabor
        {
            get { return this.Material.Labor; }
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
            set
            {
                double old = _totalLabor;
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
            double newCost = (this.UnitCost * Length);
            double newLabor = (this.UnitLabor * Length);

            double deltaCost = newCost - TotalCost;
            double deltaLabor = newLabor - TotalLabor;

            TotalCost = newCost;
            TotalLabor = newLabor;

            return new CostBatch(deltaCost, deltaLabor, CostType.Electrical);
        }
        #endregion
    }
}
