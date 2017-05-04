﻿using EstimatingLibrary;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModelExtensions
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ControllersPanelsViewModel : ViewModelBase, IDropTarget
    {
        #region Properties

        private TECBid _bid;
        public TECBid Bid
        {
            get { return _bid; }
            private set
            {
                unregisterBidChanges();
                _bid = value;
                RaisePropertyChanged("Bid");
                registerBidChanges();
            }
        }

        private TECController _selectedController;
        public TECController SelectedController
        {
            get { return _selectedController; }
            set
            {
                _selectedController = value;
                RaisePropertyChanged("SelectedController");
                SelectionChanged?.Invoke(value);
            }
        }
        private TECPanel _selectedPanel;
        public TECPanel SelectedPanel
        {
            get { return _selectedPanel; }
            set
            {
                _selectedPanel = value;
                RaisePropertyChanged("SelectedPanel");
                SelectionChanged?.Invoke(value);
            }
        }

        private ObservableCollection<ControllerInPanel> _controllerCollection;
        public ObservableCollection<ControllerInPanel> ControllerCollection
        {
            get
            {
                return _controllerCollection;
            }
            set
            {
                _controllerCollection = value;
                ControllerCollection.CollectionChanged -= collectionChanged;
                RaisePropertyChanged("ControllerCollection");
                ControllerCollection.CollectionChanged += collectionChanged;
            }
        }

        private ObservableCollection<TECPanel> _panelSelections;
        public ObservableCollection<TECPanel> PanelSelections
        {
            get { return _panelSelections; }
            set
            {
                _panelSelections = value;
                RaisePropertyChanged("PanelSelections");
            }
        }

        private ControllerInPanel _selectedControllerInPanel;
        public ControllerInPanel SelectedControllerInPanel
        {
            get { return _selectedControllerInPanel; }
            set
            {
                _selectedControllerInPanel = value;
                RaisePropertyChanged("SelectedControllerInPanel");
                if (value != null)
                {
                    SelectionChanged?.Invoke(value.Controller);
                }
            }
        }

        private VisibilityModel _dataGridVisibilty;
        public VisibilityModel DataGridVisibilty
        {
            get { return _dataGridVisibilty; }
            set
            {
                _dataGridVisibilty = value;
                RaisePropertyChanged("DataGridVisibilty");
            }
        }

        private TECPanel _nonePanel;
        public TECPanel NonePanel
        {
            get { return _nonePanel; }
            set
            {
                _nonePanel = value;
                RaisePropertyChanged("NonePanel");
            }
        }

        private Dictionary<TECController, ControllerInPanel> controllersIndex;

        #region Delegates
        public Action<IDropInfo> DragHandler;
        public Action<IDropInfo> DropHandler;

        public Action<Object> SelectionChanged;
        #endregion

        #endregion

        #region Constructor
        public ControllersPanelsViewModel(TECBid bid)
        {
            Bid = bid;
            setup();
        }
        #endregion

        #region Methods
        public void Refresh(TECBid bid)
        {
            Bid = bid;
            setup();
        }

        private void setup()
        {
            populateControllerCollection();
            populatePanelSelections();
        }

        private void populateControllerCollection()
        {
            ControllerCollection = new ObservableCollection<ControllerInPanel>();
            controllersIndex = new Dictionary<TECController, ControllerInPanel>();
            foreach (TECController controller in Bid.Controllers)
            {
                TECController controllerToAdd = controller;
                TECPanel panelToAdd = null;
                foreach (TECPanel panel in Bid.Panels)
                {
                    if (panel.Controllers.Contains(controller))
                    {
                        panelToAdd = panel;
                        break;
                    }
                }
                var controllerInPanelToAdd = new ControllerInPanel(controllerToAdd, panelToAdd);
                ControllerCollection.Add(controllerInPanelToAdd);
                controllersIndex[controller] = controllerInPanelToAdd;
            }
        }
        private void populatePanelSelections()
        {
            PanelSelections = new ObservableCollection<TECPanel>();
            var nonePanel = new TECPanel();
            nonePanel.Name = "None";
            NonePanel = nonePanel;
            PanelSelections.Add(NonePanel);
            foreach (TECPanel panel in Bid.Panels)
            {
                PanelSelections.Add(panel);
                panel.PropertyChanged += Panel_PropertyChanged;
            }
        }

        private void Panel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "AddRelationship")
            {
                foreach(TECController controller in (sender as TECPanel).Controllers)
                {
                    controllersIndex[controller].Panel = sender as TECPanel;
                }
            }
        }
        
        private void registerBidChanges()
        {
            Bid.Controllers.CollectionChanged += collectionChanged;
            Bid.Panels.CollectionChanged += collectionChanged;
        }
        private void unregisterBidChanges()
        {
            if (Bid != null)
            {
                Bid.Controllers.CollectionChanged -= collectionChanged;
                Bid.Panels.CollectionChanged -= collectionChanged;
            }
        }

        private void collectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item is TECController)
                    {
                        addController(item as TECController);
                    }
                    else if (item is TECPanel)
                    {
                        addPanel(item as TECPanel);
                        (item as TECPanel).PropertyChanged += Panel_PropertyChanged;
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item is ControllerInPanel)
                    {
                        foreach (TECPanel panel in Bid.Panels)
                        {
                            if (panel.Controllers.Contains((item as ControllerInPanel).Controller))
                            {
                                panel.Controllers.Remove((item as ControllerInPanel).Controller);
                            }
                        }
                        Bid.Controllers.Remove((item as ControllerInPanel).Controller);
                    }
                    if (item is TECController)
                    {
                        removeController(item as TECController);
                    }
                    else if (item is TECPanel)
                    {
                        removePanel(item as TECPanel);
                        (item as TECPanel).PropertyChanged -= Panel_PropertyChanged;
                    }
                }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.ControllerInPanelDragOver(dropInfo);
        }
        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is TECController)
            {
                UIHelpers.ControllerInPanelDrop(dropInfo, Bid.Controllers);
            }
            else
            {
                UIHelpers.StandardDrop(dropInfo);
            }
        }

        private void addController(TECController controller)
        {
            TECController controllerToAdd = controller;
            TECPanel panelToAdd = null;
            
            var controllerInPanelToAdd = new ControllerInPanel(controllerToAdd, panelToAdd);
            ControllerCollection.Add(controllerInPanelToAdd);
            controllersIndex[controller] = controllerInPanelToAdd;
        }
        private void addPanel(TECPanel panel)
        {
            PanelSelections.Add(panel);
            //foreach(TECController controller in panel.Controllers)
            //{
            //    ControllerCollection.Remove(controllersIndex[controller]);
            //    controllersIndex[controller] = new ControllerInPanel(controller, panel);
            //    ControllerCollection.Add(controllersIndex[controller]);
            //}
        }
        private void removeController(TECController controller)
        {
            ControllerCollection.Remove(controllersIndex[controller]);
            controllersIndex.Remove(controller);
        }
        private void removePanel(TECPanel panel)
        {
            PanelSelections.Remove(panel);
        }
        #endregion
    }
}