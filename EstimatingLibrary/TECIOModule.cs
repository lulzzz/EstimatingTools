﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.ObjectModel;

namespace EstimatingLibrary
{
    public class TECIOModule : TECHardware, ICatalog<TECIOModule>, ICatalogContainer
    {
        private const CostType COST_TYPE = CostType.TEC;

        public ObservableCollection<TECIO> IO { get; } = new ObservableCollection<TECIO>();

        public IOCollection IOCollection { get { return new IOCollection(IO); } }
        
        public TECIOModule(Guid guid, TECManufacturer manufacturer) : base(guid, manufacturer, COST_TYPE)
        {
            IO.CollectionChanged += ioCollectionChanged;
        }
        public TECIOModule(TECManufacturer manufacturer) : this(Guid.NewGuid(), manufacturer) { }
        public TECIOModule(TECIOModule ioModuleSource) : this(ioModuleSource.Manufacturer)
        {
            copyPropertiesFromHardware(ioModuleSource);
            IO = ioModuleSource.IO;
            IO.CollectionChanged += ioCollectionChanged;
        }

        private void ioCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            collectionChanged(sender, e, "IO");
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        {
            CollectionChangedHandlers.CollectionChangedHandler(sender, e, propertyName, this,
                notifyCombinedChanged, notifyReorder: false);
        }

        protected override RelatableMap propertyObjects()
        {
            RelatableMap saveMap = new RelatableMap();
            saveMap.AddRange(base.propertyObjects());
            saveMap.AddRange(IO, "IO");
            return saveMap;
        }

        public TECIOModule CatalogCopy()
        {
            return new TECIOModule(this);
        }
    }
}
