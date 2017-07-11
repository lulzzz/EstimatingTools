﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EstimatingLibrary;
using EstimatingUtilitiesLibrary;

namespace Tests
{
    /// <summary>
    /// Summary description for SaveStackTests
    /// </summary>
    [TestClass]
    public class SaveStackTests
    {
        public SaveStackTests()
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

        [TestMethod]
        public void Bid_AddSystem()
        {
            TECBid bid = new TECBid();
            ChangeStack stack = new ChangeStack(bid);
            int expectedCount = 1;
            
            bid.Systems.Add(new TECSystem());

            Assert.AreEqual(expectedCount, stack.SaveStack.Count);
        }

        [TestMethod]
        public void Bid_AddSystemInstance()
        {
            TECBid bid = new TECBid();
            ChangeStack stack = new ChangeStack(bid);
            TECSystem system = new TECSystem();
            bid.Systems.Add(system);
            
            int initialCount = stack.SaveStack.Count;
            int expectedCount = 1;

            TECSystem instance = system.AddInstance(bid);
            StackItem expectedItem = new StackItem(Change.Add, system, instance);
            int actualCount = stack.SaveStack.Count - initialCount;

            Assert.AreEqual(expectedCount, actualCount);
            checkStackItem(expectedItem, stack.SaveStack[stack.SaveStack.Count - 1]);
        }

       
        public void checkStackItem(StackItem expectedItem, StackItem actualItem)
        {
            Assert.AreEqual(expectedItem.Change, actualItem.Change);
            Assert.AreEqual(expectedItem.ReferenceObject, actualItem.ReferenceObject);
            Assert.AreEqual(expectedItem.TargetObject, actualItem.TargetObject);
            Assert.AreEqual(expectedItem.ReferenceType, actualItem.ReferenceType);
            Assert.AreEqual(expectedItem.TargetType, actualItem.TargetType);
        }
    }
}
