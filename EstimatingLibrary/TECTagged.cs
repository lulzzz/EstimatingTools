﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EstimatingLibrary
{
    public abstract class TECTagged : TECObject, IRelatable, ICatalogContainer
    {
        #region Properties

        protected string _name;
        protected string _description;
        
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != Name)
                {
                    var old = Name;
                    _name = value;
                    notifyCombinedChanged(Change.Edit, "Name", this, value, old);
                }
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != Description)
                {
                    var old = Description;
                    _description = value;
                    notifyCombinedChanged(Change.Edit, "Description", this, value, old);
                }
            }
        }

        public ObservableCollection<TECTag> Tags { get; } = new ObservableCollection<TECTag>();
        
        public RelatableMap PropertyObjects
        {
            get
            {
                return propertyObjects();
            }
        }
        public RelatableMap LinkedObjects
        {
            get
            {
                RelatableMap map = linkedObjects();
                return map;
            }
        }
        #endregion

        #region Constructors
        public TECTagged(Guid guid) : base(guid)
        {
            _name = "";
            _description = "";
            _guid = guid;

            Tags.CollectionChanged += (sender, args) => taggedCollectionChanged(sender, args, "Tags");
        }

        #endregion 

        #region Methods
        protected void copyPropertiesFromTagged(TECTagged tagged)
        {
            _name = tagged.Name;
            _description = tagged.Description;
            Tags.ObservablyClear();
            foreach (TECTag tag in tagged.Tags)
            { Tags.Add(tag); }
        }
        protected virtual void taggedCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e, string propertyName)
        //Is virtual so that it can be overridden in TECTypical
        {
            CollectionChangedHandlers.CollectionChangedHandler(sender, e, propertyName, this, notifyCombinedChanged);
        }
        
        protected virtual RelatableMap propertyObjects()
        {
            RelatableMap saveList = new RelatableMap();
            saveList.AddRange(this.Tags, "Tags");
            return saveList;
        }
        protected virtual RelatableMap linkedObjects()
        {
            RelatableMap relatedList = new RelatableMap();
            relatedList.AddRange(this.Tags, "Tags");
            return relatedList;
        }

        #endregion Methods

        #region ICatalogContainer
        public virtual bool RemoveCatalogItem<T>(T item, T replacement) where T : class, ICatalog
        {
            if (item is TECTag tag)
            {
                return (CommonUtilities.OptionallyReplaceAll(tag, this.Tags, replacement as TECTag));
            }
            return false;
        }
        #endregion
    }
}