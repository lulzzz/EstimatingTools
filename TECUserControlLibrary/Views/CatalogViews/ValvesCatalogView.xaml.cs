﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TECUserControlLibrary.ViewModels.CatalogVMs;

namespace TECUserControlLibrary.Views.CatalogViews
{
    /// <summary>
    /// Interaction logic for ValvesCatalogView.xaml
    /// </summary>
    public partial class ValvesCatalogView : UserControl
    {
        public ValvesCatalogVM VM
        {
            get { return (ValvesCatalogVM)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register("VM", typeof(ValvesCatalogVM), typeof(ValvesCatalogView));

        public static RoutedEvent StartModal = EventManager.RegisterRoutedEvent(
            "StartModal", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(ValvesCatalogView));

        public ValvesCatalogView()
        {
            InitializeComponent();
        }

        private void startModal(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(StartModal, this));
        }
    }
}
