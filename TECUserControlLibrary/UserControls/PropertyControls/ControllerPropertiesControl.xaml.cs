﻿using EstimatingLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.UserControls.PropertyControls
{
    /// <summary>
    /// Interaction logic for ControllerPropertiesControl.xaml
    /// </summary>
    public partial class ControllerPropertiesControl : UserControl
    {

        public TECController Selected
        {
            get { return (TECController)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Selected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(TECController), typeof(ControllerPropertiesControl));



        public IEnumerable<TECControllerType> TypeSource
        {
            get { return (IEnumerable<TECControllerType>)GetValue(TypeSourceProperty); }
            set { SetValue(TypeSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TypeSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeSourceProperty =
            DependencyProperty.Register("TypeSource", typeof(IEnumerable<TECControllerType>), typeof(ControllerPropertiesControl));



        public ControllerPropertiesControl()
        {
            InitializeComponent();
        }
    }

    public class ControllerToPropertiesItemConverter : BaseConverter, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value as TECProvidedController != null)
            {
                return new ProvidedControllerPropertiesItem(value as TECProvidedController);

            } else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }
        #endregion
    }
}
