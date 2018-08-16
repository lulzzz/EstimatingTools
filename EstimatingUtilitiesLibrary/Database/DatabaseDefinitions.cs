﻿using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EstimatingUtilitiesLibrary.Database
{
    #region Table Definitions
    #region Object Tables
    internal class MetadataTable : TableBase
    {
        public static string TableName = "Metadata";
        public static Type HelperType = typeof(HelperProperties);

        public static TableField Version = new TableField("Version", "TEXT", HelperType.GetProperty("DBVersion"));

        private List<TableField> primaryKeys = new List<TableField>() {
            Version
        };
        private List<TableField> fields = new List<TableField>() {
            Version
        };

        public override List<TableField> PrimaryKeys
        {
            get { return primaryKeys; }
        }
        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return new List<Type>(); } }
        public override List<string> PropertyNames { get { return new List<string>(); } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class BidInfoTable : TableBase
    {
        #region static
        public static string TableName = "BidInfo";
        public static Type ObjectType = typeof(TECBid);

        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Number = new TableField("Number", "TEXT", ObjectType.GetProperty("BidNumber"));
        public static TableField DueDate = new TableField("DueDate", "TEXT", ObjectType.GetProperty("DueDate"));
        public static TableField Salesperson = new TableField("Salesperson", "TEXT", ObjectType.GetProperty("Salesperson"));
        public static TableField Estimator = new TableField("Estimator", "TEXT", ObjectType.GetProperty("Estimator"));
        public static TableField Duration = new TableField("Duration", "REAL", ObjectType.GetProperty("Duration"), defaultValue: "0");
        #endregion

        private List<TableField> fields = new List<TableField>()
        {
            Name,
            ID,
            Number,
            DueDate,
            Salesperson,
            Estimator,
            Duration
        };
        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>() {
            ObjectType
        };
        private List<string> propertyNames = new List<string>();

        public override string NameString { get { return TableName; } }
        public override List<TableField> Fields { get { return fields; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }

    }
    internal class TemplatesInfoTable : TableBase
    {
        public static string TableName = "TemplatesInfo";
        public static Type ObjectType = typeof(TECTemplates);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<string> propertyNames = new List<string>();

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScopeTemplatesTable : TableBase
    {
        #region static
        public static string TableName = "ScopeTemplates";
        public static Type ObjectType = typeof(TECScopeTemplates);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        #endregion

        private List<TableField> fields = new List<TableField>()
        {
            ID
        };
        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>() {
            ObjectType
        };
        private List<string> propertyNames = new List<string>()
        {
            "Templates"
        };

        public override string NameString { get { return TableName; } }
        public override List<TableField> Fields { get { return fields; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }

    }

    internal class ParametersTable : TableBase
    {
        public static string TableName = "Parameters";
        public static Type ParameterType = typeof(TECParameters);

        public static TableField ID = new TableField("ID", "TEXT", ParameterType.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ParameterType.GetProperty("Label"), defaultValue: "Parameters");

        public static TableField Escalation = new TableField("Escalation", "REAL", ParameterType.GetProperty("Escalation"), defaultValue: "0.0");
        public static TableField Markup = new TableField("Markup", "REAL", ParameterType.GetProperty("Markup"), defaultValue: "20.0");
        public static TableField SubcontractorEscalation = new TableField("SubcontractorEscalation", "REAL", ParameterType.GetProperty("SubcontractorEscalation"), defaultValue: "0.0");
        public static TableField Warranty = new TableField("Warranty", "REAL", ParameterType.GetProperty("Warranty"), defaultValue: "3.0");
        public static TableField Shipping = new TableField("Shipping", "REAL", ParameterType.GetProperty("Shipping"), defaultValue: "3.0");
        public static TableField Tax = new TableField("Tax", "REAL", ParameterType.GetProperty("Tax"), defaultValue: "8.75");
        public static TableField SubcontractorWarranty = new TableField("SubcontractorWarranty", "REAL", ParameterType.GetProperty("SubcontractorWarranty"), defaultValue: "3.0");
        public static TableField SubcontractorShipping = new TableField("SubcontractorShipping", "REAL", ParameterType.GetProperty("SubcontractorShipping"), defaultValue: "3.0");
        public static TableField BondRate = new TableField("BondRate", "REAL", ParameterType.GetProperty("BondRate"), defaultValue: "1.5");
        public static TableField OvertimeRatio = new TableField("OvertimeRatio", "REAL", ParameterType.GetProperty("OvertimeRatio"), defaultValue: "1.5");

        public static TableField IsTaxExempt = new TableField("IsTaxExempt", "INTEGER", ParameterType.GetProperty("IsTaxExempt"), defaultValue: "0");
        public static TableField RequiresBond = new TableField("RequiresBond", "INTEGER", ParameterType.GetProperty("RequiresBond"), defaultValue: "0");
        public static TableField RequiresWrapUp = new TableField("RequiresWrapUp", "INTEGER", ParameterType.GetProperty("RequiresWrapUp"), defaultValue: "0");

        #region Labor
        public static TableField DesiredConfidence = new TableField("DesiredConfidence", "TEXT", ParameterType.GetProperty("DesiredConfidence"), defaultValue: "NinetyFive");

        public static TableField PMCoefStdError = new TableField("PMCoefStdError", "REAL", ParameterType.GetProperty("PMCoefStdError"), defaultValue: "1.0");
        public static TableField PMCoef = new TableField("PMCoef", "REAL", ParameterType.GetProperty("PMCoef"), defaultValue: "0.0");
        public static TableField PMRate = new TableField("PMRate", "REAL", ParameterType.GetProperty("PMRate"), defaultValue: "0.0");

        public static TableField ENGCoefStdError = new TableField("ENGCoefStdError", "REAL", ParameterType.GetProperty("ENGCoefStdError"), defaultValue: "1.0");
        public static TableField ENGCoef = new TableField("ENGCoef", "REAL", ParameterType.GetProperty("ENGCoef"), defaultValue: "0.0");
        public static TableField ENGRate = new TableField("ENGRate", "REAL", ParameterType.GetProperty("ENGRate"), defaultValue: "0.0");

        public static TableField CommCoefStdError = new TableField("CommCoefStdError", "REAL", ParameterType.GetProperty("CommCoefStdError"), defaultValue: "1.0");
        public static TableField CommCoef = new TableField("CommCoef", "REAL", ParameterType.GetProperty("CommCoef"), defaultValue: "0.0");
        public static TableField CommRate = new TableField("CommRate", "REAL", ParameterType.GetProperty("CommRate"), defaultValue: "0.0");

        public static TableField SoftCoefStdError = new TableField("SoftCoefStdError", "REAL", ParameterType.GetProperty("SoftCoefStdError"), defaultValue: "1.0");
        public static TableField SoftCoef = new TableField("SoftCoef", "REAL", ParameterType.GetProperty("SoftCoef"), defaultValue: "0.0");
        public static TableField SoftRate = new TableField("SoftRate", "REAL", ParameterType.GetProperty("SoftRate"), defaultValue: "0.0");

        public static TableField GraphCoefStdError = new TableField("GraphCoefStdError", "REAL", ParameterType.GetProperty("GraphCoefStdError"), defaultValue: "1.0");
        public static TableField GraphCoef = new TableField("GraphCoef", "REAL", ParameterType.GetProperty("GraphCoef"), defaultValue: "0.0");
        public static TableField GraphRate = new TableField("GraphRate", "REAL", ParameterType.GetProperty("GraphRate"), defaultValue: "0.0");
        #endregion

        #region SubContractor
        public static TableField ElectricalRate = new TableField("ElectricalRate", "REAL", ParameterType.GetProperty("ElectricalRate"), defaultValue: "0.0");
        public static TableField ElectricalSuperRate = new TableField("ElectricalSuperRate", "REAL", ParameterType.GetProperty("ElectricalSuperRate"), defaultValue: "0.0");
        public static TableField ElectricalNonUnionRate = new TableField("ElectricalNonUnionRate", "REAL", ParameterType.GetProperty("ElectricalNonUnionRate"), defaultValue: "0.0");
        public static TableField ElectricalSuperNonUnionRate = new TableField("ElectricalSuperNonUnionRate", "REAL", ParameterType.GetProperty("ElectricalSuperNonUnionRate"), defaultValue: "0.0");
        public static TableField ElectricalSuperRatio = new TableField("ElectricalSuperRatio", "REAL", ParameterType.GetProperty("ElectricalSuperRatio"), defaultValue: "1.0");

        public static TableField ElectricalIsOnOvertime = new TableField("ElectricalIsOnOvertime", "INTEGER", ParameterType.GetProperty("ElectricalIsOnOvertime"), defaultValue: "0");
        public static TableField ElectricalIsUnion = new TableField("ElectricalIsUnion", "INTEGER", ParameterType.GetProperty("ElectricalIsUnion"), defaultValue: "1");
        #endregion

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ParameterType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Label,
            Escalation,
            Markup,
            SubcontractorEscalation,
            Warranty,
            Shipping,
            Tax,
            SubcontractorWarranty,
            SubcontractorShipping,
            BondRate,
            OvertimeRatio,
            IsTaxExempt,
            RequiresBond,
            RequiresWrapUp,
            DesiredConfidence,
            PMCoef,
            PMCoefStdError,
            PMRate,
            ENGCoef,
            ENGCoefStdError,
            ENGRate,
            CommCoef,
            CommCoefStdError,
            CommRate,
            SoftCoef,
            SoftCoefStdError,
            SoftRate,
            GraphCoef,
            GraphCoefStdError,
            GraphRate,
            ElectricalRate,
            ElectricalSuperRate,
            ElectricalNonUnionRate,
            ElectricalSuperNonUnionRate,
            ElectricalSuperRatio,
            ElectricalIsOnOvertime,
            ElectricalIsUnion

        };
        private List<string> propertyNames = new List<string>()
        {
            "Parameters"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ExtraLaborTable : TableBase
    {
        public static string TableName = "ExtraLabor";
        public static Type ObjectType =  typeof(TECExtraLabor);

        public static TableField ID = new TableField("BidID", "TEXT", ObjectType.GetProperty("Guid"));

        public static TableField PMExtraHours = new TableField("PMExtraHours", "REAL", ObjectType.GetProperty("PMExtraHours"));
        public static TableField ENGExtraHours = new TableField("ENGExtraHours", "REAL", ObjectType.GetProperty("ENGExtraHours"));
        public static TableField CommExtraHours = new TableField("CommExtraHours", "REAL", ObjectType.GetProperty("CommExtraHours"));
        public static TableField SoftExtraHours = new TableField("SoftExtraHours", "REAL", ObjectType.GetProperty("SoftExtraHours"));
        public static TableField GraphExtraHours = new TableField("GraphExtraHours", "REAL", ObjectType.GetProperty("GraphExtraHours"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };

        private List<TableField> fields = new List<TableField>()
        {
            ID,
            PMExtraHours,
            ENGExtraHours,
            CommExtraHours,
            SoftExtraHours,
            GraphExtraHours
        };
        private List<string> propertyNames = new List<string>()
        {
            "ExtraLabor"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class NoteTable : TableBase
    {
        public static string TableName = "Note";
        public static Type ObjectType = typeof(TECLabeled);
        
        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField NoteText = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            NoteText
        };
        private List<string> propertyNames = new List<string>()
        {
            "Notes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ExclusionTable : TableBase
    {
        public static string TableName = "Exclusion";
        public static Type ObjectType = typeof(TECLabeled);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ExclusionText = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            ExclusionText
        };
        private List<string> propertyNames = new List<string>()
        {
            "Exclusions"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ScopeBranchTable : TableBase
    {
        public static string TableName = "ScopeBranch";
        public static Type ObjectType = typeof(TECScopeBranch);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Label
        };
        private List<string> propertyNames = new List<string>()
        {
            "Branches",
            "ScopeBranches",
            "ScopeTree"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SystemTable : TableBase
    {
        public static string TableName = "System";
        public static Type ObjectType = typeof(TECSystem);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description

        };
        private List<string> propertyNames = new List<string>()
        {
            "Systems",
            "Instances",
            "SystemTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class EquipmentTable : TableBase
    {
        public static string TableName = "Equipment";
        public static Type ObjectType = typeof(TECEquipment);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Equipment",
            "EquipmentTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopeTable : TableBase
    {
        public static string TableName = "SubScope";
        public static Type ObjectType = typeof(TECSubScope);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "SubScope",
            "SubScopeTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class PointTable : TableBase
    {
        public static string TableName = "Point";
        public static Type ObjectType = typeof(TECPoint);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Label"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Quantity,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "Points"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class TagTable : TableBase
    {
        public static string TableName = "Tag";
        public static Type ObjectType = typeof(TECTag);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TagString = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>()
        { ID };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            TagString
        };
        private List<string> propertyNames = new List<string>()
        {
            "Tags"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ManufacturerTable : TableBase
    {
        public static string TableName = "Manufacturer";
        public static Type ObjectType = typeof(TECManufacturer);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Label"));
        public static TableField Multiplier = new TableField("Multiplier", "REAL", ObjectType.GetProperty("Multiplier"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Multiplier
        };
        private List<string> propertyNames = new List<string>()
        {
            "Manufacturers"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class LocationTable : TableBase
    {
        public static string TableName = "Location";
        public static Type ObjectType = typeof(TECLocation);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"), defaultValue: ""); 
        public static TableField Label = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"), defaultValue: "");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Label
        };
        private List<string> propertyNames = new List<string>()
        {
            "Locations"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopeConnectionTable : TableBase
    {
        public static string TableName = "SubScopeConnection";
        public static Type ObjectType = typeof(TECHardwiredConnection);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.GetProperty("ConduitLength"));
        public static TableField IsPlenum = new TableField("IsPlenum", "INTEGER", ObjectType.GetProperty("IsPlenum"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Length,
            ConduitLength,
            IsPlenum
        };
        private List<string> propertyNames = new List<string>()
        {
            "ChildrenConnections"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class NetworkConnectionTable : TableBase
    {
        public static string TableName = "NetworkConnection";
        public static Type ObjectType = typeof(TECNetworkConnection);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.GetProperty("ConduitLength"));
        public static TableField IsPlenum = new TableField("IsPlenum", "INTEGER", ObjectType.GetProperty("IsPlenum"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Length,
            ConduitLength,
            IsPlenum
        };
        private List<string> propertyNames = new List<string>()
        {
            "ChildrenConnections"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class InterlockConnectionTable : TableBase
    {
        public static string TableName = "InterlockConnection";
        public static Type ObjectType = typeof(TECInterlockConnection);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Length = new TableField("Length", "REAL", ObjectType.GetProperty("Length"));
        public static TableField ConduitLength = new TableField("ConduitLength", "REAL", ObjectType.GetProperty("ConduitLength"));
        public static TableField IsPlenum = new TableField("IsPlenum", "INTEGER", ObjectType.GetProperty("IsPlenum"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Length,
            ConduitLength,
            IsPlenum
        };
        private List<string> propertyNames = new List<string>()
        {
            "Interlocks"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ProvidedControllerTable : TableBase
    {
        public static string TableName = "ProvidedController";
        public static Type ObjectType = typeof(TECProvidedController);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers",
            "ControllerTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class FBOControllerTable : TableBase
    {
        public static string TableName = "FBOController";
        public static Type ObjectType = typeof(TECFBOController);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers",
            "ControllerTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelTable : TableBase
    {
        public static string TableName = "Panel";
        public static Type ObjectType = typeof(TECPanel);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description
        };
        private List<string> propertyNames = new List<string>()
        {
            "Panels",
            "PanelTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class IOTable : TableBase
    {
        public static string TableName = "IO";
        public static Type IOObjectType = typeof(TECIO);

        public static TableField ID = new TableField("ID", "TEXT", IOObjectType.GetProperty("Guid"));
        public static TableField IOType = new TableField("Type", "TEXT", IOObjectType.GetProperty("Type"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", IOObjectType.GetProperty("Quantity"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            IOObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            IOType,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "IO"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScheduleTable : TableBase
    {
        public static string TableName = "Schedule";
        public static Type ScheduleType = typeof(TECSchedule);

        public static TableField ID = new TableField("ID", "TEXT", ScheduleType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ScheduleType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Schedule"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScheduleTableTable : TableBase
    {
        public static string TableName = "ScheduleTable";
        public static Type ScheduleType = typeof(TECScheduleTable);

        public static TableField ID = new TableField("ID", "TEXT", ScheduleType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ScheduleType.GetProperty("Name"));
        
        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ScheduleType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name
        };
        private List<string> propertyNames = new List<string>()
        {
            "Tables"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScheduleItemTable : TableBase
    {
        public static string TableName = "ScheduleItem";
        public static Type ScheduleType = typeof(TECScheduleItem);

        public static TableField ID = new TableField("ID", "TEXT", ScheduleType.GetProperty("Guid"));
        public static TableField Tag = new TableField("Tag", "TEXT", ScheduleType.GetProperty("Tag"));
        public static TableField Service = new TableField("Service", "TEXT", ScheduleType.GetProperty("Service"));
        
        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ScheduleType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Tag,
            Service
        };
        private List<string> propertyNames = new List<string>()
        {
            "Items"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ProposalItemTable : TableBase
    {
        public static string TableName = "ProposalItem";
        public static Type ProposalType = typeof(TECProposalItem);

        public static TableField ID = new TableField("ID", "TEXT", ProposalType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ProposalType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ProposalItems"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    #region ICost
    internal class MiscTable : TableBase
    {
        public static string TableName = "Misc";
        public static Type ObjectType = typeof(TECMisc);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", ObjectType.GetProperty("Quantity"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor,
            Quantity,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCosts",
            "MiscCostTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class AssociatedCostTable : TableBase
    {
        public static string TableName = "AssociatedCost";
        public static Type ObjectType = typeof(TECAssociatedCost);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor,
            Type
        };
        private List<string> propertyNames = new List<string>()
        {
            "AssociatedCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ConnectionTypeTable : TableBase
    {
        public static string TableName = "ConnectionType";
        public static Type ObjectType = typeof(TECConnectionType);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField PlenumCost = new TableField("PlenumCost", "REAL", ObjectType.GetProperty("PlenumCost"), defaultValue: "0.0");
        public static TableField PlenumLabor = new TableField("PlenumLabor", "REAL", ObjectType.GetProperty("PlenumLabor"), defaultValue: "0.0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor,
            PlenumCost,
            PlenumLabor
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ConduitTypeTable : TableBase
    {
        public static string TableName = "ConduitType";
        public static Type ObjectType = typeof(TECElectricalMaterial);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Cost = new TableField("Cost", "REAL", ObjectType.GetProperty("Cost"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Cost,
            Labor
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConduitTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    
    #region TECHardware
    internal class DeviceTable : TableBase
    {
        public static string TableName = "Device";
        public static Type ObjectType = typeof(TECDevice);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"), defaultValue: "0");
        public static TableField RequireQuote = new TableField("RequireQuote", "INTEGER", ObjectType.GetProperty("RequireQuote"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            RequireQuote
        };
        private List<string> propertyNames = new List<string>()
        {
            "Devices"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ValveTable : TableBase
    {
        public static string TableName = "Valve";
        public static Type ObjectType = typeof(TECValve);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"));
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));
        public static TableField Cv = new TableField("Cv", "Real", ObjectType.GetProperty("Cv"));
        public static TableField Size = new TableField("Size", "Real", ObjectType.GetProperty("Size"));
        public static TableField Style = new TableField("Style", "TEXT", ObjectType.GetProperty("Style"));
        public static TableField PressureRating = new TableField("PressureRating", "Real", ObjectType.GetProperty("PressureRating"), defaultValue: "0");
        public static TableField RequireQuote = new TableField("RequireQuote", "INTEGER", ObjectType.GetProperty("RequireQuote"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            Type,
            Cv,
            Size,
            Style,
            PressureRating,
            RequireQuote
        };
        private List<string> propertyNames = new List<string>()
        {
            "Valves"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelTypeTable : TableBase
    {
        public static string TableName = "PanelType";
        public static Type ObjectType = typeof(TECPanelType);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"), defaultValue: "");
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"), defaultValue: "0");
        public static TableField RequireQuote = new TableField("RequireQuote", "INTEGER", ObjectType.GetProperty("RequireQuote"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            RequireQuote
        };
        private List<string> propertyNames = new List<string>()
        {
            "PanelTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerTypeTable : TableBase
    {
        public static string TableName = "ControllerType";
        public static Type ObjectType = typeof(TECControllerType);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ObjectType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", ObjectType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", ObjectType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", ObjectType.GetProperty("Labor"), defaultValue: "0");
        public static TableField Type = new TableField("Type", "TEXT", ObjectType.GetProperty("Type"));
        public static TableField RequireQuote = new TableField("RequireQuote", "INTEGER", ObjectType.GetProperty("RequireQuote"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            Type,
            RequireQuote
        };
        private List<string> propertyNames = new List<string>()
        {
            "ControllerTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class IOModuleTable : TableBase
    {
        public static string TableName = "IOModule";
        public static Type IOModuleType = typeof(TECIOModule);

        public static TableField ID = new TableField("ID", "TEXT", IOModuleType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", IOModuleType.GetProperty("Name"));
        public static TableField Description = new TableField("Description", "TEXT", IOModuleType.GetProperty("Description"));
        public static TableField Price = new TableField("Price", "REAL", IOModuleType.GetProperty("Price"));
        public static TableField Labor = new TableField("Labor", "REAL", IOModuleType.GetProperty("Labor"), defaultValue: "0");
        public static TableField RequireQuote = new TableField("RequireQuote", "INTEGER", IOModuleType.GetProperty("RequireQuote"), defaultValue: "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };

        private List<Type> types = new List<Type>()
        {
            IOModuleType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name,
            Description,
            Price,
            Labor,
            RequireQuote
        };
        private List<string> propertyNames = new List<string>()
        {
            "IOModules"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }

    #endregion
    #endregion
    
    internal class InternalNoteTable : TableBase
    {
        public static string TableName = "InternalNote";
        public static Type NoteType = typeof(TECInternalNote);

        public static TableField ID = new TableField("ID", "TEXT", NoteType.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", NoteType.GetProperty("Label"));
        public static TableField Body = new TableField("Body", "TEXT", NoteType.GetProperty("Body"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            NoteType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Label,
            Body
        };
        private List<string> propertyNames = new List<string>()
        {
            "InternalNotes"
        };
        
        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class DistributionContactTable : TableBase
    {
        public static string TableName = "DistributionContact";
        public static Type ContactType = typeof(TECDistributionContact);

        public static TableField ID = new TableField("ID", "TEXT", ContactType.GetProperty("Guid"));
        public static TableField Name = new TableField("Name", "TEXT", ContactType.GetProperty("Name"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ID
        };
        private List<Type> types = new List<Type>()
        {
            ContactType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Name
        };
        private List<string> propertyNames = new List<string>()
        {
            "DistributionList"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class ProtocolTable : TableBase
    {
        public static string TableName = "Protocol";
        public static Type ObjectType = typeof(TECProtocol);

        public static TableField ID = new TableField("ID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField Label = new TableField("Label", "TEXT", ObjectType.GetProperty("Label"));

        private List<TableField> primaryKeys = new List<TableField>()
        { ID };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ID,
            Label
        };
        private List<string> propertyNames = new List<string>()
        {
            "Protocols"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    #endregion

    #region Relationship Tables
    internal class BidScopeBranchTable : TableBase
    {
        public static string TableName = "BidScopeBranch";
        public static Type ObjectType = typeof(TECBid);
        public static Type ReferenceType = typeof(TECScopeBranch);

        public static TableField BidID = new TableField("BidID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ScopeBranchID = new TableField("ScopeBranchID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };

        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            ScopeBranchID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ScopeTree"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidMiscTable : TableBase
    {
        public static string TableName = "BidMisc";
        public static Type BidType = typeof(TECBid);
        public static Type CostType = typeof(TECMisc);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MiscID", "TEXT", CostType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "MiscCosts", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            MiscID
        };

        private List<Type> types = new List<Type>()
        {
            BidType,
            CostType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            MiscID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCosts"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidLocationTable : TableBase
    {
        public static string TableName = "BidLocation";
        public static Type BidType = typeof(TECBid);
        public static Type LocationType = typeof(TECLocation);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField LocationID = new TableField("LocationID", "TEXT", LocationType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Locations", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            LocationID
        };

        private List<Type> types = new List<Type>()
        {
            BidType,
            LocationType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            LocationID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Locations"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidSystemTable : TableBase
    {
        public static string TableName = "BidSystem";
        public static Type BidType = typeof(TECBid);
        public static Type SystemType = typeof(TECSystem);

        public static Type Helpers = typeof(HelperProperties);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Systems", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            SystemID
        };
        private List<Type> types = new List<Type>()
        {
            BidType,
            SystemType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            SystemID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Systems"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidControllerTable : TableBase
    {
        public static string TableName = "BidController";
        public static Type BidType = typeof(TECBid);
        public static Type ControllerType = typeof(TECController);

        public static Type Helpers = typeof(HelperProperties);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Controllers", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            ControllerID
        };
        private List<Type> types = new List<Type>()
        {
            BidType,
            ControllerType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            ControllerID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidPanelTable : TableBase
    {
        public static string TableName = "BidPanel";
        public static Type BidType = typeof(TECBid);
        public static Type PanelType = typeof(TECPanel);

        public static Type Helpers = typeof(HelperProperties);

        public static TableField BidID = new TableField("BidID", "TEXT", BidType.GetProperty("Guid"));
        public static TableField PanelID = new TableField("PanelID", "TEXT", PanelType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Panels", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            PanelID
        };
        private List<Type> types = new List<Type>()
        {
            BidType,
            PanelType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            PanelID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Panels"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidInternalNoteTable : TableBase
    {
        public static string TableName = "BidInternalNote";
        public static Type ObjectType = typeof(TECBid);
        public static Type ReferenceType = typeof(TECInternalNote);

        public static TableField BidID = new TableField("BidID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField NoteID = new TableField("NoteID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type HelperType = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "InternalNotes", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            NoteID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            NoteID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "InternalNotes"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class BidDistributionContactTable : TableBase
    {
        public static string TableName = "BidDistributionContact";
        public static Type ObjectType = typeof(TECBid);
        public static Type ReferenceType = typeof(TECDistributionContact);

        public static TableField BidID = new TableField("BidID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ContactID = new TableField("ContactID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type HelperType = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "DistributionList", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            BidID,
            ContactID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            BidID,
            ContactID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "DistributionList"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class SystemEquipmentTable : TableBase
    {
        public static string TableName = "SystemEquipment";
        public static Type ObjectType = typeof(TECSystem);
        public static Type ReferenceType = typeof(TECEquipment);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SystemID = new TableField("SystemID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Equipment", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            EquipmentID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            EquipmentID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Equipment"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SystemControllerTable : TableBase
    {
        public static string TableName = "SystemController";
        public static Type SystemType = typeof(TECSystem);
        public static Type ControllerType = typeof(TECController);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Controllers", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            ControllerID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            ControllerType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            ControllerID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemPanelTable : TableBase
    {
        public static string TableName = "SystemPanel";
        public static Type SystemType = typeof(TECSystem);
        public static Type PanelType = typeof(TECPanel);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField PanelID = new TableField("PanelID", "TEXT", PanelType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Panels", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            PanelID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            PanelType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            PanelID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Panels"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
   
    }
    internal class SystemScopeBranchTable : TableBase
    {
        public static string TableName = "SystemScopeBranch";
        public static Type SystemType = typeof(TECSystem);
        public static Type ScopeBranchType = typeof(TECScopeBranch);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField BranchID = new TableField("BranchID", "TEXT", ScopeBranchType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "ScopeBranches", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            BranchID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            ScopeBranchType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            BranchID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "ScopeBranches"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemHierarchyTable : TableBase
    {
        public static string TableName = "SystemHierarchy";
        public static Type SystemType = typeof(TECSystem);

        public static TableField ParentID = new TableField("ParentID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", SystemType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Instances", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ParentID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            SystemType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ParentID,
            ChildID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Instances"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemMiscTable : TableBase
    {
        public static string TableName = "SystemMisc";
        public static Type SystemType = typeof(TECSystem);
        public static Type MiscType = typeof(TECMisc);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MiscID", "TEXT", MiscType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "MiscCosts", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            MiscID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            MiscType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            MiscID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCosts"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class SystemProposalItemTable : TableBase
    {
        public static string TableName = "SystemProposalItem";
        public static Type SystemType = typeof(TECSystem);
        public static Type ProposalType = typeof(TECProposalItem);

        public static TableField SystemID = new TableField("SystemID", "TEXT", SystemType.GetProperty("Guid"));
        public static TableField ProposalID = new TableField("ProposalID", "TEXT", ProposalType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "ProposalItems", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SystemID,
            ProposalID
            };
        private List<Type> types = new List<Type>()
        {
            SystemType,
            ProposalType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SystemID,
            ProposalID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "ProposalItems"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class EquipmentSubScopeTable : TableBase
    {
        public static string TableName = "EquipmentSubScope";
        public static Type ObjectType = typeof(TECEquipment);
        public static Type ReferenceType = typeof(TECSubScope);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "SubScope", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            EquipmentID,
            SubScopeID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            EquipmentID,
            SubScopeID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "SubScope"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }

    internal class SubScopeDeviceTable : TableBase
    {
        public static string TableName = "SubScopeDevice";
        public static Type ObjectType = typeof(TECSubScope);
        public static Type ReferenceType = typeof(IEndDevice);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "Devices");
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Devices", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SubScopeID,
            DeviceID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SubScopeID,
            DeviceID,
            Quantity,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Devices"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopePointTable : TableBase
    {
        public static string TableName = "SubScopePoint";
        public static Type ObjectType = typeof(TECSubScope);
        public static Type ReferenceType = typeof(TECPoint);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField PointID = new TableField("PointID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Points", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SubScopeID,
            PointID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SubScopeID,
            PointID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Points"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class SubScopeScopeBranchTable : TableBase
    {
        public static string TableName = "SubScopeScopeBranch";
        public static Type SubScopeType = typeof(TECSubScope);
        public static Type ScopeBranchType = typeof(TECScopeBranch);

        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", SubScopeType.GetProperty("Guid"));
        public static TableField BranchID = new TableField("BranchID", "TEXT", ScopeBranchType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "ScopeBranches", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            SubScopeID,
            BranchID
            };
        private List<Type> types = new List<Type>()
        {
            SubScopeType,
            ScopeBranchType
        };
        private List<TableField> fields = new List<TableField>()
        {
            SubScopeID,
            BranchID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "ScopeBranches"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class InterlockableInterlockTable : TableBase
    {
        public static string TableName = "InterlockableInterlock";
        public static Type ParentType = typeof(IInterlockable);
        public static Type ChildType = typeof(TECInterlockConnection);

        public static TableField ParentID = new TableField("ParentID", "TEXT", ParentType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", ChildType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ParentID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ParentType,
            ChildType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ParentID,
            ChildID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Interlocks"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class InterlockConnectionConnectionTypeTable : TableBase
    {
        public static string TableName = "InterlockConnectionConnectionType";
        public static Type ConnectionType = typeof(TECInterlockConnection);
        public static Type ReferenceType = typeof(TECConnectionType);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "ConnectionTypes");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            TypeID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class FBOControllerPointTable : TableBase
    {
        public static string TableName = "FBOControllerPointTable";
        public static Type ObjectType = typeof(TECFBOController);
        public static Type ReferenceType = typeof(TECPoint);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField PointID = new TableField("PointID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ControllerID,
            PointID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            PointID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Points"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ProvidedControllerIOModuleTable : TableBase
    {
        public static string TableName = "ProvidedControllerIOModule";
        public static Type ObjectType = typeof(TECProvidedController);
        public static Type ReferenceType = typeof(TECIOModule);
        public static Type HelperType = typeof(HelperProperties);
        
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ModuleID = new TableField("ModuleID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "IOModules");

        private List<TableField> primaryKeys = new List<TableField>() {
            ControllerID,
            ModuleID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            ModuleID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "IOModules"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ProvidedControllerControllerTypeTable : TableBase
    {
        public static string TableName = "ProvidedControllerControllerType";
        public static Type ControllerType = typeof(TECProvidedController);
        public static Type TypeType = typeof(TECControllerType);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ControllerType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("TypeID", "TEXT", TypeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ControllerID,
            TypeID
            };
        private List<Type> types = new List<Type>()
        {
            ControllerType,
            TypeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            TypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Type"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerConnectionTable : TableBase
    {
        public static string TableName = "ControllerConnection";
        public static Type ObjectType = typeof(TECController);
        public static Type ReferenceType = typeof(IControllerConnection);

        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ControllerID,
            ConnectionID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ControllerID,
            ConnectionID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ChildrenConnections"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ConnectionConduitTypeTable : TableBase
    {
        public static string TableName = "ConnectionConduitType";
        public static Type ObjectType = typeof(IControllerConnection);
        public static Type ReferenceType = typeof(TECElectricalMaterial);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConduitTypeID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConduitType"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class NetworkConnectionChildrenTable : TableBase
    {
        public static string TableName = "NetworkConnectionChildren";
        public static Type ConnectionType = typeof(TECNetworkConnection);
        public static Type ChildType = typeof(IConnectable);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", ChildType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Children", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ConnectionID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ChildType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            ChildID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Children"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class HardwiredConnectionChildrenTable : TableBase
    {
        public static string TableName = "HardwiredConnectionChild";
        public static Type ConnectionType = typeof(TECHardwiredConnection);
        public static Type ChildType = typeof(IConnectable);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ConnectableID", "TEXT", ChildType.GetProperty("Guid"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            ConnectionID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ChildType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            ChildID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Child"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class NetworkConnectionProtocolTable : TableBase
    {
        public static string TableName = "NetworkConnectionProtocol";
        public static Type ConnectionType = typeof(TECNetworkConnection);
        public static Type ProtocolType = typeof(TECProtocol);

        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField ProtocolID = new TableField("ProtocolID", "TEXT", ProtocolType.GetProperty("Guid"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            ConnectionID,
            ProtocolID
            };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ProtocolType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            ProtocolID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Protocol"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ProtocolConnectionTypeTable : TableBase
    {
        public static string TableName = "ProtocolConnectionType";
        public static Type ObjectType = typeof(TECProtocol);
        public static Type ReferenceType = typeof(TECConnectionType);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ProtocolID = new TableField("ProtocolID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "ConnectionTypes", defaultValue: "1");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ProtocolID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ProtocolID,
            TypeID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class IOProtocolTable : TableBase
    {
        public static string TableName = "IOProtocol";
        public static Type IOType = typeof(TECIO);
        public static Type ProtocolType = typeof(TECProtocol);

        public static TableField IOID = new TableField("IOID", "TEXT", IOType.GetProperty("Guid"));
        public static TableField ProtocolID = new TableField("ProtocolID", "TEXT", ProtocolType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            IOID,
            ProtocolID
            };
        private List<Type> types = new List<Type>()
        {
            IOType,
            ProtocolType
        };
        private List<TableField> fields = new List<TableField>()
        {
            IOID,
            ProtocolID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Protocol"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class HardwiredConnectionConnectionTypeTable : TableBase
    {
        public static string TableName = "HardwiredConnectionConnectionType";
        public static Type ConnectionType = typeof(TECHardwiredConnection);
        public static Type ReferenceType = typeof(TECConnectionType);

        public static Type HelperType = typeof(HelperProperties);
        
        public static TableField ConnectionID = new TableField("ConnectionID", "TEXT", ConnectionType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "ConnectionTypes");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ConnectionID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ConnectionType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ConnectionID,
            TypeID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "ConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class ScopeTagTable : TableBase
    {
        public static string TableName = "ScopeTag";
        public static Type ObjectType = typeof(TECTagged);
        public static Type ReferenceType = typeof(TECTag);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TagID = new TableField("TagID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ScopeID,
            TagID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScopeID,
            TagID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Tags"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScopeAssociatedCostTable : TableBase
    {
        public static string TableName = "ScopeAssociatedCost";
        public static Type ObjectType = typeof(TECScope);
        public static Type ReferenceType = typeof(TECAssociatedCost);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField AssociatedCostID = new TableField("AssociatedCostID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "AssociatedCosts");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ScopeID,
            AssociatedCostID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScopeID,
            AssociatedCostID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "AssociatedCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class LocatedLocationTable : TableBase
    {
        public static string TableName = "LocatedLocation";
        public static Type ObjectType = typeof(TECLocated);
        public static Type ReferenceType = typeof(TECLocation);

        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField LocationID = new TableField("LocationID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ScopeID,
            LocationID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScopeID,
            LocationID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Location"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class HardwareManufacturerTable : TableBase
    {
        public static string TableName = "HardwareManufacturer";
        public static Type ObjectType = typeof(TECHardware);
        public static Type ReferenceType = typeof(TECManufacturer);

        public static TableField HardwareID = new TableField("HardwareID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ManufacturerID = new TableField("ManufacturerID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            HardwareID,
            ManufacturerID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            HardwareID,
            ManufacturerID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Manufacturer"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class HardwareQuoteTable : TableBase
    {
        public static string TableName = "HardwareQuote";
        public static Type ObjectType = typeof(TECHardware);

        public static TableField HardwareID = new TableField("HardwareID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField QuotedPrice = new TableField("QuotedPrice", "TEXT", ObjectType.GetProperty("QuotedPrice"));

        private List<TableField> primaryKeys = new List<TableField>() {
            HardwareID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            HardwareID,
            QuotedPrice
        };
        private List<string> propertyNames = new List<string>()
        {
            "QuotedPrice"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

        public override bool IsExtensionTable => true;
    }
    internal class ControllerTypeIOModuleTable : TableBase
    {
        public static string TableName = "ControllerTypeIOModule";
        public static Type ObjectType = typeof(TECControllerType);
        public static Type ReferenceType = typeof(TECIOModule);
        public static Type HelperType = typeof(HelperProperties);

        public static TableField TypeID = new TableField("TypeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ModuleID = new TableField("ModuleID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "IOModules");

        private List<TableField> primaryKeys = new List<TableField>() {
            TypeID,
            ModuleID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TypeID,
            ModuleID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "IOModules"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class IOModuleIOTable : TableBase
    {
        public static string TableName = "IOModuleIO";
        public static Type ObjectType = typeof(TECIOModule);
        public static Type ReferenceType = typeof(TECIO);

        public static TableField ModuleID = new TableField("IOModuleID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField IOID = new TableField("IOID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ModuleID,
            IOID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ModuleID,
            IOID
        };
        private List<string> propertyNames = new List<string>()
        {
            "IO"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ControllerTypeIOTable : TableBase
    {
        public static string TableName = "ControllerTypeIO";
        public static Type ObjectType = typeof(TECControllerType);
        public static Type ReferenceType = typeof(TECIO);

        public static TableField TypeID = new TableField("TypeID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField IOID = new TableField("IOID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            TypeID,
            IOID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TypeID,
            IOID
        };
        private List<string> propertyNames = new List<string>()
        {
            "IO"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class DeviceConnectionTypeTable : TableBase
    {
        public static string TableName = "DeviceConnectionType";
        public static Type ObjectType = typeof(TECDevice);
        public static Type ReferenceType = typeof(TECConnectionType);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TypeID = new TableField("ConnectionTypeID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "HardwiredConnectionTypes");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            DeviceID,
            TypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            DeviceID,
            TypeID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "HardwiredConnectionTypes"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class DeviceProtocolTable : TableBase
    {
        public static string TableName = "DeviceProtocol";
        public static Type ObjectType = typeof(TECDevice);
        public static Type ReferenceType = typeof(TECProtocol);
        
        public static TableField DeviceID = new TableField("DeviceID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ProtocolID = new TableField("ProtocolID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            DeviceID,
            ProtocolID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            DeviceID,
            ProtocolID
        };
        private List<string> propertyNames = new List<string>()
        {
            "PossibleProtocols"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ElectricalMaterialRatedCostTable : TableBase
    {
        public static string TableName = "ElectricalMaterialRatedCost";
        public static Type ObjectType = typeof(TECElectricalMaterial);
        public static Type ReferenceType = typeof(TECAssociatedCost);

        public static Type HelperType = typeof(HelperProperties);

        public static TableField ComponentID = new TableField("ComponentID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField CostID = new TableField("CostID", "TEXT", ReferenceType.GetProperty("Guid"));
        public static TableField Quantity = new TableField("Quantity", "INTEGER", HelperType.GetProperty("Quantity"), "RatedCosts");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ComponentID,
            CostID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ComponentID,
            CostID,
            Quantity
        };
        private List<string> propertyNames = new List<string>()
        {
            "RatedCosts"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelPanelTypeTable : TableBase
    {
        public static string TableName = "PanelPanelType";
        public static Type ObjectType = typeof(TECPanel);
        public static Type ReferenceType = typeof(TECPanelType);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField PanelTypeID = new TableField("PanelTypeID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            PanelID,
            PanelTypeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            PanelID,
            PanelTypeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Type"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class PanelControllerTable : TableBase
    {
        public static string TableName = "PanelController";
        public static Type ObjectType = typeof(TECPanel);
        public static Type ReferenceType = typeof(TECController);

        public static TableField PanelID = new TableField("PanelID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ReferenceType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            PanelID,
            ControllerID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            PanelID,
            ControllerID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Controllers"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ValveActuatorTable : TableBase
    {
        public static string TableName = "ValveActuator";
        public static Type ValveType = typeof(TECValve);
        public static Type ActuatorType = typeof(TECDevice);

        public static TableField ValveID = new TableField("ValveID", "TEXT", ValveType.GetProperty("Guid"));
        public static TableField ActuatorID = new TableField("ActuatorID", "TEXT", ActuatorType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            ValveID,
            ActuatorID
        };
        private List<Type> types = new List<Type>()
        {
            ValveType,
            ActuatorType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ValveID,
            ActuatorID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Actuator"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class ScopeBranchHierarchyTable : TableBase
    {
        public static string TableName = "ScopeBranchHierarchy";
        public static Type ObjectType = typeof(TECScopeBranch);
        public static Type ReferenceType = typeof(TECScopeBranch);

        public static TableField ParentID = new TableField("ParentID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ChildID = new TableField("ChildID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "Branches", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ParentID,
            ChildID
            };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ParentID,
            ChildID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Branches"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }

    }
    internal class ProposalItemContainingScopeTable : TableBase
    {
        public static string TableName = "ProposalItemContainingScope";
        public static Type ProposalType = typeof(TECProposalItem);
        public static Type ScopeType = typeof(TECEquipment);

        public static TableField ProposalID = new TableField("ProposalID", "TEXT", ProposalType.GetProperty("Guid"));
        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ScopeType.GetProperty("Guid"));

        public static Type Helpers = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", Helpers.GetProperty("Index"), "ContainingScope", "0");

        private List<TableField> primaryKeys = new List<TableField>() {
            ProposalID,
            ScopeID
            };
        private List<Type> types = new List<Type>()
        {
            ProposalType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ProposalID,
            ScopeID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "ContainingScope"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ProposalItemDisplayScopeTable : TableBase
    {
        public static string TableName = "ProposalItemDisplayScope";
        public static Type ProposalType = typeof(TECProposalItem);
        public static Type ScopeType = typeof(TECEquipment);

        public static TableField ProposalID = new TableField("ProposalID", "TEXT", ProposalType.GetProperty("Guid"));
        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ScopeType.GetProperty("Guid"));
        
        private List<TableField> primaryKeys = new List<TableField>() {
            ProposalID,
            ScopeID
            };
        private List<Type> types = new List<Type>()
        {
            ProposalType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ProposalID,
            ScopeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "DisplayScope"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    
    internal class TypicalInstanceTable : TableBase
    {
        public static string TableName = "TypicalInstance";
        public static Type ScopeType = typeof(TECObject);

        public static TableField TypicalID = new TableField("TypicalID", "TEXT", ScopeType.GetProperty("Guid"));
        public static TableField InstanceID = new TableField("InstanceID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>() {
            TypicalID,
            InstanceID
            };
        private List<Type> types = new List<Type>()
        {
            ScopeType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TypicalID,
            InstanceID
        };
        private List<string> propertyNames = new List<string>()
        {
            "TypicalInstanceDictionary"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class TemplatesSystemTable : TableBase
    {
        public static string TableName = "TemplatesSystem";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ScopeType = typeof(TECSystem);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField SystemID = new TableField("SystemID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            SystemID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            SystemID
        };
        private List<string> propertyNames = new List<string>()
        {
            "SystemTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class TemplatesEquipmentTable : TableBase
    {
        public static string TableName = "TemplatesEquipment";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ScopeType = typeof(TECEquipment);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField EquipmentID = new TableField("EquipmentID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            EquipmentID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            EquipmentID
        };
        private List<string> propertyNames = new List<string>()
        {
            "EquipmentTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class TemplatesSubScopeTable : TableBase
    {
        public static string TableName = "TemplatesSubScope";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ScopeType = typeof(TECSubScope);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField SubScopeID = new TableField("SubScopeID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            SubScopeID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            SubScopeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "SubScopeTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class TemplatesControllerTable : TableBase
    {
        public static string TableName = "TemplatesController";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ScopeType = typeof(TECController);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField ControllerID = new TableField("ControllerID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            ControllerID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            ControllerID
        };
        private List<string> propertyNames = new List<string>()
        {
            "ControllerTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class TemplatesMiscCostTable : TableBase
    {
        public static string TableName = "TemplatesMiscCost";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ScopeType = typeof(TECMisc);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField MiscID = new TableField("MiscID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            MiscID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            MiscID
        };
        private List<string> propertyNames = new List<string>()
        {
            "MiscCostTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class TemplatesPanelTable : TableBase
    {
        public static string TableName = "TemplatesPanel";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ScopeType = typeof(TECPanel);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField PanelID = new TableField("PanelID", "TEXT", ScopeType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            PanelID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ScopeType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            PanelID
        };
        private List<string> propertyNames = new List<string>()
        {
            "PanelTemplates"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class TemplatesParametersTable : TableBase
    {
        public static string TableName = "TemplatesParameters";
        public static Type TemplatesType = typeof(TECScopeTemplates);
        public static Type ParametersType = typeof(TECParameters);

        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", TemplatesType.GetProperty("Guid"));
        public static TableField ParametersID = new TableField("ParametersID", "TEXT", ParametersType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplatesID,
            ParametersID
        };
        private List<Type> types = new List<Type>()
        {
            TemplatesType,
            ParametersType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplatesID,
            ParametersID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Parameters"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class TemplateReferenceTable : TableBase
    {
        public static string TableName = "TemplateReference";
        public static Type ObjectType = typeof(TECObject);

        public static TableField TemplateID = new TableField("TemplateID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ReferenceID = new TableField("ReferenceID", "TEXT", ObjectType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TemplateID,
            ReferenceID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ObjectType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TemplateID,
            ReferenceID
        };
        private List<string> propertyNames = new List<string>()
        {
            "TemplateRelationship"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class ScheduleScheduleTableTable : TableBase
    {
        public static string TableName = "ScheduleScheduleTable";
        public static Type ObjectType = typeof(TECSchedule);
        public static Type ReferenceType = typeof(TECScheduleTable);

        public static TableField ScheduleID = new TableField("ScheduleID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TableID = new TableField("TableID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type HelperType = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Tables", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ScheduleID,
            TableID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScheduleID,
            TableID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Tables"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScheduleTableScheduleItemTable : TableBase
    {
        public static string TableName = "ScheduleScheduleItem";
        public static Type ObjectType = typeof(TECScheduleTable);
        public static Type ReferenceType = typeof(TECScheduleItem);

        public static TableField TableID = new TableField("TableID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ItemID = new TableField("ItemID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type HelperType = typeof(HelperProperties);
        public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Items", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            TableID,
            ItemID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            TableID,
            ItemID,
            Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Items"
        };

        public override string NameString { get { return TableName; } }
        public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScheduleItemScopeTable : TableBase
    {
        public static string TableName = "ScheduleItemScope";
        public static Type ObjectType = typeof(TECScheduleItem);
        public static Type ReferenceType = typeof(TECScope);

        public static TableField ItemID = new TableField("ItemID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField ScopeID = new TableField("ScopeID", "TEXT", ReferenceType.GetProperty("Guid"));
        
        private List<TableField> primaryKeys = new List<TableField>()
        {
            ItemID,
            ScopeID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ItemID,
            ScopeID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Scope"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    internal class ScheduleItemLocationTable : TableBase
    {
        public static string TableName = "ScheduleItemLocation";
        public static Type ScheduleType = typeof(TECScheduleItem);
        public static Type LocationType = typeof(TECLocation);

        public static TableField ScheduleItemID = new TableField("ScheduleItemID", "TEXT", ScheduleType.GetProperty("Guid"));
        public static TableField LocationID = new TableField("LocationID", "TEXT", LocationType.GetProperty("Guid"));

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ScheduleItemID,
            LocationID
        };
        private List<Type> types = new List<Type>()
        {
            ScheduleType,
            LocationType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ScheduleItemID,
            LocationID
        };
        private List<string> propertyNames = new List<string>()
        {
            "Location"
        };

        public override string NameString { get { return TableName; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }

    internal class ManagerTemplatesTable : TableBase
    {
        public static string TableName = "ManagerTemplates";
        public static Type ObjectType = typeof(TECScopeManager);
        public static Type ReferenceType = typeof(TECScopeTemplates);

        public static TableField ManagerID = new TableField("ManagerID", "TEXT", ObjectType.GetProperty("Guid"));
        public static TableField TemplatesID = new TableField("TemplatesID", "TEXT", ReferenceType.GetProperty("Guid"));

        public static Type HelperType = typeof(HelperProperties);
        //public static TableField Index = new TableField("ScopeIndex", "INTEGER", HelperType.GetProperty("Index"), "Templates", "0");

        private List<TableField> primaryKeys = new List<TableField>()
        {
            ManagerID,
            TemplatesID
        };
        private List<Type> types = new List<Type>()
        {
            ObjectType,
            ReferenceType
        };
        private List<TableField> fields = new List<TableField>()
        {
            ManagerID,
            TemplatesID//,
            //Index
        };
        private List<string> propertyNames = new List<string>()
        {
            "Templates"
        };

        public override string NameString { get { return TableName; } }
        //public override string IndexString { get { return Index.Name; } }
        public override List<Type> Types { get { return types; } }
        public override List<string> PropertyNames { get { return propertyNames; } }
        public override List<TableField> PrimaryKeys { get { return primaryKeys; } }
        public override List<TableField> Fields { get { return fields; } }
    }
    
    #endregion

    internal static class AllBidTables
    {
        public static List<TableBase> Tables = new List<TableBase>() {
            new MetadataTable(),
            new BidInfoTable(),
            new ParametersTable(),
            new ExtraLaborTable(),
            new NoteTable(),
            new ExclusionTable(),
            new ScopeBranchTable(),
            new FBOControllerTable(),
            new ProvidedControllerTable(),
            new SystemTable(),
            new EquipmentTable(),
            new SubScopeTable(),
            new DeviceTable(),
            new PointTable(),
            new TagTable(),
            new ManufacturerTable(),
            new LocationTable(),
            new MiscTable(),
            new PanelTable(),
            new PanelTypeTable(),
            new SubScopeConnectionTable(),
            new NetworkConnectionTable(),
            new IOTable(),
            new IOModuleTable(),
            new ControllerTypeTable(),
            new ValveTable(),
            new ScheduleTable(),
            new ScheduleTableTable(),
            new ScheduleItemTable(),
            new InternalNoteTable(),
            new ProtocolTable(),
            new InterlockConnectionTable(),
            new ScopeTemplatesTable(),
            new ProposalItemTable(),
            new DistributionContactTable(),
          
            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new ScopeBranchHierarchyTable(),
            new BidSystemTable(),
            new BidControllerTable(),
            new BidPanelTable(),
            new SystemEquipmentTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new HardwareManufacturerTable(),
            new LocatedLocationTable(),
            new AssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ControllerConnectionTable(),
            new ControllerTypeIOTable(),
            new IOModuleIOTable(),
            new ProvidedControllerIOModuleTable(),
            new FBOControllerPointTable(),
            new ControllerTypeIOModuleTable(),
            new BidScopeBranchTable(),
            new DeviceConnectionTypeTable(),
            new DeviceProtocolTable(),
            new ScopeAssociatedCostTable(),
            new ProvidedControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemScopeBranchTable(),
            new SystemHierarchyTable(),
            new SystemMiscTable(),
            new TypicalInstanceTable(),
            new BidMiscTable(),
            new PanelPanelTypeTable(),
            new PanelControllerTable(),
            new HardwiredConnectionChildrenTable(),
            new NetworkConnectionChildrenTable(),
            new ValveActuatorTable(),
            new ScheduleScheduleTableTable(),
            new ScheduleTableScheduleItemTable(),
            new ScheduleItemScopeTable(),
            new BidLocationTable(),
            new BidInternalNoteTable(),
            new NetworkConnectionProtocolTable(),
            new ProtocolConnectionTypeTable(),
            new IOProtocolTable(),
            new HardwiredConnectionConnectionTypeTable(),
            new InterlockableInterlockTable(),
            new InterlockConnectionConnectionTypeTable(),
            new ScheduleItemLocationTable(),
            new ManagerTemplatesTable(),
            new TemplatesSystemTable(),
            new TemplatesEquipmentTable(),
            new TemplatesSubScopeTable(),
            new TemplatesControllerTable(),
            new TemplatesMiscCostTable(),
            new TemplatesPanelTable(),
            new TemplatesParametersTable(),
            new SystemProposalItemTable(),
            new SubScopeScopeBranchTable(),
            new ProposalItemContainingScopeTable(),
            new ProposalItemDisplayScopeTable(),
            new HardwareQuoteTable(),
            new BidDistributionContactTable()
            };
    }

    internal static class AllTemplateTables
    {
        public static List<TableBase> Tables = new List<TableBase>()
        {
            new MetadataTable(),
            new TemplatesInfoTable(),
            new ParametersTable(),
            new SystemTable(),
            new EquipmentTable(),
            new SubScopeTable(),
            new DeviceTable(),
            new PointTable(),
            new TagTable(),
            new ManufacturerTable(),
            new PanelTypeTable(),
            new PanelTable(),
            new MiscTable(),
            new SubScopeConnectionTable(),
            new NetworkConnectionTable(),
            new IOModuleTable(),
            new IOTable(),
            new ScopeBranchTable(),
            new ControllerTypeTable(),
            new ValveTable(),
            new ProtocolTable(),
            new InterlockConnectionTable(),
            new ScopeTemplatesTable(),
            new ProposalItemTable(),

            new ConnectionTypeTable(),
            new ConduitTypeTable(),
            new AssociatedCostTable(),
            new SystemEquipmentTable(),
            new ControllerConnectionTable(),
            new EquipmentSubScopeTable(),
            new SubScopeDeviceTable(),
            new SubScopePointTable(),
            new ScopeTagTable(),
            new ProvidedControllerTable(),
            new FBOControllerTable(),
            new ControllerTypeIOTable(),
            new IOModuleIOTable(),
            new FBOControllerPointTable(),
            new ProvidedControllerIOModuleTable(),
            new ControllerTypeIOModuleTable(),
            new HardwareManufacturerTable(),
            new DeviceConnectionTypeTable(),
            new DeviceProtocolTable(),
            new ScopeAssociatedCostTable(),
            new ElectricalMaterialRatedCostTable(),
            new ProvidedControllerControllerTypeTable(),
            new ConnectionConduitTypeTable(),
            new PanelPanelTypeTable(),
            new SystemControllerTable(),
            new SystemPanelTable(),
            new SystemHierarchyTable(),
            new SystemScopeBranchTable(),
            new SystemMiscTable(),
            new PanelControllerTable(),
            new HardwiredConnectionChildrenTable(),
            new NetworkConnectionChildrenTable(),
            new ScopeBranchHierarchyTable(),
            new ValveActuatorTable(),
            new TemplatesSystemTable(),
            new TemplatesEquipmentTable(),
            new TemplatesSubScopeTable(),
            new TemplatesControllerTable(),
            new TemplatesMiscCostTable(),
            new TemplatesPanelTable(),
            new TemplateReferenceTable(),
            new NetworkConnectionProtocolTable(),
            new ProtocolConnectionTypeTable(),
            new IOProtocolTable(),
            new HardwiredConnectionConnectionTypeTable(),
            new InterlockableInterlockTable(),
            new InterlockConnectionConnectionTypeTable(),
            new ManagerTemplatesTable(),
            new TemplatesParametersTable(),
            new SystemProposalItemTable(),
            new SubScopeScopeBranchTable(),
            new ProposalItemContainingScopeTable(),
            new ProposalItemDisplayScopeTable()
        };
    }

    internal static class AllTables
    {
        public static List<TableBase> Tables = getTables();
        private static List<TableBase> getTables()
        {
            List<TableBase> tables = new List<TableBase>();
            tables.AddRange(AllBidTables.Tables);
            tables.AddRange(AllTemplateTables.Tables);
            return tables.Distinct().ToList();
        }
    }

    internal abstract class TableBase
    {
        public abstract string NameString { get; }
        public virtual string IndexString { get { return ""; } }
        public abstract List<Type> Types { get; }
        public abstract List<string> PropertyNames { get; }

        public abstract List<TableField> PrimaryKeys { get; }
        public abstract List<TableField> Fields { get; }
        public virtual string QuantityString
        {
            get
            {
                string qtyField = "";
                foreach (TableField field in Fields)
                {
                    if (field.Property.DeclaringType == typeof(HelperProperties) && field.Property.Name == "Quantity")
                    {
                        qtyField = field.Name;
                        break;
                    }
                }
                return qtyField;
            }
        }
        
        public virtual bool IsExtensionTable { get { return false; } }
    }
    
    internal class TableField
    {
        private string _defaultValue;

        public string Name { get; }
        public string FieldType { get; }
        public PropertyInfo Property { get; }
        public string HelperContext { get; }
        public string DefaultValue
        {
            get
            {
                if (_defaultValue == null)
                {
                    throw new NotImplementedException(string.Format("TableField {0} doesn't have a default value.", Name));
                }
                else
                {
                    return _defaultValue;
                }
            }
        }

        public TableField(string name, string fieldType, PropertyInfo property, string helperContext = "", string defaultValue = null)
        {
            Name = name;
            FieldType = fieldType;
            Property = property;
            HelperContext = helperContext;
            _defaultValue = defaultValue;
        }
    }

    internal class HelperProperties
    {
        public bool Index { get; set; }
        public bool Quantity { get; set; }

        public HelperProperties()
        {
            Index = true;
            Quantity = true;
        }

    }
    
    #endregion
}
