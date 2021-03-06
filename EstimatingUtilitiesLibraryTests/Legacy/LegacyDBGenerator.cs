﻿using System;
using System.Collections.Generic;

namespace Legacy
{
    internal class LegacyDBGenerator
    {
        static private EstimatingUtilitiesLibrary.Database.SQLiteDatabase SQLiteDB;

        public static void CreateTestBid_1_6(string path)
        {
            SQLiteDB = new EstimatingUtilitiesLibrary.Database.SQLiteDatabase(path);
            CreateBidDB();

            SQLiteDB.NonQueryCommand("BEGIN TRANSACTION");

            addToMetadataTable();
            AddToBidInfoTable();
            AddToBidParametersTable();
            AddToLaborConstantsTable();
            AddToSubcontractorConstantsTable();
            AddToUserAdjustmentsTable();
            AddToNoteTable();
            AddToExlusionTable();
            AddToScopeBranchTable();
            AddToSystemTable();
            AddToEquipmentTable();
            AddToSubScopeTable();
            AddToDeviceTable();
            AddToPointTable();
            AddToTagTable();
            AddToManufacturerTable();
            AddToDrawingTable();
            AddToPageTable();
            AddToLocationTable();
            AddToVisualScopeTable();
            AddToConnectionTypeTable();
            AddToConduitTypeTable();
            AddToAssociatedCostTable();
            AddToSubScopeConnectionTable();
            AddToNetworkConnectionTable();
            AddToControllerTable();
            AddToMiscTable();
            AddToPanelTypeTable();
            AddToPanelTable();
            AddToIOModuleTable();
            AddToIOTable();

            AddToBidLaborTable();
            AddToBidBidParametersTable();
            AddToBidScopeBranchTable();
            AddToBidMiscTable();
            AddToControllerIOTable();
            AddToIOModuleManufacturerTable();
            AddToIOIOModuleTable();
            AddToControllerConnectionTable();
            AddToScopeBranchHierarchyTable();
            AddToBidSystemTable();
            AddToSystemEquipmentTable();
            AddToEquipmentSubScopeTable();
            AddToSubScopeDeviceTable();
            AddToSubScopePointTable();
            AddToScopeTagTable();
            AddToDeviceManufacturerTable();
            AddToDeviceConnectionTypeTable();
            AddToDrawingPageTable();
            AddToPageVisualScopeTable();
            AddToVisualScopeScopeTable();
            AddToLocationScopeTable();
            AddToScopeAssociatedCostTable();
            AddElectricalComponentRatedCostTable();
            AddToControllerManufacturerTable();
            AddToConnectionConduitTypeTable();
            AddToNetworkConnectionConnectionTypeTable();
            AddToNetworkConnectionControllerTable();
            AddToSubScopeConnectionChildrenTable();
            AddToPanelPanelTypeTable();
            AddToPanelControllerTable();
            AddToSystemControllerTable();
            AddToSystemPanelTable();
            AddToSystemScopeBranchTable();
            AddToSystemHierarchyTable();
            AddToSystemMiscTable();
            AddToCharacteristicScopeInstanceScopeTable();

            SQLiteDB.NonQueryCommand("END TRANSACTION");
            SQLiteDB.Connection.Close();
        }

        public static void CreateTestTemplates(string path)
        {
            SQLiteDB = new EstimatingUtilitiesLibrary.Database.SQLiteDatabase(path);
            CreateTemplateDB();
            SQLiteDB.NonQueryCommand("BEGIN TRANSACTION");

            addToMetadataTable();
            AddToTemplatesInfoTable();
            AddToLaborConstantsTable();
            AddToSubcontractorConstantsTable();
            AddToSystemTable();
            AddToEquipmentTable();
            AddToSubScopeTable();
            AddToDeviceTable();
            AddToPointTable();
            AddToTagTable();
            AddToManufacturerTable();
            AddToConnectionTypeTable();
            AddToConduitTypeTable();
            AddToAssociatedCostTable();
            AddToSubScopeConnectionTable();
            AddToControllerTable();
            AddToMiscTable();
            AddToPanelTypeTable();
            AddToPanelTable();
            AddToIOModuleTable();
            AddToIOTable();
            AddToScopeBranchTable();

            AddToControllerIOTable();
            AddToIOModuleManufacturerTable();
            AddToIOIOModuleTable();
            AddToControllerConnectionTable();
            AddToSystemEquipmentTable();
            AddToEquipmentSubScopeTable();
            AddToScopeTagTable();
            AddToScopeAssociatedCostTable();
            AddElectricalComponentRatedCostTable();
            AddToControllerManufacturerTable();
            AddToConnectionConduitTypeTable();
            AddToSubScopeConnectionChildrenTable();
            AddToPanelPanelTypeTable();
            AddToPanelControllerTable();
            AddToSystemControllerTable();
            AddToSystemPanelTable();
            AddToSystemHierarchyTable();
            AddToSystemMiscTable();

            AddToSubScopeDeviceTable();
            AddToSubScopePointTable();
            AddToSystemScopeBranchTable();
            AddToScopeBranchHierarchyTable();
            AddToDeviceManufacturerTable();
            AddToDeviceConnectionTypeTable();

            SQLiteDB.NonQueryCommand("END TRANSACTION");
            SQLiteDB.Connection.Close();
        }

        private static void AddDataToTable(TableBase table, List<string> values)
        {
            TableInfo info = new TableInfo(table);
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (info.Fields.Count != values.Count)
            {
                throw new Exception("There must be one value per field");
            }
            for (int x = 0; x < info.Fields.Count; x++)
            {
                var field = info.Fields[x];
                var value = values[x];
                data[field.Name] = value;
            }

            SQLiteDB.Insert(info.Name, data);
        }

        #region Object Tables
        private static void addToMetadataTable()
        {
            List<string> values = new List<string>();
            values.Add("1");
            AddDataToTable(new MetadataTable(), values);
        }


        private static void AddToBidInfoTable()
        {
            List<string> values = new List<string>();
            //values.Add("1.6.0.13");
            values.Add("Testimate");
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("7357");
            values.Add("1969-07-20T00:00:00.0000000");
            values.Add("Mrs. Salesperson");
            values.Add("Mr. Estimator");
            AddDataToTable(new BidInfoTable(), values);

        }
        private static void AddToTemplatesInfoTable()
        {
            List<string> values = new List<string>();
            values.Add("28561e73-2843-4f56-9c47-2b32031472f2");
            //values.Add("1.6.0.13");
            AddDataToTable(new TemplatesInfoTable(), values);
        }
        private static void AddToBidParametersTable()
        {
            List<string> values = new List<string>();
            values.Add("655ed4a6-4ce4-431f-ae4b-7185e28d20ef");
            values.Add("10");
            values.Add("20");
            values.Add("20");
            values.Add("20");
            values.Add("10");
            values.Add("0");
            values.Add("0");
            values.Add("0");
            AddDataToTable(new BidParametersTable(), values);

        }
        private static void AddToLaborConstantsTable()
        {
            List<string> values = new List<string>();
            values.Add("ab534ec6-73ec-4145-9c58-3abbbc9ae3d5");
            values.Add("2");
            values.Add("30");
            values.Add("2");
            values.Add("40");
            values.Add("2");
            values.Add("50");
            values.Add("2");
            values.Add("60");
            values.Add("2");
            values.Add("70");
            AddDataToTable(new LaborConstantsTable(), values);
        }
        private static void AddToSubcontractorConstantsTable()
        {
            List<string> values = new List<string>();
            values.Add("ab534ec6-73ec-4145-9c58-3abbbc9ae3d5");
            values.Add("50");
            values.Add("60");
            values.Add("30");
            values.Add("40");
            values.Add("0.25");
            values.Add("0");
            values.Add("1");
            AddDataToTable(new SubcontractorConstantsTable(), values);
        }
        private static void AddToUserAdjustmentsTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("120");
            values.Add("110");
            values.Add("100");
            values.Add("90");
            values.Add("80");
            AddDataToTable(new UserAdjustmentsTable(), values);
        }
        private static void AddToNoteTable()
        {
            List<string> values = new List<string>();
            values.Add("50f3a707-fc1b-4eb3-9413-1dbde57b1d90");
            values.Add("Test Note");
            AddDataToTable(new NoteTable(), values);
        }
        private static void AddToExlusionTable()
        {
            List<string> values = new List<string>();
            values.Add("15692e12-e728-4f1b-b65c-de365e016e7a");
            values.Add("Test Exclusion");
            AddDataToTable(new ExclusionTable(), values);
        }
        private static void AddToScopeBranchTable()
        {
            List<string> values = new List<string>();
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            values.Add("Bid Scope Branch");
            values.Add("Bid Scope Branch Description");
            AddDataToTable(new ScopeBranchTable(), values);

            values = new List<string>();
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            values.Add("System Scope Branch");
            values.Add("System Scope Branch Description");
            AddDataToTable(new ScopeBranchTable(), values);

            values = new List<string>();
            values.Add("81adfc62-20ec-466f-a2a0-430e1223f64f");
            values.Add("Bid Child Branch");
            values.Add("Bid Child Branch Description");
            AddDataToTable(new ScopeBranchTable(), values);

            values = new List<string>();
            values.Add("542802f6-a7b1-4020-9be4-e58225c433a8");
            values.Add("System Child Branch");
            values.Add("System Child Branch Description");
            AddDataToTable(new ScopeBranchTable(), values);
        }
        private static void AddToSystemTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("Typical System");
            values.Add("Typical System Description");
            values.Add("1");
            values.Add("100");
            values.Add("1");
            AddDataToTable(new SystemTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("Instance System");
            values.Add("Instance System Description");
            values.Add("1");
            values.Add("100");
            values.Add("0");
            AddDataToTable(new SystemTable(), values);
        }
        private static void AddToEquipmentTable()
        {
            List<string> values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("Typical Equip");
            values.Add("Typical Equip Description");
            values.Add("1");
            values.Add("50");
            AddDataToTable(new EquipmentTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("Instance Equip");
            values.Add("Instance Equip Description");
            values.Add("1");
            values.Add("50");
            AddDataToTable(new EquipmentTable(), values);

            values = new List<string>();
            values.Add("1645886c-fce7-4380-a5c3-295f91961d16");
            values.Add("Template Equip");
            values.Add("Template Equip Description");
            values.Add("1");
            values.Add("25");
            AddDataToTable(new EquipmentTable(), values);
        }
        private static void AddToSubScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("Typical SS");
            values.Add("Typical SS Description");
            values.Add("1");
            AddDataToTable(new SubScopeTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("Instance SS");
            values.Add("Instance SS Description");
            values.Add("1");
            AddDataToTable(new SubScopeTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("Template SS");
            values.Add("Template SS Description");
            values.Add("1");
            AddDataToTable(new SubScopeTable(), values);

            values = new List<string>();
            values.Add("214dc8d1-22be-4fbf-8b6b-d66c21105f61");
            values.Add("Child SS");
            values.Add("Child SS Description");
            values.Add("1");
            AddDataToTable(new SubScopeTable(), values);
        }
        private static void AddToDeviceTable()
        {
            List<string> values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("Test Device");
            values.Add("Test Device Description");
            values.Add("123.45");
            AddDataToTable(new DeviceTable(), values);
        }
        private static void AddToPointTable()
        {
            List<string> values = new List<string>();
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            values.Add("Typical Point");
            values.Add("Typical Point Description");
            values.Add("1");
            values.Add("AI");
            AddDataToTable(new PointTable(), values);

            values = new List<string>();
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            values.Add("Instance Point");
            values.Add("Instance Point Description");
            values.Add("1");
            values.Add("AI");
            AddDataToTable(new PointTable(), values);

            values = new List<string>();
            values.Add("6776a30b-0325-42ad-8aa3-3c065b4bb908");
            values.Add("Child Point");
            values.Add("Child Point Description");
            values.Add("1");
            values.Add("BO");
            AddDataToTable(new PointTable(), values);
        }
        private static void AddToTagTable()
        {
            List<string> values = new List<string>();
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            values.Add("Test Tag");
            AddDataToTable(new TagTable(), values);
        }
        private static void AddToManufacturerTable()
        {
            List<string> values = new List<string>();
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            values.Add("Test Manufacturer");
            values.Add("0.5");
            AddDataToTable(new ManufacturerTable(), values);
        }
        private static void AddToDrawingTable()
        {

        }
        private static void AddToPageTable()
        {

        }
        private static void AddToLocationTable()
        {
            List<string> values = new List<string>();
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            values.Add("Test Location");
            AddDataToTable(new LocationTable(), values);
        }
        private static void AddToVisualScopeTable()
        {

        }
        private static void AddToConnectionTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            values.Add("Test Connection Type");
            values.Add("12.48");
            values.Add("84.21");
            AddDataToTable(new ConnectionTypeTable(), values);
        }
        private static void AddToConduitTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            values.Add("Test Conduit Type");
            values.Add("45.67");
            values.Add("76.54");
            AddDataToTable(new ConduitTypeTable(), values);
        }
        private static void AddToAssociatedCostTable()
        {
            List<string> values = new List<string>();
            values.Add("1c2a7631-9e3b-4006-ada7-12d6cee52f08");
            values.Add("Test TEC Associated Cost");
            values.Add("31");
            values.Add("13");
            values.Add("TEC");
            AddDataToTable(new AssociatedCostTable(), values);

            values = new List<string>();
            values.Add("63ed1eb7-c05b-440b-9e15-397f64ff05c7");
            values.Add("Test Electrical Associated Cost");
            values.Add("42");
            values.Add("24");
            values.Add("Electrical");
            AddDataToTable(new AssociatedCostTable(), values);

            values = new List<string>();
            values.Add("b7c01526-c195-442f-a1f1-28d07db61144");
            values.Add("Rated Cost");
            values.Add("10");
            values.Add("5");
            values.Add("Electrical");
            AddDataToTable(new AssociatedCostTable(), values);
        }
        private static void AddToSubScopeConnectionTable()
        {
            List<string> values = new List<string>();
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            values.Add("40");
            values.Add("20");
            AddDataToTable(new SubScopeConnectionTable(), values);

            values = new List<string>();
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            values.Add("50");
            values.Add("30");
            AddDataToTable(new SubScopeConnectionTable(), values);
        }
        private static void AddToNetworkConnectionTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("100");
            values.Add("80");
            values.Add("BACnetIP");
            AddDataToTable(new NetworkConnectionTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("90");
            values.Add("70");
            values.Add("BACnetIP");
            AddDataToTable(new NetworkConnectionTable(), values);

            values = new List<string>();
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            values.Add("80");
            values.Add("60");
            values.Add("BACnetIP");
            AddDataToTable(new NetworkConnectionTable(), values);

            values = new List<string>();
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            values.Add("70");
            values.Add("50");
            values.Add("BACnetIP");
            AddDataToTable(new NetworkConnectionTable(), values);
        }
        private static void AddToControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("Bid Controller");
            values.Add("Bid Controller Description");
            values.Add("1812");
            values.Add("Server");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("Typical Controller");
            values.Add("Typical Controller Description");
            values.Add("1776");
            values.Add("0");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("Instance Controller");
            values.Add("Instance Controller Description");
            values.Add("1776");
            values.Add("DDC");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            values.Add("Child Bid Controller");
            values.Add("");
            values.Add("0");
            values.Add("0");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            values.Add("Child Instance Controller");
            values.Add("");
            values.Add("0");
            values.Add("0");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            values.Add("Daisy 1");
            values.Add("");
            values.Add("0");
            values.Add("0");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            values.Add("Daisy 2");
            values.Add("");
            values.Add("0");
            values.Add("0");
            AddDataToTable(new ControllerTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("Child Typical Controller");
            values.Add("");
            values.Add("0");
            values.Add("0");
            AddDataToTable(new ControllerTable(), values);
        }
        private static void AddToMiscTable()
        {
            List<string> values = new List<string>();
            values.Add("5df99701-1d7b-4fbe-843d-40793f4145a8");
            values.Add("Bid Misc");
            values.Add("1298");
            values.Add("8921");
            values.Add("2");
            values.Add("Electrical");
            AddDataToTable(new MiscTable(), values);

            values = new List<string>();
            values.Add("e3ecee54-1f90-415a-b493-90a78f618476");
            values.Add("System Misc");
            values.Add("1492");
            values.Add("2941");
            values.Add("3");
            values.Add("TEC");
            AddDataToTable(new MiscTable(), values);
        }
        private static void AddToPanelTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            values.Add("Test Panel Type");
            values.Add("1324");
            values.Add("4231");
            AddDataToTable(new PanelTypeTable(), values);
        }
        private static void AddToPanelTable()
        {
            List<string> values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("Bid Panel");
            values.Add("Bid Panel Description");
            values.Add("1");
            AddDataToTable(new PanelTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("Typical Panel");
            values.Add("Typical Panel Description");
            values.Add("1");
            AddDataToTable(new PanelTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("Instance Panel");
            values.Add("Instance Panel Description");
            values.Add("1");
            AddDataToTable(new PanelTable(), values);
        }
        private static void AddToIOModuleTable()
        {
            List<string> values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("Test IO Module");
            values.Add("Test IO Module Description");
            values.Add("2233");
            values.Add("10");
            AddDataToTable(new IOModuleTable(), values);

        }
        private static void AddToIOTable()
        {
            List<string> values = new List<string>();
            values.Add("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            values.Add("BACnetIP");
            values.Add("2");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("fbae3851-3320-4e94-a674-ddec86bc4964");
            values.Add("BACnetIP");
            values.Add("2");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("434bc312-f933-40c8-b8bd-f4e22f19f606");
            values.Add("BACnetIP");
            values.Add("2");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("de2d29b7-c63d-4ddf-8b9c-987915e58cd2");
            values.Add("BACnetIP");
            values.Add("3");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("81827dfc-1375-476f-bfd8-290764403545");
            values.Add("BACnetIP");
            values.Add("3");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("6afb5c4d-4ee9-4c86-b37a-fa26a7be64b0");
            values.Add("BACnetIP");
            values.Add("3");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("7cb05a42-07fe-44f6-8f33-df0eab7416a5");
            values.Add("BACnetIP");
            values.Add("3");
            AddDataToTable(new IOTable(), values);

            values = new List<string>();
            values.Add("4ce8ee4b-03f3-4460-9387-b14ec5bdc5db");
            values.Add("BACnetIP");
            values.Add("3");
            AddDataToTable(new IOTable(), values);
        }
        #endregion

        #region Relationship Tables
        private static void AddToBidLaborTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("ab534ec6-73ec-4145-9c58-3abbbc9ae3d5");
            AddDataToTable(new BidLaborTable(), values);
        }
        private static void AddToBidBidParametersTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("655ed4a6-4ce4-431f-ae4b-7185e28d20ef");
            AddDataToTable(new BidBidParametersTable(), values);
        }
        private static void AddToBidScopeBranchTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            AddDataToTable(new BidScopeBranchTable(), values);
        }
        private static void AddToBidMiscTable()
        {
            List<string> values = new List<string>();
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("5df99701-1d7b-4fbe-843d-40793f4145a8");
            AddDataToTable(new BidMiscTable(), values);
        }
        private static void AddToControllerIOTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("fbae3851-3320-4e94-a674-ddec86bc4964");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("434bc312-f933-40c8-b8bd-f4e22f19f606");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            values.Add("de2d29b7-c63d-4ddf-8b9c-987915e58cd2");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            values.Add("81827dfc-1375-476f-bfd8-290764403545");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            values.Add("6afb5c4d-4ee9-4c86-b37a-fa26a7be64b0");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            values.Add("7cb05a42-07fe-44f6-8f33-df0eab7416a5");
            AddDataToTable(new ControllerIOTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("4ce8ee4b-03f3-4460-9387-b14ec5bdc5db");
            AddDataToTable(new ControllerIOTable(), values);
        }
        private static void AddToIOModuleManufacturerTable()
        {
            List<string> values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new IOModuleManufacturerTable(), values);
        }
        private static void AddToIOIOModuleTable()
        {
            List<string> values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("1f6049cc-4dd6-4b50-a9d5-045b629ae6fb");
            AddDataToTable(new IOIOModuleTable(), values);

            values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("fbae3851-3320-4e94-a674-ddec86bc4964");
            AddDataToTable(new IOIOModuleTable(), values);

            values = new List<string>();
            values.Add("b346378d-dc72-4dda-b275-bbe03022dd12");
            values.Add("434bc312-f933-40c8-b8bd-f4e22f19f606");
            AddDataToTable(new IOIOModuleTable(), values);
        }
        private static void AddToControllerConnectionTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            AddDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            AddDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            AddDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            AddDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            AddDataToTable(new ControllerConnectionTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            AddDataToTable(new ControllerConnectionTable(), values);
        }
        private static void AddToScopeBranchHierarchyTable()
        {
            List<string> values = new List<string>();
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            values.Add("81adfc62-20ec-466f-a2a0-430e1223f64f");
            AddDataToTable(new ScopeBranchHierarchyTable(), values);

            values = new List<string>();
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            values.Add("542802f6-a7b1-4020-9be4-e58225c433a8");
            AddDataToTable(new ScopeBranchHierarchyTable(), values);
        }
        private static void AddToBidSystemTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("d8788062-92d2-4889-b9f2-02a7a28aff05");
            values.Add("0");
            AddDataToTable(new BidSystemTable(), values);
        }
        private static void AddToSystemEquipmentTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("0");
            AddDataToTable(new SystemEquipmentTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("0");
            AddDataToTable(new SystemEquipmentTable(), values);
        }
        private static void AddToEquipmentSubScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("0");
            AddDataToTable(new EquipmentSubScopeTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("0");
            AddDataToTable(new EquipmentSubScopeTable(), values);

            values = new List<string>();
            values.Add("1645886c-fce7-4380-a5c3-295f91961d16");
            values.Add("214dc8d1-22be-4fbf-8b6b-d66c21105f61");
            values.Add("0");
            AddDataToTable(new EquipmentSubScopeTable(), values);
        }
        private static void AddToSubScopeDeviceTable()
        {
            List<string> values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("2");
            values.Add("0");
            AddDataToTable(new SubScopeDeviceTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("2");
            values.Add("0");
            AddDataToTable(new SubScopeDeviceTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("3");
            values.Add("0");
            AddDataToTable(new SubScopeDeviceTable(), values);
        }
        private static void AddToSubScopePointTable()
        {
            List<string> values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            AddDataToTable(new SubScopePointTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            AddDataToTable(new SubScopePointTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("6776a30b-0325-42ad-8aa3-3c065b4bb908");
            AddDataToTable(new SubScopePointTable(), values);
        }
        private static void AddToScopeTagTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("5df99701-1d7b-4fbe-843d-40793f4145a8");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("25e815fa-4ac7-4b69-9640-5ae220f0cd40");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("e3ecee54-1f90-415a-b493-90a78f618476");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("81adfc62-20ec-466f-a2a0-430e1223f64f");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("542802f6-a7b1-4020-9be4-e58225c433a8");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("1645886c-fce7-4380-a5c3-295f91961d16");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);

            values = new List<string>();
            values.Add("3ebdfd64-5249-4332-a832-ff3cc0cdb309");
            values.Add("09fd531f-94f9-48ee-8d16-00e80c1d58b9");
            AddDataToTable(new ScopeTagTable(), values);
        }
        private static void AddToDeviceManufacturerTable()
        {
            List<string> values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new DeviceManufacturerTable(), values);
        }
        private static void AddToDeviceConnectionTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("95135fdf-7565-4d22-b9e4-1f177febae15");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            values.Add("2");
            AddDataToTable(new DeviceConnectionTypeTable(), values);
        }
        private static void AddToDrawingPageTable()
        {

        }
        private static void AddToPageVisualScopeTable()
        {

        }
        private static void AddToVisualScopeScopeTable()
        {

        }
        private static void AddToLocationScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            AddDataToTable(new LocationScopeTable(), values);

            values = new List<string>();
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            AddDataToTable(new LocationScopeTable(), values);

            values = new List<string>();
            values.Add("4175d04b-82b1-486b-b742-b2cc875405cb");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            AddDataToTable(new LocationScopeTable(), values);
        }
        private static void AddToScopeAssociatedCostTable()
        {
            string tecCostGuid = "1c2a7631-9e3b-4006-ada7-12d6cee52f08";
            string electricalCostGuid = "63ed1eb7-c05b-440b-9e15-397f64ff05c7";
            string scopeGuid = "";

            List<string> values = new List<string>();
            scopeGuid = "98e6bc3e-31dc-4394-8b54-9ca53c193f46";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "a8cdd31c-e690-4eaa-81ea-602c72904391";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "ebdbcc85-10f4-46b3-99e7-d896679f874a";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "fbe0a143-e7cd-4580-a1c4-26eff0cd55a6";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "03a16819-9205-4e65-a16b-96616309f171";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "1bb86714-2512-4fdd-a80f-46969753d8a0";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "e7695d68-d79f-44a2-92f5-b303436186af";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "ba2e71d4-a2b9-471a-9229-9fbad7432bf7";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "94726d87-b468-46a8-9421-3ff9725d5239";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "e60437bc-09a1-47eb-9fd5-78711d942a12";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "f22913a6-e348-4a77-821f-80447621c6e0";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "10b07f6c-4374-49fc-ba6f-84db65b61ffa";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "95135fdf-7565-4d22-b9e4-1f177febae15";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "f38867c8-3846-461f-a6fa-c941aeb723c7";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "8d442906-efa2-49a0-ad21-f6b27852c9ef";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            scopeGuid = "04e3204c-b35f-4e1a-8a01-db07f7eb055e";
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            //Template Equipment
            scopeGuid = "1645886c-fce7-4380-a5c3-295f91961d16";
            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            //Template SubScope
            scopeGuid = "3ebdfd64-5249-4332-a832-ff3cc0cdb309";
            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(tecCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

            values = new List<string>();
            values.Add(scopeGuid);
            values.Add(electricalCostGuid);
            values.Add("1");
            AddDataToTable(new ScopeAssociatedCostTable(), values);

        }
        private static void AddElectricalComponentRatedCostTable()
        {
            List<string> values = new List<string>();
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            values.Add("b7c01526-c195-442f-a1f1-28d07db61144");
            values.Add("1");
            AddDataToTable(new ElectricalMaterialRatedCostTable(), values);

            values = new List<string>();
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            values.Add("b7c01526-c195-442f-a1f1-28d07db61144");
            values.Add("1");
            AddDataToTable(new ElectricalMaterialRatedCostTable(), values);

        }
        private static void AddToControllerManufacturerTable()
        {
            List<string> values = new List<string>();
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("90cd6eae-f7a3-4296-a9eb-b810a417766d");
            AddDataToTable(new ControllerManufacturerTable(), values);
        }
        private static void AddToConnectionConduitTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            AddDataToTable(new ConnectionConduitTypeTable(), values);

            values = new List<string>();
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            AddDataToTable(new ConnectionConduitTypeTable(), values);

            values = new List<string>();
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            values.Add("8d442906-efa2-49a0-ad21-f6b27852c9ef");
            AddDataToTable(new ConnectionConduitTypeTable(), values);
        }
        private static void AddToNetworkConnectionConnectionTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            AddDataToTable(new NetworkConnectionConnectionTypeTable(), values);

            values = new List<string>();
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            AddDataToTable(new NetworkConnectionConnectionTypeTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            AddDataToTable(new NetworkConnectionConnectionTypeTable(), values);

            values = new List<string>();
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            values.Add("f38867c8-3846-461f-a6fa-c941aeb723c7");
            AddDataToTable(new NetworkConnectionConnectionTypeTable(), values);
        }
        private static void AddToNetworkConnectionControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("4f93907a-9aab-4ed5-8e55-43aab2af5ef8");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            AddDataToTable(new NetworkConnectionControllerTable(), values);

            values = new List<string>();
            values.Add("6aca8c22-5115-4534-a5b1-698b7e42d6c2");
            values.Add("973e6100-31f7-40b0-bfe7-9d64630c1c56");
            AddDataToTable(new NetworkConnectionControllerTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("bf17527a-18ba-4765-a01e-8ab8de5664a3");
            AddDataToTable(new NetworkConnectionControllerTable(), values);

            values = new List<string>();
            values.Add("99aea45e-ebeb-4c1a-8407-1d1a3540ceeb");
            values.Add("7b6825df-57da-458a-a859-a9459c15907b");
            AddDataToTable(new NetworkConnectionControllerTable(), values);

            values = new List<string>();
            values.Add("e503fdd4-f299-4618-8d54-6751c3b2bc25");
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            AddDataToTable(new NetworkConnectionControllerTable(), values);
        }
        private static void AddToSubScopeConnectionChildrenTable()
        {
            List<string> values = new List<string>();
            values.Add("5723e279-ac5c-4ee0-ae01-494a0c524b5c");
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            AddDataToTable(new SubScopeConnectionChildrenTable(), values);

            values = new List<string>();
            values.Add("560ffd84-444d-4611-a346-266074f62f6f");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            AddDataToTable(new SubScopeConnectionChildrenTable(), values);
        }
        private static void AddToPanelPanelTypeTable()
        {
            List<string> values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            AddDataToTable(new PanelPanelTypeTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            AddDataToTable(new PanelPanelTypeTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("04e3204c-b35f-4e1a-8a01-db07f7eb055e");
            AddDataToTable(new PanelPanelTypeTable(), values);
        }
        private static void AddToPanelControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("a8cdd31c-e690-4eaa-81ea-602c72904391");
            values.Add("98e6bc3e-31dc-4394-8b54-9ca53c193f46");
            AddDataToTable(new PanelControllerTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            AddDataToTable(new PanelControllerTable(), values);

            values = new List<string>();
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            AddDataToTable(new PanelControllerTable(), values);
        }
        private static void AddToSystemControllerTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            AddDataToTable(new SystemControllerTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            AddDataToTable(new SystemControllerTable(), values);

            values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            AddDataToTable(new SystemControllerTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            AddDataToTable(new SystemControllerTable(), values);
        }
        private static void AddToSystemPanelTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            AddDataToTable(new SystemPanelTable(), values);

            values = new List<string>();
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            AddDataToTable(new SystemPanelTable(), values);
        }
        private static void AddToSystemScopeBranchTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("814710f1-f2dd-4ae6-9bc4-9279288e4994");
            AddDataToTable(new SystemScopeBranchTable(), values);
        }
        private static void AddToSystemHierarchyTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("ba2e71d4-a2b9-471a-9229-9fbad7432bf7");
            AddDataToTable(new SystemHierarchyTable(), values);
        }
        private static void AddToSystemMiscTable()
        {
            List<string> values = new List<string>();
            values.Add("ebdbcc85-10f4-46b3-99e7-d896679f874a");
            values.Add("e3ecee54-1f90-415a-b493-90a78f618476");
            AddDataToTable(new SystemMiscTable(), values);
        }
        private static void AddToCharacteristicScopeInstanceScopeTable()
        {
            List<string> values = new List<string>();
            values.Add("8a9bcc02-6ae2-4ac9-bbe1-e33d9a590b0e");
            values.Add("cdd9d7f7-ff3e-44ff-990f-c1b721e0ff8d");
            AddDataToTable(new CharacteristicScopeInstanceScopeTable(), values);

            values = new List<string>();
            values.Add("fbe0a143-e7cd-4580-a1c4-26eff0cd55a6");
            values.Add("94726d87-b468-46a8-9421-3ff9725d5239");
            AddDataToTable(new CharacteristicScopeInstanceScopeTable(), values);

            values = new List<string>();
            values.Add("03a16819-9205-4e65-a16b-96616309f171");
            values.Add("e60437bc-09a1-47eb-9fd5-78711d942a12");
            AddDataToTable(new CharacteristicScopeInstanceScopeTable(), values);

            values = new List<string>();
            values.Add("1bb86714-2512-4fdd-a80f-46969753d8a0");
            values.Add("f22913a6-e348-4a77-821f-80447621c6e0");
            AddDataToTable(new CharacteristicScopeInstanceScopeTable(), values);

            values = new List<string>();
            values.Add("e7695d68-d79f-44a2-92f5-b303436186af");
            values.Add("10b07f6c-4374-49fc-ba6f-84db65b61ffa");
            AddDataToTable(new CharacteristicScopeInstanceScopeTable(), values);

            values = new List<string>();
            values.Add("95032348-c661-470f-9bea-47dd750a47a5");
            values.Add("ec965fe3-b1f7-4125-a545-ec47cc1e671b");
            AddDataToTable(new CharacteristicScopeInstanceScopeTable(), values);
        }
        #endregion
        
        private static void CreateBidDB()
        {
            foreach(Object table in AllBidTables_1_6.Tables)
            {
                createTableFromDefinition(table as TableBase);
            }
        } 
        private static void CreateTemplateDB()
        {
            foreach (Object table in AllTemplateTables_1_6.Tables)
            {
                createTableFromDefinition(table as TableBase);
            }
        }

        static private void createTableFromDefinition(TableBase table)
        {
            var tableInfo = new TableInfo(table);
            string tableName = tableInfo.Name;
            List<TableField> primaryKey = tableInfo.PrimaryFields;
            List<TableField> fields = tableInfo.Fields;

            string createString = "CREATE TABLE '" + tableName + "' (";
            foreach (TableField field in fields)
            {
                createString += "'" + field.Name + "' " + field.FieldType;
                if (fields.IndexOf(field) < (fields.Count - 1))
                { createString += ", "; }
            }
            if (primaryKey.Count != 0)
            { createString += ", PRIMARY KEY("; }
            foreach (TableField pk in primaryKey)
            {
                createString += "'" + pk.Name + "' ";
                if (primaryKey.IndexOf(pk) < (primaryKey.Count - 1))
                { createString += ", "; }
                else
                { createString += ")"; }
            }
            createString += ")";
            SQLiteDB.NonQueryCommand(createString);
        }
    }
}
