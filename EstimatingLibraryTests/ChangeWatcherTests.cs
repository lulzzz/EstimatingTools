﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tests.CostTestingUtilities;
using static Tests.TestHelper;

namespace Tests
{
    [TestClass]
    public class ChangeWatcherTests
    {
        private TECBid bid;
        private ChangeWatcher cw;

        private TECChangedEventArgs changedArgs;
        private TECChangedEventArgs instanceChangedArgs;
        private CostBatch costDelta;
        private int pointDelta;

        private bool changedRaised;
        private bool instanceChangedRaised;
        private bool costChangedRaised;
        private bool pointChangedRaised;

        [TestInitialize]
        public void TestInitialize()
        {
            bid = TestHelper.CreateTestBid();
            cw = new ChangeWatcher(bid);

            resetRaised();

            cw.Changed += (args) =>
            {
                changedArgs = args;
                changedRaised = true;
            };
            cw.InstanceChanged += (args) =>
            {
                instanceChangedArgs = args;
                instanceChangedRaised = true;
            };
            cw.CostChanged += (costs) =>
            {
                costDelta = costs;
                costChangedRaised = true;
            };
            cw.PointChanged += (numPoints) =>
            {
                pointDelta = numPoints;
                pointChangedRaised = true;
            };
        }

        #region Change Events Tests
        #region Add Tests
        [TestMethod]
        public void AddTypicalToBid()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            //Ensure typical has points and cost:
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);

            resetRaised();

            //Act
            bid.Systems.Add(typical);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Systems", bid, typical);
        }

        [TestMethod]
        public void AddControllerToBid()
        {
            //Arrange
            TECControllerType controllerType = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(controllerType);

            //Act
            bid.Controllers.Add(controller);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Controllers", bid, controller);
            checkCostDelta(controller.CostBatch);
        }

        [TestMethod]
        public void AddPanelToBid()
        {
            //Arrange
            TECPanelType panelType = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(panelType);

            //Act
            bid.Panels.Add(panel);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Panels", bid, panel);
            checkCostDelta(panel.CostBatch);
        }

        [TestMethod]
        public void AddMiscToBid()
        {
            //Arrange
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);

            //Act
            bid.MiscCosts.Add(misc);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "MiscCosts", bid, misc);
            checkCostDelta(misc.CostBatch);
        }

        [TestMethod]
        public void AddScopeBranchToBid()
        {
            //Arrange
            TECScopeBranch sb = new TECScopeBranch();

            //Act
            bid.ScopeTree.Add(sb);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Add, "ScopeTree", bid, sb);
        }

        [TestMethod]
        public void AddNoteToBid()
        {
            //Arrange
            TECLabeled note = new TECLabeled();

            //Act
            bid.Notes.Add(note);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Add, "Notes", bid, note);
        }

        [TestMethod]
        public void AddExclusionToBid()
        {
            //Arrange
            TECLabeled exclusion = new TECLabeled();

            //Act
            bid.Exclusions.Add(exclusion);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Add, "Exclusions", bid, exclusion);
        }

        [TestMethod]
        public void AddLocationToBid()
        {
            //Arrange
            TECLabeled location = new TECLabeled();

            //Act
            bid.Locations.Add(location);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Add, "Locations", bid, location);
        }

        [TestMethod]
        public void AddInstanceToTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            //Ensure typical has points and cost:
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            bid.Systems.Add(typical);

            resetRaised();

            //Act
            TECSystem instance = typical.AddInstance(bid);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Add, "Instances", typical, instance);
            checkCostDelta(instance.CostBatch);
            checkPointDelta(instance.PointNumber);
        }

        [TestMethod]
        public void AddEquipmentToTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECEquipment equip = new TECEquipment();
            //Ensure equip has points and cost:
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);

            resetRaised();

            //Act
            typical.Equipment.Add(equip);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Equipment", typical, equip);
        }

        [TestMethod]
        public void AddControllerToTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(type);

            resetRaised();

            //Act
            typical.Controllers.Add(controller);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Controllers", typical, controller);
        }

        [TestMethod]
        public void AddPanelToTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECPanelType type = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(type);

            resetRaised();

            //Act
            typical.Panels.Add(panel);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Panels", typical, panel);
        }

        [TestMethod]
        public void AddMiscCostToTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);

            resetRaised();

            //Act
            typical.MiscCosts.Add(misc);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "MiscCosts", typical, misc);
        }

        [TestMethod]
        public void AddScopeBranchToTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECScopeBranch sb = new TECScopeBranch();

            resetRaised();

            //Act
            typical.ScopeBranches.Add(sb);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "ScopeBranches", typical, sb);
        }

        [TestMethod]
        public void AddEquipmentToInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);

            resetRaised();

            //Act
            instance.Equipment.Add(equip);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Add, "Equipment", instance, equip);
            checkCostDelta(equip.CostBatch);
            checkPointDelta(equip.PointNumber);
        }

        [TestMethod]
        public void AddControllerToInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(type);

            resetRaised();

            //Act
            instance.Controllers.Add(controller);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Controllers", instance, controller);
            checkCostDelta(controller.CostBatch);
        }

        [TestMethod]
        public void AddPanelToInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECPanelType panelType = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(panelType);

            resetRaised();

            //Act
            instance.Panels.Add(panel);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Panels", instance, panel);
            checkCostDelta(panel.CostBatch);
        }

        [TestMethod]
        public void AddMiscCostToInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);

            resetRaised();

            //Act
            instance.MiscCosts.Add(misc);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "MiscCosts", instance, misc);
            checkCostDelta(misc.CostBatch);
        }

        [TestMethod]
        public void AddSubScopeToTypicalEquipment()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment();
            typical.Equipment.Add(equip);
            bid.Systems.Add(typical);
            TECSubScope ss = new TECSubScope();
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            TECPoint point = new TECPoint();
            point.Type = PointTypes.BI;
            point.Quantity = 2;
            ss.Points.Add(point);

            resetRaised();

            //Act
            equip.SubScope.Add(ss);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "SubScope", equip, ss);
        }

        [TestMethod]
        public void AddDeviceToTypicalSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();

            resetRaised();

            //Act
            ss.Devices.Add(dev);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Devices", ss, dev);
        }

        [TestMethod]
        public void AddPointToTypicalSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;

            resetRaised();

            //Act
            ss.Points.Add(point);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Add, "Points", ss, point);
        }

        [TestMethod]
        public void AddSubScopeToInstanceEquipment()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            instance.Equipment.Add(equip);

            TECSubScope ss = new TECSubScope();
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);

            resetRaised();

            //Act
            equip.SubScope.Add(ss);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Add, "SubScope", equip, ss);
            checkCostDelta(ss.CostBatch);
            checkPointDelta(ss.PointNumber);
        }

        [TestMethod]
        public void AddDeviceToInstanceSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            instance.Equipment.Add(equip);

            TECDevice dev = bid.Catalogs.Devices.RandomObject();

            resetRaised();

            //Act
            ss.Devices.Add(dev);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "Devices", ss, dev);
            checkCostDelta(dev.CostBatch);
        }

        [TestMethod]
        public void AddPointToInstanceSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            instance.Equipment.Add(equip);

            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;

            resetRaised();

            //Act
            ss.Points.Add(point);

            //Assert
            checkRaised(true, false, true);
            checkInstanceChangedArgs(Change.Add, "Points", ss, point);
            checkPointDelta(point.PointNumber);
        }

        [TestMethod]
        public void AddNetworkConnectionToBidController()
        {
            //Arrange
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController parentController = new TECController(type);
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(type);
            bid.Controllers.Add(childController);

            TECElectricalMaterial connectionType = bid.Catalogs.ConnectionTypes.RandomObject();

            resetRaised();

            //Act
            TECNetworkConnection netConnect = parentController.AddController(childController, connectionType);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "ChildrenConnections", parentController, netConnect);
            checkCostDelta(netConnect.CostBatch);
        }

        [TestMethod]
        public void AddNetworkConnectionToInstanceController()
        {
            //Arrange
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController parentController = new TECController(type);
            bid.Controllers.Add(parentController);

            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);

            TECController childController = new TECController(type);
            typical.Controllers.Add(childController);

            TECElectricalMaterial connectionType = bid.Catalogs.ConnectionTypes.RandomObject();
            
            resetRaised();

            //Act
            TECNetworkConnection netConnect = parentController.AddController(childController, connectionType);
            netConnect.Length = 10;

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "ChildrenConnections", parentController, netConnect);
            checkCostDelta(netConnect.CostBatch);
        }

        [TestMethod]
        public void AddSubScopeConnectionToBidController()
        {
            //Arrange
            TECTypical system = new TECTypical();
            TECEquipment equipment = new TECEquipment();
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            TECSubScope instanceSubScope = system.Instances[0].Equipment[0].SubScope[0];

            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            bid.Controllers.Add(controller);

            resetRaised();

            //Act
            TECSubScopeConnection connection = controller.AddSubScope(instanceSubScope);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "ChildrenConnections", controller, connection);
            checkCostDelta(connection.CostBatch);
        }

        [TestMethod]
        public void AddSubScopeConnectionToTypicalController()
        {
            //Arrange
            TECTypical system = new TECTypical();
            TECEquipment equipment = new TECEquipment();
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
           
            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            system.Controllers.Add(controller);

            resetRaised();

            //Act
            TECSubScopeConnection connection = controller.AddSubScope(subScope);

            //Assert
            checkRaised(false, false, false);
            checkInstanceChangedArgs(Change.Add, "ChildrenConnections", controller, connection);
            checkCostDelta(connection.CostBatch);
        }

        [TestMethod]
        public void AddSubScopeConnectionToInstanceController()
        {
            //Arrange
            TECTypical system = new TECTypical();
            TECEquipment equipment = new TECEquipment();
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            system.Controllers.Add(controller);
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);
            TECSubScope instanceSubScope = instance.Equipment[0].SubScope[0];
            TECController instanceController = instance.Controllers[0];

            resetRaised();

            //Act
            TECSubScopeConnection connection = instanceController.AddSubScope(instanceSubScope);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Add, "ChildrenConnections", controller, connection);
            checkCostDelta(connection.CostBatch);
        }

        [TestMethod]
        public void AddControllerToNetworkConnection()
        {
            //Arrange
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController parentController = new TECController(type);
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(type);
            bid.Controllers.Add(childController);

            TECController daisyController = new TECController(type);

            TECElectricalMaterial connectionType = bid.Catalogs.ConnectionTypes.RandomObject();
            TECNetworkConnection netConnect = parentController.AddController(childController, connectionType);

            resetRaised();

            //Act
            parentController.AddController(daisyController, netConnect);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Add, "ChildrenControllers", netConnect, daisyController);
        }
        #endregion

        #region Remove Tests
        [TestMethod]
        public void RemoveTypicalFromBid()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            //Ensure typical has points and cost:
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            bid.Systems.Add(typical);

            resetRaised();

            //Act
            bid.Systems.Remove(typical);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "Systems", bid, typical);
        }

        [TestMethod]
        public void RemoveControllerFormBid()
        {
            //Arrange
            TECControllerType controllerType = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(controllerType);
            bid.Controllers.Add(controller);

            resetRaised();

            //Act
            bid.Controllers.Remove(controller);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "Controllers", bid, controller);
            checkCostDelta(controller.CostBatch * -1);
        }

        [TestMethod]
        public void RemovePanelFromBid()
        {
            //Arrange
            TECPanelType panelType = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(panelType);
            bid.Panels.Add(panel);

            resetRaised();
            //Act
            bid.Panels.Remove(panel);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "Panels", bid, panel);
            checkCostDelta(panel.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveMiscFromBid()
        {
            //Arrange
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);
            bid.MiscCosts.Add(misc);

            resetRaised();
            //Act
            bid.MiscCosts.Remove(misc);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "MiscCosts", bid, misc);
            checkCostDelta(misc.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveScopeBranchFromBid()
        {
            //Arrange
            TECScopeBranch sb = new TECScopeBranch();
            bid.ScopeTree.Add(sb);

            resetRaised();
            //Act
            bid.ScopeTree.Remove(sb);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Remove, "ScopeTree", bid, sb);
        }

        [TestMethod]
        public void RemoveNoteFromBid()
        {
            //Arrange
            TECLabeled note = new TECLabeled();
            bid.Notes.Add(note);

            resetRaised();
            //Act
            bid.Notes.Remove(note);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Remove, "Notes", bid, note);
        }

        [TestMethod]
        public void RemoevExclusionFromBid()
        {
            //Arrange
            TECLabeled exclusion = new TECLabeled();
            bid.Exclusions.Add(exclusion);

            resetRaised();
            //Act
            bid.Exclusions.Remove(exclusion);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Remove, "Exclusions", bid, exclusion);
        }

        [TestMethod]
        public void RemoveLocationFromBid()
        {
            //Arrange
            TECLabeled location = new TECLabeled();
            bid.Locations.Add(location);

            resetRaised();
            //Act
            bid.Locations.Add(location);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Remove, "Locations", bid, location);
        }

        [TestMethod]
        public void RemoveInstanceFromTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            //Ensure typical has points and cost:
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);

            resetRaised();

            //Act
            typical.Instances.Remove(instance);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Remove, "Instances", typical, instance);
            checkCostDelta(instance.CostBatch * -1);
            checkPointDelta(instance.PointNumber * -1);
        }

        [TestMethod]
        public void RemoveEquipmentFromTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECEquipment equip = new TECEquipment();
            //Ensure equip has points and cost:
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);

            resetRaised();

            //Act
            typical.Equipment.Remove(equip);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "Equipment", typical, equip);
        }

        [TestMethod]
        public void RemoveControllerFromTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(type);
            typical.Controllers.Add(controller);

            resetRaised();

            //Act
            typical.Controllers.Remove(controller);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "Controllers", typical, controller);
        }

        [TestMethod]
        public void RemovePanelFromTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECPanelType type = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(type);
            typical.Panels.Add(panel);

            resetRaised();

            //Act
            typical.Panels.Remove(panel);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "Panels", typical, panel);
        }

        [TestMethod]
        public void RemoveMiscCostFromTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);
            typical.MiscCosts.Add(misc);

            resetRaised();

            //Act
            typical.MiscCosts.Remove(misc);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "MiscCosts", typical, misc);
        }

        [TestMethod]
        public void RemoveScopeBranchFromTypicalSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECScopeBranch sb = new TECScopeBranch();
            typical.ScopeBranches.Add(sb);

            resetRaised();

            //Act
            typical.ScopeBranches.Remove(sb);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "ScopeBranches", typical, sb);
        }

        [TestMethod]
        public void RemoveEquipmentFromInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            equip.SubScope.Add(ss);
            instance.Equipment.Add(equip);

            resetRaised();

            //Act
            instance.Equipment.Remove(equip);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Remove, "Equipment", instance, equip);
            checkCostDelta(equip.CostBatch * -1);
            checkPointDelta(equip.PointNumber * -1);
        }

        [TestMethod]
        public void RemoveControllerFromInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController controller = new TECController(type);
            instance.Controllers.Add(controller);

            resetRaised();

            //Act
            instance.Controllers.Remove(controller);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "Controllers", instance, controller);
            checkCostDelta(controller.CostBatch * -1);
        }

        [TestMethod]
        public void RemovePanelFromInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECPanelType panelType = bid.Catalogs.PanelTypes.RandomObject();
            TECPanel panel = new TECPanel(panelType);
            instance.Panels.Add(panel);

            resetRaised();

            //Act
            instance.Panels.Remove(panel);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "Panels", instance, panel);
            checkCostDelta(panel.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveMiscCostFromInstanceSystem()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Cost = RandomDouble(1, 100);
            misc.Labor = RandomDouble(1, 100);
            instance.MiscCosts.Add(misc);

            resetRaised();

            //Act
            instance.MiscCosts.Remove(misc);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "MiscCosts", instance, misc);
            checkCostDelta(misc.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveSubScopeFromTypicalEquipment()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment();
            typical.Equipment.Add(equip);
            bid.Systems.Add(typical);
            TECSubScope ss = new TECSubScope();
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            TECPoint point = new TECPoint();
            point.Type = PointTypes.BI;
            point.Quantity = 2;
            ss.Points.Add(point);
            equip.SubScope.Add(ss);

            resetRaised();

            //Act
            equip.SubScope.Remove(ss);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "SubScope", equip, ss);
        }

        [TestMethod]
        public void RemoveDeviceFromTypicalSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);

            resetRaised();

            //Act
            ss.Devices.Remove(dev);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "Devices", ss, dev);
        }

        [TestMethod]
        public void RemovePointFromTypicalSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            typical.Equipment.Add(equip);
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);

            resetRaised();

            //Act
            ss.Points.Remove(point);

            //Assert
            checkRaised(false, false, false);
            checkChangedArgs(Change.Remove, "Points", ss, point);
        }

        [TestMethod]
        public void RemoveSubScopeFromInstanceEquipment()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            instance.Equipment.Add(equip);

            TECSubScope ss = new TECSubScope();
            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);
            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);
            equip.SubScope.Add(ss);

            resetRaised();

            //Act
            equip.SubScope.Remove(ss);

            //Assert
            checkRaised(true, true, true);
            checkInstanceChangedArgs(Change.Remove, "SubScope", equip, ss);
            checkCostDelta(ss.CostBatch * -1);
            checkPointDelta(ss.PointNumber * -1);
        }

        [TestMethod]
        public void RemoveDeviceFromInstanceSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            instance.Equipment.Add(equip);

            TECDevice dev = bid.Catalogs.Devices.RandomObject();
            ss.Devices.Add(dev);

            resetRaised();

            //Act
            ss.Devices.Remove(dev);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "Devices", ss, dev);
            checkCostDelta(dev.CostBatch * -1);
        }

        [TestMethod]
        public void RemovePointFromInstanceSubScope()
        {
            //Arrange
            TECTypical typical = new TECTypical();
            bid.Systems.Add(typical);
            TECSystem instance = typical.AddInstance(bid);
            TECEquipment equip = new TECEquipment();
            TECSubScope ss = new TECSubScope();
            equip.SubScope.Add(ss);
            instance.Equipment.Add(equip);

            TECPoint point = new TECPoint();
            point.Type = PointTypes.AI;
            point.Quantity = 2;
            ss.Points.Add(point);

            resetRaised();

            //Act
            ss.Points.Remove(point);

            //Assert
            checkRaised(true, false, true);
            checkInstanceChangedArgs(Change.Remove, "Points", ss, point);
            checkPointDelta(point.PointNumber * -1);
        }

        [TestMethod]
        public void RemoveNetworkConnectionFromBidController()
        {
            //Arrange
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController parentController = new TECController(type);
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(type);
            bid.Controllers.Add(childController);

            TECElectricalMaterial connectionType = bid.Catalogs.ConnectionTypes.RandomObject();

            TECNetworkConnection netConnect = parentController.AddController(childController, connectionType);

            resetRaised();

            //Act
            parentController.RemoveController(childController);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "ChildrenConnections", parentController, netConnect);
            checkCostDelta(netConnect.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveNetworkConnectionFromInstanceController()
        {
            //Arrange
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController parentController = new TECController(type);
            bid.Controllers.Add(parentController);
            
            TECTypical system = new TECTypical();
            bid.Systems.Add(system);

            TECController childController = new TECController(type);
            system.Controllers.Add(childController);

            TECElectricalMaterial connectionType = bid.Catalogs.ConnectionTypes.RandomObject();

            TECNetworkConnection netConnect = parentController.AddController(childController, connectionType);
            netConnect.Length = 10;

            resetRaised();

            //Act
            parentController.RemoveController(childController);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "ChildrenConnections", parentController, netConnect);
            checkCostDelta(netConnect.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveSubScopeConenctionFromBidController()
        {
            //Arrange
            TECTypical system = new TECTypical();
            TECEquipment equipment = new TECEquipment();
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);
            system.AddInstance(bid);
            TECSubScope instanceSubScope = system.Instances[0].Equipment[0].SubScope[0];

            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            bid.Controllers.Add(controller);
            TECSubScopeConnection connection = controller.AddSubScope(instanceSubScope);
            connection.Length = 10;
            resetRaised();

            //Act
            controller.RemoveSubScope(instanceSubScope);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "ChildrenConnections", controller, connection);
            checkCostDelta(connection.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveSubScopeConnectionFromTypicalController()
        {
            //Arrange
            TECTypical system = new TECTypical();
            TECEquipment equipment = new TECEquipment();
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            bid.Systems.Add(system);

            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            system.Controllers.Add(controller);
            TECSubScopeConnection connection = controller.AddSubScope(subScope);
            connection.Length = 10;
            resetRaised();

            //Act
            controller.RemoveSubScope(subScope);

            //Assert
            checkRaised(false, false, false);
            checkInstanceChangedArgs(Change.Remove, "ChildrenConnections", controller, connection);
            checkCostDelta(connection.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveSubScopeConnectionFromInstanceController()
        {
            //Arrange
            TECTypical system = new TECTypical();
            TECEquipment equipment = new TECEquipment();
            TECSubScope subScope = new TECSubScope();
            equipment.SubScope.Add(subScope);
            system.Equipment.Add(equipment);
            TECController controller = new TECController(bid.Catalogs.ControllerTypes.RandomObject());
            system.Controllers.Add(controller);
            bid.Systems.Add(system);
            TECSystem instance = system.AddInstance(bid);
            TECSubScope instanceSubScope = instance.Equipment[0].SubScope[0];
            TECController instanceController = instance.Controllers[0];
            TECSubScopeConnection connection = instanceController.AddSubScope(instanceSubScope);
            connection.Length = 10;
            resetRaised();

            //Act
            instanceController.RemoveSubScope(instanceSubScope);

            //Assert
            checkRaised(true, true, false);
            checkInstanceChangedArgs(Change.Remove, "ChildrenConnections", controller, connection);
            checkCostDelta(connection.CostBatch * -1);
        }

        [TestMethod]
        public void RemoveControllerFromNetworkConnection()
        {
            //Arrange
            TECControllerType type = bid.Catalogs.ControllerTypes.RandomObject();
            TECController parentController = new TECController(type);
            bid.Controllers.Add(parentController);

            TECController childController = new TECController(type);
            bid.Controllers.Add(childController);

            TECController daisyController = new TECController(type);

            TECElectricalMaterial connectionType = bid.Catalogs.ConnectionTypes.RandomObject();
            TECNetworkConnection netConnect = parentController.AddController(childController, connectionType);
            parentController.AddController(daisyController, netConnect);

            resetRaised();

            //Act
            parentController.RemoveController(daisyController);

            //Assert
            checkRaised(true, false, false);
            checkInstanceChangedArgs(Change.Remove, "ChildrenControllers", netConnect, daisyController);
            checkCostDelta(netConnect.CostBatch * -1);
        }
        #endregion

        #endregion

        private void resetRaised()
        {
            changedRaised = false;
            instanceChangedRaised = false;
            costChangedRaised = false;
            pointChangedRaised = false;
        }

        #region Check Methods
        private void checkRaised(bool instanceChanged, bool costChanged, bool pointChanged)
        {
            Assert.IsTrue(changedRaised, "Changed event on the ChangeWatcher wasn't raised.");

            if (instanceChanged)
            {
                Assert.IsTrue(instanceChangedRaised, "InstanceChanged event on the ChangeWatcher wasn't raised when it should have been.");
            }
            else
            {
                Assert.IsFalse(instanceChangedRaised, "InstanceChanged event on the ChangeWatcher was raised when it shouldn't have been.");
            }
            
            if (costChanged)
            {
                Assert.IsTrue(costChangedRaised, "CostChanged event on the ChangeWatcher wasn't raised when it should have been.");
            }
            else
            {
                Assert.IsFalse(costChangedRaised, "CostChanged event on the ChangeWatcher was raised when it shouldn't have been.");
            }
            
            if (pointChanged)
            {
                Assert.IsTrue(pointChangedRaised, "PointChanged event on the ChangeWatcher wasn't raised when it should have been.");
            }
            else
            {
                Assert.IsFalse(pointChangedRaised, "PointChanged event on the ChangeWatcher was raised when it shouldn't have been.");
            }
        }

        private void checkChangedArgs(Change change, string propertyName, TECObject sender, object value, object oldValue = null)
        {
            Assert.AreEqual(changedArgs.Change, change, "Change type is wrong.");
            Assert.AreEqual(changedArgs.PropertyName, propertyName, "PropertyName is wrong.");
            Assert.AreEqual(changedArgs.Sender, sender, "Sender is wrong.");
            Assert.AreEqual(changedArgs.Value, value, "Value is wrong.");

            if (oldValue != null)
            {
                Assert.AreEqual(changedArgs.OldValue, oldValue, "OldValue is wrong.");
            }
        }
        private void checkInstanceChangedArgs(Change change, string propertyName, TECObject sender, object value, object oldValue = null)
        {
            checkChangedArgs(change, propertyName, sender, value, oldValue);

            Assert.AreEqual(instanceChangedArgs.Change, change, "Change type is wrong.");
            Assert.AreEqual(instanceChangedArgs.PropertyName, propertyName, "PropertyName is wrong.");
            Assert.AreEqual(instanceChangedArgs.Sender, sender, "Sender is wrong.");
            Assert.AreEqual(instanceChangedArgs.Value, value, "Value is wrong.");

            if (oldValue != null)
            {
                Assert.AreEqual(instanceChangedArgs.OldValue, oldValue, "OldValue is wrong.");
            }
        }

        private void checkCostDelta(CostBatch cb)
        {
            Assert.AreEqual(cb.GetCost(CostType.TEC), costDelta.GetCost(CostType.TEC), DELTA, "ChangeWatcher TEC Cost delta is wrong.");
            Assert.AreEqual(cb.GetLabor(CostType.TEC), costDelta.GetLabor(CostType.TEC), DELTA, "ChangeWatcher TEC Labor delta is wrong.");
            Assert.AreEqual(cb.GetCost(CostType.Electrical), costDelta.GetCost(CostType.Electrical), DELTA, "ChangeWatcher Elec Cost delta is wrong.");
            Assert.AreEqual(cb.GetLabor(CostType.Electrical), costDelta.GetLabor(CostType.Electrical), DELTA, "ChangeWatcher Elec Labor delta is wrong.");
        }

        private void checkPointDelta(int points)
        {
            Assert.AreEqual(points, pointDelta, "ChangeWatcher point delta is wrong.");
        }
        #endregion
    }
}
