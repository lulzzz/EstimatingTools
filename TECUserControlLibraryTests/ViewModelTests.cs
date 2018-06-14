﻿using EstimatingLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECUserControlLibrary.Models;

namespace Tests
{
    /// <summary>
    /// Summary description for ViewModelTests
    /// </summary>
    [TestClass]
    public class ViewModelTests
    {
        public ViewModelTests()
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion
            
        #region ControllerInPanel
        [TestMethod]
        public void ControllerInPanel_AddPanel()
        {
            TECPanel panel = new TECPanel(new TECPanelType(new TECManufacturer()), false);
            TECController controller = new TECProvidedController(new TECControllerType(new TECManufacturer()), false);

            ControllerInPanel controllerInPanel = new ControllerInPanel(controller, null);

            controllerInPanel.Panel = panel;

            Assert.AreEqual(panel.Controllers.Count, 1, "Controller not added to panel");
        }

        [TestMethod]
        public void ControllerInPanel_RemovePanel()
        {
            TECPanel panel = new TECPanel(new TECPanelType(new TECManufacturer()), false);
            TECController controller = new TECProvidedController(new TECControllerType( new TECManufacturer()), false);

            ControllerInPanel controllerInPanel = new ControllerInPanel(controller, panel);

            controllerInPanel.Panel = null;

            Assert.AreEqual(panel.Controllers.Count, 0, "Controller not removed to panel");
        }
        #endregion

        
    }
}
