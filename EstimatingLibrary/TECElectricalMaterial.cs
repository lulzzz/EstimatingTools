﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace EstimatingLibrary
{
    public class TECElectricalMaterial : TECCost, ICatalog<TECElectricalMaterial>, IDDCopiable
    {
        #region Properties
        private ObservableCollection<TECAssociatedCost> _ratedCosts;
        public ObservableCollection<TECAssociatedCost> RatedCosts
        {
            get { return _ratedCosts; }
            set
            {
                var old = RatedCosts;
                RatedCosts.CollectionChanged -= (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
                _ratedCosts = value;
                RatedCosts.CollectionChanged += (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
                notifyCombinedChanged(Change.Edit, "RatedCosts", this, value, old);
            }
        }
        #endregion

        public TECElectricalMaterial(Guid guid) : base(guid, CostType.Electrical)
        {
            _ratedCosts = new ObservableCollection<TECAssociatedCost>();
            RatedCosts.CollectionChanged += (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
        }
        public TECElectricalMaterial() : this(Guid.NewGuid()) { }
        public TECElectricalMaterial(TECElectricalMaterial materialSource) : this()
        {
            copyPropertiesFromCost(materialSource);
            var ratedCosts = new ObservableCollection<TECAssociatedCost>(materialSource.RatedCosts);
            RatedCosts.CollectionChanged -= (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
            _ratedCosts = ratedCosts;
            RatedCosts.CollectionChanged += (sender, args) => RatedCosts_CollectionChanged(sender, args, "RatedCosts");
        }

        public object DragDropCopy(TECScopeManager scopeManager)
        {
            return this;
        }

        public CostBatch GetCosts(double length)
        {
            CostBatch costBatch = new CostBatch(Cost, Labor, Type);
            foreach (TECCost ratedCost in RatedCosts)
            {
                costBatch.AddCost(ratedCost);
            }
            costBatch *= length;
            foreach (TECCost assocCost in AssociatedCosts)
            {
                costBatch.AddCost(assocCost);
            }
            return costBatch;
        }
        protected override SaveableMap propertyObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.propertyObjects());
            saveList.AddRange(this.RatedCosts.Distinct(), "RatedCosts");
            return saveList;
        }
        protected override SaveableMap linkedObjects()
        {
            SaveableMap saveList = new SaveableMap();
            saveList.AddRange(base.linkedObjects());
            saveList.AddRange(this.RatedCosts.Distinct(), "RatedCosts");
            return saveList;
        }

        private void RatedCosts_CollectionChanged(object sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (TECCost item in e.NewItems)
                {
                    notifyCombinedChanged(Change.Add, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (TECCost item in e.OldItems)
                {
                    notifyCombinedChanged(Change.Remove, propertyName, this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                notifyCombinedChanged(Change.Edit, propertyName, this, sender, sender);
            }
        }

        public TECElectricalMaterial CatalogCopy()
        {
            return new TECElectricalMaterial(this);
        }
    }
}
