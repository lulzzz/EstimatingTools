﻿using System.Windows;
using System.Windows.Controls;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for MiscCostsView.xaml
    /// </summary>
    public partial class MiscCostsView : UserControl
    {
        public MiscCostsVM ViewModel
        {
            get { return (MiscCostsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MiscCostsVM),
                typeof(MiscCostsView));
        
        public object DropHandler
        {
            get { return (object)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DropHandler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(object), typeof(MiscCostsView));

        public bool ReadOnly
        {
            get { return (bool)GetValue(ReadOnlyProperty); }
            set { SetValue(ReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadOnlyProperty =
            DependencyProperty.Register("ReadOnly", typeof(bool), typeof(MiscCostsView), new PropertyMetadata(false));



        public MiscCostsView()
        {
            InitializeComponent();
        }
    }
}
