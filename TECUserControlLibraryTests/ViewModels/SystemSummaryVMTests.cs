﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECUserControlLibrary.ViewModels.SummaryVMs;
using EstimatingLibrary.Utilities;
using EstimatingLibrary;
using TECUserControlLibrary.Models;
using EstimatingLibrary.Interfaces;
using System.Linq;
using TestLibrary.ModelTestingUtilities;

namespace ViewModels
{
    /// <summary>
    /// Summary description for SystemSummaryVMTests
    /// </summary>
    [TestClass]
    public class SystemSummaryVMTests
    {
        Random rand;

        public SystemSummaryVMTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void TestIinitialize()
        {
            rand = new Random(0);
        }

        [TestMethod]
        public void TotalMatches()
        {
            var bid = new TECBid();
            var parameters = ModelCreation.TestParameters(rand, bid);
            var catalogs = ModelCreation.TestCatalogs(rand);
            bid.Parameters = parameters;
            bid.Catalogs = catalogs;
            ChangeWatcher watcher = new ChangeWatcher(bid);

            SystemSummaryVM summaryVM = new SystemSummaryVM(bid, watcher);

            List<Tuple<TECEstimator, TECTypical>> estimates = new List<Tuple<TECEstimator, TECTypical>>();

            int x = 6;
            for (int i = 0; i < x; i++)
            {
                TECTypical typical1 = createTypical(bid);
                TECEstimator estimate1 = new TECEstimator(typical1, bid.Parameters, new TECExtraLabor(Guid.NewGuid()), bid.Duration, new ChangeWatcher(typical1));
                estimates.Add(new Tuple<TECEstimator, TECTypical>(estimate1, typical1));
            }

            double previous = estimates[0].Item1.TotalPrice;
            foreach(var thing in estimates)
            {
                Assert.AreEqual(thing.Item1.TotalPrice, previous);
                previous = thing.Item1.TotalPrice;
            }

            int y = 0;
            foreach (var item in estimates)
            {
                foreach(SystemSummaryItem system in summaryVM.Systems)
                {
                    Assert.AreEqual(item.Item1.TotalPrice, system.Estimate.TotalPrice);
                    if (system.Typical == item.Item2)
                    {
                        Assert.AreEqual(system.Estimate.TotalPrice, item.Item1.TotalPrice);
                        y++;
                        break;
                    }
                }
            }
        }

        private TECTypical createTypical(TECBid bid)
        {
            TECTypical typical = new TECTypical();
            typical.Name = "test";
            TECEquipment equipment = new TECEquipment();
            equipment.Name = "test equipment";
            TECSubScope ss = new TECSubScope();
            ss.Name = "Test Subscope";
            ss.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point = new TECPoint();
            point.Type = IOType.AI;
            point.Quantity = 1;
            ss.Points.Add(point);
            equipment.SubScope.Add(ss);
            typical.Equipment.Add(equipment);

            TECSubScope connected = new TECSubScope();
            connected.Name = "Connected";
            connected.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point2 = new TECPoint();
            point2.Type = IOType.AI;
            point2.Quantity = 1;
            connected.Points.Add(point2);
            equipment.SubScope.Add(connected);

            TECSubScope toConnect = new TECSubScope();
            toConnect.Name = "To Connect";
            toConnect.Devices.Add(bid.Catalogs.Devices[0]);
            TECPoint point3 = new TECPoint();
            point3.Type = IOType.AI;
            point3.Quantity = 1;
            toConnect.Points.Add(point3);
            equipment.SubScope.Add(toConnect);

            TECControllerType controllerType = new TECControllerType(new TECManufacturer());
            controllerType.IOModules.Add(bid.Catalogs.IOModules[0]);
            TECIO io = new TECIO(IOType.AI);
            io.Quantity = 10;
            controllerType.IO.Add(io);
            bid.Catalogs.IOModules[0].IO.Add(io);
            controllerType.Name = "Test Type";

            TECProvidedController controller = new TECProvidedController(controllerType);
            controller.IOModules.Add(bid.Catalogs.IOModules[0]);
            controller.Name = "Test Controller";
            typical.AddController(controller);
            TECController otherController = new TECProvidedController(controllerType);
            otherController.Name = "Other Controller";
            typical.AddController(otherController);
            IControllerConnection connection = controller.Connect(connected, (connected as IConnectable).AvailableProtocols.First(y => y is TECHardwiredProtocol));
            connection.Length = 10;
            connection.ConduitLength = 20;
            connection.ConduitType = bid.Catalogs.ConduitTypes[1];

            TECPanelType panelType = new TECPanelType(new TECManufacturer());
            panelType.Name = "test type";

            TECPanel panel = new TECPanel(panelType);
            panel.Name = "Test Panel";
            typical.Panels.Add(panel);

            TECMisc misc = new TECMisc(CostType.TEC);
            misc.Name = "test Misc";
            typical.MiscCosts.Add(misc);

            bid.Systems.Add(typical);
            typical.AddInstance();
            return typical;
        }
    }
}
