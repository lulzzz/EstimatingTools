﻿using EstimatingLibrary.Interfaces;
using System;

namespace EstimatingLibrary
{
    public class TECLabeled : TECObject, IDDCopiable
    {
        #region Properties

        protected string _label;

        public string Label
        {
            get { return _label; }
            set
            {
                var old = Label;
                _label = value;
                // Call raisePropertyChanged whenever the property is updated
                notifyCombinedChanged(Change.Edit, "Label", this, value, old);
            }
        }

        #endregion //Properties

        #region Constructors
        public TECLabeled(Guid guid) : base(guid)
        {
            _label = "";
        }
        public TECLabeled() : this(Guid.NewGuid()) { }

        public TECLabeled(TECLabeled source) : this()
        {
            _label = source.Label;
        }

        #endregion //Constructors
        
        public virtual object DragDropCopy(TECScopeManager scopeManager)
        {
            return new TECLabeled(this);
        }
        
    }

    
}
