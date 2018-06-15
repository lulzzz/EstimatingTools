﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EstimatingUtilitiesLibraryTests
{
    [TestClass]
    public class SaveStackTests
    {
        #region Add
        #region Bid
        #region Controller
        [TestMethod]
        public void Bid_AddController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECProvidedController controller = new TECProvidedController(type, false);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;
            
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = controller.Guid.ToString();
            data[ProvidedControllerTable.Name.Name] = controller.Name;
            data[ProvidedControllerTable.Description.Name] = controller.Name;
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerControllerTypeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.AddController(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        [TestMethod]
        public void Bid_AddControllerAssociatedCost()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECProvidedController controller = new TECProvidedController(type, false);
            bid.AddController(controller);

            TECAssociatedCost cost = new TECAssociatedCost(CostType.TEC);
            controller.AssociatedCosts.Add(cost);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;
            
            data = new Dictionary<string, string>();
            data[ScopeAssociatedCostTable.ScopeID.Name] = controller.Guid.ToString();
            data[ScopeAssociatedCostTable.AssociatedCostID.Name] = cost.Guid.ToString();
            data[ScopeAssociatedCostTable.Quantity.Name] = "2";
            expectedItems.Add(new UpdateItem(Change.Add, ScopeAssociatedCostTable.TableName, data));

            int expectedCount = expectedItems.Count;

            controller.AssociatedCosts.Add(cost);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, false);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name;
            data[PanelTable.Description.Name] = panel.Name;
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));

            int expectedCount = expectedItems.Count;
            
            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddControllerToPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, false);
            bid.Panels.Add(panel);

            TECControllerType controllerType = new TECControllerType(new TECManufacturer());
            TECProvidedController controller = new TECProvidedController(controllerType, false);
            bid.AddController(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[PanelControllerTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            panel.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECMisc misc = new TECMisc(CostType.TEC, false);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName,data));

            data = new Dictionary<string, string>();
            data[BidMiscTable.BidID.Name] = bid.Guid.ToString();
            data[BidMiscTable.MiscID.Name] = misc.Guid.ToString();
            data[BidMiscTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, BidMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;


            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch(false);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ScopeBranchTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidScopeBranchTable.BidID.Name] = bid.Guid.ToString();
            data[BidScopeBranchTable.ScopeBranchID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, BidScopeBranchTable.TableName, data));
            int expectedCount = expectedItems.Count;

            bid.ScopeTree.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        [TestMethod]
        public void Bid_AddChildBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch parentBranch = new TECScopeBranch(false);
            bid.ScopeTree.Add(parentBranch);

            TECScopeBranch scopeBranch = new TECScopeBranch(false);
            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ScopeBranchTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ScopeBranchHierarchyTable.ParentID.Name] = parentBranch.Guid.ToString();
            data[ScopeBranchHierarchyTable.ChildID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, ScopeBranchHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            parentBranch.Branches.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Notes etc.
        [TestMethod]
        public void Bid_AddNote()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECLabeled note = new TECLabeled();
            note.Label = "Note";

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[NoteTable.ID.Name] = note.Guid.ToString();
            data[NoteTable.NoteText.Name] = note.Label.ToString();

            UpdateItem expectedItem = new UpdateItem(Change.Add, NoteTable.TableName, data);
            int expectedCount = 1;

            bid.Notes.Add(note);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        [TestMethod]
        public void Bid_AddExclusion()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECLabeled exclusion = new TECLabeled();
            exclusion.Label = "Exclusion";

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ExclusionTable.ID.Name] = exclusion.Guid.ToString();
            data[ExclusionTable.ExclusionText.Name] = exclusion.Label.ToString();

            UpdateItem expectedItem = new UpdateItem(Change.Add, ExclusionTable.TableName, data);
            int expectedCount = 1;

            bid.Exclusions.Add(exclusion);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        [TestMethod]
        public void Bid_AddInternalNote()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECInternalNote note = new TECInternalNote();
            note.Label = "Note";

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[InternalNoteTable.ID.Name] = note.Guid.ToString();
            data[InternalNoteTable.Label.Name] = note.Label.ToString();
            data[InternalNoteTable.Body.Name] = note.Body.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, InternalNoteTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidInternalNoteTable.BidID.Name] = bid.Guid.ToString();
            data[BidInternalNoteTable.NoteID.Name] = note.Guid.ToString();
            data[BidInternalNoteTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, BidInternalNoteTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.InternalNotes.Add(note);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Locations
        [TestMethod]
        public void Bid_AddLocation()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECLocation location = new TECLocation();
            location.Label = "Location";

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[LocationTable.ID.Name] = location.Guid.ToString();
            data[LocationTable.Name.Name] = location.Name.ToString();
            data[LocationTable.Label.Name] = location.Label.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, LocationTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidLocationTable.BidID.Name] = bid.Guid.ToString();
            data[BidLocationTable.LocationID.Name] = location.Guid.ToString();
            data[BidLocationTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, BidLocationTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Locations.Add(location);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_AddSystem()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = system.Guid.ToString();
            data[SystemTable.Name.Name] = system.Name.ToString();
            data[SystemTable.Description.Name] = system.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = system.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidSystemTable.BidID.Name] = bid.Guid.ToString();
            data[BidSystemTable.SystemID.Name] = system.Guid.ToString();
            data[BidSystemTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, BidSystemTable.TableName, data));

            int expectedCount = expectedItems.Count;
            
            bid.Systems.Add(system);
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            TECSystem instance = system.AddInstance(bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();

            expectedItems.Add( new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #region Equipment
        [TestMethod]
        public void Bid_AddEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = system.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Equipment.Add(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddEquipmentToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); 
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            typical.Equipment.Add(equip);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            int expectedCount = expectedItems.Count;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_AddSubScopeConnectionToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equipment.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = system.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECSystem instance = system.AddInstance(bid);
            
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_AddPointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            TECPoint point = new TECPoint(true);
            subScope.Points.Add(point);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            data[PointTable.Name.Name] = point.Label.ToString();
            data[PointTable.Type.Name] = point.Type.ToString();
            data[PointTable.Quantity.Name] = point.Quantity.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            data[SubScopePointTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
            
        }

        [TestMethod]
        public void Bid_AddPointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECPoint point = new TECPoint(true);
            subScope.Points.Add(point);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[PointTable.Name.Name] = instance.Equipment[0].SubScope[0].Points[0].Label.ToString();
            data[PointTable.Type.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            data[PointTable.Quantity.Name] = instance.Equipment[0].SubScope[0].Points[0].Quantity.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[SubScopePointTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            data[PointTable.Name.Name] = point.Label.ToString();
            data[PointTable.Type.Name] = point.Type.ToString();
            data[PointTable.Quantity.Name] = point.Quantity.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            data[SubScopePointTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid(); 
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint(true);
            subScope.Points.Add(point);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECSystem instance = system.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[PointTable.Name.Name] = instance.Equipment[0].SubScope[0].Points[0].Label.ToString();
            data[PointTable.Type.Name] = instance.Equipment[0].SubScope[0].Points[0].Type.ToString();
            data[PointTable.Quantity.Name] = instance.Equipment[0].SubScope[0].Points[0].Quantity.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            data[SubScopePointTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopePointTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_AddDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            subScope.Devices.Add(device);

            Dictionary < string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            UpdateItem expectedItem = new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_AddDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            subScope.Devices.Add(device);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECSystem instance = system.AddInstance(bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Valve
        [TestMethod]
        public void Bid_AddValveToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            TECValve valve = new TECValve(new TECManufacturer(), device);
            subScope.Devices.Add(valve);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            UpdateItem expectedItem = new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_AddValveToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            TECValve valve = new TECValve(new TECManufacturer(), device);
            subScope.Devices.Add(valve);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithValve()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            TECValve valve = new TECValve(new TECManufacturer(), device);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.Valves.Add(valve);
            subScope.Devices.Add(valve);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECSystem instance = system.AddInstance(bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            data[SubScopeDeviceTable.Quantity.Name] = "1";
            data[SubScopeDeviceTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeDeviceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";

            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_AddControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECProvidedController controller = new TECProvidedController(type, true);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = controller.Guid.ToString();
            data[ProvidedControllerTable.Name.Name] = controller.Name.ToString();
            data[ProvidedControllerTable.Description.Name] = controller.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerControllerTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = system.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            data[SystemControllerTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.AddController(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); 
            TECTypical typical = new TECTypical();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(type, true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            typical.AddController(controller);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = instance.Controllers[0].Guid.ToString();
            data[ProvidedControllerTable.Name.Name] = instance.Controllers[0].Name.ToString();
            data[ProvidedControllerTable.Description.Name] = instance.Controllers[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[SystemControllerTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = controller.Guid.ToString();
            data[ProvidedControllerTable.Name.Name] = controller.Name.ToString();
            data[ProvidedControllerTable.Description.Name] = controller.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            data[SystemControllerTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();

            int expectedCount = expectedItems.Count;

            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);

            TECController controller = new TECProvidedController(type, true);
            bid.Systems.Add(typical);
            typical.AddController(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = instance.Controllers[0].Guid.ToString();
            data[ProvidedControllerTable.Name.Name] = instance.Controllers[0].Name.ToString();
            data[ProvidedControllerTable.Description.Name] = instance.Controllers[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();

            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[SystemControllerTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemControllerTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name.ToString();
            data[PanelTable.Description.Name] = panel.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = system.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            data[SystemPanelTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddControllerToPanelInTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(system);

            system.Panels.Add(panel);

            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType ControllerType = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(ControllerType, true);
            system.AddController(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[PanelControllerTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            panel.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddPanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            typical.Panels.Add(panel);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelTable.Name.Name] = instance.Panels[0].Name.ToString();
            data[PanelTable.Description.Name] = instance.Panels[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[SystemPanelTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name.ToString();
            data[PanelTable.Description.Name] = panel.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            data[SystemPanelTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));
            int expectedCount = expectedItems.Count;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelTable.Name.Name] = instance.Panels[0].Name.ToString();
            data[PanelTable.Description.Name] = instance.Panels[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[SystemPanelTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemPanelTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECMisc misc = new TECMisc(CostType.TEC, true);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = system.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            data[SystemMiscTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECMisc misc = new TECMisc(CostType.TEC, true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            typical.MiscCosts.Add(misc);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = misc.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.MiscCosts[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = instance.MiscCosts[0].Guid.ToString();
            data[MiscTable.Name.Name] = instance.MiscCosts[0].Name.ToString();
            data[MiscTable.Cost.Name] = instance.MiscCosts[0].Cost.ToString();
            data[MiscTable.Labor.Name] = instance.MiscCosts[0].Labor.ToString();
            data[MiscTable.Quantity.Name] = instance.MiscCosts[0].Quantity.ToString();
            data[MiscTable.Type.Name] = instance.MiscCosts[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = instance.MiscCosts[0].Guid.ToString();
            data[SystemMiscTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemMiscTable.TableName, data));

            data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            data[SystemMiscTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemMiscTable.TableName, data));


            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECMisc misc = new TECMisc(CostType.TEC, true);
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = misc.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.MiscCosts[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = instance.MiscCosts[0].Guid.ToString();
            data[MiscTable.Name.Name] = instance.MiscCosts[0].Name.ToString();
            data[MiscTable.Cost.Name] = instance.MiscCosts[0].Cost.ToString();
            data[MiscTable.Labor.Name] = instance.MiscCosts[0].Labor.ToString();
            data[MiscTable.Quantity.Name] = instance.MiscCosts[0].Quantity.ToString();
            data[MiscTable.Type.Name] = instance.MiscCosts[0].Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = instance.MiscCosts[0].Guid.ToString();
            data[SystemMiscTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemMiscTable.TableName, data));

            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECScopeBranch scopeBranch = new TECScopeBranch(true);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ScopeBranchTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemScopeBranchTable.SystemID.Name] = system.Guid.ToString();
            data[SystemScopeBranchTable.BranchID.Name] = scopeBranch.Guid.ToString();
            data[SystemScopeBranchTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemScopeBranchTable.TableName, data));
            int expectedCount = expectedItems.Count;

            system.ScopeBranches.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Interlocks
        [TestMethod]
        public void Bid_AddInterlockToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            TECInterlockConnection interlock = new TECInterlockConnection(new List<TECConnectionType>(), true);
            subScope.Interlocks.Add(interlock);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = interlock.Guid.ToString();
            data[InterlockConnectionTable.Name.Name] = interlock.Name.ToString();
            data[InterlockConnectionTable.Description.Name] = interlock.Description.ToString();
            data[InterlockConnectionTable.Length.Name] = interlock.Length.ToString();
            data[InterlockConnectionTable.ConduitLength.Name] = interlock.ConduitLength.ToString();
            data[InterlockConnectionTable.IsPlenum.Name] = interlock.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = subScope.Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = interlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockableInterlockTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }

        [TestMethod]
        public void Bid_AddInterlockToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            TECInterlockConnection interlock = new TECInterlockConnection(new List<TECConnectionType>(), true);
            subScope.Interlocks.Add(interlock);

            var instanceSubScope = instance.Equipment[0].SubScope[0];
            var instanceInterlock = instanceSubScope.Interlocks[0];

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = interlock.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Interlocks[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = instanceInterlock.Guid.ToString();
            data[InterlockConnectionTable.Name.Name] = instanceInterlock.Name.ToString();
            data[InterlockConnectionTable.Description.Name] = instanceInterlock.Description.ToString();
            data[InterlockConnectionTable.Length.Name] = instanceInterlock.Length.ToString();
            data[InterlockConnectionTable.ConduitLength.Name] = instanceInterlock.ConduitLength.ToString();
            data[InterlockConnectionTable.IsPlenum.Name] = instanceInterlock.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockConnectionTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = instanceSubScope.Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = instanceInterlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockableInterlockTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = interlock.Guid.ToString();
            data[InterlockConnectionTable.Name.Name] = interlock.Name.ToString();
            data[InterlockConnectionTable.Description.Name] = interlock.Description.ToString();
            data[InterlockConnectionTable.Length.Name] = interlock.Length.ToString();
            data[InterlockConnectionTable.ConduitLength.Name] = interlock.ConduitLength.ToString();
            data[InterlockConnectionTable.IsPlenum.Name] = interlock.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = subScope.Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = interlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockableInterlockTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithInterlock()
        {
            //Arrange
            TECBid bid = new TECBid();
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECInterlockConnection interlock = new TECInterlockConnection(new List<TECConnectionType>(), true);
            subScope.Interlocks.Add(interlock);

            //Act
            ChangeWatcher watcher = new ChangeWatcher(bid);
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            TECSystem instance = system.AddInstance(bid);

            var instanceSubScope = instance.Equipment[0].SubScope[0];
            var instanceInterlock = instanceSubScope.Interlocks[0];

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = interlock.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Interlocks[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentTable.Name.Name] = instance.Equipment[0].Name.ToString();
            data[EquipmentTable.Description.Name] = instance.Equipment[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeTable.Name.Name] = instance.Equipment[0].SubScope[0].Name.ToString();
            data[SubScopeTable.Description.Name] = instance.Equipment[0].SubScope[0].Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = instanceInterlock.Guid.ToString();
            data[InterlockConnectionTable.Name.Name] = instanceInterlock.Name.ToString();
            data[InterlockConnectionTable.Description.Name] = instanceInterlock.Description.ToString();
            data[InterlockConnectionTable.Length.Name] = instanceInterlock.Length.ToString();
            data[InterlockConnectionTable.ConduitLength.Name] = instanceInterlock.ConduitLength.ToString();
            data[InterlockConnectionTable.IsPlenum.Name] = instanceInterlock.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = instanceSubScope.Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = instanceInterlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, InterlockableInterlockTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[EquipmentSubScopeTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, EquipmentSubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[SystemEquipmentTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            data[SystemHierarchyTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion

        #region Connections
        [TestMethod]
        public void Bid_AddNetworkConnectionInBid()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            IConnectable child = new TECProvidedController(type, false);
            bid.AddController(controller);
            bid.AddController(child as TECController);

            TECProtocol protocol = new TECProtocol(new List<TECConnectionType>());
            type.IO.Add(new TECIO(protocol));

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = controller.Connect(child, child.AvailableProtocols.First());

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[NetworkConnectionTable.ID.Name] = connection.Guid.ToString();
            data[NetworkConnectionTable.Length.Name] = connection.Length.ToString(); ;
            data[NetworkConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            //data[NetworkConnectionTable.IOType.Name] = connection.IOType.ToString();
            data[NetworkConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();

            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionTable.TableName, data));


            data = new Dictionary<string, string>();
            data[NetworkConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionChildrenTable.ChildID.Name] = child.Guid.ToString();
            data[NetworkConnectionChildrenTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionProtocolTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionProtocolTable.ProtocolID.Name] = protocol.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionProtocolTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));
            int expectedCount = expectedItems.Count;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddNetworkConnectionToInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);

            TECController child = new TECProvidedController(type, false);
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            typical.AddController(child);
            TECSystem system = typical.AddInstance(bid);
            TECController instanceController = system.Controllers[0];
            
            TECProtocol protocol= new TECProtocol(new List<TECConnectionType>());
            type.IO.Add(new TECIO(protocol));
            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = controller.Connect(instanceController, (instanceController as IConnectable).AvailableProtocols.First());

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[NetworkConnectionTable.ID.Name] = connection.Guid.ToString();
            data[NetworkConnectionTable.Length.Name] = connection.Length.ToString(); ;
            data[NetworkConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            //data[NetworkConnectionTable.IOType.Name] = connection.IOType.ToString();
            data[NetworkConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();

            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionChildrenTable.ChildID.Name] = instanceController.Guid.ToString();
            data[NetworkConnectionChildrenTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionChildrenTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[NetworkConnectionProtocolTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionProtocolTable.ProtocolID.Name] = protocol.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionProtocolTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddNetworkConnectionBetweenInstances()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);

            TECController controller = new TECProvidedController(type, false);
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            typical.AddController(controller);
            TECSystem system = typical.AddInstance(bid);
            TECController instanceController = system.Controllers[0];

            TECController otherController = new TECProvidedController(type, false);
            TECTypical otherTypical = new TECTypical();
            bid.Systems.Add(otherTypical);
            otherTypical.AddController(otherController);
            TECSystem otherSystem = otherTypical.AddInstance(bid);
            TECController otherInstanceController = otherSystem.Controllers[0];

            TECProtocol protocol = new TECProtocol(new List<TECConnectionType>());
            type.IO.Add(new TECIO(protocol));

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = instanceController.Connect(otherInstanceController, otherInstanceController.AvailableProtocols.First());

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[NetworkConnectionTable.ID.Name] = connection.Guid.ToString();
            data[NetworkConnectionTable.Length.Name] = connection.Length.ToString(); ;
            data[NetworkConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            //data[NetworkConnectionTable.IOType.Name] = connection.IOType.ToString();
            data[NetworkConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();

            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionChildrenTable.ChildID.Name] = otherInstanceController.Guid.ToString();
            data[NetworkConnectionChildrenTable.Index.Name] = "0";
            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionProtocolTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionProtocolTable.ProtocolID.Name] = protocol.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, NetworkConnectionProtocolTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = instanceController.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));

            
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeConnectionConnectionToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);

            TECTypical typical = new TECTypical();
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);
            bid.Systems.Add(typical);
            
            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = controller.Connect(subScope, (subScope as IConnectable).AvailableProtocols.First());

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionTable.Length.Name] = connection.Length.ToString();
            data[SubScopeConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            data[SubScopeConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.Quantity.Name] = "1";
            expectedItems.Add(new UpdateItem(Change.Add, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeConnectionConnectionToInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);

            TECTypical typical = new TECTypical();
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);

            subScope.Devices.Add(device);
            bid.Systems.Add(typical);
            TECSystem system = typical.AddInstance(bid);
            TECSubScope instanceSubScope = system.Equipment[0].SubScope[0];

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = controller.Connect(instanceSubScope, (instanceSubScope as IConnectable).AvailableProtocols.First());

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionTable.Length.Name] = connection.Length.ToString();
            data[SubScopeConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            data[SubScopeConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = instanceSubScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.Quantity.Name] = "1";
            expectedItems.Add(new UpdateItem(Change.Add, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeConnectionConnectionInTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);

            TECTypical typical = new TECTypical();
            typical.AddController(controller);
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);
            bid.Systems.Add(typical);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = controller.Connect(subScope, subScope.AvailableProtocols.First());

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionTable.Length.Name] = connection.Length.ToString();
            data[SubScopeConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            data[SubScopeConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.Quantity.Name] = "1";
            expectedItems.Add(new UpdateItem(Change.Add, HardwiredConnectionConnectionTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeConnectionConnectionInTypicalWithInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);

            TECTypical typical = new TECTypical();
            typical.AddController(controller);
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);
            bid.Systems.Add(typical);
            TECSystem system = typical.AddInstance(bid);

            TECSubScope instanceSubScope = system.Equipment[0].SubScope[0];
            TECController instanceController = system.Controllers[0];

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            IControllerConnection connection = controller.Connect(subScope, (subScope as IConnectable).AvailableProtocols.First());
            IControllerConnection instanceConnection = instanceController.Connect(instanceSubScope, (instanceSubScope as IConnectable).AvailableProtocols.First());
            Dictionary<string, string> data;
            
            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionTable.Length.Name] = connection.Length.ToString();
            data[SubScopeConnectionTable.ConduitLength.Name] = connection.ConduitLength.ToString();
            data[SubScopeConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.Quantity.Name] = "1";
            expectedItems.Add(new UpdateItem(Change.Add, HardwiredConnectionConnectionTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = instanceConnection.Guid.ToString();
            data[SubScopeConnectionTable.Length.Name] = instanceConnection.Length.ToString();
            data[SubScopeConnectionTable.ConduitLength.Name] = instanceConnection.ConduitLength.ToString();
            data[SubScopeConnectionTable.IsPlenum.Name] = connection.IsPlenum.ToInt().ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = instanceConnection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = instanceSubScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = instanceConnection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.Quantity.Name] = "1";
            expectedItems.Add(new UpdateItem(Change.Add, HardwiredConnectionConnectionTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = instanceController.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = instanceConnection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerConnectionTable.TableName, data));
            

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion

        #region Remove
        #region Bid
        #region Controller
        [TestMethod]
        public void Bid_RemoveController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerControllerTypeTable.TableName, data));
            
            int expectedCount = expectedItems.Count;

            bid.RemoveController(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, false);
            bid.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveControllerFromPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, false);
            bid.Panels.Add(panel);

            TECControllerType controllerType = new TECControllerType(new TECManufacturer());
            TECController controller = new TECProvidedController(controllerType, false);
            bid.AddController(controller);
            panel.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[PanelControllerTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            panel.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECMisc misc = new TECMisc(CostType.TEC, false);
            bid.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidMiscTable.BidID.Name] = bid.Guid.ToString();
            data[BidMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, BidMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch(false);
            bid.ScopeTree.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ScopeBranchTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidScopeBranchTable.BidID.Name] = bid.Guid.ToString();
            data[BidScopeBranchTable.ScopeBranchID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, BidScopeBranchTable.TableName, data));
            int expectedCount = expectedItems.Count;

            bid.ScopeTree.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        [TestMethod]
        public void Bid_RemoveChildBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch(false);
            TECScopeBranch parentBranch = new TECScopeBranch(false);
            bid.ScopeTree.Add(parentBranch);
            parentBranch.Branches.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ScopeBranchTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ScopeBranchHierarchyTable.ParentID.Name] = parentBranch.Guid.ToString();
            data[ScopeBranchHierarchyTable.ChildID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ScopeBranchHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            parentBranch.Branches.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Notes Exclsuions
        [TestMethod]
        public void Bid_RemoveNote()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECLabeled note = new TECLabeled();
            bid.Notes.Add(note);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[NoteTable.ID.Name] = note.Guid.ToString();
            UpdateItem expectedItem = new UpdateItem(Change.Remove, NoteTable.TableName, data);
            int expectedCount = 1;

            bid.Notes.Remove(note);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        [TestMethod]
        public void Bid_RemoveExclusion()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECLabeled exclusion = new TECLabeled();
            bid.Exclusions.Add(exclusion);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ExclusionTable.ID.Name] = exclusion.Guid.ToString();
            UpdateItem expectedItem = new UpdateItem(Change.Remove, ExclusionTable.TableName, data);
            int expectedCount = 1;

            bid.Exclusions.Remove(exclusion);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #region Locations
        [TestMethod]
        public void Bid_RemoveLocation()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECLocation location = new TECLocation();
            bid.Locations.Add(location);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            data[LocationTable.ID.Name] = location.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, LocationTable.TableName, data));

            data = new Dictionary<string, string>();
            data[BidLocationTable.BidID.Name] = bid.Guid.ToString();
            data[BidLocationTable.LocationID.Name] = location.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, BidLocationTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Locations.Remove(location);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_RemoveSystem()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            Dictionary<string, string> data = new Dictionary<string, string>();
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            data[SystemTable.ID.Name] = system.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));


            data = new Dictionary<string, string>();
            data[BidSystemTable.BidID.Name] = bid.Guid.ToString();
            data[BidSystemTable.SystemID.Name] = system.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, BidSystemTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Systems.Remove(system);


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            system.Instances.Remove(instance);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #region Equipment
        [TestMethod]
        public void Bid_RemoveEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            bid.Systems.Add(system);
            system.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = system.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));

            int expectedCount = 2;

            system.Equipment.Remove(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveEquipmentFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            var removed = instance.Equipment[0];
            typical.Equipment.Remove(equip);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string>
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            int expectedCount = expectedItems.Count;
            

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment(true);
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equip.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            equipment.SubScope.Remove(subScope);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equipment.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            var removed = instance.Equipment[0].SubScope[0];
            equipment.SubScope.Remove(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = system.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            
            
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_RemovePointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint(true);
            subScope.Points.Add(point);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            subScope.Points.Remove(point);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemovePointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECPoint point = new TECPoint(true);
            subScope.Points.Add(point);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            var removed = instance.Equipment[0].SubScope[0].Points[0];
            subScope.Points.Remove(point);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = point.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint(true);
            subScope.Points.Add(point);
            TECSystem instance = system.AddInstance(bid);
            
            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = point.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PointTable.ID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PointTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopePointTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopePointTable.PointID.Name] = instance.Equipment[0].SubScope[0].Points[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopePointTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            
            
            
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_RemoveDeviceFromTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            subScope.Devices.Remove(device);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            UpdateItem expectedItem = new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            subScope.Devices.Remove(device);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = device.Guid.ToString();;
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            
            
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_RemoveValveFromTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            TECValve valve = new TECValve(new TECManufacturer(), device);
            subScope.Devices.Add(valve);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            subScope.Devices.Remove(valve);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            UpdateItem expectedItem = new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveValveFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            TECValve valve = new TECValve(new TECManufacturer(), device);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.Valves.Add(valve);
            subScope.Devices.Add(valve);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            subScope.Devices.Remove(valve);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithValve()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), new TECManufacturer());
            TECValve valve = new TECValve(new TECManufacturer(), device);
            bid.Catalogs.Devices.Add(device);
            bid.Catalogs.Valves.Add(valve);
            subScope.Devices.Add(valve);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeDeviceTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[SubScopeDeviceTable.DeviceID.Name] = valve.Guid.ToString(); ;
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeDeviceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));


            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_RemoveControllerFromTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(type, true);
            bid.Systems.Add(system);
            system.AddController(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = system.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.RemoveController(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveControllerFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(type, true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.AddController(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            var removed = instance.Controllers[0];
            typical.RemoveController(controller);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = removed.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));
            data = new Dictionary<string, string>();

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(type, true);
            bid.Systems.Add(typical);
            typical.AddController(controller);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = controller.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerTable.ID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerTable.TableName, data));
            data = new Dictionary<string, string>();
            data[ProvidedControllerControllerTypeTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            data[ProvidedControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ProvidedControllerControllerTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemControllerTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemControllerTable.ControllerID.Name] = instance.Controllers[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemControllerTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(system);
            system.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = system.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveControllerFromPanelInTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(system);

            system.Panels.Add(panel);

            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType ControllerType = new TECControllerType(manufacturer);
            TECController controller = new TECProvidedController(ControllerType, true);
            system.AddController(controller);
            panel.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[PanelControllerTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelControllerTable.ControllerID.Name] = controller.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelControllerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            panel.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemovePanelFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            var removed = instance.Panels[0];
            typical.Panels.Remove(panel);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = removed.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = panel.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));
            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type, true);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = panel.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelTable.ID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelTable.TableName, data));
            data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, PanelPanelTypeTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemPanelTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemPanelTable.PanelID.Name] = instance.Panels[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemPanelTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            
            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMiscFromTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECMisc misc = new TECMisc(CostType.TEC, true);
            bid.Systems.Add(system);
            system.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = system.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemMiscTable.TableName, data));

            int expectedCount = expectedItems.Count;

            system.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveMiscFormTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECMisc misc = new TECMisc(CostType.TEC, true);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.MiscCosts.Add(misc);
            var removed = instance.MiscCosts[0];

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = misc.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemMiscTable.TableName, data));

            data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = misc.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemMiscTable.TableName, data));
            
            int expectedCount = expectedItems.Count;
            
            typical.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical typical = new TECTypical();
            TECMisc misc = new TECMisc(CostType.TEC, true);
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            typical.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = misc.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.MiscCosts[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[MiscTable.ID.Name] = instance.MiscCosts[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, MiscTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemMiscTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemMiscTable.MiscID.Name] = instance.MiscCosts[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemMiscTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            TECScopeBranch scopeBranch = new TECScopeBranch(false);
            bid.Systems.Add(system);
            system.ScopeBranches.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ScopeBranchTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemScopeBranchTable.SystemID.Name] = system.Guid.ToString();
            data[SystemScopeBranchTable.BranchID.Name] = scopeBranch.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemScopeBranchTable.TableName, data));
            int expectedCount = expectedItems.Count;

            system.ScopeBranches.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Interlocks
        [TestMethod]
        public void Bid_RemoveInterlockFromTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECInterlockConnection interlock = new TECInterlockConnection(new List<TECConnectionType>(), true);
            subScope.Interlocks.Add(interlock);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            subScope.Interlocks.Remove(interlock);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = interlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = subScope.Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = interlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockableInterlockTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInterlockFromTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECInterlockConnection interlock = new TECInterlockConnection(new List<TECConnectionType>(), true);
            subScope.Interlocks.Add(interlock);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            var removed = instance.Equipment[0].SubScope[0].Interlocks[0];
            subScope.Interlocks.Remove(interlock);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = interlock.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockConnectionTable.TableName, data));
            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = removed.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockableInterlockTable.TableName, data));
            data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = interlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockConnectionTable.TableName, data));
            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = system.Equipment[0].SubScope[0].Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = interlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockableInterlockTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceFromSystemWithInterlock()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment(true);
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            equipment.SubScope.Add(subScope);
            TECInterlockConnection interlock = new TECInterlockConnection(new List<TECConnectionType>(), true);
            subScope.Interlocks.Add(interlock);
            TECSystem instance = system.AddInstance(bid);

            var instanceInterlock = instance.Equipment[0].SubScope[0].Interlocks[0];

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            system.Instances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data;
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = equipment.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = subScope.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[TypicalInstanceTable.TypicalID.Name] = interlock.Guid.ToString();
            data[TypicalInstanceTable.InstanceID.Name] = instance.Equipment[0].SubScope[0].Interlocks[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, TypicalInstanceTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemTable.ID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemTable.TableName, data));
            data = new Dictionary<string, string>();
            data[EquipmentTable.ID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SubScopeTable.ID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[InterlockConnectionTable.ID.Name] = instanceInterlock.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockConnectionTable.TableName, data));
            data = new Dictionary<string, string>();
            data[InterlockableInterlockTable.ParentID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            data[InterlockableInterlockTable.ChildID.Name] = instance.Equipment[0].SubScope[0].Interlocks[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, InterlockableInterlockTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = instance.Equipment[0].SubScope[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, EquipmentSubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SystemHierarchyTable.TableName, data));
            

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion

        #region Connections
        [TestMethod]
        public void Bid_RemoveNetworkConnectionInBid()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            TECController child = new TECProvidedController(type, false);
            bid.AddController(controller);
            bid.AddController(child);

            TECProtocol protocol = new TECProtocol(new List<TECConnectionType>());
            type.IO.Add(new TECIO(protocol));
            IControllerConnection connection = controller.Connect(child, child.AvailableProtocols.First());

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            controller.RemoveNetworkConnection(connection as TECNetworkConnection);

            List<UpdateItem> expectedItems = new List<UpdateItem>();


            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[NetworkConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionChildrenTable.ChildID.Name] = child.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionProtocolTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionProtocolTable.ProtocolID.Name] = protocol.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionProtocolTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveNetworkConnectionToInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);

            TECController child = new TECProvidedController(type, false);
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            typical.AddController(child);
            TECSystem system = typical.AddInstance(bid);
            TECController instanceController = system.Controllers[0];

            TECProtocol protocol = new TECProtocol(new List<TECConnectionType>());
            type.IO.Add(new TECIO(protocol));
            IControllerConnection connection = controller.Connect(instanceController, instanceController.AvailableProtocols.First());

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            controller.RemoveNetworkConnection(connection as TECNetworkConnection);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[NetworkConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionChildrenTable.ChildID.Name] = instanceController.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionProtocolTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionProtocolTable.ProtocolID.Name] = protocol.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionProtocolTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveNetworkConnectionBetweenInstances()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);

            TECController controller = new TECProvidedController(type, false);
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            typical.AddController(controller);
            TECSystem system = typical.AddInstance(bid);
            TECController instanceController = system.Controllers[0];

            TECController otherController = new TECProvidedController(type, false);
            TECTypical otherTypical = new TECTypical();
            bid.Systems.Add(otherTypical);
            otherTypical.AddController(otherController);
            TECSystem otherSystem = otherTypical.AddInstance(bid);
            TECController otherInstanceController = otherSystem.Controllers[0];

            TECProtocol protocol = new TECProtocol(new List<TECConnectionType>());
            type.IO.Add(new TECIO(protocol));
            IControllerConnection connection = instanceController.Connect(otherInstanceController, otherInstanceController.AvailableProtocols.First());
            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            instanceController.RemoveNetworkConnection(connection as TECNetworkConnection);

            List<UpdateItem> expectedItems = new List<UpdateItem>();


            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[NetworkConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionChildrenTable.ChildID.Name] = otherInstanceController.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[NetworkConnectionProtocolTable.ConnectionID.Name] = connection.Guid.ToString();
            data[NetworkConnectionProtocolTable.ProtocolID.Name] = protocol.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, NetworkConnectionProtocolTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = instanceController.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeConnectionFromTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());

            TECTypical typical = new TECTypical();
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            bid.Systems.Add(typical);
            IControllerConnection connection = controller.Connect(subScope, (subScope as IConnectable).AvailableProtocols.First());

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            controller.Disconnect(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeConnectionFromInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            bid.AddController(controller);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);

            TECTypical typical = new TECTypical();
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            bid.Systems.Add(typical);
            TECSystem system = typical.AddInstance(bid);
            TECSubScope instanceSubScope = system.Equipment[0].SubScope[0];
            IControllerConnection connection = controller.Connect(instanceSubScope, (instanceSubScope as IConnectable).AvailableProtocols.First());


            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            controller.Disconnect(instanceSubScope);

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = instanceSubScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeConnectionInTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);

            TECTypical typical = new TECTypical();
            typical.AddController(controller);
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            bid.Systems.Add(typical);

            IControllerConnection connection = controller.Connect(subScope, (subScope as IConnectable).AvailableProtocols.First());


            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            controller.Disconnect(subScope);

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeConnectionInTypicalWithInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECControllerType type = new TECControllerType(new TECManufacturer());
            TECIO io = new TECIO(IOType.AI);
            type.IO.Add(io);
            TECController controller = new TECProvidedController(type, false);
            TECConnectionType connType = new TECConnectionType();
            TECDevice device = new TECDevice(new List<TECConnectionType> { connType }, new List<TECProtocol>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);

            TECTypical typical = new TECTypical();
            typical.AddController(controller);
            TECEquipment equipment = new TECEquipment(true);
            typical.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope(true);
            subScope.Devices.Add(device);
            equipment.SubScope.Add(subScope);
            bid.Systems.Add(typical);
            TECSystem system = typical.AddInstance(bid);

            TECSubScope instanceSubScope = system.Equipment[0].SubScope[0];
            TECController instanceController = system.Controllers[0];

            IControllerConnection connection = controller.Connect(subScope, (subScope as IConnectable).AvailableProtocols.First());
            IControllerConnection instanceConnection = instanceController.Connect(instanceSubScope, (instanceSubScope as IConnectable).AvailableProtocols.First());

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            controller.Disconnect(subScope);
            instanceController.Disconnect(instanceSubScope);

            Dictionary<string, string> data;

            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = connection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = connection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = connection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));
            
            data = new Dictionary<string, string>();
            data[SubScopeConnectionTable.ID.Name] = instanceConnection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SubScopeConnectionChildrenTable.ConnectionID.Name] = instanceConnection.Guid.ToString();
            data[SubScopeConnectionChildrenTable.ChildID.Name] = instanceSubScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, SubScopeConnectionChildrenTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwiredConnectionConnectionTypeTable.ConnectionID.Name] = instanceConnection.Guid.ToString();
            data[HardwiredConnectionConnectionTypeTable.TypeID.Name] = connType.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, HardwiredConnectionConnectionTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerConnectionTable.ControllerID.Name] = instanceController.Guid.ToString();
            data[ControllerConnectionTable.ConnectionID.Name] = instanceConnection.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ControllerConnectionTable.TableName, data));

            

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
        }
        #endregion
        #endregion

        #region Edit
        #region Bid
        [TestMethod]
        public void Bid_EditInfo()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            string edit = "edit";

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            Tuple<string, string> keyData = new Tuple<string, string>(BidInfoTable.ID.Name, bid.Guid.ToString());

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[BidInfoTable.Name.Name] = edit;
            expectedItems.Add(new UpdateItem(Change.Edit, BidInfoTable.TableName, data, keyData));
            
            int expectedCount = expectedItems.Count;
            
            bid.Name = edit;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }

        [TestMethod]
        public void Bid_SetParameters()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECParameters parameters = new TECParameters(bid.Guid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ParametersTable.ID.Name] = parameters.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, ParametersTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ParametersTable.ID.Name] = parameters.Guid.ToString();
            data[ParametersTable.Label.Name] = parameters.Label.ToString();
            data[ParametersTable.Escalation.Name] = parameters.Escalation.ToString();
            data[ParametersTable.Markup.Name] = parameters.Markup.ToString();
            data[ParametersTable.SubcontractorEscalation.Name] = parameters.SubcontractorEscalation.ToString();
            data[ParametersTable.Warranty.Name] = parameters.Warranty.ToString();
            data[ParametersTable.Shipping.Name] = parameters.Shipping.ToString();
            data[ParametersTable.Tax.Name] = parameters.Tax.ToString();
            data[ParametersTable.SubcontractorWarranty.Name] = parameters.SubcontractorWarranty.ToString();
            data[ParametersTable.SubcontractorShipping.Name] = parameters.SubcontractorShipping.ToString();
            data[ParametersTable.BondRate.Name] = parameters.BondRate.ToString();
            data[ParametersTable.OvertimeRatio.Name] = parameters.OvertimeRatio.ToString();
            data[ParametersTable.IsTaxExempt.Name] = parameters.IsTaxExempt.ToInt().ToString();
            data[ParametersTable.RequiresBond.Name] = parameters.RequiresBond.ToInt().ToString();
            data[ParametersTable.RequiresWrapUp.Name] = parameters.RequiresWrapUp.ToInt().ToString();
            data[ParametersTable.DesiredConfidence.Name] = parameters.DesiredConfidence.ToString();
            data[ParametersTable.PMCoef.Name] = parameters.PMCoef.ToString();
            data[ParametersTable.PMCoefStdError.Name] = parameters.PMCoefStdError.ToString();
            data[ParametersTable.PMRate.Name] = parameters.PMRate.ToString();
            data[ParametersTable.ENGCoef.Name] = parameters.ENGCoef.ToString();
            data[ParametersTable.ENGCoefStdError.Name] = parameters.ENGCoefStdError.ToString();
            data[ParametersTable.ENGRate.Name] = parameters.ENGRate.ToString();
            data[ParametersTable.CommCoef.Name] = parameters.CommCoef.ToString();
            data[ParametersTable.CommCoefStdError.Name] = parameters.CommCoefStdError.ToString();
            data[ParametersTable.CommRate.Name] = parameters.CommRate.ToString();
            data[ParametersTable.SoftCoef.Name] = parameters.SoftCoef.ToString();
            data[ParametersTable.SoftCoefStdError.Name] = parameters.SoftCoefStdError.ToString();
            data[ParametersTable.SoftRate.Name] = parameters.SoftRate.ToString();
            data[ParametersTable.GraphCoef.Name] = parameters.GraphCoef.ToString();
            data[ParametersTable.GraphCoefStdError.Name] = parameters.GraphCoefStdError.ToString();
            data[ParametersTable.GraphRate.Name] = parameters.GraphRate.ToString();
            data[ParametersTable.ElectricalRate.Name] = parameters.ElectricalRate.ToString();
            data[ParametersTable.ElectricalSuperRate.Name] = parameters.ElectricalSuperRate.ToString();
            data[ParametersTable.ElectricalNonUnionRate.Name] = parameters.ElectricalNonUnionRate.ToString();
            data[ParametersTable.ElectricalSuperNonUnionRate.Name] = parameters.ElectricalSuperNonUnionRate.ToString();
            data[ParametersTable.ElectricalSuperRatio.Name] = parameters.ElectricalSuperRatio.ToString();
            data[ParametersTable.ElectricalIsOnOvertime.Name] = parameters.ElectricalIsOnOvertime.ToInt().ToString();
            data[ParametersTable.ElectricalIsUnion.Name] = parameters.ElectricalIsUnion.ToInt().ToString();

            expectedItems.Add(new UpdateItem(Change.Add, ParametersTable.TableName, data));

            int expectedCount = expectedItems.Count;

            bid.Parameters = parameters;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }

        [TestMethod]
        public void Bid_MoveLocation()
        {
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);

            TECLocation location1 = new TECLocation();
            TECLocation location2 = new TECLocation();
            TECLocation location3 = new TECLocation();
            bid.Locations.Add(location1);
            bid.Locations.Add(location2);
            bid.Locations.Add(location3);

            DeltaStacker stack = new DeltaStacker(watcher, bid);
            
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[BidLocationTable.Index.Name] = "0";
            Tuple<string, string> keyData = new Tuple<string, string>(BidLocationTable.LocationID.Name, location2.Guid.ToString());
            expectedItems.Add(new UpdateItem(Change.Edit, BidLocationTable.TableName, data, keyData));

            data = new Dictionary<string, string>();
            data[BidLocationTable.Index.Name] = "1";
            keyData = new Tuple<string, string>(BidLocationTable.LocationID.Name, location1.Guid.ToString());
            expectedItems.Add(new UpdateItem(Change.Edit, BidLocationTable.TableName, data, keyData));

            data = new Dictionary<string, string>();
            data[BidLocationTable.Index.Name] = "2";
            keyData = new Tuple<string, string>(BidLocationTable.LocationID.Name, location3.Guid.ToString());
            expectedItems.Add(new UpdateItem(Change.Edit, BidLocationTable.TableName, data, keyData));
             
            int expectedCount = expectedItems.Count;

            bid.Locations.Move(0, 1);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);
            
        }
        #endregion
        #region System
        [TestMethod]
        public void System_Edit()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            string edit = "edit";

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            Tuple<string, string> keyData = new Tuple<string, string>(SystemTable.ID.Name, system.Guid.ToString());

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.Name.Name] = edit;
            expectedItems.Add(new UpdateItem(Change.Edit, SystemTable.TableName, data, keyData));

            int expectedCount = expectedItems.Count;

            system.Name = edit;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }

        [TestMethod]
        public void System_EditLocation()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);
            TECLocation originalLocation = new TECLocation();
            TECLocation newLocation = new TECLocation();
            bid.Locations.Add(originalLocation);
            bid.Locations.Add(newLocation);

            instance.Location = originalLocation;

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);
            
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[LocatedLocationTable.ScopeID.Name] = instance.Guid.ToString();
            data[LocatedLocationTable.LocationID.Name] = originalLocation.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, LocatedLocationTable.TableName, data));

            data = new Dictionary<string, string>();
            data[LocatedLocationTable.ScopeID.Name] = instance.Guid.ToString();
            data[LocatedLocationTable.LocationID.Name] = newLocation.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, LocatedLocationTable.TableName, data));

            int expectedCount = expectedItems.Count;

            instance.Location = newLocation;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }
        #endregion
        #region Device
        [TestMethod]
        public void Device_EditManufacturer()
        {
            //Arrange
            TECBid bid = new TECBid();
            ChangeWatcher watcher = new ChangeWatcher(bid);
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);
            TECEquipment equip = new TECEquipment(true);
            system.Equipment.Add(equip);
            TECSubScope subScope = new TECSubScope(true);
            equip.SubScope.Add(subScope);
            TECManufacturer originalManufacturer = new TECManufacturer();
            TECManufacturer newManufacturer = new TECManufacturer();
            bid.Catalogs.Manufacturers.Add(originalManufacturer);
            bid.Catalogs.Manufacturers.Add(newManufacturer);
            TECDevice device = new TECDevice(new List<TECConnectionType>(), new List<TECProtocol>(), originalManufacturer);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher, bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[HardwareManufacturerTable.HardwareID.Name] = device.Guid.ToString();
            data[HardwareManufacturerTable.ManufacturerID.Name] = originalManufacturer.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Remove, HardwareManufacturerTable.TableName, data));

            data = new Dictionary<string, string>();
            data[HardwareManufacturerTable.HardwareID.Name] = device.Guid.ToString();
            data[HardwareManufacturerTable.ManufacturerID.Name] = newManufacturer.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, HardwareManufacturerTable.TableName, data));

            int expectedCount = expectedItems.Count;

            device.Manufacturer = newManufacturer;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            CheckUpdateItems(expectedItems, stack);

        }
        #endregion
        #endregion

        public static void CheckUpdateItem(UpdateItem expectedItem, UpdateItem actualItem)
        {
            Assert.AreEqual(expectedItem.Table, actualItem.Table, "Tables do not match on UpdateItems");
            Assert.AreEqual(expectedItem.FieldData.Count, actualItem.FieldData.Count, "FieldData does not match on UpdateItems");
            Assert.AreEqual(expectedItem.Change, actualItem.Change, "Change does not match on UpdateItems");
            Assert.IsTrue((expectedItem.PrimaryKey == null && actualItem.PrimaryKey == null) || (expectedItem.PrimaryKey != null && actualItem.PrimaryKey != null), "Primary key data asymmetrically added");
            if(expectedItem.PrimaryKey != null)
            {
                Assert.AreEqual(expectedItem.PrimaryKey.Item1, actualItem.PrimaryKey.Item1, "Primary key fields do not match");
                Assert.AreEqual(expectedItem.PrimaryKey.Item2, actualItem.PrimaryKey.Item2, "Primary key values do not match");
            }
            foreach (KeyValuePair<string, string> fieldEntry in expectedItem.FieldData)
            {
                Assert.IsTrue(actualItem.FieldData.ContainsKey(fieldEntry.Key), String.Format("Field missing in actualItem for table {0}", actualItem.Table));
                Assert.AreEqual(fieldEntry.Value, actualItem.FieldData[fieldEntry.Key], String.Format("Values do not match on UpdateItems for table {0}", actualItem.Table));
            }
        }

        public static void CheckUpdateItems(List<UpdateItem> expectedItems, DeltaStacker stack)
        {
            int numToCheck = expectedItems.Count;
            foreach(UpdateItem item in expectedItems)
            {
                CheckUpdateItem(item, stack.CleansedStack()[stack.CleansedStack().Count - numToCheck]);
                numToCheck--;
            }
        }
    }
}
