﻿using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.ViewModels;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for ControllerInPanelListControl.xaml
    /// </summary>
    public partial class ControllerInPanelListControl : BaseListControl<ControllerInPanel>
    {

        public ControllersPanelsVM ViewModel
        {
            get { return (ControllersPanelsVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(ControllersPanelsVM), typeof(ControllerInPanelListControl));

        public ICommand ChangeTypeCommand
        {
            get { return (ICommand)GetValue(ChangeTypeCommandProperty); }
            set { SetValue(ChangeTypeCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ChangeTypeCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChangeTypeCommandProperty =
            DependencyProperty.Register("ChangeTypeCommand", typeof(ICommand), typeof(ControllerInPanelListControl));

        public ControllerInPanelListControl()
        {
            InitializeComponent();
        }
    }
}
