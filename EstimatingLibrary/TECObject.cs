﻿using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public enum Flavor { Tag=1, Location, Note, Exclusion, Wire, Conduit }
    public enum Change { Add, Remove, Edit }

    public abstract class TECObject : INotifyPropertyChanged
    {
        #region Properties
        protected Guid _guid;
        
        public Guid Guid
        {
            get { return _guid; }
        }

        public Flavor Flavor;
        #endregion

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<PropertyChangedExtendedEventArgs> TECChanged;

        protected void NotifyPropertyChanged(Change change, string propertyName, TECObject sender,
            object value, object oldValue = null)
        {
            RaiseExtendedPropertyChanged(this, new PropertyChangedExtendedEventArgs(change, propertyName, sender,
                value, oldValue));
        }
        protected void RaiseExtendedPropertyChanged(object sender, PropertyChangedExtendedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
            TECChanged?.Invoke(args);
        }

        protected void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion //Property Changed

        public TECObject(Guid guid)
        {
            _guid = guid;
        }
    }
}
