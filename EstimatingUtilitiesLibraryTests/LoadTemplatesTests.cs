﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibraryTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace EstimatingUtilitiesLibraryTests
{
    [TestClass]
    public class LoadTemplatesTests
    {
        static TECTemplates actualTemplates;

        static Guid TEST_TAG_GUID = new Guid("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
        static Guid TEST_TEC_COST_GUID = new Guid("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
        static Guid TEST_ELECTRICAL_COST_GUID = new Guid("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
        static Guid TEST_LOCATION_GUID = new Guid("4175d04b-82b1-486b-b742-b2cc875405cb");
        static Guid TEST_RATED_COST_GUID = new Guid("b7c01526-c195-442f-a1f1-28d07db61144");

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

        [ClassInitialize]
        public static void ClassInitialize(TestContext TestContext)
        {
            string path = Path.GetTempFileName();
            TestDBHelper.CreateTestTemplates(path);
            actualTemplates = EULTestHelper.LoadTestTemplates(path);
        }

        [TestMethod]
        public void Load_Templates_LaborConsts()
        {
            //Assert
            double expectedPMCoef = 2;
            double expectedPMRate = 30;
            Assert.AreEqual(expectedPMCoef, actualTemplates.Templates.Parameters[0].PMCoef, "PM Coefficient didn't load properly.");
            Assert.AreEqual(expectedPMRate, actualTemplates.Templates.Parameters[0].PMRate, "PM Rate didn't load properly.");

            double expectedENGCoef = 2;
            double expectedENGRate = 40;
            Assert.AreEqual(expectedENGCoef, actualTemplates.Templates.Parameters[0].ENGCoef, "ENG Coefficient didn't load properly.");
            Assert.AreEqual(expectedENGRate, actualTemplates.Templates.Parameters[0].ENGRate, "ENG Rate didn't load properly.");

            double expectedCommCoef = 2;
            double expectedCommRate = 50;
            Assert.AreEqual(expectedCommCoef, actualTemplates.Templates.Parameters[0].CommCoef, "Comm Coefficient didn't load properly.");
            Assert.AreEqual(expectedCommRate, actualTemplates.Templates.Parameters[0].CommRate, "Comm Rate didn't load properly.");

            double expectedSoftCoef = 2;
            double expectedSoftRate = 60;
            Assert.AreEqual(expectedSoftCoef, actualTemplates.Templates.Parameters[0].SoftCoef, "Software Coefficient didn't load properly.");
            Assert.AreEqual(expectedSoftRate, actualTemplates.Templates.Parameters[0].SoftRate, "Software Rate didn't load properly.");

            double expectedGraphCoef = 2;
            double expectedGraphRate = 70;
            Assert.AreEqual(expectedGraphCoef, actualTemplates.Templates.Parameters[0].GraphCoef, "Graphics Coefficient didn't load properly.");
            Assert.AreEqual(expectedGraphRate, actualTemplates.Templates.Parameters[0].GraphRate, "Graphics Rate didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_SubcontractorConsts()
        {
            //Assert
            double expectedElectricalRate = 50;
            double expectedElectricalSuperRate = 60;
            double expectedElectricalNonUnionRate = 30;
            double expectedElectricalSuperNonUnionRate = 40;
            double expectedElectricalSuperRatio = 0.25;
            bool expectedOT = false;
            bool expectedUnion = true;
            Assert.AreEqual(expectedElectricalRate, actualTemplates.Templates.Parameters[0].ElectricalRate, "Electrical rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRate, actualTemplates.Templates.Parameters[0].ElectricalSuperRate, "Electrical Supervision rate didn't load properly.");
            Assert.AreEqual(expectedElectricalNonUnionRate, actualTemplates.Templates.Parameters[0].ElectricalNonUnionRate, "Electrical Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperNonUnionRate, actualTemplates.Templates.Parameters[0].ElectricalSuperNonUnionRate, "Electrical Supervision Non-Union rate didn't load properly.");
            Assert.AreEqual(expectedElectricalSuperRatio, actualTemplates.Templates.Parameters[0].ElectricalSuperRatio, "Electrical Supervision time ratio didn't load properly.");
            Assert.AreEqual(expectedOT, actualTemplates.Templates.Parameters[0].ElectricalIsOnOvertime, "Electrical overtime bool didn't load properly.");
            Assert.AreEqual(expectedUnion, actualTemplates.Templates.Parameters[0].ElectricalIsUnion, "Electrical union bool didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_System()
        {
            //Arrange
            Guid expectedGuid = new Guid("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            string expectedName = "Typical System";
            string expectedDescription = "Typical System Description";
            bool expectedProposeEquipment = true;

            Guid childEquipment = new Guid("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            Guid childController = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid childPanel = new Guid("e7695d68-d79f-44a2-92f5-b303436186af");
            Guid childScopeBranch = new Guid("814710f1-f2dd-4ae6-9bc4-9279288e4994");

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                if (system.Guid == expectedGuid)
                {
                    actualSystem = system;
                    break;
                }
            }

            bool foundEquip = false;
            foreach (TECEquipment equip in actualSystem.Equipment)
            {
                if (equip.Guid == childEquipment)
                {
                    foundEquip = true;
                    break;
                }
            }
            bool foundControl = false;
            foreach (TECController control in actualSystem.Controllers)
            {
                if (control.Guid == childController)
                {
                    foundControl = true;
                    break;
                }
            }
            bool foundPanel = false;
            foreach (TECPanel panel in actualSystem.Panels)
            {
                if (panel.Guid == childPanel)
                {
                    foundPanel = true;
                    break;
                }
            }
            bool foundScopeBranch = false;
            foreach(TECScopeBranch branch in actualSystem.ScopeBranches)
            {
                if (branch.Guid == childScopeBranch)
                {
                    foundScopeBranch = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSystem.Name);
            Assert.AreEqual(expectedDescription, actualSystem.Description);
            Assert.AreEqual(expectedProposeEquipment, actualSystem.IsSingleton);

            //foreach (TECSystem instance in actualSystem.Instances)
            //{
            //    Assert.AreEqual(actualSystem.Equipment.Count, instance.Equipment.Count);
            //    Assert.AreEqual(actualSystem.Panels.Count, instance.Panels.Count);
            //    Assert.AreEqual(actualSystem.Controllers.Count, instance.Controllers.Count);
            //}

            Assert.IsTrue(foundEquip, "Equipment not loaded properly into system.");
            Assert.IsTrue(foundControl, "Controller not loaded properly into system.");
            Assert.IsTrue(foundPanel, "Panel not loaded properly into system.");
            Assert.IsTrue(foundScopeBranch, "Scope branch not loaded properly into system.");

            testForTag(actualSystem);
            testForCosts(actualSystem);
        }

        [TestMethod]
        public void Load_Templates_Equipment()
        {
            Guid expectedGuid = new Guid("1645886c-fce7-4380-a5c3-295f91961d16");
            string expectedName = "Template Equip";
            string expectedDescription = "Template Equip Description";

            Guid childSubScope = new Guid("214dc8d1-22be-4fbf-8b6b-d66c21105f61");

            TECEquipment actualEquipment = null;
            foreach(TECEquipment equip in actualTemplates.Templates.EquipmentTemplates)
            {
                if (equip.Guid == expectedGuid)
                {
                    actualEquipment = equip;
                    break;
                }
            }

            bool foundSubScope = false;
            foreach (TECSubScope ss in actualEquipment.SubScope)
            {
                if (ss.Guid == childSubScope)
                {
                    foundSubScope = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualEquipment.Name);
            Assert.AreEqual(expectedDescription, actualEquipment.Description);

            Assert.IsTrue(foundSubScope, "Subscope not loaded properly into equipment.");

            testForTag(actualEquipment);
            testForCosts(actualEquipment);
        }

        [TestMethod]
        public void Load_Templates_SubScope()
        {
            //Arrange
            Guid expectedGuid = new Guid("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            string expectedName = "Template SS";
            string expectedDescription = "Template SS Description";

            Guid childPoint = new Guid("6776a30b-0325-42ad-8aa3-3c065b4bb908");
            Guid childDevice = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");

            TECSubScope actualSubScope = null;
            foreach(TECSubScope ss in actualTemplates.Templates.SubScopeTemplates)
            {
                if (ss.Guid == expectedGuid)
                {
                    actualSubScope = ss;
                    break;
                }
            }

            bool foundPoint = false;
            foreach (TECPoint point in actualSubScope.Points)
            {
                if (point.Guid == childPoint)
                {
                    foundPoint = true;
                    break;
                }
            }
            bool foundDevice = false;
            foreach (TECDevice device in actualSubScope.Devices)
            {
                if (device.Guid == childDevice)
                {
                    foundDevice = true;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualSubScope.Name, "Name not loaded");
            Assert.AreEqual(expectedDescription, actualSubScope.Description, "Description not loaded");

            Assert.IsTrue(foundPoint, "Point not loaded into subscope properly.");
            Assert.IsTrue(foundDevice, "Device not loaded into subscope properly.");

            testForTag(actualSubScope);
            testForCosts(actualSubScope);
        }

        [TestMethod]
        public void Load_Templates_Device()
        {
            Guid expectedGuid = new Guid("95135fdf-7565-4d22-b9e4-1f177febae15");
            string expectedName = "Test Device";
            string expectedDescription = "Test Device Description";
            double expectedCost = 123.45;

            Guid manufacturerGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            Guid connectionTypeGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");

            TECDevice actualDevice = null;
            foreach (TECDevice dev in actualTemplates.Catalogs.Devices)
            {
                if (dev.Guid == expectedGuid)
                {
                    actualDevice = dev;
                    break;
                }
            }

            bool foundConnectionType = false;
            foreach (TECElectricalMaterial connectType in actualDevice.HardwiredConnectionTypes)
            {
                if (connectType.Guid == connectionTypeGuid)
                {
                    foundConnectionType = true;
                    break;
                }
            }

            Assert.AreEqual(expectedName, actualDevice.Name, "Device name didn't load properly.");
            Assert.AreEqual(expectedDescription, actualDevice.Description, "Device description didn't load properly.");
            Assert.AreEqual(expectedCost, actualDevice.Price, "Device cost didn't load properly.");
            Assert.AreEqual(manufacturerGuid, actualDevice.Manufacturer.Guid, "Manufacturer didn't load properly into device.");

            Assert.IsTrue(foundConnectionType, "Connection type didn't load properly into device.");

            testForTag(actualDevice);
            testForCosts(actualDevice);
        }

        [TestMethod]
        public void Load_Templates_Manufacturer()
        {
            //Arrange
            Guid expectedGuid = new Guid("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            string expectedName = "Test Manufacturer";
            double expectedMultiplier = 0.5;


            TECManufacturer actualManufacturer = null;
            foreach (TECManufacturer man in actualTemplates.Catalogs.Manufacturers)
            {
                if (man.Guid == expectedGuid)
                {
                    actualManufacturer = man;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualManufacturer.Label);
            Assert.AreEqual(expectedMultiplier, actualManufacturer.Multiplier);
        }

        //[TestMethod]
        //public void Load_Templates_Controller()
        //{
        //    //Arrange
        //    Guid expectedGuid = new Guid("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
        //    string expectedName = "Bid Controller";
        //    string expectedDescription = "Bid Controller Description";
        //    double expectedCost = 1812;
        //    NetworkType expectedType = NetworkType.Server;
        //    bool expectedGlobalStatus = true;

        //    TECController actualController = null;
        //    foreach (TECController controller in actualTemplates.ControllerTemplates)
        //    {
        //        if (controller.Guid == expectedGuid)
        //        {
        //            actualController = controller;
        //            break;
        //        }
        //    }
            
        //    Guid expectedIOGuid = new Guid("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");

        //    bool hasIO = false;
        //    foreach (TECIO io in actualController.IO)
        //    {
        //        if (io.Guid == expectedIOGuid)
        //        {
        //            hasIO = true;
        //            break;
        //        }
        //    }

        //    //Assert
        //    Assert.AreEqual(expectedName, actualController.Name);
        //    Assert.AreEqual(expectedDescription, actualController.Description);
        //    Assert.AreEqual(expectedCost, actualController.Cost);
        //    Assert.AreEqual(expectedType, actualController.NetworkType);
        //    Assert.AreEqual(expectedGlobalStatus, actualController.IsGlobal);
        //    Assert.IsTrue(hasIO);
        //    testForTag(actualController);
        //    testForCosts(actualController);
        //}

        [TestMethod]
        public void Load_Templates_ConnectionType()
        {
            //Arrange
            Guid expectedGuid = new Guid("f38867c8-3846-461f-a6fa-c941aeb723c7");
            string expectedName = "Test Connection Type";
            double expectedCost = 12.48;
            double expectedLabor = 84.21;

            TECElectricalMaterial actualConnectionType = null;
            foreach (TECElectricalMaterial connectionType in actualTemplates.Catalogs.ConnectionTypes)
            {
                if (connectionType.Guid == expectedGuid)
                {
                    actualConnectionType = connectionType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualConnectionType.Name);
            Assert.AreEqual(expectedCost, actualConnectionType.Cost);
            Assert.AreEqual(expectedLabor, actualConnectionType.Labor);

            testForCosts(actualConnectionType);
            testForRatedCosts(actualConnectionType);
        }

        [TestMethod]
        public void Load_Templates_ConduitType()
        {
            //Arrange
            Guid expectedGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            string expectedName = "Test Conduit Type";
            double expectedCost = 45.67;
            double expectedLabor = 76.54;

            TECElectricalMaterial actualConduitType = null;
            foreach (TECElectricalMaterial conduitType in actualTemplates.Catalogs.ConduitTypes)
            {
                if (conduitType.Guid == expectedGuid)
                {
                    actualConduitType = conduitType;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualConduitType.Name);
            Assert.AreEqual(expectedCost, actualConduitType.Cost);
            Assert.AreEqual(expectedLabor, actualConduitType.Labor);

            testForCosts(actualConduitType);
            testForRatedCosts(actualConduitType);
        }

        [TestMethod]
        public void Load_Templates_AssociatedCosts()
        {
            Guid expectedTECGuid = new Guid("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
            string expectedTECName = "Test TEC Associated Cost";
            double expectedTECCost = 31;
            double expectedTECLabor = 13;
            CostType expectedTECType = CostType.TEC;

            Guid expectedElectricalGuid = new Guid("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
            string expectedElectricalName = "Test Electrical Associated Cost";
            double expectedElectricalCost = 42;
            double expectedElectricalLabor = 24;
            CostType expectedElectricalType = CostType.Electrical;

            TECAssociatedCost actualTECCost = null;
            TECAssociatedCost actualElectricalCost = null;
            foreach (TECAssociatedCost cost in actualTemplates.Catalogs.AssociatedCosts)
            {
                if (cost.Guid == expectedTECGuid)
                {
                    actualTECCost = cost;
                }
                else if (cost.Guid == expectedElectricalGuid)
                {
                    actualElectricalCost = cost;
                }
                if (actualTECCost != null && actualElectricalCost != null)
                {
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedTECName, actualTECCost.Name, "TEC cost name didn't load properly.");
            Assert.AreEqual(expectedTECCost, actualTECCost.Cost, "TEC cost cost didn't load properly.");
            Assert.AreEqual(expectedTECLabor, actualTECCost.Labor, "TEC cost labor didn't load properly.");
            Assert.AreEqual(expectedTECType, actualTECCost.Type, "TEC cost type didn't load properly.");

            Assert.AreEqual(expectedElectricalName, actualElectricalCost.Name, "Electrical cost name didn't load properly.");
            Assert.AreEqual(expectedElectricalCost, actualElectricalCost.Cost, "Electrical cost cost didn't load properly.");
            Assert.AreEqual(expectedElectricalLabor, actualElectricalCost.Labor, "Electrical cost labor didn't load properly.");
            Assert.AreEqual(expectedElectricalType, actualElectricalCost.Type, "Electrical cost type didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_Tag()
        {
            Guid expectedGuid = new Guid("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            string expectedString = "Test Tag";

            TECLabeled actualTag = null;
            foreach (TECTag tag in actualTemplates.Catalogs.Tags)
            {
                if (tag.Guid == expectedGuid)
                {
                    actualTag = tag;
                    break;
                }
            }

            Assert.AreEqual(expectedString, actualTag.Label, "Tag text didn't load properly.");
        }

        [TestMethod]
        public void Load_Templates_MiscCost()
        {
            //Arrange
            Guid expectedGuid = new Guid("5df99701-1d7b-4fbe-843d-40793f4145a8");
            string expectedName = "Bid Misc";
            double expectedCost = 1298;
            double expectedLabor = 8921;
            double expectedQuantity = 2;
            CostType expectedType = CostType.Electrical;
            TECMisc actualMisc = null;
            foreach (TECMisc misc in actualTemplates.Templates.MiscCostTemplates)
            {
                if (misc.Guid == expectedGuid)
                {
                    actualMisc = misc;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualMisc.Name);
            Assert.AreEqual(expectedQuantity, actualMisc.Quantity);
            Assert.AreEqual(expectedCost, actualMisc.Cost);
            Assert.AreEqual(expectedLabor, actualMisc.Labor);
            Assert.AreEqual(expectedType, actualMisc.Type);
        }

        [TestMethod]
        public void Load_Templates_IOModule()
        {
            //Arrange
            Guid expectedGuid = new Guid("b346378d-dc72-4dda-b275-bbe03022dd12");
            string expectedName = "Test IO Module";
            string expectedDescription = "Test IO Module Description";
            double expectedCost = 2233;
            double expectedIOPerModule = 3;

            TECIOModule actualModule = null;
            foreach (TECIOModule module in actualTemplates.Catalogs.IOModules)
            {
                if (module.Guid == expectedGuid)
                {
                    actualModule = module;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualModule.Name);
            Assert.AreEqual(expectedDescription, actualModule.Description);
            Assert.AreEqual(expectedCost, actualModule.Price);
            Assert.AreEqual(expectedIOPerModule, actualModule.IO.Count);
        }

        [TestMethod]
        public void Load_Templates_PanelType()
        {
            //Arrange
            Guid expectedGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            string expectedName = "Test Panel Type";
            double expectedCost = 1324;
            double expectedLabor = 4231;

            TECPanelType actualType = null;
            foreach (TECPanelType type in actualTemplates.Catalogs.PanelTypes)
            {
                if (type.Guid == expectedGuid)
                {
                    actualType = type;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualType.Name);
            Assert.AreEqual(expectedCost, actualType.Price);
            Assert.AreEqual(expectedLabor, actualType.Labor);
        }

        [TestMethod]
        public void Load_Templates_Panel()
        {
            //Arrange
            Guid expectedGuid = new Guid("a8cdd31c-e690-4eaa-81ea-602c72904391");
            string expectedName = "Bid Panel";
            string expectedDescription = "Bid Panel Description";

            Guid expectedTypeGuid = new Guid("04e3204c-b35f-4e1a-8a01-db07f7eb055e");

            TECPanel actualPanel = null;
            foreach (TECPanel panel in actualTemplates.Templates.PanelTemplates)
            {
                if (panel.Guid == expectedGuid)
                {
                    actualPanel = panel;
                    break;
                }
            }

            //Assert
            Assert.AreEqual(expectedName, actualPanel.Name);
            Assert.AreEqual(expectedDescription, actualPanel.Description);
            Assert.AreEqual(expectedTypeGuid, actualPanel.Type.Guid);
            testForCosts(actualPanel);
        }

        [TestMethod]
        public void Load_Templates_ScopeBranch()
        {
            Guid expectedGuid = new Guid("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            string expectedName = "System Scope Branch";

            Guid childGuid = new Guid("542802f6-a7b1-4020-9be4-e58225c433a8");

            TECScopeBranch actualBranch = null;
            foreach(TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                foreach(TECScopeBranch branch in system.ScopeBranches)
                {
                    if (branch.Guid == expectedGuid)
                    {
                        actualBranch = branch;
                        break;
                    }
                }
                if (actualBranch != null) break;
            }

            bool foundChildBranch = false;
            foreach(TECScopeBranch branch in actualBranch.Branches)
            {
                if (branch.Guid == childGuid)
                {
                    foundChildBranch = true;
                    break;
                }
            }

            Assert.AreEqual(expectedName, actualBranch.Label, "Scope branch name didn't load properly.");

            Assert.IsTrue(foundChildBranch, "Child branch didn't load properly into scope branch.");
        }

        [TestMethod]
        public void Load_Templates_SubScopeConnection()
        {
            Guid expectedGuid = new Guid("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            double expectedWireLength = 40;
            double expectedConduitLength = 20;

            Guid expectedParentControllerGuid = new Guid("1bb86714-2512-4fdd-a80f-46969753d8a0");
            Guid expectedConduitTypeGuid = new Guid("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            Guid expectedSubScopeGuid = new Guid("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");

            TECHardwiredConnection actualSSConnect = null;
            foreach (TECSystem typical in actualTemplates.Templates.SystemTemplates)
            {
                foreach (TECController controller in typical.Controllers)
                {
                    foreach (IControllerConnection connection in controller.ChildrenConnections)
                    {
                        if (connection.Guid == expectedGuid)
                        {
                            actualSSConnect = (connection as TECHardwiredConnection);
                            break;
                        }
                    }
                    if (actualSSConnect != null) break;
                }
                if (actualSSConnect != null) break;
            }

            //Assert
            Assert.AreEqual(expectedWireLength, actualSSConnect.Length, "Length didn't load properly in subscope connection.");
            Assert.AreEqual(expectedConduitLength, actualSSConnect.ConduitLength, "ConduitLength didn't load properly in subscope connection.");

            Assert.AreEqual(expectedParentControllerGuid, actualSSConnect.ParentController.Guid, "Parent controller didn't load properly in subscope connection.");
            Assert.AreEqual(expectedConduitTypeGuid, actualSSConnect.ConduitType.Guid, "Conduit type didn't load properly in subscope connection.");
            Assert.AreEqual(expectedSubScopeGuid, actualSSConnect.Child.Guid, "Subscope didn't load properly in subscope connection.");
        }

        [TestMethod]
        public void Load_Templates_SubScopeSynchronizer()
        {
            Guid expectedTemplateGuid = new Guid("826ae232-c1c5-4924-8e10-cf2d7a1d1ec4");
            Guid expectedEquipmentGuid = new Guid("7e61613f-62ec-4b06-b875-84b14e432758");
            Guid expectedReferenceGuid = new Guid("020f58a8-afe0-409c-ab32-043297dba625");

            TECEquipment actualEquipment = null;
            foreach(TECEquipment equipment in actualTemplates.Templates.EquipmentTemplates)
            {
                if(equipment.Guid == expectedEquipmentGuid)
                {
                    actualEquipment = equipment;
                    break;
                }
            }
            Assert.IsNotNull(actualEquipment, "Equipment not found");
            TECSubScope actualReferenceSubScope = null;
            foreach(TECSubScope subScope in actualEquipment.SubScope)
            {
                if(subScope.Guid == expectedReferenceGuid)
                {
                    actualReferenceSubScope = subScope;
                    break;
                }
            }
            Assert.IsNotNull(actualReferenceSubScope, "SubScope not found in equipment");
            TECSubScope actualTemplateSubScope = null;
            foreach (TECSubScope subScope in actualTemplates.Templates.SubScopeTemplates)
            {
                if (subScope.Guid == expectedTemplateGuid)
                {
                    actualTemplateSubScope = subScope;
                    break;
                }
            }
            Assert.IsNotNull(actualTemplateSubScope, "SubScope template not found");

            Assert.IsTrue(actualTemplates.SubScopeSynchronizer.Contains(actualTemplateSubScope), "Template SubScope not in synchronizer");
            Assert.IsTrue(actualTemplates.SubScopeSynchronizer.Contains(actualReferenceSubScope), "Reference SubScope not in synchronizer");
            Assert.IsTrue(actualTemplates.SubScopeSynchronizer.GetFullDictionary()[actualTemplateSubScope].Contains(actualReferenceSubScope),
                "Reference is not synchronized with template");

        }

        [TestMethod]
        public void Load_Templates_EquipmentSynchronizer()
        {
            Guid expectedTemplateGuid = new Guid("5e5c034a-8c88-4ae4-92a8-a1bac716af82");
            Guid expectedSystemGuid = new Guid("e096ffb5-82f3-41c2-b767-c73b22c6875b");
            Guid expectedReferenceGuid = new Guid("87d06d89-10b7-49c7-8b08-65707a5967a4");

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                if (system.Guid == expectedSystemGuid)
                {
                    actualSystem = system;
                    break;
                }
            }
            Assert.IsNotNull(actualSystem, "System not found");
            TECEquipment actualReferenceEquipment = null;
            foreach (TECEquipment equipment in actualSystem.Equipment)
            {
                if (equipment.Guid == expectedReferenceGuid)
                {
                    actualReferenceEquipment = equipment;
                    break;
                }
            }
            Assert.IsNotNull(actualReferenceEquipment, "Equipment not found in system");
            TECEquipment actualTemplateEquipment = null;
            foreach (TECEquipment equipment in actualTemplates.Templates.EquipmentTemplates)
            {
                if (equipment.Guid == expectedTemplateGuid)
                {
                    actualTemplateEquipment = equipment;
                    break;
                }
            }
            Assert.IsNotNull(actualTemplateEquipment, "Equipment template not found");

            Assert.IsTrue(actualTemplates.EquipmentSynchronizer.Contains(actualTemplateEquipment), "Template Equipment not in synchronizer");
            Assert.IsTrue(actualTemplates.EquipmentSynchronizer.Contains(actualReferenceEquipment), "Reference Equipment not in synchronizer");
            Assert.IsTrue(actualTemplates.EquipmentSynchronizer.GetFullDictionary()[actualTemplateEquipment].Contains(actualReferenceEquipment),
                "Reference is not synchronized with template");

        }

        [TestMethod]
        public void Load_Templates_CombinedSynchronizer()
        {
            Guid parentSystemGuid = new Guid("d562049c-ea9e-449c-8c1f-eaa7fbcb70d3");

            Guid equipmentTemplateGuid = new Guid("adced9c6-41c1-478b-b9db-3833f1618378");
            Guid equipmentReferenceGuid = new Guid("53d8c07f-872c-41de-8dd1-8aa349978ef4");

            Guid subScopeTemplateGuid = new Guid("59d6adb3-7f48-4448-82fa-f77cdfac47ad");
            Guid subScopeChildTemplateGuid = new Guid("a26ab8aa-3b44-4321-a48e-872b250490a9");
            Guid subScopeChildReferenceGuid = new Guid("c96120a1-b9e7-40e8-b015-e7383feca57d");

            TECSystem actualSystem = null;
            foreach (TECSystem system in actualTemplates.Templates.SystemTemplates)
            {
                if (system.Guid == parentSystemGuid)
                {
                    actualSystem = system;
                    break;
                }
            }
            Assert.IsNotNull(actualSystem, "Parent system not found.");

            TECEquipment actualTemplateEquip = null;
            foreach(TECEquipment equip in actualTemplates.Templates.EquipmentTemplates)
            {
                if (equip.Guid == equipmentTemplateGuid)
                {
                    actualTemplateEquip = equip;
                    break;
                }
            }
            Assert.IsNotNull(actualTemplateEquip, "Equipment template not found.");

            TECEquipment actualRefEquip = null;
            foreach(TECEquipment equip in actualSystem.Equipment)
            {
                if (equip.Guid == equipmentReferenceGuid)
                {
                    actualRefEquip = equip;
                    break;
                }
            }
            Assert.IsNotNull(actualRefEquip, "Equipment reference not found.");

            TECSubScope actualTemplateSS = null;
            foreach(TECSubScope ss in actualTemplates.Templates.SubScopeTemplates)
            {
                if (ss.Guid == subScopeTemplateGuid)
                {
                    actualTemplateSS = ss;
                    break;
                }
            }
            Assert.IsNotNull(actualTemplateSS, "SubScope template not found.");

            TECSubScope actualChildTemplateSS = null;
            foreach(TECSubScope ss in actualTemplateEquip.SubScope)
            {
                if (ss.Guid == subScopeChildTemplateGuid)
                {
                    actualChildTemplateSS = ss;
                    break;
                }
            }
            Assert.IsNotNull(actualChildTemplateSS, "Child subScope in template equipment not found.");

            TECSubScope actualChildRefSS = null;
            foreach(TECSubScope ss in actualRefEquip.SubScope)
            {
                if (ss.Guid == subScopeChildReferenceGuid)
                {
                    actualChildRefSS = ss;
                    break;
                }
            }
            Assert.IsNotNull(actualChildRefSS, "Child subScope in reference equipment not found.");

            TemplateSynchronizer<TECSubScope> ssSync = actualTemplates.SubScopeSynchronizer;
            TemplateSynchronizer<TECEquipment> equipSync = actualTemplates.EquipmentSynchronizer;

            Assert.IsTrue(equipSync.Contains(actualTemplateEquip), "Equipment template not in synchronizer.");
            Assert.IsTrue(equipSync.Contains(actualRefEquip), "Equipment reference not in synchronizer.");
            Assert.IsTrue(equipSync.GetFullDictionary()[actualTemplateEquip].Contains(actualRefEquip),
                "Equipment template not synchronized with equipment reference.");

            Assert.IsTrue(ssSync.Contains(actualTemplateSS), "SubScope template not in synchroninzer.");
            Assert.IsTrue(ssSync.Contains(actualChildTemplateSS), "SubScope reference not in synchronizer.");
            Assert.IsTrue(ssSync.Contains(actualChildRefSS), "Child SubScope reference not in synchronizer.");
            Assert.IsTrue(ssSync.GetFullDictionary()[actualTemplateSS].Contains(actualChildTemplateSS),
                "SubScope template not synchronized with subScope reference.");
            Assert.IsTrue(ssSync.GetFullDictionary()[actualChildTemplateSS].Contains(actualChildRefSS),
                "Child subScope template not synchronized with child subScope reference.");
        }

        private void testForTag(TECScope scope)
        {
            bool foundTag = false;

            foreach (TECTag tag in scope.Tags)
            {
                if (tag.Guid == TEST_TAG_GUID)
                {
                    foundTag = true;
                    break;
                }
            }

            Assert.IsTrue(foundTag, "Tag not loaded properly into scope.");
        }
        private void testForCosts(TECScope scope)
        {
            bool foundTECCost = false;
            bool foundElectricalCost = false;

            foreach (TECAssociatedCost cost in scope.AssociatedCosts)
            {
                if (cost.Guid == TEST_TEC_COST_GUID)
                {
                    foundTECCost = true;
                    break;
                }
            }
            foreach (TECAssociatedCost cost in scope.AssociatedCosts)
            {
                if (cost.Guid == TEST_ELECTRICAL_COST_GUID)
                {
                    foundElectricalCost = true;
                    break;
                }
            }

            Assert.IsTrue(foundTECCost, "TEC Cost not loaded properly into scope.");
            Assert.IsTrue(foundElectricalCost, "Electrical Cost not loaded properly into scope.");
        }

        private void testForRatedCosts(TECElectricalMaterial component)
        {
            bool foundCost = false;

            foreach (TECAssociatedCost cost in component.RatedCosts)
            {
                if (cost.Guid == TEST_RATED_COST_GUID)
                {
                    foundCost = true;
                    break;
                }
            }

            Assert.IsTrue(foundCost, "Rated Cost not loaded properly into scope.");
        }
    }
}
