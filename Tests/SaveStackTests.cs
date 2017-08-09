﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using System.Collections.ObjectModel;
using EstimatingLibrary.Utilities;

namespace Tests
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
            TECController controller = new TECController(type);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ControllerControllerTypeTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerControllerTypeTable.TypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerControllerTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[ControllerTable.ControllerID.Name] = controller.Guid.ToString();
            data[ControllerTable.Name.Name] = controller.Name;
            data[ControllerTable.Description.Name] = controller.Name;
            data[ControllerTable.Type.Name] = controller.Type.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, ControllerTable.TableName, data));

            int expectedCount = 2;

            bid.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[PanelPanelTypeTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelPanelTypeTable.PanelTypeID.Name] = type.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, PanelPanelTypeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[PanelTable.PanelID.Name] = panel.Guid.ToString();
            data[PanelTable.Name.Name] = panel.Name;
            data[PanelTable.Description.Name] = panel.Name;
            expectedItems.Add(new UpdateItem(Change.Add, PanelTable.TableName, data));

            int expectedCount = 2;
            
            bid.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECMisc misc = new TECMisc();

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[MiscTable.MiscID.Name] = misc.Guid.ToString();
            data[MiscTable.Name.Name] = misc.Name.ToString();
            data[MiscTable.Cost.Name] = misc.Cost.ToString();
            data[MiscTable.Labor.Name] = misc.Labor.ToString();
            data[MiscTable.Type.Name] = misc.Type.ToString();
            data[MiscTable.Quantity.Name] = misc.Quantity.ToString();
            
            UpdateItem expectedItem = new UpdateItem(Change.Add, MiscTable.TableName,data);
            int expectedCount = 1;

            bid.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch();

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[ScopeBranchTable.ScopeBranchID.Name] = scopeBranch.Guid.ToString();
            data[ScopeBranchTable.Label.Name] = scopeBranch.Label.ToString();

            UpdateItem expectedItem = new UpdateItem(Change.Add, ScopeBranchTable.TableName, data);
            int expectedCount = 1;

            bid.ScopeTree.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_AddSystem()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.SystemID.Name] = system.Guid.ToString();
            data[SystemTable.Name.Name] = system.Name.ToString();
            data[SystemTable.Description.Name] = system.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = system.ProposeEquipment.ToString();

            UpdateItem expectedItem = new UpdateItem(Change.Add, SystemTable.TableName, data);

            bid.Systems.Add(system);

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_AddSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = system.AddInstance(bid);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[SystemTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();

            expectedItems.Add( new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = system.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = 2;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #region Equipment
        [TestMethod]
        public void Bid_AddEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();

            Dictionary<string, string> data = new Dictionary<string, string>();
            data[EquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            data[EquipmentTable.Name.Name] = equip.Name.ToString();
            data[EquipmentTable.Description.Name] = equip.Description.ToString();

            expectedItems.Add(new UpdateItem(Change.Add, EquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = system.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = 2;

            system.Equipment.Add(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddEquipmentToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Equipment.Add(equip);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[CharacteristicScopeInstanceScopeTable.CharacteristicID.Name] = equip.Guid.ToString();
            data[CharacteristicScopeInstanceScopeTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, CharacteristicScopeInstanceScopeTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = typical.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = equip.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));
            
            int expectedCount = 3;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            Dictionary<string, string> data = new Dictionary<string, string>();
            data[CharacteristicScopeInstanceScopeTable.CharacteristicID.Name] = equip.Guid.ToString();
            data[CharacteristicScopeInstanceScopeTable.InstanceID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, CharacteristicScopeInstanceScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemEquipmentTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemEquipmentTable.EquipmentID.Name] = instance.Equipment[0].Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemEquipmentTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemTable.SystemID.Name] = instance.Guid.ToString();
            data[SystemTable.Name.Name] = instance.Name.ToString();
            data[SystemTable.Description.Name] = instance.Description.ToString();
            data[SystemTable.ProposeEquipment.Name] = instance.ProposeEquipment.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemTable.TableName, data));

            data = new Dictionary<string, string>();
            data[SystemHierarchyTable.ParentID.Name] = typical.Guid.ToString();
            data[SystemHierarchyTable.ChildID.Name] = instance.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_AddSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            Dictionary<string, string>  data = new Dictionary<string, string>();
            data[SubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            data[SubScopeTable.Name.Name] = subScope.Name.ToString();
            data[SubScopeTable.Description.Name] = subScope.Description.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SubScopeTable.TableName, data));

            data = new Dictionary<string, string>();
            data[EquipmentSubScopeTable.EquipmentID.Name] = equipment.Guid.ToString();
            data[EquipmentSubScopeTable.SubScopeID.Name] = subScope.Guid.ToString();
            expectedItems.Add(new UpdateItem(Change.Add, SystemHierarchyTable.TableName, data));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(Change.Add, system.Equipment[0], subScope));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSystem instance = system.AddInstance(bid);
            
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.AddRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(Change.Add, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_AddPointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);
            
            UpdateItem expectedItem = new UpdateItem(Change.Add, subScope, point);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_AddPointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new UpdateItem(Change.Add, system.Equipment[0].SubScope[0], point));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSystem instance = system.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.AddRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new UpdateItem(Change.Add, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_AddDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            subScope.Devices.Add(device);

            UpdateItem expectedItem = new UpdateItem(Change.Add, subScope, device);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_AddDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            subScope.Devices.Add(device);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new UpdateItem(Change.Add, system.Equipment[0].SubScope[0], device));

            int expectedCount = 2;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            TECSystem instance = system.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.AddRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new UpdateItem(Change.Add, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_AddControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.Add, controller, type));
            expectedItems.Add(new UpdateItem(Change.Add, system, controller));

            int expectedCount = 2;

            system.Controllers.Add(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Controllers.Add(controller);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Controllers[0], type));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new UpdateItem(Change.Add, controller, type));
            expectedItems.Add(new UpdateItem(Change.Add, typical, controller));

            int expectedCount = 5;

            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);

            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            typical.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Controllers[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Controllers[0], type));
            expectedItems.Add(new UpdateItem(Change.Add, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_AddPanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.Add, panel, type));
            expectedItems.Add(new UpdateItem(Change.Add, system, panel));
            int expectedCount = 2;

            system.Panels.Add(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddPanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.Panels.Add(panel);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Panels[0], type));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new UpdateItem(Change.Add, panel, type));
            expectedItems.Add(new UpdateItem(Change.Add, typical, panel));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.AddRelationship, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(Change.Add, instance, instance.Panels[0]));
            expectedItems.Add(new UpdateItem(Change.Add, instance.Panels[0], type));
            expectedItems.Add(new UpdateItem(Change.Add, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_AddMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(Change.Add, system, misc);
            int expectedCount = 1;

            system.MiscCosts.Add(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_AddMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.MiscCosts.Add(misc);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.Add, typical, misc));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_AddInstanceToTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            TECSystem instance = typical.AddInstance(bid);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(Change.Add, typical, instance));

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_AddScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(Change.Add, system, scopeBranch);
            int expectedCount = 1;

            system.ScopeBranches.Add(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
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
            TECController controller = new TECController(type);
            bid.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, controller, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, bid, controller));

            int expectedCount = 2;

            bid.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, panel, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, bid, panel));
            int expectedCount = 2;

            bid.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECMisc misc = new TECMisc();
            bid.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, bid, misc);
            int expectedCount = 1;

            bid.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranch()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.ScopeTree.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, bid, scopeBranch);
            int expectedCount = 1;

            bid.ScopeTree.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #endregion

        #region System
        [TestMethod]
        public void Bid_RemoveSystem()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, bid, system);

            bid.Systems.Remove(system);

            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveSystemInstance()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            system.SystemInstances.Remove(instance);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, system, instance);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #region Equipment
        [TestMethod]
        public void Bid_RemoveEquipmentToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(system);
            system.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, system, equip);
            int expectedCount = 1;

            system.Equipment.Remove(equip);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveEquipmentToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Equipment.Add(equip);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Equipment[0];
            typical.Equipment.Remove(equip);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, equip, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, removed));
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, equip));

            int expectedCount = 3;
            

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithEquipment()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECEquipment equip = new TECEquipment();
            bid.Systems.Add(typical);
            typical.Equipment.Add(equip);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, equip, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, instance));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region SubScope
        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            equipment.SubScope.Remove(subScope);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, equipment, subScope);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveSubScopeToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSystem instance = system.AddInstance(bid);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Equipment[0].SubScope[0];
            equipment.SubScope.Remove(subScope);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, subScope, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0], removed));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system.Equipment[0], subScope));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToSystemWithSubScope()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            system.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system, instance));

            int expectedCount = 5;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Point
        [TestMethod]
        public void Bid_RemovePointToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            subScope.Points.Remove(point);

            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, subScope, point);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemovePointToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Equipment[0].SubScope[0].Points[0];
            subScope.Points.Remove(point);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, point, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0].SubScope[0], removed));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system.Equipment[0].SubScope[0], point));

            int expectedCount = 3;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToSystemWithPoint()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECPoint point = new TECPoint();
            subScope.Points.Add(point);
            TECSystem instance = system.AddInstance(bid);
            
            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            system.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Points[0]));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, point, instance.Equipment[0].SubScope[0].Points[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system, instance));

            int expectedCount = 7;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Device
        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            subScope.Devices.Remove(device);

            UpdateItem expectedItem = new UpdateItem(StackChange.RemoveRelationship, subScope, device);
            int expectedCount = 1;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveDeviceToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECSystem instance = system.AddInstance(bid);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            subScope.Devices.Remove(device);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, instance.Equipment[0].SubScope[0], device));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, system.Equipment[0].SubScope[0], device));

            int expectedCount = 2;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToSystemWithDevice()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            TECEquipment equipment = new TECEquipment();
            system.Equipment.Add(equipment);
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            TECDevice device = new TECDevice(new ObservableCollection<TECElectricalMaterial>(), new TECManufacturer());
            bid.Catalogs.Devices.Add(device);
            subScope.Devices.Add(device);
            TECSystem instance = system.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            system.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, instance.Equipment[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0], instance.Equipment[0].SubScope[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Equipment[0].SubScope[0], instance.Equipment[0].SubScope[0].Devices[0]));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, equipment, instance.Equipment[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, subScope, instance.Equipment[0].SubScope[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Controller
        [TestMethod]
        public void Bid_RemoveControllerToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(system);
            system.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, controller, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system, controller));

            int expectedCount = 2;

            system.Controllers.Remove(controller);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveControllerToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Controllers.Add(controller);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Controllers[0];
            typical.Controllers.Remove(controller);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, controller, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, removed, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, removed));
            expectedItems.Add(new UpdateItem(StackChange.Remove, controller, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, controller));

            int expectedCount = expectedItems.Count;
            
            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithController()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECManufacturer manufacturer = new TECManufacturer();
            TECControllerType type = new TECControllerType(manufacturer);
            TECController controller = new TECController(type);
            bid.Systems.Add(typical);
            typical.Controllers.Add(controller);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            typical.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, instance.Controllers[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Controllers[0], instance.Controllers[0].Type));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, controller, instance.Controllers[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Panel
        [TestMethod]
        public void Bid_RemovePanelToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(system);
            system.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, panel, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, system, panel));
            int expectedCount = 2;

            system.Panels.Remove(panel);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemovePanelToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.Panels.Add(panel);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            var removed = instance.Panels[0];
            typical.Panels.Remove(panel);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, panel, removed, typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, removed, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, removed));
            expectedItems.Add(new UpdateItem(StackChange.Remove, panel, type));
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, panel));

            int expectedCount = expectedItems.Count;


            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithPanel()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECPanelType type = new TECPanelType(new TECManufacturer());
            TECPanel panel = new TECPanel(type);
            bid.Systems.Add(typical);
            typical.Panels.Add(panel);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            typical.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance, instance.Panels[0]));
            expectedItems.Add(new UpdateItem(StackChange.Remove, instance.Panels[0], type));
            expectedItems.Add(new UpdateItem(StackChange.RemoveRelationship, panel, instance.Panels[0], typeof(TECScope), typeof(TECScope)));
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Misc
        [TestMethod]
        public void Bid_RemoveMiscToTypicalWithout()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(system);
            system.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, system, misc);
            int expectedCount = 1;

            system.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }

        [TestMethod]
        public void Bid_RemoveMiscToTypicalWith()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            typical.MiscCosts.Add(misc);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, misc));
            int expectedCount = expectedItems.Count;
            typical.MiscCosts.Remove(misc);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }

        [TestMethod]
        public void Bid_RemoveInstanceToTypicalWithMisc()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem typical = new TECSystem();
            TECMisc misc = new TECMisc();
            bid.Systems.Add(typical);
            typical.MiscCosts.Add(misc);
            TECSystem instance = typical.AddInstance(bid);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            typical.SystemInstances.Remove(instance);

            List<UpdateItem> expectedItems = new List<UpdateItem>();
            expectedItems.Add(new UpdateItem(StackChange.Remove, typical, instance));

            int expectedCount = expectedItems.Count;

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItems(expectedItems, stack);
        }
        #endregion
        #region Scope Branch
        [TestMethod]
        public void Bid_RemoveScopeBranchToTypical()
        {
            //Arrange
            TECBid bid = new TECBid(); ChangeWatcher watcher = new ChangeWatcher(bid);
            TECSystem system = new TECSystem();
            TECScopeBranch scopeBranch = new TECScopeBranch();
            bid.Systems.Add(system);
            system.ScopeBranches.Add(scopeBranch);

            //Act
            DeltaStacker stack = new DeltaStacker(watcher);
            UpdateItem expectedItem = new UpdateItem(StackChange.Remove, system, scopeBranch);
            int expectedCount = 1;

            system.ScopeBranches.Remove(scopeBranch);

            //Assert
            Assert.AreEqual(expectedCount, stack.CleansedStack().Count);
            checkUpdateItem(expectedItem, stack.CleansedStack()[stack.CleansedStack().Count - 1]);
        }
        #endregion
        #endregion
        #endregion

        public void checkUpdateItem(UpdateItem expectedItem, UpdateItem actualItem)
        {
            Assert.AreEqual(expectedItem.Change, actualItem.Change);
            Assert.AreEqual(expectedItem.ReferenceObject, actualItem.ReferenceObject);
            Assert.AreEqual(expectedItem.TargetObject, actualItem.TargetObject);
            Assert.AreEqual(expectedItem.ReferenceType, actualItem.ReferenceType);
            Assert.AreEqual(expectedItem.TargetType, actualItem.TargetType);
        }

        public void checkUpdateItems(List<UpdateItem> expectedItems, DeltaStacker stack)
        {
            int numToCheck = expectedItems.Count;
            foreach(UpdateItem item in expectedItems)
            {
                checkUpdateItem(item, stack.CleansedStack()[stack.CleansedStack().Count - numToCheck]);
                numToCheck--;
            }
        }
    }
}
