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
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for ValveScopeItemListControl.xaml
    /// </summary>
    public partial class ValveScopeItemListControl : BaseListControl<ValveScopeItem>
    {
        public ValveScopeItemListControl()
        {
            InitializeComponent();
        }
    }
}
