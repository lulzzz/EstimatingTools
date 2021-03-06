﻿using EstimatingLibrary;
using EstimatingUtilitiesLibrary;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace TECUserControlLibrary.Utilities
{
    [System.Windows.Markup.MarkupExtensionReturnType(typeof(IValueConverter))]
    

    public class VisbilityToBooleanConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (Visibility)value == Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion
    }

    public class BooleanToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolVal)
            {
                if (parameter is bool boolParam)
                {
                    if (!boolParam)
                    {
                        boolVal = !boolVal;
                    }
                }

                if (boolVal)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                throw new InvalidCastException("Value not bool.");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NullReferenceException();
        }

        #endregion
    }

    public class VisbilityToContentConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((Visibility)value == Visibility.Visible)
            {
                return "-";
            }
            else
            {
                return "+";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    
    public class PercentageToNumberConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string inString = (string)value;
            inString = inString.Trim(new Char[] { '%' });

            return inString.ToDouble(0);
        }

        #endregion
    }
    
    public class SelectedLocationToLocationConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var location = new TECLabeled();
                location.Label = "None";
                return location;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            else
            {
                if (((TECLabeled)value).Label == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }

        #endregion
    }
    
    public class PercentageConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string percentageStr = (Math.Round((double)value * 100)).ToString();
            return percentageStr + "%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class CountVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
         }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ZeroCountToVisibileConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class NoCountVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((int)value > 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class NullToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class NullToCollapsedConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class NullToBoolConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedItemToConduitTypeConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var conduitType = new TECElectricalMaterial();
                conduitType.Name = "None";
                return conduitType;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            else
            {
                if (((TECElectricalMaterial)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }
        }
        #endregion
    }

    public class ConnectionIOConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<IOType> val = (List<IOType>)value;
            if (val.Count > 0)
            {
                return val[0];
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<IOType> io = new List<IOType>();
            io.Add((IOType)value);
            return io;
        }

        #endregion
    }

    public class IOTypeConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<IOType>)
            {
                ObservableCollection<IOType> ioTypes = value as ObservableCollection<IOType>;
                ObservableCollection<string> stringTypes = new ObservableCollection<string>();

                foreach (IOType type in ioTypes)
                {
                    stringTypes.Add(type.ToString());
                }

                return stringTypes;
            }
            else if (value is IOType)
            {
                return (value.ToString());
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return (UtilitiesMethods.StringToEnum<IOType>(str));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
    public class SelectedItemToIOModuleConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                var module = new TECIOModule(new TECManufacturer());
                module.Name = "None";
                return module;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }
            else
            {
                if (((TECIOModule)value).Name == "None")
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }

        }
        #endregion
    }
    public class DataContextToEnableConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == "{NewItemPlaceholder}")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }
        #endregion
    }

    public class NoneValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return System.Windows.Data.Binding.DoNothing;
            }
            if (values[0] != null)
            {
                return values[0];
            }
            else
            {
                return values[1];
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            object outObject;
            if (value == null)
            {
                outObject = System.Windows.Data.Binding.DoNothing;
            }
            else if ((value as TECScope).Name == "None")
            {
                outObject = null;
            }
            else
            {
                outObject = value;
            }

            return new object[] { outObject, System.Windows.Data.Binding.DoNothing };
        }

    }

    public class StringToQuoteConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ("\"" + (string)value + "\"");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }
        #endregion
    }

    public class WidthDoubleToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if((double)value == 0.0)
            {
                return Visibility.Visible;
            } else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class WidthDoubleToCollpasedConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((double)value == 0.0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TypicalSelectionTemplateWidthConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((TypicalInstanceEnum)value == TypicalInstanceEnum.Typical)
            {
                return parameter;
            }
            else
            {
                return 0.0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class HeightToGridMarginConverter : BaseConverter, IMultiValueConverter
    {
        #region IValueConverter Members

        public object Convert(object[] values, Type targetType,
           object parameter, System.Globalization.CultureInfo culture)
        {
            double edges = (double)values[0];
            double topOffset = (double)values[1];
            double top = topOffset != 0 ? topOffset : edges;
            double bottom = topOffset != 0 ? 0 : edges;
            return new Thickness(edges, top, edges, bottom);
        }
        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TypeToVisibilityConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type = (Type)parameter;
            if (value.GetType() == type)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedComponentToCommandConverter : BaseConverter, IMultiValueConverter
    {
        #region IValueConverter Members

        public object Convert(object[] values, Type targetType,
           object parameter, System.Globalization.CultureInfo culture)
        {
            int index = System.Convert.ToInt32(values[0]);
            if(index + 1 >= values.Length)
            {
                return null;
            }
            return values[index + 1];
        }
        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class SelectedComponentToSpanConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SystemComponentIndex index = (SystemComponentIndex)value;
            switch(index)
            {
                case SystemComponentIndex.Connections:
                case SystemComponentIndex.Misc:
                case SystemComponentIndex.Proposal:
                case SystemComponentIndex.Controllers:
                case SystemComponentIndex.Valves:
                    return 2;
                default:
                    return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class BooleanChoiceConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is bool boolParam)
            {
                if (boolParam)
                {
                    return values[1];
                }
                else
                {
                    return values[2];
                }
            }
            else
            {
                return values[3];
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullChoiceConverter : BaseConverter, IMultiValueConverter
    {
        //First value returns if none of the objects are null
        //Second values returns if any of the objects are null
        //Subsequent values are the objects to check
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = false;
            for(int i = 2; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    isNull = true;
                    break;
                }
            }

            if (isNull)
            {
                return values[1];
            }
            else
            {
                return values[0];
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class InvertBoolConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolVal)
            {
                return !boolVal;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class CancelEventArgsWithSenderConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            var args = (CancelEventArgs)value;
            var window = (Window)parameter;

            return (args, window);
        }
    }

    public class IOCollectionToListConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IOCollection collection)
            {
                return collection.ToList();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class TabsToTypicalConverter : BaseConverter, IMultiValueConverter
    {
        #region IValueConverter Members

        public object Convert(object[] values, Type targetType,
           object parameter, System.Globalization.CultureInfo culture)
        {
            TypicalInstanceEnum systemSelect = (TypicalInstanceEnum)values[0];
            GridIndex tabSelect = (GridIndex)values[1];
            double width = (double)values[2];
            if(tabSelect == GridIndex.Systems && systemSelect == TypicalInstanceEnum.Instance)
            {
                return new GridLength(0.0);
            } else
            {
                return new GridLength(width);
            }

        }
        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class FullPathToNameConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string path)
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            else if (value == null)
            {
                return "";
            }
            else
            {
                throw new Exception("Value must be string");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class EmptyStringToCollapsedConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is String text && text == "")
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class IOToTypeString : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TECIO io)
            {
                return io.Type == IOType.Protocol ? io.Protocol.Label : io.Type.ToString();
            }
            else if (value == null)
            {
                return "";
            }
            else
            {
                throw new Exception("Value must be IO");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    

    public class ControllerDescriptionDisplayConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TECControllerType controllerType)
            {
                if(controllerType.Description == "")
                {
                    return controllerType.Name;
                }
                else
                {
                    return String.Format("{0}, {1}", controllerType.Name, controllerType.Description);
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #region Enumeration Converters

    public class EditIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EditIndex)value;
        }

        #endregion
    }

    public class SystemsSubIndexIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (SystemsSubIndex)value;
        }

        #endregion
    }

    public class GridIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (GridIndex)value;
        }

        #endregion
    }

    public class TemplateGridIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (TemplateGridIndex)value;
        }

        #endregion
    }

    public class ScopeCollectionIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (ScopeCollectionIndex)value;
        }

        #endregion
    }

    public class LocationScopeTypeToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (LocationScopeType)value;
        }

        #endregion
    }

    public class ProposalIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (ProposalIndex)value;
        }

        #endregion
    }

    public class MaterialSummaryIndexToIntegerConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int outInt = System.Convert.ToInt32(value);
            return outInt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (MaterialSummaryIndex)value;
        }

        #endregion
    }
    #endregion
}
