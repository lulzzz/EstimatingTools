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
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for ReplaceActuatorView.xaml
    /// </summary>
    public partial class ReplaceActuatorView : UserControl
    {
        public static readonly RoutedEvent DoneEvent =
        EventManager.RegisterRoutedEvent("Done", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(ReplaceActuatorView));

        public event RoutedEventHandler Done
        {
            add { AddHandler(DoneEvent, value); }
            remove { RemoveHandler(DoneEvent, value); }
        }

        public ReplaceActuatorVM VM
        {
            get { return (ReplaceActuatorVM)GetValue(VMProperty); }
            set { SetValue(VMProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register("VM", typeof(ReplaceActuatorVM), typeof(ReplaceActuatorView));



        public ReplaceActuatorView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DoneEvent, this));
        }
    }
}
