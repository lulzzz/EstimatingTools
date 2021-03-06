﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    [TestClass]
    public class TemplateSynchronizerTests
    {
        #region Base Class Tests
        private TECSubScope copySubScope(TECSubScope template)
        {
            return new TECSubScope(template);
        }
        private void syncSubScope(TemplateSynchronizer<TECSubScope> synchronizer,
            TECSubScope templateSS, TECSubScope toSync, TECChangedEventArgs args)
        {
            toSync.CopyPropertiesFromScope(templateSS);
            foreach (TECSubScope reference in synchronizer.GetFullDictionary()[templateSS].Where(item => item != toSync))
            {
                reference.CopyPropertiesFromScope(toSync);
            }
        }

        [TestMethod]
        public void NewItemTest()
        {
            //Arrange
            TECScopeTemplates templates = new TECScopeTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer =
                new TemplateSynchronizer<TECSubScope>(copySubScope, item => { }, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";

            synchronizer.NewGroup(templateSS);

            //Act
            TECSubScope copySS = synchronizer.NewItem(templateSS);

            //Assert
            Assert.AreEqual(templateSS.Name, copySS.Name);
            Assert.AreNotEqual(templateSS.Guid, copySS.Guid);
        }

        [TestMethod]
        public void NewItemNoGroupTest()
        {
            //Arrange
            TECScopeTemplates templates = new TECScopeTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer =
                new TemplateSynchronizer<TECSubScope>(copySubScope, item => { }, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";

            //Act
            TECSubScope copySS = synchronizer.NewItem(templateSS);

            //Assert
            Assert.AreEqual(templateSS.Name, copySS.Name);
            Assert.AreNotEqual(templateSS.Guid, copySS.Guid);
        }

        [TestMethod]
        public void ChangeTemplateTest()
        {
            //Arrange
            TECScopeTemplates templates = new TECScopeTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer =
                new TemplateSynchronizer<TECSubScope>(copySubScope, item => { }, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";

            TECSubScope copySS = synchronizer.NewItem(templateSS);

            //Act
            templateSS.Description = "Test Description";

            //Assert
            Assert.AreEqual(templateSS.Description, copySS.Description);
        }

        [TestMethod]
        public void ChangeInstanceTest()
        {
            //Arrange
            TECScopeTemplates templates = new TECScopeTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer =
                new TemplateSynchronizer<TECSubScope>(copySubScope, item => { }, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";

            TECSubScope copySS = synchronizer.NewItem(templateSS);

            //Act
            copySS.Description = "Test Description";

            //Assert
            Assert.AreEqual(templateSS.Description, copySS.Description);
        }

        [TestMethod]
        public void LinkExistingTest()
        {
            //Arrange
            TECScopeTemplates templates = new TECScopeTemplates();

            TemplateSynchronizer<TECSubScope> synchronizer =
                new TemplateSynchronizer<TECSubScope>(copySubScope, item => { }, syncSubScope, templates);

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";

            List<TECSubScope> newReferenceSS = new List<TECSubScope>();
            newReferenceSS.Add(new TECSubScope());
            newReferenceSS.Add(new TECSubScope());

            //Act
            synchronizer.LinkExisting(templateSS, newReferenceSS);
            templateSS.Description = "Test Description";

            //Assert
            foreach (TECSubScope refSS in newReferenceSS)
            {
                Assert.AreEqual(templateSS.Description, refSS.Description);
            }
        }

        #endregion

        #region TECTemplates Integration Tests
        [TestMethod]
        public void SubScopeTemplateChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";
            templates.Templates.SubScopeTemplates.Add(templateSS);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;
            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(equip);
            equip.SubScope.Add(refSS);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(),
                new List<TECProtocol>(),
                new TECManufacturer());
            templates.Catalogs.Add(dev);

            TECPoint point = new TECPoint();
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECAssociatedCost cost = new TECAssociatedCost(CostType.TEC);
            templates.Catalogs.Add(cost);

            TECTag tag = new TECTag();
            templates.Catalogs.Add(tag);

            //Act
            templateSS.Description = "Test Description";
            templateSS.Devices.Add(dev);
            templateSS.AddPoint(point);
            templateSS.AssociatedCosts.Add(cost);
            templateSS.Tags.Add(tag);

            //Assert
            //Assert.AreEqual(templateSS.Description, refSS.Description, "Description didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Devices[0], refSS.Devices[0], "Devices didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Label, refSS.Points[0].Label, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Type, refSS.Points[0].Type, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Points[0].Quantity, refSS.Points[0].Quantity, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.AssociatedCosts[0], refSS.AssociatedCosts[0], "Associated costs didn't sync properly between SubScope.");
            Assert.AreEqual(templateSS.Tags[0], refSS.Tags[0], "Tags didn't sync properly between SubScope.");
        }

        [TestMethod]
        public void SubScopeReferenceChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "Template SubScope";
            templates.Templates.SubScopeTemplates.Add(templateSS);

            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;
            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);

            TECEquipment equip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(equip);
            equip.SubScope.Add(refSS);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(),
                new List<TECProtocol>(),
                new TECManufacturer());
            templates.Catalogs.Add(dev);

            TECPoint point = new TECPoint();
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECAssociatedCost cost = new TECAssociatedCost(CostType.TEC);
            templates.Catalogs.Add(cost);

            TECTag tag = new TECTag();
            templates.Catalogs.Add(tag);

            //Act
            refSS.Description = "Test Description";
            refSS.Devices.Add(dev);
            refSS.AddPoint(point);
            refSS.AssociatedCosts.Add(cost);
            refSS.Tags.Add(tag);

            //Assert
            Assert.AreEqual(refSS.Devices[0], templateSS.Devices[0], "Devices didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Points[0].Label, templateSS.Points[0].Label, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Points[0].Type, templateSS.Points[0].Type, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Points[0].Quantity, templateSS.Points[0].Quantity, "Points didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.AssociatedCosts[0], templateSS.AssociatedCosts[0], "Associated costs didn't sync properly between SubScope.");
            Assert.AreEqual(refSS.Tags[0], templateSS.Tags[0], "Tags didn't sync properly between SubScope.");
        }

        [TestMethod]
        public void EquipmentTemplateChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECEquipment templateEquip = new TECEquipment();
            templateEquip.Name = "Template Equip";
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem();
            templates.Templates.SystemTemplates.Add(sys);
            sys.Equipment.Add(refEquip);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(),
                new List<TECProtocol>(),
                new TECManufacturer());
            templates.Catalogs.Add(dev);

            TECPoint point = new TECPoint();
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECAssociatedCost cost = new TECAssociatedCost(CostType.TEC);
            templates.Catalogs.Add(cost);

            TECTag tag = new TECTag();
            templates.Catalogs.Add(tag);

            TECSubScope ss = new TECSubScope();
            ss.Description = "Test Description";
            ss.Devices.Add(dev);
            ss.AddPoint(point);
            ss.AssociatedCosts.Add(cost);
            ss.Tags.Add(tag);

            templates.Templates.SubScopeTemplates.Add(ss);

            //Act
            templateEquip.Description = "Test Description";
            templateEquip.SubScope.Add(ss);
            templateEquip.AssociatedCosts.Add(cost);
            templateEquip.Tags.Add(tag);

            //Assert
            //Assert.AreEqual(templateEquip.Description, refEquip.Description, "Description didn't sync properly between Equipment.");

            Assert.IsNotNull(refEquip.SubScope[0], "SubScope didn't sync properly between Equipment.");

            TECSubScope templateSubScope = templateEquip.SubScope[0];
            TECSubScope refSubScope = refEquip.SubScope[0];

            //Assert.AreEqual(templateSubScope.Description, refSubScope.Description, "Description didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Devices[0], refSubScope.Devices[0], "Devices didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Points[0].Label, refSubScope.Points[0].Label, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Points[0].Type, refSubScope.Points[0].Type, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Points[0].Quantity, refSubScope.Points[0].Quantity, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.AssociatedCosts[0], refSubScope.AssociatedCosts[0], "AssociatedCosts didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(templateSubScope.Tags[0], refSubScope.Tags[0], "Tags didn't sync properly between SubScope.");

            Assert.AreEqual(templateEquip.AssociatedCosts[0], refEquip.AssociatedCosts[0], "AssociatedCosts didn't sync properly between Equipment.");
            Assert.AreEqual(templateEquip.Tags[0], refEquip.Tags[0], "Tags didn't sync properly in Equipment.");
        }

        [TestMethod]
        public void EquipmentReferenceChanged()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();

            TECEquipment templateEquip = new TECEquipment();
            templateEquip.Name = "Template Equip";
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);

            TECSystem sys = new TECSystem();
            templates.Templates.SystemTemplates.Add(sys);
            sys.Equipment.Add(refEquip);

            TECDevice dev = new TECDevice(new List<TECConnectionType>(),
                new List<TECProtocol>(),
                new TECManufacturer());
            templates.Catalogs.Add(dev);

            TECPoint point = new TECPoint();
            point.Label = "Test Point";
            point.Type = IOType.AI;
            point.Quantity = 5;

            TECAssociatedCost cost = new TECAssociatedCost(CostType.TEC);
            templates.Catalogs.Add(cost);

            TECTag tag = new TECTag();
            templates.Catalogs.Add(tag);

            TECSubScope ss = new TECSubScope();
            ss.Description = "Test Description";
            ss.Devices.Add(dev);
            ss.AddPoint(point);
            ss.AssociatedCosts.Add(cost);
            ss.Tags.Add(tag);

            templates.Templates.SubScopeTemplates.Add(ss);

            //Act
            refEquip.Description = "Test Description";
            refEquip.SubScope.Add(ss);
            refEquip.AssociatedCosts.Add(cost);
            refEquip.Tags.Add(tag);

            //Assert
            //Assert.AreEqual(refEquip.Description, templateEquip.Description, "Description didn't sync properly between Equipment.");

            Assert.IsNotNull(templateEquip.SubScope[0], "SubScope didn't sync properly between Equipment.");

            TECSubScope templateSubScope = templateEquip.SubScope[0];
            TECSubScope refSubScope = refEquip.SubScope[0];

            //Assert.AreEqual(refSubScope.Description, templateSubScope.Description, "Description didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Devices[0], templateSubScope.Devices[0], "Devices didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Points[0].Label, templateSubScope.Points[0].Label, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Points[0].Type, templateSubScope.Points[0].Type, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Points[0].Quantity, templateSubScope.Points[0].Quantity, "Points didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.AssociatedCosts[0], templateSubScope.AssociatedCosts[0], "AssociatedCosts didn't sync properly between SubScope in Equipment.");
            Assert.AreEqual(refSubScope.Tags[0], templateSubScope.Tags[0], "Tags didn't sync properly between SubScope.");

            Assert.AreEqual(refEquip.AssociatedCosts[0], templateEquip.AssociatedCosts[0], "AssociatedCosts didn't sync properly between Equipment.");
            Assert.AreEqual(refEquip.Tags[0], templateEquip.Tags[0], "Tags didn't sync properly in Equipment.");
        }

        [TestMethod]
        public void TemplateSubScopeRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECSubScope> synchronizer = templates.SubScopeSynchronizer;

            TECSubScope templateSS = new TECSubScope();
            templateSS.Name = "First Name";
            templates.Templates.SubScopeTemplates.Add(templateSS);

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);
            TECSubScope ss1 = synchronizer.NewItem(templateSS);
            TECSubScope ss2 = synchronizer.NewItem(templateSS);
            templateEquip.SubScope.Add(ss1);
            templateEquip.SubScope.Add(ss2);

            //Act
            templates.Templates.SubScopeTemplates.Remove(templateSS);

            ss2.Name = "Second Name";

            //Assert
            Assert.IsFalse(synchronizer.Contains(templateSS));
            Assert.IsFalse(synchronizer.Contains(ss1));
            Assert.IsFalse(synchronizer.Contains(ss2));
            Assert.AreEqual("First Name", ss1.Name);
            Assert.AreEqual("Second Name", ss2.Name);
        }

        [TestMethod]
        public void ReferenceSubScopeRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECSubScope> synchronizer = templates.SubScopeSynchronizer;

            TECSubScope templateSS = new TECSubScope();
            templates.Templates.SubScopeTemplates.Add(templateSS);

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);
            TECSubScope refSS = synchronizer.NewItem(templateSS);
            templateEquip.SubScope.Add(refSS);

            //Act
            templateEquip.SubScope.Remove(refSS);

            //Assert
            Assert.IsFalse(synchronizer.Contains(refSS));
            Assert.IsTrue(synchronizer.Contains(templateSS));
        }

        [TestMethod]
        public void TemplateEquipmentRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> synchronizer = templates.EquipmentSynchronizer;

            TECEquipment templateEquip = new TECEquipment();
            templateEquip.Name = "First Name";
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TECSystem templateSys = new TECSystem();
            templates.Templates.SystemTemplates.Add(templateSys);
            TECEquipment equip1 = synchronizer.NewItem(templateEquip);
            TECEquipment equip2 = synchronizer.NewItem(templateEquip);
            templateSys.Equipment.Add(equip1);
            templateSys.Equipment.Add(equip2);

            //Act
            templates.Templates.EquipmentTemplates.Remove(templateEquip);

            equip2.Name = "Second Name";

            //Assert
            Assert.IsFalse(synchronizer.Contains(templateEquip));
            Assert.IsFalse(synchronizer.Contains(equip1));
            Assert.IsFalse(synchronizer.Contains(equip2));
            Assert.AreEqual("First Name", equip1.Name);
            Assert.AreEqual("Second Name", equip2.Name);
        }

        [TestMethod]
        public void ReferenceEquipmentRemoved()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> synchronizer = templates.EquipmentSynchronizer;

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TECSystem templateSys = new TECSystem();
            templates.Templates.SystemTemplates.Add(templateSys);
            TECEquipment refEquip = synchronizer.NewItem(templateEquip);
            templateSys.Equipment.Add(refEquip);

            //Act
            templateSys.Equipment.Remove(refEquip);

            //Assert
            Assert.IsFalse(synchronizer.Contains(refEquip));
            Assert.IsTrue(synchronizer.Contains(templateEquip));
        }

        [TestMethod]
        public void TemplatedSubScopeRemovedFromTemplateEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TECSubScope templateSS = new TECSubScope();
            templates.Templates.SubScopeTemplates.Add(templateSS);

            TECSubScope instanceSS = ssSynchronizer.NewItem(templateSS);
            templateEquip.SubScope.Add(instanceSS);

            TECEquipment instanceEquip = equipSynchronizer.NewItem(templateEquip);
            templates.Templates.EquipmentTemplates.Add(instanceEquip);

            //Act
            templateEquip.SubScope.Remove(instanceSS);

            //Assert
            Assert.IsTrue(instanceEquip.SubScope.Count == 0, "SubScope not removed properly from equipment reference.");

            Assert.IsFalse(ssSynchronizer.Contains(instanceSS), "Reference SubScope not removed properly from synchronizer.");
            Assert.IsTrue(ssSynchronizer.Contains(templateSS), "Template SubScope was removed from synchronizer when it shouldn't have been.");
        }

        [TestMethod]
        public void InstanceSubScopeRemovedFromTemplateEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TECSubScope ss = new TECSubScope();
            templateEquip.SubScope.Add(ss);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);
            TECSubScope refSS = refEquip.SubScope[0];

            //Act
            templateEquip.SubScope.Remove(ss);

            //Assert
            Assert.IsTrue(refEquip.SubScope.Count == 0, "SubScope not removed properly from equipment reference.");

            Assert.IsFalse(ssSynchronizer.Contains(ss), "SubScope was not removed properly from synchronizer.");
            Assert.IsFalse(ssSynchronizer.Contains(refSS), "Reference SubScope was not removed properly from synchronizer.");
        }

        [TestMethod]
        public void TemplatedSubScopeRemovedFromReferenceEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TECSubScope templateSS = new TECSubScope();
            templates.Templates.SubScopeTemplates.Add(templateSS);

            TECSystem system = new TECSystem();
            templates.Templates.SystemTemplates.Add(system);

            TECEquipment refEquip = equipSynchronizer.NewItem(templateEquip);
            system.Equipment.Add(refEquip);

            TECSubScope refSS = ssSynchronizer.NewItem(templateSS);
            refEquip.SubScope.Add(refSS);

            //TECSubScope refSS = refEquip.SubScope[0];

            //Act
            refEquip.SubScope.Remove(refSS);

            //Assert
            Assert.IsTrue(templateEquip.SubScope.Count == 0, "SubScope not removed properly from Equipment template.");

            Assert.IsFalse(ssSynchronizer.Contains(refSS), "Reference SubScope was not removed properly from synchronizer.");
            Assert.IsTrue(ssSynchronizer.Contains(templateSS), "Template SubScope was removed from synchronizer.");
        }

        [TestMethod]
        public void InstanceSubScopeRemovedFromReferenceEquipment()
        {
            //Arrange
            TECTemplates templates = new TECTemplates();
            TemplateSynchronizer<TECEquipment> equipSynchronizer = templates.EquipmentSynchronizer;
            TemplateSynchronizer<TECSubScope> ssSynchronizer = templates.SubScopeSynchronizer;

            TECEquipment templateEquip = new TECEquipment();
            templates.Templates.EquipmentTemplates.Add(templateEquip);

            TECSubScope ss = new TECSubScope();
            templateEquip.SubScope.Add(ss);

            TECEquipment refEqiup = equipSynchronizer.NewItem(templateEquip);
            TECSubScope refSS = refEqiup.SubScope[0];

            //Act
            refEqiup.SubScope.Remove(refSS);

            //Assert
            Assert.IsTrue(templateEquip.SubScope.Count == 0, "SubScope not removed properly from Equipment template.");

            Assert.IsFalse(ssSynchronizer.Contains(ss), "SubScope was not removed properly from synchronizer.");
            Assert.IsFalse(ssSynchronizer.Contains(refSS), "Reference SubScope was not removed properly from synchronizer.");
        }
        #endregion

        [TestMethod()]
        public void NewGroupTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            Assert.IsNotNull(synchronizer.GetFullDictionary()[template]);

        }

        [TestMethod()]
        public void RemoveGroupTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            synchronizer.RemoveGroup(template);

            Assert.IsFalse(synchronizer.GetFullDictionary().ContainsKey(template));
        }

        [TestMethod()]
        public void NewItemTest1()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var newItem = synchronizer.NewItem(template);

            Assert.IsNotNull(newItem);
            Assert.AreNotEqual(template, newItem);
        }

        [TestMethod()]
        public void RemoveItemTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var newItem = synchronizer.NewItem(template);
            synchronizer.RemoveItem(newItem);
            
            Assert.IsFalse(synchronizer.GetFullDictionary()[template].Contains(newItem));
        }

        [TestMethod()]
        public void RemoveItemTest1()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var newItem = synchronizer.NewItem(template);
            synchronizer.RemoveItem(template, newItem);

            Assert.IsFalse(synchronizer.GetFullDictionary()[template].Contains(newItem));
        }

        [TestMethod()]
        public void LinkExistingTest1()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var existing = new TestObject();
            synchronizer.LinkExisting(template, existing);
            
            Assert.IsTrue(synchronizer.GetFullDictionary()[template].Contains(existing));
        }

        [TestMethod()]
        public void LinkExistingTest2()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var existing1 = new TestObject();
            var existing2 = new TestObject();
            var existing = new List<TestObject> { existing1, existing2 };
            synchronizer.LinkExisting(template, existing);

            Assert.IsTrue(synchronizer.GetFullDictionary()[template].Contains(existing1));
            Assert.IsTrue(synchronizer.GetFullDictionary()[template].Contains(existing2));
        }

        [TestMethod()]
        public void LinkNewTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            bool changed = false;

            synchronizer.TECChanged += arg =>
            {
                changed = true;
            };

            var existing = new TestObject();
            synchronizer.LinkNew(template, existing);

            Assert.IsTrue(synchronizer.GetFullDictionary()[template].Contains(existing));
            Assert.IsTrue(changed);
        }

        [TestMethod()]
        public void ContainsTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var existing = new TestObject();
            synchronizer.LinkExisting(template, existing);

            Assert.IsTrue(synchronizer.Contains(existing));
        }

        [TestMethod()]
        public void ContainsTest1()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var newItem = synchronizer.NewItem(template);
            synchronizer.RemoveItem(template, newItem);

            Assert.IsFalse(synchronizer.Contains(newItem));
        }

        [TestMethod()]
        public void GetFullDictionaryTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            synchronizer.NewItem(template);
            synchronizer.NewGroup(new TestObject());

            Assert.IsTrue(synchronizer.GetFullDictionary()[template].Count == 1);
            Assert.IsTrue(synchronizer.GetFullDictionary().Keys.Count == 2);
        }

        [TestMethod()]
        public void GetTemplateTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var newItem = synchronizer.NewItem(template);

            Assert.AreEqual(template, synchronizer.GetTemplate(newItem));
            Assert.AreEqual(template, synchronizer.GetTemplate(template));

        }

        [TestMethod()]
        public void GetParentTest()
        {
            TemplateSynchronizer<TestObject> synchronizer = new TemplateSynchronizer<TestObject>(obj => new TestObject(), obj => { }, (sync, obj1, obj2, e) => { }, new TECScopeTemplates());
            var template = new TestObject();
            synchronizer.NewGroup(template);

            var newItem = synchronizer.NewItem(template);

            Assert.AreEqual(template, synchronizer.GetParent(newItem));
            Assert.IsNull(synchronizer.GetParent(template));
        }

        private class TestObject : TECObject
        {
            public TestObject() : base(Guid.NewGuid()) { }

        }
    }
}