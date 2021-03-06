﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.Views
{
    /// <summary>
    /// Interaction logic for CatalogsView.xaml
    /// </summary>
    public partial class CatalogsView : UserControl
    {
        /// <summary>
        /// Gets or sets the SelectedScopeType which is displayed
        /// </summary>
        public MaterialType SelectedMaterialType
        {
            get { return (MaterialType)GetValue(SelectedMaterialTypeProperty); }
            set { SetValue(SelectedMaterialTypeProperty, value); }
        }

        /// <summary>
        /// Identified the SelectedScopeType dependency property
        /// </summary>
        public static readonly DependencyProperty SelectedMaterialTypeProperty =
            DependencyProperty.Register("SelectedMaterialType", typeof(MaterialType),
              typeof(CatalogsView), new PropertyMetadata(default(MaterialType)));

        public object Selected
        {
            get { return (object)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        public static readonly DependencyProperty SelectedProperty =
            DependencyProperty.Register("Selected", typeof(object),
                typeof(CatalogsView), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        public MaterialVM ViewModel
        {
            get { return (MaterialVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(MaterialVM),
                typeof(CatalogsView));


        public double ModalHeight
        {
            get { return (double)GetValue(ModalHeightProperty); }
            set { SetValue(ModalHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ModalHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModalHeightProperty =
            DependencyProperty.Register("ModalHeight", typeof(double), typeof(CatalogsView), new PropertyMetadata(1.0));
        
        public CatalogsView()
        {
            InitializeComponent();
            SizeChanged += handleSizeChanged;

        }

        private void handleSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.HeightChanged)
            {
                if (ModalHeight != 0.0)
                {
                    ModalHeight = e.NewSize.Height;
                }
            }
        }

        private void modalIn_Completed(object sender, EventArgs e)
        {
            ModalHeight = 0;
        }

        private void modalOut_Completed(object sender, EventArgs e)
        {
            ModalHeight = this.ActualHeight;
        }

        private void Remove_Clicked(object sender, RoutedEventArgs e)
        {
            Storyboard moveBack = (Storyboard)FindResource("modalIn");
            moveBack.Begin();
        }
    }
}
