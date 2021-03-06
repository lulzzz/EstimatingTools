﻿using System;
using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace TECUserControlLibrary.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProposalVM : ViewModelBase
    {
        public TECBid Bid { get; }

        public RelayCommand<TECScopeBranch> AddScopeBranchCommand { get; private set; }
        public RelayCommand<TECSystem> AddSystemNoteCommand { get; private set; }
        public RelayCommand AddNoteCommand { get; private set; }
        public RelayCommand AddExclusionCommand { get; private set; }

        public ProposalVM(TECBid bid)
        {
            Bid = bid;
            AddScopeBranchCommand = new RelayCommand<TECScopeBranch>(addScopBranchExecute);
            AddNoteCommand = new RelayCommand(addNoteExecute);
            AddExclusionCommand = new RelayCommand(addExclusionExecute);
            AddSystemNoteCommand = new RelayCommand<TECSystem>(addSystemNoteExecute);
        }

        private void addSystemNoteExecute(TECSystem obj)
        {
            obj.ScopeBranches.Add(new TECScopeBranch());
        }
        private void addNoteExecute()
        {
            Bid.Notes.Add(new TECLabeled());
        }
        private void addExclusionExecute()
        {
            Bid.Exclusions.Add(new TECLabeled());
        }
        private void addScopBranchExecute(TECScopeBranch obj)
        {
            if(obj == null)
            {
                Bid.ScopeTree.Add(new TECScopeBranch());
            } else
            {
                obj.Branches.Add(new TECScopeBranch());
            }
        }
    }
}