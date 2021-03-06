﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Models
{
    public class FilteredConnectablesGroup : ViewModelBase, IDragDropable
    {
        private readonly ConnectableFilter filter;

        private string _name;
        private bool _passesFilter;
        
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public bool PassesFilter
        {
            get { return _passesFilter; }
            set
            {
                if (_passesFilter != value)
                {
                    _passesFilter = value;
                    RaisePropertyChanged("PassesFilter");
                }
            }
        }

        public string Type
        {
            get
            {
                return Scope.ToTECTypeString();
            }
        }
        
        public ITECScope Scope { get; }
        public ObservableCollection<FilteredConnectablesGroup> ChildrenGroups { get; } = new ObservableCollection<FilteredConnectablesGroup>();
        private Dictionary<ITECScope, FilteredConnectablesGroup> scopeDictionary = new Dictionary<ITECScope, FilteredConnectablesGroup>();

        public FilteredConnectablesGroup(string name, ConnectableFilter filter)
        {
            this.Name = name;
            
            this.filter = filter;
            this.filter.FilterChanged += filterChanged;
        }
        public FilteredConnectablesGroup(ITECScope scope, ConnectableFilter filter) : this(scope.Name, filter)
        {
            this.Scope = scope;
            scopeDictionary.Add(scope, this);
            this.Scope.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Name")
                {
                    Name = scope.Name;
                }
            };
            
            if (scope is IConnectable connectable)
            {
                this.PassesFilter = filter.PassesFilter(connectable);
            }
            else
            {
                this.PassesFilter = false;
            }
        }

        public FilteredConnectablesGroup Add(ITECScope child)
        {
            FilteredConnectablesGroup newGroup = new FilteredConnectablesGroup(child, this.filter);
            this.Add(newGroup);
            return newGroup;
        }
        public void Add(FilteredConnectablesGroup child)
        {
            foreach (var pair in child.scopeDictionary)
            {
                this.scopeDictionary.Add(pair.Key, pair.Value);
            }
            child.scopeDictionary = this.scopeDictionary;
            child.PropertyChanged += childPropertyChanged;
            this.ChildrenGroups.Add(child);
            this.PassesFilter = thisPassesFilter();
        }

        public bool Remove(ITECScope child)
        {
            FilteredConnectablesGroup groupToRemove = scopeDictionary.ContainsKey(child) ? scopeDictionary[child] : null;

            if (groupToRemove == null)
            {
                return false;
            }
            else
            {
                this.Remove(groupToRemove);
                return true;
            }
        }
        public bool Remove(FilteredConnectablesGroup child)
        {
            var moreChildren = new List<FilteredConnectablesGroup>(child.ChildrenGroups);
            foreach (var childGroup in moreChildren)
            {
                child.Remove(childGroup);
            }
            child.scopeDictionary = null;
            this.scopeDictionary.Remove(child.Scope);
            child.PropertyChanged -= childPropertyChanged;
            if (this.ChildrenGroups.Remove(child))
            {
                this.PassesFilter = thisPassesFilter();
                return true;
            }
            else
            {
                return false;
            }
        }

        public FilteredConnectablesGroup GetGroup(ITECScope scope)
        {
            if (scope == null) return null;
            if (this.scopeDictionary.ContainsKey(scope))
            {
                return scopeDictionary[scope];
            }
            else
            {
                return null;
            }

            //if (this.Scope == scope)
            //{
            //    return this;
            //}

            //foreach (FilteredConnectablesGroup group in this.ChildrenGroups)
            //{
            //    FilteredConnectablesGroup childGroup = group.GetGroup(scope);
            //    if (childGroup != null)
            //    {
            //        return childGroup;
            //    }
            //}

            //return null;
        }
        public List<FilteredConnectablesGroup> GetPath(ITECScope scope)
        {
            List<FilteredConnectablesGroup> path = new List<FilteredConnectablesGroup>();
            if (this.Scope == scope)
            {
                path.Add(this);
                return path;
            }

            foreach (FilteredConnectablesGroup childGroup in this.ChildrenGroups)
            {
                List<FilteredConnectablesGroup> childPath = childGroup.GetPath(scope);
                if (childPath.Count > 0)
                {
                    path.Add(this);
                    path.AddRange(childPath);
                    return path;
                }
            }

            return path;
        }
        
        private List<ITECScope> allScope()
        {
            List<ITECScope> scope = new List<ITECScope>();
            if (this.Scope != null)
            {
                scope.Add(this.Scope);
            }
            foreach (var child in ChildrenGroups)
            {
                scope.AddRange(child.allScope());
            }
            return scope;
        }

        private void filterChanged()
        {
            this.PassesFilter = thisPassesFilter();
        }

        private void childPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PassesFilter")
            {
                this.PassesFilter = thisPassesFilter();
            }
        }

        private bool thisPassesFilter()
        {
            if (this.Scope is IConnectable connectable)
            {
                return this.filter.PassesFilter(connectable);
            }
            else
            {
                //Child Passes Filter
                foreach (FilteredConnectablesGroup child in ChildrenGroups)
                {
                    if (child.PassesFilter)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        object IDragDropable.DropData()
        {
            return ChildrenGroups.Count == 0 ? Scope as object : allScope();
        }
    }
}
