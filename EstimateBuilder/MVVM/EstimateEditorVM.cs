﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.ViewModels;
using TECUserControlLibrary.ViewModels.SummaryVMs;

namespace EstimateBuilder.MVVM
{
    public class EstimateEditorVM : EditorVM
    {
        public ScopeEditorVM ScopeEditorVM { get; }
        public LaborVM LaborVM { get; }
        public ReviewVM ReviewVM { get; }
        public ProposalVM ProposalVM { get; }
        public ItemizedSummaryVM ItemizedSummaryVM { get; }
        public MaterialSummaryVM MaterialSummaryVM { get; }
        public RiserVM RiserVM { get; }
        public ScheduleVM ScheduleVM { get; }
        public BidPropertiesVM BidPropertiesVM { get; }
        
        public EstimateEditorVM(TECBid bid, TECTemplates templates, ChangeWatcher watcher, TECEstimator estimate)
        {
            ScopeEditorVM = new ScopeEditorVM(bid, templates, watcher);
            LaborVM = new LaborVM(bid, templates, estimate);
            ReviewVM = new ReviewVM(bid, estimate);
            ProposalVM = new ProposalVM(bid);
            ItemizedSummaryVM = new ItemizedSummaryVM(bid, watcher);
            MaterialSummaryVM = new MaterialSummaryVM(bid, watcher);
            RiserVM = new RiserVM(bid, watcher);
            ScheduleVM = new ScheduleVM(bid, watcher);
            BidPropertiesVM = new BidPropertiesVM(bid);
        }
    }
}
