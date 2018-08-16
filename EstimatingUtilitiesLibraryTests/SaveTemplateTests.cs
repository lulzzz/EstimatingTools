﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using TestLibrary.ModelTestingUtilities;

namespace EstimatingUtilitiesLibraryTests
{
    [TestClass]
    public class SaveTemplateTests
    {
        static double DELTA = 0.0001;
        Random rand;

        TECTemplates templates;
        DeltaStacker testStack;
        string path;

        private TestContext testContextInstance;
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
        public void TestInitialize()
        {
            rand = new Random(0);
            path = Path.GetTempFileName();
            templates = ModelCreation.TestTemplates(rand);
            ChangeWatcher watcher = new ChangeWatcher(templates);
            testStack = new DeltaStacker(watcher, templates);
            DatabaseManager<TECTemplates> manager = new DatabaseManager<TECTemplates>(path);
            manager.New(templates);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete(path);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #region Save Labor

        [TestMethod]
        public void Save_Templates_Labor_PMCoef()
        {
            //Act
            double expectedPM = 0.123;
            templates.Templates.Parameters[0].PMCoef = expectedPM;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualPM = actualTemplates.Templates.Parameters[0].PMCoef;

            //Assert
            Assert.AreEqual(expectedPM, actualPM);
        }

        [TestMethod]
        public void Save_Templates_Labor_PMRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Templates.Parameters[0].PMRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].PMRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ENGCoef()
        {
            //Act
            double expectedENG = 0.123;
            templates.Templates.Parameters[0].ENGCoef = expectedENG;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualENG = actualTemplates.Templates.Parameters[0].ENGCoef;

            //Assert
            Assert.AreEqual(expectedENG, actualENG);
        }

        [TestMethod]
        public void Save_Templates_Labor_ENGRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Templates.Parameters[0].ENGRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].ENGRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_CommCoef()
        {
            //Act
            double expectedComm = 0.123;
            templates.Templates.Parameters[0].CommCoef = expectedComm;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualComm = actualTemplates.Templates.Parameters[0].CommCoef;

            //Assert
            Assert.AreEqual(expectedComm, actualComm);
        }

        [TestMethod]
        public void Save_Templates_Labor_CommRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Templates.Parameters[0].CommRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].CommRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_SoftCoef()
        {
            //Act
            double expectedSoft = 0.123;
            templates.Templates.Parameters[0].SoftCoef = expectedSoft;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualSoft = actualTemplates.Templates.Parameters[0].SoftCoef;

            //Assert
            Assert.AreEqual(expectedSoft, actualSoft);
        }

        [TestMethod]
        public void Save_Templates_Labor_SoftRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Templates.Parameters[0].SoftRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].SoftRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_GraphCoef()
        {
            //Act
            double expectedGraph = 0.123;
            templates.Templates.Parameters[0].GraphCoef = expectedGraph;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualGraph = actualTemplates.Templates.Parameters[0].GraphCoef;

            //Assert
            Assert.AreEqual(expectedGraph, actualGraph);
        }

        [TestMethod]
        public void Save_Templates_Labor_GraphRate()
        {
            //Act
            double expectedRate = 564.05;
            templates.Templates.Parameters[0].GraphRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].GraphRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecRate()
        {
            ////Act
            double expectedRate = 0.123;
            templates.Templates.Parameters[0].ElectricalRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].ElectricalRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecNonUnionRate()
        {
            //Act
            double expectedRate = 0.456;
            templates.Templates.Parameters[0].ElectricalNonUnionRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].ElectricalNonUnionRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecSuperRate()
        {
            //Act
            double expectedRate = 0.123;
            templates.Templates.Parameters[0].ElectricalSuperRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].ElectricalSuperRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }

        [TestMethod]
        public void Save_Templates_Labor_ElecSuperNonUnionRate()
        {
            //Act
            double expectedRate = 23.94;
            templates.Templates.Parameters[0].ElectricalSuperNonUnionRate = expectedRate;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
            double actualRate = actualTemplates.Templates.Parameters[0].ElectricalSuperNonUnionRate;

            //Assert
            Assert.AreEqual(expectedRate, actualRate);
        }


        #endregion Save Labor

        #region Save System

        [TestMethod]
        public void Save_Templates_Add_System()
        {
            //Act
            TECSystem expectedSystem = new TECSystem();
            expectedSystem.Name = "New system";
            expectedSystem.Description = "New system desc";

            templates.Templates.SystemTemplates.Add(expectedSystem);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                if (expectedSystem.Guid == system.Guid)
                {
                    actualSystem = system;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
        }

        [TestMethod]
        public void Save_Templates_Remove_System()
        {
            //Act
            int oldNumSystems = templates.Templates.SystemTemplates.Count;
            TECSystem systemToRemove = templates.Templates.SystemTemplates[0];

            templates.Templates.SystemTemplates.Remove(systemToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsupdate) = DatabaseLoader.Load(path);
            TECTemplates expectedTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECSystem system in templates.Templates.SystemTemplates)
            {
                if (system.Guid == systemToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumSystems - 1), templates.Templates.SystemTemplates.Count);
        }

        #region Edit System
        [TestMethod]
        public void Save_Templates_System_Name()
        {
            //Act
            TECSystem expectedSystem = templates.Templates.SystemTemplates[0];
            expectedSystem.Name = "Save System Name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Name, actualSystem.Name);
        }

        [TestMethod]
        public void Save_Templates_System_Description()
        {
            //Act
            TECSystem expectedSystem = templates.Templates.SystemTemplates[0];
            expectedSystem.Description = "Save System Description";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                if (system.Guid == expectedSystem.Guid)
                {
                    actualSystem = system;
                }
            }

            //Assert
            Assert.AreEqual(expectedSystem.Description, actualSystem.Description);
        }
        
        #endregion Edit System
        #endregion Save System

        #region Save Equipment
        [TestMethod]
        public void Save_Templates_Add_Equipment()
        {
            //Act
            TECEquipment expectedEquipment = new TECEquipment();
            expectedEquipment.Name = "New Equipment";
            expectedEquipment.Description = "New Equipment desc";

            templates.Templates.EquipmentTemplates.Add(expectedEquipment);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.Templates.EquipmentTemplates)
            {
                if (expectedEquipment.Guid == Equipment.Guid)
                {
                    actualEquipment = Equipment;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
        }

        [TestMethod]
        public void Save_Templates_Remove_Equipment()
        {
            //Act
            int oldNumEquipments = templates.Templates.EquipmentTemplates.Count;
            TECEquipment EquipmentToRemove = templates.Templates.EquipmentTemplates[0];

            templates.Templates.EquipmentTemplates.Remove(EquipmentToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECEquipment Equipment in actualTemplates.Templates.EquipmentTemplates)
            {
                if (Equipment.Guid == EquipmentToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumEquipments - 1), actualTemplates.Templates.EquipmentTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_Add_Equipment_InSystem()
        {
            //Act
            TECEquipment expectedEquipment = new TECEquipment();
            expectedEquipment.Name = "New System Equipment";
            expectedEquipment.Description = "System equipment description";

            TECSystem sysToModify = templates.Templates.SystemTemplates[0];

            sysToModify.Equipment.Add(expectedEquipment);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSystem actualSystem = null;
            foreach (TECSystem sys in actualTemplates.Templates.SystemTemplates)
            {
                if (sys.Guid == sysToModify.Guid)
                {
                    actualSystem = sys;
                    break;
                }
            }

            TECEquipment actualEquipment = null;
            foreach (TECEquipment equip in actualSystem.Equipment)
            {
                if (equip.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            //Assert
            Assert.IsNotNull(actualEquipment);
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
            foreach (TECEquipment equip in actualTemplates.Templates.EquipmentTemplates)
            {
                if (equip.Guid == actualEquipment.Guid) Assert.Fail();
            }
        }

        [TestMethod]
        public void Save_Templates_Remove_Equipment_FromSystem()
        {
            //Act
            TECSystem sysToModify = null;
            TECEquipment equipToRemove = null;
            int oldNumEquip = 0;
            foreach (TECSystem sys in templates.Templates.SystemTemplates)
            {
                if (sys.Equipment.Count > 0)
                {
                    sysToModify = sys;
                    equipToRemove = sysToModify.Equipment[0];
                    oldNumEquip = sysToModify.Equipment.Count;
                    break;
                }
            }

            sysToModify.Equipment.Remove(equipToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            TECSystem actualSystem = null;
            foreach (TECSystem sys in actualTemplates.Templates.SystemTemplates)
            {
                if (sys.Guid == sysToModify.Guid)
                {
                    actualSystem = sys;
                    foreach (TECEquipment equip in actualSystem.Equipment)
                    {
                        if (equip.Guid == equipToRemove.Guid)
                        {
                            Assert.Fail();
                        }
                    }
                    break;
                }
            }

            Assert.AreEqual((oldNumEquip - 1), actualSystem.Equipment.Count);
        }

        [TestMethod]
        public void Save_Templates_Equipment_Name()
        {
            //Act
            TECEquipment expectedEquipment = templates.Templates.EquipmentTemplates[0];
            expectedEquipment.Name = "Save Equipment Name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.Templates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Name, actualEquipment.Name);
        }

        [TestMethod]
        public void Save_Templates_Equipment_Description()
        {
            //Act
            TECEquipment expectedEquipment = templates.Templates.EquipmentTemplates[0];
            expectedEquipment.Description = "Save Equipment Description";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECEquipment actualEquipment = null;
            foreach (TECEquipment Equipment in actualTemplates.Templates.EquipmentTemplates)
            {
                if (Equipment.Guid == expectedEquipment.Guid)
                {
                    actualEquipment = Equipment;
                }
            }

            //Assert
            Assert.AreEqual(expectedEquipment.Description, actualEquipment.Description);
        }
        
        #endregion Save Equipment

        #region Save SubScope
        [TestMethod]
        public void Save_Templates_Add_SubScope()
        {
            //Act
            TECSubScope expectedSubScope = new TECSubScope();
            expectedSubScope.Name = "New SubScope";
            expectedSubScope.Description = "New SubScope desc";

            templates.Templates.SubScopeTemplates.Add(expectedSubScope);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope subScope in actualTemplates.Templates.SubScopeTemplates)
            {
                if (expectedSubScope.Guid == subScope.Guid)
                {
                    actualSubScope = subScope;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
        }

        [TestMethod]
        public void Save_Templates_Remove_SubScope()
        {
            //Act
            int oldNumSubScopes = templates.Templates.SubScopeTemplates.Count;
            TECSubScope SubScopeToRemove = templates.Templates.SubScopeTemplates[0];

            templates.Templates.SubScopeTemplates.Remove(SubScopeToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECSubScope SubScope in actualTemplates.Templates.SubScopeTemplates)
            {
                if (SubScope.Guid == SubScopeToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumSubScopes - 1), actualTemplates.Templates.SubScopeTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_SubScope_Name()
        {
            //Act
            TECSubScope expectedSubScope = templates.Templates.SubScopeTemplates[0];
            expectedSubScope.Name = "Save SubScope Name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope SubScope in actualTemplates.Templates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Name, actualSubScope.Name);
        }

        [TestMethod]
        public void Save_Templates_SubScope_Description()
        {
            //Act
            TECSubScope expectedSubScope = templates.Templates.SubScopeTemplates[0];
            expectedSubScope.Description = "Save SubScope Description";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSubScope actualSubScope = null;
            foreach (TECSubScope SubScope in actualTemplates.Templates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                }
            }

            //Assert
            Assert.AreEqual(expectedSubScope.Description, actualSubScope.Description);
        }
        
        [TestMethod]
        public void Save_Templates_SubScope_AssociatedCosts()
        {
            //Act
            TECSubScope expectedSubScope = templates.Templates.SubScopeTemplates[0];

            TECAssociatedCost expectedCost = templates.Catalogs.AssociatedCosts[1];
            expectedSubScope.AssociatedCosts.Add(templates.Catalogs.AssociatedCosts[1]);
            int expectedNumCosts = expectedSubScope.AssociatedCosts.Count;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSubScope actualSubScope = null;
            TECAssociatedCost actualCost = null;
            foreach (TECSubScope SubScope in actualTemplates.Templates.SubScopeTemplates)
            {
                if (SubScope.Guid == expectedSubScope.Guid)
                {
                    actualSubScope = SubScope;
                    foreach (TECAssociatedCost cost in actualSubScope.AssociatedCosts)
                    {
                        if (cost.Guid == expectedCost.Guid)
                        {
                            actualCost = cost;
                            break;
                        }
                    }
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedNumCosts, actualSubScope.AssociatedCosts.Count);
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost, DELTA);

        }
        #endregion Save SubScope

        #region Save Device
        [TestMethod]
        public void Save_Templates_Add_Device()
        {
            //Act
            ObservableCollection<TECConnectionType> types = new ObservableCollection<TECConnectionType>();
            types.Add(templates.Catalogs.ConnectionTypes[0]);
            TECDevice expectedDevice = new TECDevice(Guid.NewGuid(), 
                types,
                new List<TECProtocol>(),
                templates.Catalogs.Manufacturers[0]);
            expectedDevice.Name = "New Device";
            expectedDevice.Description = "New Device desc";
            expectedDevice.Price = 11.54;

            templates.Catalogs.Add(expectedDevice);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice device in actualTemplates.Catalogs.Devices)
            {
                if (device.Guid == expectedDevice.Guid)
                {
                    actualDevice = device;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
            Assert.AreEqual(expectedDevice.Cost, actualDevice.Cost, DELTA);
            Assert.AreEqual(expectedDevice.HardwiredConnectionTypes[0].Name, actualDevice.HardwiredConnectionTypes[0].Name);
        }

        [TestMethod]
        public void Save_Templates_Replace_Device()
        {
            //Arrange
            TECDevice deviceToRemove = templates.Catalogs.Devices.First();
            TECDevice newDevice = new TECDevice(deviceToRemove.HardwiredConnectionTypes, deviceToRemove.PossibleProtocols, deviceToRemove.Manufacturer);

            //Act
            templates.RemoveCatalogItem(deviceToRemove, newDevice);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            Assert.IsTrue(actualTemplates.Catalogs.Devices.Any(dev => (dev.Guid == newDevice.Guid)));
            Assert.IsFalse(actualTemplates.Catalogs.Devices.Any(dev => (dev.Guid == deviceToRemove.Guid)));

            Assert.AreEqual(templates.Catalogs.Devices.Count, actualTemplates.Catalogs.Devices.Count);
        }

        [TestMethod]
        public void Save_Templates_Device_Name()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            expectedDevice.Name = "Save Device Name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Name, actualDevice.Name);
        }

        [TestMethod]
        public void Save_Templates_Device_Description()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            expectedDevice.Description = "Save Device Description";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Description, actualDevice.Description);
        }

        [TestMethod]
        public void Save_Templates_Device_Cost()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            expectedDevice.Price = 46.89;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Price, actualDevice.Price);
        }

        [TestMethod]
        public void Save_Templates_Device_ConnectionType()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            var testConnectionType = new TECConnectionType();
            testConnectionType.Name = "Test Add Connection Type Device";
            templates.Catalogs.Add(testConnectionType);
            expectedDevice.HardwiredConnectionTypes.Add(testConnectionType);
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            foreach(TECElectricalMaterial expectedConnectionType in expectedDevice.HardwiredConnectionTypes)
            {
                bool found = false;
                foreach(TECElectricalMaterial actualConnectionType in actualDevice.HardwiredConnectionTypes)
                {
                    if (expectedConnectionType.Guid == actualConnectionType.Guid)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Assert.Fail("Connectiontype not found in device.");
                }
            }
        }

        [TestMethod]
        public void Save_Templates_Device_Manufacturer()
        {
            //Act
            TECDevice expectedDevice = templates.Catalogs.Devices[0];
            TECManufacturer manToAdd = new TECManufacturer();
            manToAdd.Label = "Test";
            manToAdd.Multiplier = 1;
            templates.Catalogs.Add(manToAdd);
            expectedDevice.Manufacturer = manToAdd;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECDevice actualDevice = null;
            foreach (TECDevice Device in actualTemplates.Catalogs.Devices)
            {
                if (Device.Guid == expectedDevice.Guid)
                {
                    actualDevice = Device;
                }
            }

            //Assert
            Assert.AreEqual(expectedDevice.Manufacturer.Guid, actualDevice.Manufacturer.Guid);
        }
        #endregion Save Device

        #region Save Controller
        [TestMethod]
        public void Save_Templates_Add_Controller()
        {
            //Act
            TECController expectedController = new TECProvidedController(Guid.NewGuid(), templates.Catalogs.ControllerTypes[0]);
            expectedController.Name = "Test Controller";
            expectedController.Description = "Test description";

            templates.Templates.ControllerTemplates.Add(expectedController);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
            Assert.AreEqual(expectedController.Description, actualController.Description);
        }

        [TestMethod]
        public void Save_Templates_Remove_Controller()
        {
            //Act
            int oldNumControllers = templates.Templates.ControllerTemplates.Count;
            TECController controllerToRemove = templates.Templates.ControllerTemplates[0];

            templates.Templates.ControllerTemplates.Remove(controllerToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
            {
                if (controller.Guid == controllerToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumControllers - 1), actualTemplates.Templates.ControllerTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_Controller_Name()
        {
            //Act
            TECController expectedController = templates.Templates.ControllerTemplates[0];
            expectedController.Name = "Test save controller name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Name, actualController.Name);
        }

        [TestMethod]
        public void Save_Templates_Controller_Description()
        {
            //Act
            TECController expectedController = templates.Templates.ControllerTemplates[0];
            expectedController.Description = "Save Device Description";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECController actualController = null;
            foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
            {
                if (controller.Guid == expectedController.Guid)
                {
                    actualController = controller;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedController.Description, actualController.Description);
        }

        //[TestMethod]
        //public void Save_Templates_Controller_Cost()
        //{
        //    //Act
        //    TECController expectedController = templates.Templates.ControllerTemplates[0];
        //    expectedController.Cost = 46.89;
        //    DatabaseUpdater.Update(path, testStack.CleansedStack());

        //    (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

        //    TECController actualController = null;
        //    foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
        //    {
        //        if (controller.Guid == expectedController.Guid)
        //        {
        //            actualController = controller;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(expectedController.Cost, actualController.Cost);
        //}

        //[TestMethod]
        //public void Save_Templates_Controller_Manufacturer()
        //{
        //    //Act
        //    TECController expectedController = templates.Templates.ControllerTemplates[0];
        //    var testManufacturer = new TECManufacturer();
        //    templates.Catalogs.Add(testManufacturer);
        //    expectedController.Manufacturer = testManufacturer;
        //    DatabaseUpdater.Update(path, testStack.CleansedStack());

        //    (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

        //    TECController actualController = null;
        //    foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
        //    {
        //        if (controller.Guid == expectedController.Guid)
        //        {
        //            actualController = controller;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(expectedController.Manufacturer.Guid, actualController.Manufacturer.Guid);
        //}

        #region Controller IO
        //[TestMethod]
        //public void Save_Templates_Controller_Add_IO()
        //{
        //    //Act
        //    TECController expectedController = templates.Templates.ControllerTemplates[0];
        //    var testio = new TECIO();
        //    testio.Type = IOType.BACnetIP;
        //    expectedController.IO.Add(testio);
        //    bool hasBACnetIP = false;
        //    DatabaseUpdater.Update(path, testStack.CleansedStack());

        //    (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;
        //    TECController actualController = null;
        //    foreach (TECController controller in actualTemplates.Templates.ControllerTemplates)
        //    {
        //        if (controller.Guid == expectedController.Guid)
        //        {
        //            actualController = controller;
        //            break;
        //        }
        //    }

        //    //Assert
        //    foreach (TECIO io in actualController.IO)
        //    {
        //        if (io.Type == IOType.BACnetIP)
        //        {
        //            hasBACnetIP = true;
        //        }
        //    }

        //    Assert.IsTrue(hasBACnetIP);

        //}

        //[TestMethod]
        //public void Save_Templates_Controller_Remove_IO()
        //{
        //    //Act
        //    TECController expectedController = templates.Templates.ControllerTemplates[0];
        //    int oldNumIO = expectedController.IO.Count;
        //    TECIO ioToRemove = expectedController.IO[0];

        //    expectedController.IO.Remove(ioToRemove);

        //    DatabaseUpdater.Update(path, testStack.CleansedStack());

        //    (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

        //    TECController actualController = null;
        //    foreach (TECController con in actualTemplates.Templates.ControllerTemplates)
        //    {
        //        if (con.Guid == expectedController.Guid)
        //        {
        //            actualController = con;
        //            break;
        //        }
        //    }

        //    //Assert
        //    foreach (TECIO io in actualController.IO)
        //    {
        //        if (io.Type == ioToRemove.Type) { Assert.Fail(); }
        //    }

        //    Assert.AreEqual((oldNumIO - 1), actualController.IO.Count);
        //}

        //[TestMethod]
        //public void Save_Templates_Controller_IO_Quantity()
        //{
        //    //Act
        //    TECController expectedController = templates.Templates.ControllerTemplates[0];
        //    TECIO ioToChange = expectedController.IO[0];
        //    ioToChange.Quantity = 69;

        //    DatabaseUpdater.Update(path, testStack.CleansedStack());

        //    (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

        //    TECController actualController = null;
        //    foreach (TECController con in actualTemplates.Templates.ControllerTemplates)
        //    {
        //        if (con.Guid == expectedController.Guid)
        //        {
        //            actualController = con;
        //            break;
        //        }
        //    }

        //    //Assert
        //    foreach (TECIO io in actualController.IO)
        //    {
        //        if (io.Type == ioToChange.Type)
        //        {
        //            Assert.AreEqual(ioToChange.Quantity, io.Quantity);
        //            break;
        //        }
        //    }
        //}
        #endregion Controller IO

        #endregion

        #region Save Manufacturer
        [TestMethod]
        public void Save_Templates_Add_Manufacturer()
        {
            //Act
            int oldNumManufacturers = templates.Catalogs.Manufacturers.Count;
            TECManufacturer expectedManufacturer = new TECManufacturer();
            expectedManufacturer.Label = "Test Add Manufacturer";
            expectedManufacturer.Multiplier = 21.34;

            templates.Catalogs.Add(expectedManufacturer);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECManufacturer actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualManufacturer = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedManufacturer.Label, actualManufacturer.Label);
            Assert.AreEqual(expectedManufacturer.Multiplier, actualManufacturer.Multiplier);
            Assert.AreEqual((oldNumManufacturers + 1), actualTemplates.Catalogs.Manufacturers.Count);

        }

        [TestMethod]
        public void Save_Templates_Manufacturer_Name()
        {
            //Act
            TECManufacturer expectedManufacturer = templates.Catalogs.Manufacturers[0];
            expectedManufacturer.Label = "Test save manufacturer name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECManufacturer actualMan = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualMan = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedManufacturer.Label, actualMan.Label);
        }

        [TestMethod]
        public void Save_Templates_Manufacturer_Multiplier()
        {
            //Act
            TECManufacturer expectedManufacturer = templates.Catalogs.Manufacturers[0];
            expectedManufacturer.Multiplier = 987.41;
            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECManufacturer actualMan = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedManufacturer.Guid)
                {
                    actualMan = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedManufacturer.Multiplier, actualMan.Multiplier);


        }
        #endregion SaveManufacturer

        #region Save Tag
        [TestMethod]
        public void Save_Templates_Add_Tag()
        {
            //Act
            int oldNumTags = templates.Catalogs.Tags.Count;
            TECTag expectedTag = new TECTag();
            expectedTag.Label = "Test add tag";

            templates.Catalogs.Add(expectedTag);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECLabeled actualTag = null;
            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == expectedTag.Guid)
                {
                    actualTag = tag;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedTag.Label, actualTag.Label);
            Assert.AreEqual((oldNumTags + 1), actualTemplates.Catalogs.Tags.Count);
        }

        [TestMethod]
        public void Save_Templates_Remove_Tag()
        {
            //Act
            int oldNumTags = templates.Catalogs.Tags.Count;
            TECTag tagToRemove = templates.Catalogs.Tags[0];

            templates.RemoveCatalogItem(tagToRemove, null);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == tagToRemove.Guid) { Assert.Fail(); }
            }

            Assert.AreEqual((oldNumTags - 1), actualTemplates.Catalogs.Tags.Count);
        }
        #endregion Save Tag

        #region Save Connection Type
        [TestMethod]
        public void Save_Templates_Add_ConnectionType()
        {
            //Act
            int oldNumConnectionTypes = templates.Catalogs.ConnectionTypes.Count;
            TECConnectionType expectedConnectionType = new TECConnectionType();
            expectedConnectionType.Name = "Test Add Connection Type";
            expectedConnectionType.Cost = 21.34;

            templates.Catalogs.Add(expectedConnectionType);

            TECAssociatedCost expectedCost = templates.Catalogs.AssociatedCosts[0];
            expectedConnectionType.AssociatedCosts.Add(expectedCost);
            int expectedCostCount = expectedConnectionType.AssociatedCosts.Count;

            TECAssociatedCost expectedRated = null;
            foreach(TECAssociatedCost cost in templates.Catalogs.AssociatedCosts)
            {
                if(cost.Type == CostType.Electrical)
                {
                    expectedRated = cost;
                    break;
                }
            }
            expectedConnectionType.RatedCosts.Add(expectedRated);
            int expectedRatedCount = expectedConnectionType.RatedCosts.Count;

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECElectricalMaterial actualConnectionType = null;
            TECAssociatedCost actualCost = null;
            foreach (TECElectricalMaterial connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Guid == expectedConnectionType.Guid)
                {
                    actualConnectionType = connectionType;
                    actualCost = actualConnectionType.AssociatedCosts[0];
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedConnectionType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConnectionType.Cost, actualConnectionType.Cost, DELTA);
            Assert.AreEqual((oldNumConnectionTypes + 1), actualTemplates.Catalogs.ConnectionTypes.Count);
            Assert.AreEqual(expectedCostCount, actualConnectionType.AssociatedCosts.Count);
            Assert.AreEqual(expectedRatedCount, actualConnectionType.RatedCosts.Count);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost, DELTA);
            Assert.AreEqual(expectedCost.Name, actualCost.Name);

        }
        [TestMethod]
        public void Save_Templates_Add_ConnectionTypeRatedCost()
        {
            //Act
            int oldNumConduitTypes = templates.Catalogs.ConduitTypes.Count;
            TECElectricalMaterial expectedConnectionType = templates.Catalogs.ConnectionTypes[0];

            TECAssociatedCost expectedRated = null;
            foreach (TECAssociatedCost cost in templates.Catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    expectedRated = cost;
                    break;
                }
            }
            expectedConnectionType.RatedCosts.Add(expectedRated);
            int expectedRatedCount = expectedConnectionType.RatedCosts.Count;

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECElectricalMaterial actualConnectionType = null;
            foreach (TECElectricalMaterial connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Guid == expectedConnectionType.Guid)
                {
                    actualConnectionType = connectionType;
                    break;
                }
            }

            bool hasRated = false;
            foreach (TECAssociatedCost cost in actualConnectionType.RatedCosts)
            {
                if (cost.Guid == expectedRated.Guid)
                {
                    hasRated = true;
                    break;
                }
            }

            //Assert
            Assert.IsTrue(hasRated);

        }
        #endregion

        #region Save Conduit Type
        [TestMethod]
        public void Save_Templates_Add_ConduitType()
        {
            //Act
            int oldNumConduitTypes = templates.Catalogs.ConduitTypes.Count;
            TECElectricalMaterial expectedConduitType = new TECElectricalMaterial();
            expectedConduitType.Name = "Test Add Conduit Type";
            expectedConduitType.Cost = 21.34;

            templates.Catalogs.Add(expectedConduitType);

            TECAssociatedCost expectedRated = null;
            foreach (TECAssociatedCost cost in templates.Catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    expectedRated = cost;
                    break;
                }
            }
            expectedConduitType.RatedCosts.Add(expectedRated);
            int expectedRatedCount = expectedConduitType.RatedCosts.Count;

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECElectricalMaterial actualConnectionType = null;
            foreach (TECElectricalMaterial conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == expectedConduitType.Guid)
                {
                    actualConnectionType = conduitType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedConduitType.Name, actualConnectionType.Name);
            Assert.AreEqual(expectedConduitType.Cost, actualConnectionType.Cost);
            Assert.AreEqual(expectedRatedCount, actualConnectionType.RatedCosts.Count);
            Assert.AreEqual((oldNumConduitTypes + 1), actualTemplates.Catalogs.ConduitTypes.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_ConduitType()
        {
            //Act
            int oldNumConduitTypes = templates.Catalogs.ConduitTypes.Count;
            TECElectricalMaterial conduitTypeToRemove = templates.Catalogs.ConduitTypes[0];

            templates.RemoveCatalogItem(conduitTypeToRemove, null);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECElectricalMaterial conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == conduitTypeToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumConduitTypes - 1), actualTemplates.Catalogs.ConduitTypes.Count);
        }

        [TestMethod]
        public void Save_Templates_Add_ConduitTypeRatedCost()
        {
            //Act
            int oldNumConduitTypes = templates.Catalogs.ConduitTypes.Count;
            TECElectricalMaterial expectedConduitType = templates.Catalogs.ConduitTypes[0];
            
            TECAssociatedCost expectedRated = null;
            foreach (TECAssociatedCost cost in templates.Catalogs.AssociatedCosts)
            {
                if (cost.Type == CostType.Electrical)
                {
                    expectedRated = cost;
                    break;
                }
            }
            expectedConduitType.RatedCosts.Add(expectedRated);
            int expectedRatedCount = expectedConduitType.RatedCosts.Count;

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECElectricalMaterial actualConduitType = null;
            foreach (TECElectricalMaterial conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == expectedConduitType.Guid)
                {
                    actualConduitType = conduitType;
                    break;
                }
            }

            bool hasRated = false;
            foreach(TECAssociatedCost cost in actualConduitType.RatedCosts)
            {
                if(cost.Guid == expectedRated.Guid)
                {
                    hasRated = true;
                    break;
                }
            }

            //Assert
            Assert.IsTrue(hasRated);

        }
        #endregion

        #region Save Associated Costs
        [TestMethod]
        public void Save_Templates_Add_AssociatedCost()
        {
            //Act
            int oldNumAssociatedCosts = templates.Catalogs.AssociatedCosts.Count;
            TECAssociatedCost expectedAssociatedCost = new TECAssociatedCost(CostType.TEC);
            expectedAssociatedCost.Name = "Test Associated Cost";
            expectedAssociatedCost.Cost = 21.34;

            templates.Catalogs.Add(expectedAssociatedCost);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECAssociatedCost actualCost = null;
            foreach (TECAssociatedCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == expectedAssociatedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedAssociatedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedAssociatedCost.Cost, actualCost.Cost);
            Assert.AreEqual((oldNumAssociatedCosts + 1), actualTemplates.Catalogs.AssociatedCosts.Count);

        }

        [TestMethod]
        public void Save_Templates_Remove_AssociatedCost()
        {
            //Act
            int oldNumAssociatedCosts = templates.Catalogs.AssociatedCosts.Count;
            TECAssociatedCost costToRemove = templates.Catalogs.AssociatedCosts[0];

            templates.RemoveCatalogItem(costToRemove, null);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECAssociatedCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumAssociatedCosts - 1), actualTemplates.Catalogs.AssociatedCosts.Count);
        }
        #endregion

        #region Save Misc Cost
        [TestMethod]
        public void Save_Templates_Add_MiscCost()
        {
            //Act
            TECMisc expectedCost = new TECMisc(CostType.TEC);
            expectedCost.Name = "Add cost addition";
            expectedCost.Cost = 978.3;
            expectedCost.Quantity = 21;

            templates.Templates.MiscCostTemplates.Add(expectedCost);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.Templates.MiscCostTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
            Assert.AreEqual(expectedCost.Quantity, actualCost.Quantity);
            Assert.AreEqual(expectedCost.Type, actualCost.Type);
        }

        [TestMethod]
        public void Save_Templates_Remove_MiscCost()
        {
            //Act
            TECMisc costToRemove = templates.Templates.MiscCostTemplates[0];
            templates.Templates.MiscCostTemplates.Remove(costToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECMisc cost in actualTemplates.Templates.MiscCostTemplates)
            {
                if (cost.Guid == costToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.Templates.MiscCostTemplates.Count, actualTemplates.Templates.MiscCostTemplates.Count);
        }

        [TestMethod]
        public void Save_Templates_MiscCost_Name()
        {
            //Act
            TECMisc expectedCost = templates.Templates.MiscCostTemplates[0];
            expectedCost.Name = "Test Save Cost Name";

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.Templates.MiscCostTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_MiscCost_Cost()
        {
            //Act
            TECMisc expectedCost = templates.Templates.MiscCostTemplates[0];
            expectedCost.Cost = 489.1238;

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECMisc actualCost = null;
            foreach (TECMisc cost in actualTemplates.Templates.MiscCostTemplates)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost);
        }

        #endregion

        #region Save Panel Type
        [TestMethod]
        public void Save_Templates_Add_PanelType()
        {
            //Act
            TECPanelType expectedCost = new TECPanelType(templates.Catalogs.Manufacturers[0]);
            expectedCost.Name = "Add cost addition";
            expectedCost.Price = 978.3;

            templates.Catalogs.Add(expectedCost);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost, DELTA);
        }

        [TestMethod]
        public void Save_Templates_Replace_PanelType()
        {
            //Act
            TECPanelType typeToRemove = templates.Catalogs.PanelTypes.First();
            TECPanelType newType = new TECPanelType(templates.Catalogs.Manufacturers.First());
            templates.RemoveCatalogItem(typeToRemove, newType);

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == typeToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.Catalogs.PanelTypes.Count, actualTemplates.Catalogs.PanelTypes.Count);
        }

        [TestMethod]
        public void Save_Templates_PanelType_Name()
        {
            //Act
            TECPanelType expectedCost = templates.Catalogs.PanelTypes[0];
            expectedCost.Name = "Test Save Cost Name";

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_PanelType_Cost()
        {
            //Act
            TECPanelType expectedCost = templates.Catalogs.PanelTypes[0];
            expectedCost.Price = 489.1238;

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECPanelType actualCost = null;
            foreach (TECPanelType cost in actualTemplates.Catalogs.PanelTypes)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Price, actualCost.Price);
        }

        #endregion

        #region Save IOModule
        [TestMethod]
        public void Save_Templates_Add_IOModule()
        {
            //Act
            TECIOModule expectedModule = new TECIOModule(templates.Catalogs.Manufacturers[0]);
            expectedModule.Name = "Add IO Module";
            expectedModule.Price = 978.3;
            expectedModule.Manufacturer = templates.Catalogs.Manufacturers[0];

            templates.Catalogs.Add(expectedModule);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECIOModule actualCost = null;
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == expectedModule.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedModule.Name, actualCost.Name);
            Assert.AreEqual(expectedModule.Price, actualCost.Price);
        }

        [TestMethod]
        public void Save_Templates_Remove_IOModule()
        {
            //Act
            TECIOModule modToRemove = templates.Catalogs.IOModules[0];
            templates.RemoveCatalogItem(modToRemove, null);

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECIOModule mod in actualTemplates.Catalogs.IOModules)
            {
                if (mod.Guid == modToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual(templates.Catalogs.IOModules.Count, actualTemplates.Catalogs.IOModules.Count);
        }

        [TestMethod]
        public void Save_Templates_IOModule_Name()
        {
            //Act
            TECIOModule expectedCost = templates.Catalogs.IOModules[0];
            expectedCost.Name = "Test Save IOModule Name";

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECIOModule actualCost = null;
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Name, actualCost.Name);
        }

        [TestMethod]
        public void Save_Templates_IOModule_Cost()
        {
            //Act
            TECIOModule expectedCost = templates.Catalogs.IOModules[0];
            expectedCost.Price = 489.1238;

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECIOModule actualCost = null;
            foreach (TECIOModule cost in actualTemplates.Catalogs.IOModules)
            {
                if (cost.Guid == expectedCost.Guid)
                {
                    actualCost = cost;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedCost.Cost, actualCost.Cost, DELTA);
        }

        #endregion

        #region Save Panel
        [TestMethod]
        public void Save_Templates_Add_Panel()
        {
            //Act
            TECPanel expectedPanel = new TECPanel(templates.Catalogs.PanelTypes[0]);
            expectedPanel.Name = "Test Add Controller";
            expectedPanel.Description = "Test description";
            templates.Templates.PanelTemplates.Add(expectedPanel);

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path);
            TECTemplates actualTemplates = loaded as TECTemplates;

            TECPanel actualpanel = null;
            foreach (TECPanel panel in actualTemplates.Templates.PanelTemplates)
            {
                if (panel.Guid == expectedPanel.Guid)
                {
                    actualpanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPanel.Name, actualpanel.Name);
            Assert.AreEqual(expectedPanel.Description, actualpanel.Description);
        }

        [TestMethod]
        public void Save_Templates_Remove_Panel()
        {
            //Act
            int oldNumPanels = templates.Templates.PanelTemplates.Count;
            TECPanel panelToRemove = templates.Templates.PanelTemplates[0];

            templates.Templates.PanelTemplates.Remove(panelToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECPanel panel in actualTemplates.Templates.PanelTemplates)
            {
                if (panel.Guid == panelToRemove.Guid) Assert.Fail();
            }

            Assert.AreEqual((oldNumPanels - 1), actualTemplates.Templates.PanelTemplates.Count);

        }

        [TestMethod]
        public void Save_Templates_Panel_Name()
        {
            //Act
            TECPanel expectedPanel = templates.Templates.PanelTemplates[0];
            expectedPanel.Name = "Test save panel name";
            DatabaseUpdater.Update(path, testStack.CleansedStack());
            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path);
            TECTemplates actualTemplates = loaded as TECTemplates;
            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualTemplates.Templates.PanelTemplates)
            {
                if (panel.Guid == expectedPanel.Guid)
                {
                    actualPanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedPanel.Name, actualPanel.Name);
        }
        #endregion

        #region Save ConntrolledScope
        [TestMethod]
        public void Save_Templates_Add_ControlledScope()
        {
            //Act
            TECSystem expectedScope = new TECSystem();
            expectedScope.Name = "New controlled scope";
            expectedScope.Description = "New controlled scope desc";
            templates.Templates.SystemTemplates.Add(expectedScope);
            
            var subScope = new TECSubScope();
            subScope.Devices.Add(templates.Catalogs.Devices.First());

            var scopeEquipment = new TECEquipment();
            scopeEquipment.Name = "Test Scope System";
            scopeEquipment.Description = "Test scope system description";
            scopeEquipment.SubScope.Add(subScope);


            expectedScope.Equipment.Add(scopeEquipment);

            var scopeController = new TECProvidedController(templates.Catalogs.ControllerTypes[0]);
            scopeController.Name = "Test Scope Controller";
            expectedScope.AddController(scopeController);
            var connectedSubScope = scopeEquipment.SubScope.First(x => x.AvailableProtocols.Any(y => y is TECHardwiredProtocol));
            scopeController.Connect(connectedSubScope, connectedSubScope.AvailableProtocols.First(y => y is TECHardwiredProtocol));

            var scopePanel = new TECPanel(templates.Catalogs.PanelTypes[0]);
            scopePanel.Name = "Test Scope Name";
            expectedScope.Panels.Add(scopePanel);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path); TECTemplates actualTemplates = loaded as TECTemplates;

            TECSystem actualScope = null;
            foreach (TECSystem scope in actualTemplates.Templates.SystemTemplates)
            {
                if (expectedScope.Guid == scope.Guid)
                {
                    actualScope = scope;
                    break;
                }
            }

            TECHardwiredConnection actualSSConnection = null;
            foreach (TECHardwiredConnection ssConnect in actualScope.Controllers[0].ChildrenConnections)
            {
                if (ssConnect.Guid == scopeController.ChildrenConnections[0].Guid)
                {
                    actualSSConnection = ssConnect;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedScope.Name, actualScope.Name);
            Assert.AreEqual(expectedScope.Description, actualScope.Description);
            Assert.AreEqual(expectedScope.Equipment.Count, actualScope.Equipment.Count);
            Assert.AreEqual(expectedScope.Controllers.Count, actualScope.Controllers.Count);
            Assert.AreEqual(expectedScope.Controllers[0].ChildrenConnections.Count, actualScope.Controllers[0].ChildrenConnections.Count);
            Assert.AreEqual(expectedScope.Panels.Count, actualScope.Panels.Count);
            Assert.IsTrue(actualSSConnection.Child == actualScope.Equipment[0].SubScope.First(x=> x.Guid == connectedSubScope.Guid));
        }

        [TestMethod]
        public void Save_Templates_Remove_ControlledScope()
        {
            //Act
            int oldNumScope = templates.Templates.SystemTemplates.Count;
            TECSystem scopeToRemove = templates.Templates.SystemTemplates[0];

            templates.Templates.SystemTemplates.Remove(scopeToRemove);

            DatabaseUpdater.Update(path, testStack.CleansedStack());

            (TECScopeManager loaded, bool needsSave) = DatabaseLoader.Load(path);
            TECTemplates expectedTemplates = loaded as TECTemplates;

            //Assert
            foreach (TECSystem scope in templates.Templates.SystemTemplates)
            {
                if (scope.Guid == scopeToRemove.Guid)
                {
                    Assert.Fail();
                }
            }

            Assert.AreEqual((oldNumScope - 1), templates.Templates.SystemTemplates.Count);
        }

        #endregion
        
    }
}