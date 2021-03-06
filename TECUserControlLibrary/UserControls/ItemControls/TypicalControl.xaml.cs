﻿using EstimatingLibrary;
using System.Windows;

namespace TECUserControlLibrary.UserControls.ItemControls
{
    /// <summary>
    /// Interaction logic for TypicalControl.xaml
    /// </summary>
    public partial class TypicalControl : BaseItemControl
    {

        public TECTypical Typical
        {
            get { return (TECTypical)GetValue(TypicalProperty); }
            set { SetValue(TypicalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Typical.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypicalProperty =
            DependencyProperty.Register("Typical", typeof(TECTypical), typeof(TypicalControl));


        public TypicalControl()
        {
            InitializeComponent();
        }
    }
}
