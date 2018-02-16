﻿using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TECUserControlLibrary.Utilities;

namespace TECUserControlLibrary.UserControls.ListControls
{
    /// <summary>
    /// Interaction logic for TypicalListControl.xaml
    /// </summary>
    public partial class BaseListControl<T> : UserControl where T: class
    {
        private static RelayCommand<T> defaultDelete = new RelayCommand<T>(item => { }, item => false);
        
        public IEnumerable<T> Source
        {
            get { return (IEnumerable<T>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable<T>),
              typeof(BaseListControl<T>), new PropertyMetadata(default(IEnumerable<T>)));

        public T SelectedItem
        {
            get { return (T)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(T),
                typeof(BaseListControl<T>), new FrameworkPropertyMetadata(null)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        
        public IDropTarget DropHandler
        {
            get { return (IDropTarget)GetValue(DropHandlerProperty); }
            set { SetValue(DropHandlerProperty, value); }
        }
        
        public static readonly DependencyProperty DropHandlerProperty =
            DependencyProperty.Register("DropHandler", typeof(IDropTarget),
              typeof(BaseListControl<T>));

        public static readonly RoutedEvent SelectedEvent =
        EventManager.RegisterRoutedEvent("Selected", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(BaseListControl<T>));

        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }
        
        public static readonly RoutedEvent DroppedEvent =
        EventManager.RegisterRoutedEvent("Dropped", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(BaseListControl<T>));

        public event RoutedEventHandler Dropped
        {
            add { AddHandler(DroppedEvent, value); }
            remove { RemoveHandler(DroppedEvent, value); }
        }

        public static readonly RoutedEvent DoubleClickedEvent =
        EventManager.RegisterRoutedEvent("DoubleClicked", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(BaseListControl<T>));

        public event RoutedEventHandler DoubleClicked
        {
            add { AddHandler(DoubleClickedEvent, value); }
            remove { RemoveHandler(DoubleClickedEvent, value); }
        }

        public object ScopeParent
        {
            get { return (object)GetValue(ScopeParentProperty); }
            set { SetValue(ScopeParentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScopeParent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScopeParentProperty =
            DependencyProperty.Register("ScopeParent", typeof(object), typeof(BaseListControl<T>));
        
        public bool IsDragSource
        {
            get { return (bool)GetValue(IsDragSourceProperty); }
            set { SetValue(IsDragSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDragSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDragSourceProperty =
            DependencyProperty.Register("IsDragSource", typeof(bool), typeof(BaseListControl<T>), new PropertyMetadata(false));
        
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteCommandProperty =
            DependencyProperty.Register("DeleteCommand", typeof(ICommand), typeof(BaseListControl<T>), new PropertyMetadata(defaultDelete));
        
        public DataTemplate DragAdornerTemplate
        {
            get { return (DataTemplate)GetValue(DragAdornerTemplateProperty); }
            set { SetValue(DragAdornerTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DragAdornerTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragAdornerTemplateProperty =
            DependencyProperty.Register("DragAdornerTemplate", typeof(DataTemplate), typeof(BaseListControl<T>));
        
        protected void ListView_Selected(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }
        protected void ListView_MouseUp(object sender, RoutedEventArgs e)
        {
            //RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }
        protected void ListView_Dropped(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DroppedEvent, this));
        }
        protected void ItemControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var item = this.SelectedItem;
            ListView child = UIHelpers.FindVisualChild<ListView>(this);
            if(child != null && child.SelectedItems.Count == 1)
            {
                this.SelectedItem = null;
                this.SelectedItem = item;
            }
            
            //RaiseEvent(new RoutedEventArgs(SelectedEvent, this));
        }
        protected void ListView_MouseDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DoubleClickedEvent, this));
        }
    }
}
