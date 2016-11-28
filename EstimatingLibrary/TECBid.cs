﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECBid : TECObject
    {
        #region Properties
        private string _name;
        private string _bidNumber;
        private DateTime _dueDate;
        private string _salesperson;
        private string _estimator;
        private Guid _infoGuid;
        private TECLabor _labor;
        private ObservableCollection<TECScopeBranch> _scopeTree { get; set; }
        private ObservableCollection<TECSystem> _systems { get; set; }
        private ObservableCollection<TECDevice> _deviceCatalog { get; set; }
        private ObservableCollection<TECManufacturer> _manufacturerCatalog { get; set; }
        private ObservableCollection<TECNote> _notes { get; set; }
        private ObservableCollection<TECExclusion> _exclusions { get; set; }
        private ObservableCollection<TECTag> _tags { get; set; }
        private ObservableCollection<TECDrawing> _drawings { get; set; }
        
        public string Name {
            get { return _name; }
            set
            {
                var temp = Copy();
                _name = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("Name", temp, this);
            }
        }
        public string BidNumber
        {
            get { return _bidNumber; }
            set
            {
                var temp = Copy();
                _bidNumber = value;
                NotifyPropertyChanged("BidNumber", temp, this);
            }
        }
        public DateTime DueDate {
            get { return _dueDate; }
            set
            {
                var temp = Copy();
                _dueDate = value;
                // Call RaisePropertyChanged whenever the property is updated
                NotifyPropertyChanged("DueDate", temp, this);
            }
        }
        public string Salesperson
        {
            get { return _salesperson; }
            set
            {
                var temp = Copy();
                _salesperson = value;
                NotifyPropertyChanged("Salesperson", temp, this);
            }
        }
        public string Estimator
        {
            get { return _estimator; }
            set
            {
                var temp = Copy();
                _estimator = value;
                NotifyPropertyChanged("Estimator", temp, this);
            }
        }
        public Guid InfoGuid
        {
            get { return _infoGuid; }
        }

        public TECLabor Labor
        {
            get { return _labor; }
            set
            {
                var temp = Copy();
                _labor = value;
                NotifyPropertyChanged("Labor", temp, this);
            }
        }

        public double MaterialCost
        {
            get { return getMaterialCost(); }
        }
        public double LaborCost
        {
            get { return getLaborCost(); }
        }

        public ObservableCollection<TECScopeBranch> ScopeTree {
            get { return _scopeTree; }
            set
            {
                var temp = this.Copy();
                ScopeTree.CollectionChanged -= CollectionChanged;
                _scopeTree = value;
                ScopeTree.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ScopeTree", temp, this);
            }
        }
        public ObservableCollection<TECSystem> Systems {
            get { return _systems; }
            set
            {
                var temp = this.Copy();
                Systems.CollectionChanged -= CollectionChanged;
                _systems = value;
                Systems.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Systems", temp, this);
            }
        }
        public ObservableCollection<TECDevice> DeviceCatalog {
            get { return _deviceCatalog; }
            set
            {
                var temp = this.Copy();
                DeviceCatalog.CollectionChanged -= CollectionChanged;
                _deviceCatalog = value;
                DeviceCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("DeviceCatalog", temp, this);
            }
        }
        public ObservableCollection<TECManufacturer> ManufacturerCatalog {
            get { return _manufacturerCatalog; }
            set
            {
                var temp = this.Copy();
                ManufacturerCatalog.CollectionChanged -= CollectionChanged;
                _manufacturerCatalog = value;
                ManufacturerCatalog.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ManufacturerCatalog", temp, this);
            }
        }

        public ObservableCollection<TECNote> Notes {
            get { return _notes; }
            set
            {
                var temp = this.Copy();
                Notes.CollectionChanged -= CollectionChanged;
                _notes = value;
                Notes.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Notes", temp, this);
            }
        }
        public ObservableCollection<TECExclusion> Exclusions {
            get { return _exclusions; }
            set
            {
                var temp = this.Copy();
                Exclusions.CollectionChanged -= CollectionChanged;
                _exclusions = value;
                Exclusions.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Exclusions", temp, this);
            }
        }

        public ObservableCollection<TECTag> Tags {
            get { return _tags; }
            set
            {
                var temp = this.Copy();
                Tags.CollectionChanged -= CollectionChanged;
                _tags = value;
                Tags.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Tags", temp, this);
            }
        }

        public ObservableCollection<TECDrawing> Drawings {
            get { return _drawings; }
            set
            {
                var temp = this.Copy();
                Drawings.CollectionChanged -= CollectionChanged;
                _drawings = value;
                Drawings.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Drawings", temp, this);
            }
        }
        #endregion //Properties
        
        #region Constructors
        public TECBid(
            string name,
            string bidNumber,
            DateTime dueDate,
            string salesperson,
            string estimator,
            ObservableCollection<TECScopeBranch> scopeTree,
            ObservableCollection<TECSystem> systems, 
            ObservableCollection<TECDevice> deviceCatalog,
            ObservableCollection<TECManufacturer> manufacturerCatalog,
            ObservableCollection<TECNote> notes, 
            ObservableCollection<TECExclusion> exclusions,
            ObservableCollection<TECTag> tags, 
            Guid infoGuid)
        {
            _name = name;
            _bidNumber = bidNumber;
            _dueDate = dueDate;
            _salesperson = salesperson;
            _estimator = estimator;
            _scopeTree = scopeTree;
            _systems = systems;
            _deviceCatalog = deviceCatalog;
            _manufacturerCatalog = manufacturerCatalog;
            _notes = notes;
            _exclusions = exclusions;
            _tags = tags;
            _infoGuid = infoGuid;
            _labor = new TECLabor();
            _drawings = new ObservableCollection<TECDrawing>();

            Systems.CollectionChanged += CollectionChanged;
            ScopeTree.CollectionChanged += CollectionChanged;
            DeviceCatalog.CollectionChanged += CollectionChanged;
            ManufacturerCatalog.CollectionChanged += CollectionChanged;
            Notes.CollectionChanged += CollectionChanged;
            Exclusions.CollectionChanged += CollectionChanged;
            Tags.CollectionChanged += CollectionChanged;
            Drawings.CollectionChanged += CollectionChanged;
        }
        public TECBid(
            string name, 
            string bidNumber,
            DateTime dueDate,
            string salesperson,
            string estimator,
            ObservableCollection<TECScopeBranch> scopeTree,
            ObservableCollection<TECSystem> systems,
            ObservableCollection<TECDevice> deviceCatalog,
            ObservableCollection<TECManufacturer> manufacturerCatalog,
            ObservableCollection<TECNote> notes,
            ObservableCollection<TECExclusion> exclusions,
            ObservableCollection<TECTag> tags)
            : this(name, bidNumber, dueDate, salesperson, estimator, scopeTree, systems, deviceCatalog, manufacturerCatalog, notes, exclusions, tags, Guid.NewGuid()) { }
        public TECBid() : 
            this("", "", new DateTime(), "", "", new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), new ObservableCollection<TECDevice>(), new ObservableCollection<TECManufacturer>(), new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), new ObservableCollection<TECTag>()) { }

        //Copy Constructor
        public TECBid(TECBid bidSource) : this(bidSource.Name, bidSource.BidNumber, bidSource.DueDate, bidSource.Salesperson, bidSource.Estimator, new ObservableCollection<TECScopeBranch>(), new ObservableCollection<TECSystem>(), bidSource.DeviceCatalog, bidSource.ManufacturerCatalog, new ObservableCollection<TECNote>(), new ObservableCollection<TECExclusion>(), bidSource.Tags)
        {
            _labor = new TECLabor(bidSource.Labor);
            foreach (TECScopeBranch branch in bidSource.ScopeTree)
            {
                ScopeTree.Add(new TECScopeBranch(branch));
            }
            foreach (TECSystem system in bidSource.Systems)
            {
                Systems.Add(new TECSystem(system));
            }
            foreach (TECNote note in bidSource.Notes)
            {
                Notes.Add(new TECNote(note));
            }
            foreach (TECExclusion exclusion in bidSource.Exclusions)
            {
                Exclusions.Add(new TECExclusion(exclusion));
            }
        }

        #endregion //Constructors

        #region Methods
        private double getMaterialCost()
        {
            double cost = 0;
            foreach(TECSystem system in this.Systems)
            {
                cost += system.MaterialCost;
            }
            return cost;
        }
        private double getLaborCost()
        {
            double cost = 0;
            foreach (TECSystem system in this.Systems)
            {
                cost += system.LaborCost;
            }
            return cost;
        }
        public override object Copy()
        {
            TECBid bid = new TECBid(this);
            bid._infoGuid = InfoGuid;
            return bid;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    NotifyPropertyChanged("Add", this, item);
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    NotifyPropertyChanged("Remove", this, item);
                }
            }

        }
        #endregion


    }
}
