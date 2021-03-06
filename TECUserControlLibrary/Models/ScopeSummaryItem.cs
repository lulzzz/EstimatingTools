﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight;
using System;

namespace TECUserControlLibrary.Models
{
    public class ScopeSummaryItem : ViewModelBase
    {
        private ChangeWatcher watcher;

        public TECScope Scope { get; private set; }
        public TECEstimator Estimate { get; private set; }

        public ScopeSummaryItem(TECScope scope, TECParameters parameters, double duration = 0.0)
        {
            this.Scope = scope;
            watcher = new ChangeWatcher(scope);
            Estimate = new TECEstimator(scope, parameters, new TECExtraLabor(Guid.NewGuid()), duration, watcher);
        }
    }
}
