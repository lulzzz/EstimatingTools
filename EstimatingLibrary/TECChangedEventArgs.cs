﻿using EstimatingLibrary.Interfaces;
using System;
using System.ComponentModel;

namespace EstimatingLibrary
{
    public class TECChangedEventArgs : PropertyChangedEventArgs
    {
        public ITECObject Sender { get; private set; }
        public Object Value { get; private set; }
        public Object OldValue { get; private set; }
        public Change Change { get; private set; }

        public TECChangedEventArgs(Change change, string propertyName, ITECObject sender,
            object value, object oldValue)
            : base(propertyName)
        {
            Change = change;
            Sender = sender;
            Value = value;
            OldValue = oldValue;
        }
    }
}
