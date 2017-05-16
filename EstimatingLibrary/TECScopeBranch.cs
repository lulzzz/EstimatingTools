﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECScopeBranch : TECScope
    {//TECScopeBranch exists as an alternate object to TECSystem. It's purpose is to serve as a non-specific scope object with unlimited branches in both depth and breadth.
        #region Properties
        private ObservableCollection<TECScopeBranch> _branches;
        public ObservableCollection<TECScopeBranch> Branches
        {
            get { return _branches; }
            set
            {
                var temp = this.Copy();
                _branches = value;
                NotifyPropertyChanged("Branches", temp, this);
                Branches.CollectionChanged += Branches_CollectionChanged;
            }
        }

        #endregion //Properites

        #region Constructors
        public TECScopeBranch(Guid guid) : base(guid)
        {
            _branches = new ObservableCollection<TECScopeBranch>();
            Branches.CollectionChanged += Branches_CollectionChanged;
        }

        public TECScopeBranch() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECScopeBranch(TECScopeBranch scopeBranchSource) : this()
        {
            foreach (TECScopeBranch branch in scopeBranchSource.Branches)
            {
                Branches.Add(new TECScopeBranch(branch));
            }
            this.copyPropertiesFromScope(scopeBranchSource);
        }
        #endregion //Constructors

        public override Object Copy()
        {
            TECScopeBranch outScope = new TECScopeBranch();
            outScope._guid = Guid;

            foreach (TECScopeBranch branch in this.Branches)
            {
                outScope.Branches.Add(branch.Copy() as TECScopeBranch);
            }
            this.copyPropertiesFromScope(this);

            return outScope;
        }

        public override object DragDropCopy()
        {
            TECScopeBranch outScope = new TECScopeBranch(this);
            return outScope;
        }

        private void Branches_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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

    }
}
