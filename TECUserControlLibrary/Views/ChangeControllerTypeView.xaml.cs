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
    /// Interaction logic for ChangeControllerTypeView.xaml
    /// </summary>
    public partial class ChangeControllerTypeView : UserControl
    {
        public ChangeControllerTypeVM ViewModel
        {
            get { return (ChangeControllerTypeVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ChangeControllerTypeVM), typeof(ChangeControllerTypeView));


        public static readonly RoutedEvent DoneEvent =
        EventManager.RegisterRoutedEvent("Done", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(ChangeControllerTypeView));

        public event RoutedEventHandler Done
        {
            add { AddHandler(DoneEvent, value); }
            remove { RemoveHandler(DoneEvent, value); }
        }

        public ChangeControllerTypeView()
        {
            InitializeComponent();
        }

        protected void doneButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DoneEvent, this));
        }
    }
}
