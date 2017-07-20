﻿using EstimatingLibrary.Interfaces;
using EstimatingLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EstimatingLibrary
{
    public class TECSystem : TECScope, CostComponent, PointComponent
    {//TECSystem is the largest encapsulating object in the System-Equipment hierarchy, offering a specific structure for the needs of the estimating tool. A seperate hierarchy exists for TECScopeBranch as a more generic object.
        #region Properties
        private ObservableCollection<TECEquipment> _equipment;
        public ObservableCollection<TECEquipment> Equipment
        {
            get { return _equipment; }
            set
            {
                var temp = this.Copy();
                if (Equipment != null)
                {
                    Equipment.CollectionChanged -= CollectionChanged;
                }
                _equipment = value;
                NotifyPropertyChanged("Equipment", temp, this);
                Equipment.CollectionChanged += CollectionChanged;
            }
        }

        public double BudgetPriceModifier
        {
            get { return _budgetPriceModifier; }
            set
            {
                var temp = this.Copy();
                if (_budgetPriceModifier != value)
                {
                    if (value < 0)
                    {
                        _budgetPriceModifier = -1;
                    }
                    else
                    {
                        _budgetPriceModifier = value;
                    }
                    NotifyPropertyChanged("BudgetPriceModifier", temp, this);
                    RaisePropertyChanged("TotalBudgetPrice");
                    RaisePropertyChanged("BudgetUnitPrice");
                }
            }
        }
        private double _budgetPriceModifier;
        public double BudgetUnitPrice
        {
            get
            {
                double price = 0;
                bool priceExists = false;
                if (BudgetPriceModifier >= 0)
                {
                    price += BudgetPriceModifier;
                    priceExists = true;
                }
                foreach (TECEquipment equip in Equipment)
                {
                    if (equip.TotalBudgetPrice >= 0)
                    {
                        price += (equip.TotalBudgetPrice);
                        priceExists = true;
                    }
                }
                if (priceExists)
                { return price; }
                else
                { return -1; }
            }
        }
        new public int Quantity
        {
            get { return _quantity; }
            set
            {
                var temp = this.Copy();
                _quantity = value;
                NotifyPropertyChanged("Quantity", temp, this);
                RaisePropertyChanged("TotalBudgetPrice");
            }
        }
        public int EquipmentQuantity
        {
            get
            {
                int equipQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                { equipQuantity += equip.Quantity; }
                return equipQuantity;
            }
        }
        public int SubScopeQuantity
        {
            get
            {
                int ssQuantity = 0;
                foreach (TECEquipment equip in Equipment)
                { ssQuantity += (equip.SubScopeQuantity * equip.Quantity); }
                return ssQuantity;
            }
        }

        private bool _proposeEquipment;
        public bool ProposeEquipment
        {
            get { return _proposeEquipment; }
            set
            {
                var temp = this.Copy();
                _proposeEquipment = value;
                NotifyPropertyChanged("ProposeEquipment", temp, this);
            }
        }

        public double TotalBudgetPrice
        {
            get
            {
                if (Quantity > 0)
                {
                    return (Quantity * BudgetUnitPrice);
                }
                else
                {
                    return -1;
                }
            }
        }

        public ObservableCollection<TECSubScope> SubScope
        {
            get
            {
                var outSubScope = new ObservableCollection<TECSubScope>();
                foreach (TECEquipment equip in Equipment)
                {
                    foreach (TECSubScope sub in equip.SubScope)
                    {
                        outSubScope.Add(sub);
                    }
                }
                return outSubScope;
            }
        }

        public int PointNumber
        {
            get
            {
                return getPointNumber();
            }
        }

        private ObservableCollection<TECController> _controllers { get; set; }
        public ObservableCollection<TECController> Controllers
        {
            get { return _controllers; }
            set
            {
                var temp = this.Copy();
                if (Controllers != null)
                {
                    Controllers.CollectionChanged -= CollectionChanged;
                }
                _controllers = value;
                Controllers.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Controllers", temp, this);
            }
        }

        private ObservableCollection<TECPanel> _panels { get; set; }
        public ObservableCollection<TECPanel> Panels
        {
            get { return _panels; }
            set
            {
                var temp = this.Copy();
                if (Panels != null)
                {
                    Panels.CollectionChanged -= CollectionChanged;
                }

                _panels = value;
                Panels.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("Panels", temp, this);
            }
        }

        private ObservableCollection<TECSystem> _systemInstances;
        public ObservableCollection<TECSystem> SystemInstances
        {
            get { return _systemInstances; }
            set
            {
                var temp = this.Copy();
                if (SystemInstances != null)
                {
                    SystemInstances.CollectionChanged -= CollectionChanged;
                }

                _systemInstances = value;
                SystemInstances.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("SystemInstances", temp, this);
            }
        }

        private ObservableCollection<TECScopeBranch> _scopeBranches;
        public ObservableCollection<TECScopeBranch> ScopeBranches
        {
            get { return _scopeBranches; }
            set
            {
                var temp = this.Copy();
                if (ScopeBranches != null)
                {
                    ScopeBranches.CollectionChanged -= CollectionChanged;
                }

                _scopeBranches = value;
                ScopeBranches.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("ScopeBranches", temp, this);

            }
        }

        private ObservableCollection<TECMisc> _miscCosts;
        public ObservableCollection<TECMisc> MiscCosts
        {
            get { return _miscCosts; }
            set
            {
                var temp = this.Copy();
                if (MiscCosts != null)
                {
                    MiscCosts.CollectionChanged -= CollectionChanged;
                }
                _miscCosts = value;
                MiscCosts.CollectionChanged += CollectionChanged;
                NotifyPropertyChanged("MiscCosts", temp, this);
            }
        }

        public List<TECCost> Costs
        {
            get
            {
                return getCosts();
            }
        }
        private List<TECCost> getCosts()
        {
            var outCosts = new List<TECCost>();
            foreach(TECEquipment item in Equipment)
            {
                foreach(TECCost cost in item.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach (TECController item in Controllers)
            {
                foreach (TECCost cost in item.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach (TECPanel item in Panels)
            {
                foreach (TECCost cost in item.Costs)
                {
                    outCosts.Add(cost);
                }
            }
            foreach (TECCost item in AssociatedCosts)
            {
                outCosts.Add(item);
            }
            foreach (TECMisc item in MiscCosts)
            {
                foreach(TECSystem system in SystemInstances)
                {
                    outCosts.Add(item);
                }
            }
            return outCosts;
        }

        private ObservableItemToInstanceList<TECScope> _charactersticInstances;
        public ObservableItemToInstanceList<TECScope> CharactersticInstances
        {
            get { return _charactersticInstances; }
            set
            {
                if(CharactersticInstances != null)
                {
                    CharactersticInstances.PropertyChanged -= CharactersticInstances_PropertyChanged;
                }
                _charactersticInstances = value;
                CharactersticInstances.PropertyChanged += CharactersticInstances_PropertyChanged;

            }
        }
        private ChangeWatcher watcher;
        
        #endregion //Properties

        #region Constructors
        public TECSystem(Guid guid) : base(guid)
        {
            _budgetPriceModifier = -1;
            _proposeEquipment = false;
            base.PropertyChanged += TECSystem_PropertyChanged;
            
            
            _equipment = new ObservableCollection<TECEquipment>();

            _controllers = new ObservableCollection<TECController>();
            _panels = new ObservableCollection<TECPanel>();
            _systemInstances = new ObservableCollection<TECSystem>();
            _scopeBranches = new ObservableCollection<TECScopeBranch>();
            _miscCosts = new ObservableCollection<TECMisc>();
            _charactersticInstances = new ObservableItemToInstanceList<TECScope>();
            CharactersticInstances.PropertyChanged += CharactersticInstances_PropertyChanged;
            Equipment.CollectionChanged += CollectionChanged;
            Controllers.CollectionChanged += CollectionChanged;
            Panels.CollectionChanged += CollectionChanged;
            SystemInstances.CollectionChanged += CollectionChanged;
            ScopeBranches.CollectionChanged += CollectionChanged;
            MiscCosts.CollectionChanged += CollectionChanged;
            watcher = new ChangeWatcher(this);
            watcher.Changed += Object_PropertyChanged;
        }
        public TECSystem() : this(Guid.NewGuid()) { }

        //Copy Constructor
        public TECSystem(TECSystem source, Dictionary<Guid, Guid> guidDictionary = null,
            ObservableItemToInstanceList<TECScope> characteristicReference = null) : this()
        {
            if (guidDictionary != null)
            { guidDictionary[_guid] = source.Guid; }
            foreach (TECEquipment equipment in source.Equipment)
            {
                var toAdd = new TECEquipment(equipment, guidDictionary, characteristicReference);
                if(characteristicReference != null)
                {
                    characteristicReference.AddItem(equipment, toAdd);
                }
                Equipment.Add(toAdd);
            }
            foreach (TECController controller in source._controllers)
            {
                var toAdd = new TECController(controller, guidDictionary);
                if (characteristicReference != null)
                {
                    characteristicReference.AddItem(controller, toAdd);
                }
                _controllers.Add(toAdd);
            }
            foreach (TECPanel panel in source._panels)
            {
                var toAdd = new TECPanel(panel, guidDictionary);
                if (characteristicReference != null)
                {
                    characteristicReference.AddItem(panel, toAdd);
                }
                 _panels.Add(toAdd);
            }
            foreach(TECScopeBranch branch in source._scopeBranches)
            {
                var toAdd = new TECScopeBranch(branch);
                _scopeBranches.Add(toAdd);
            }
            foreach(TECMisc misc in source.MiscCosts)
            {
                var toAdd = new TECMisc(misc);
                _miscCosts.Add(toAdd);
            }
            _budgetPriceModifier = source.BudgetPriceModifier;
            this.copyPropertiesFromScope(source);
        }
        #endregion //Constructors

        #region Methods
        public override Object Copy()
        {
            TECSystem outSystem = new TECSystem(_guid);
            foreach (TECEquipment equipment in this.Equipment)
            { outSystem.Equipment.Add(equipment.Copy() as TECEquipment); }
            foreach (TECController controller in Controllers)
            {
                outSystem.Controllers.Add(controller.Copy() as TECController);
            }
            foreach (TECPanel panel in Panels)
            {
                outSystem.Panels.Add(panel.Copy() as TECPanel);
            }
            foreach(TECSystem system in SystemInstances)
            {
                outSystem.SystemInstances.Add(system.Copy() as TECSystem);
            }
            foreach(TECScopeBranch branch in ScopeBranches)
            {
                outSystem.ScopeBranches.Add(branch.Copy() as TECScopeBranch);
            }
            foreach(TECMisc misc in MiscCosts)
            {
                outSystem.MiscCosts.Add(misc.Copy() as TECMisc);
            }
            outSystem._budgetPriceModifier = this.BudgetPriceModifier;
            outSystem.copyPropertiesFromScope(this);
            outSystem._proposeEquipment = this.ProposeEquipment;
            ModelLinkingHelper.LinkTypicalInstanceDictionary(CharactersticInstances, outSystem);
            return outSystem;
        }
        public override object DragDropCopy(TECScopeManager scopeManager)
        {
            Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
            TECSystem outSystem = new TECSystem(this, guidDictionary);
            ModelLinkingHelper.LinkSystem(outSystem, scopeManager, guidDictionary);
            return outSystem;
        }
        
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (object item in e.NewItems)
                {
                    if (item != null)
                    {
                        NotifyPropertyChanged("Add", this, item);
                        if(item is TECController)
                        {
                            (item as TECController).IsGlobal = false;
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    if (item != null)
                    {
                         NotifyPropertyChanged("Remove", this, item);
                        if(item is TECSystem)
                        {
                            handleInstanceRemoved(item as TECSystem);
                        }
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {
                NotifyPropertyChanged("Edit", this, sender, typeof(TECSystem), typeof(TECEquipment));
            }
        }

        private void handleInstanceRemoved(TECSystem instance)
        {
            foreach(TECSubScope subScope in instance.SubScope)
            {
                if(subScope.Connection != null && subScope.Connection.ParentController.IsGlobal)
                {
                    subScope.Connection.ParentController.RemoveSubScope(subScope);
                }
            }
        }

        private void TECSystem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ObjectPropertyChanged")
            {
                var args = e as PropertyChangedExtendedEventArgs<object>;
                var oldNew = args.NewValue as Tuple<object, object>;
                foreach (TECEquipment equipment in this.Equipment)
                {
                    if (equipment.Location == oldNew.Item1)
                    {
                        equipment.SetLocationFromParent(this.Location);
                    }
                }
            }
        }

        private int getPointNumber()
        {
            var totalPoints = 0;
            foreach (TECEquipment equipment in Equipment)
            {
                totalPoints += equipment.PointNumber;
            }
            return totalPoints;
        }

        private void Object_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SystemInstances.Count > 0)
            {
                if (e is PropertyChangedExtendedEventArgs<Object>)
                {
                    PropertyChangedExtendedEventArgs<Object> args = e as PropertyChangedExtendedEventArgs<Object>;
                    object oldValue = args.OldValue;
                    object newValue = args.NewValue;
                    if (e.PropertyName == "Add" ||
                        e.PropertyName == "AddRelationship" ||
                        e.PropertyName == "AddCatalog")
                    {
                        handleAdd(newValue, oldValue);
                    }
                    else if (e.PropertyName == "Remove" ||
                        e.PropertyName == "RemoveRelationship" ||
                        e.PropertyName == "RemoveCatalog")
                    {
                        handleRemove(newValue, oldValue);
                    }
                    else if(oldValue is TECPoint && oldValue is TECPoint)
                    {
                        handlePointChanged(newValue as TECPoint, e.PropertyName);
                    }
                } else if (e.PropertyName == "Connection" && sender is TECSubScope)
                {
                    handleSubScopeConnectionChanged(sender as TECSubScope);
                }
            }
        }

        private void handlePointChanged(TECPoint point, string propertyName)
        {
            PropertyInfo property = typeof(TECPoint).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if(property != null && property.CanWrite && CharactersticInstances.ContainsKey(point))
            {
                foreach (TECPoint instance in CharactersticInstances.GetInstances(point))
                {
                    property.SetValue(instance, property.GetValue(point), null);
                }
            }
            
        }

        private void handleSubScopeConnectionChanged(TECSubScope subScope)
        {
            if (CharactersticInstances.ContainsKey(subScope))
            {
                if (subScope.Connection == null)
                {
                    foreach (TECSubScope instance in CharactersticInstances.GetInstances(subScope))
                    {
                        if(instance.Connection == null || !instance.Connection.ParentController.IsGlobal)
                        {
                            break;
                        }
                        instance.Connection.ParentController.RemoveSubScope(instance);
                    }
                }
                else if (subScope.Connection.ParentController.IsGlobal)
                {
                    foreach (TECSubScope instance in CharactersticInstances.GetInstances(subScope))
                    {
                        subScope.Connection.ParentController.AddSubScope(instance);
                    }
                }
            }
        }

        private void CharactersticInstances_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseExtendedPropertyChanged(sender, e);
        }

        private void handleAdd(object targetObject, object referenceObject)
        {
            if(referenceObject is TECSystem)
            {
                if((referenceObject as TECSystem).SystemInstances.Count == 0)
                {
                    return;
                }
            }
            if (targetObject is TECController && referenceObject is TECSystem)
            {
                var characteristicController = targetObject as TECController;
                foreach (TECSystem system in SystemInstances)
                {
                    var controllerToAdd = new TECController(characteristicController);
                    controllerToAdd.IsGlobal = false;
                    CharactersticInstances.AddItem(characteristicController, controllerToAdd);
                    system.Controllers.Add(controllerToAdd);
                }
            }
            else if (targetObject is TECPanel && referenceObject is TECSystem)
            {
                var characteristicPanel = targetObject as TECPanel;
                foreach (TECSystem system in SystemInstances)
                {
                    var panelToAdd = new TECPanel(characteristicPanel);
                    CharactersticInstances.AddItem(characteristicPanel, panelToAdd);
                    system.Panels.Add(panelToAdd);
                }
            }
            else if (targetObject is TECEquipment && referenceObject is TECSystem)
            {
                var characteristicEquipment = targetObject as TECEquipment;
                foreach (TECSystem system in SystemInstances)
                {
                    var equipmentToAdd = new TECEquipment(characteristicEquipment, characteristicReference: CharactersticInstances);
                    CharactersticInstances.AddItem(characteristicEquipment, equipmentToAdd);
                    system.Equipment.Add(equipmentToAdd);
                }
            }
            else if (targetObject is TECSubScope && referenceObject is TECEquipment)
            {
                var characteristicEquipment = referenceObject as TECEquipment;
                var characteristicSubScope = targetObject as TECSubScope;
                if (CharactersticInstances.ContainsKey(characteristicEquipment))
                {
                    foreach (TECEquipment equipment in CharactersticInstances.GetInstances(characteristicEquipment))
                    {
                        var subScopeToAdd = new TECSubScope(characteristicSubScope, characteristicReference: CharactersticInstances);
                        CharactersticInstances.AddItem(characteristicSubScope, subScopeToAdd);
                        equipment.SubScope.Add(subScopeToAdd);
                    }
                }
            }
            else if (targetObject is TECDevice && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var device = targetObject as TECDevice;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        subScope.Devices.Add(device);
                    }
                }
            }
            else if (targetObject is TECPoint && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var characteristicPoint = targetObject as TECPoint;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        var pointToAdd = new TECPoint(characteristicPoint);
                        CharactersticInstances.AddItem(characteristicPoint, pointToAdd);
                        subScope.Points.Add(pointToAdd);
                    }
                }
            }
            else if (targetObject is TECSubScopeConnection && referenceObject is TECController)
            {
                var characteristicConnection = targetObject as TECSubScopeConnection;
                var characteristicSubScope = (targetObject as TECSubScopeConnection).SubScope;
                var characteristicController = (referenceObject as TECController);
                if (CharactersticInstances.ContainsKey(characteristicSubScope) && (CharactersticInstances.ContainsKey(characteristicController) || characteristicController.IsGlobal))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECSubScope subScopeToConnect = null;
                        foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                        {
                            foreach (TECEquipment equipment in system.Equipment)
                            {
                                if (equipment.SubScope.Contains(subScope))
                                {
                                    subScopeToConnect = subScope;
                                    break;
                                }
                            }
                        }
                        if (subScopeToConnect != null)
                        {
                            if (characteristicController.IsGlobal)
                            {
                                var connection = characteristicController.AddSubScope(subScopeToConnect);
                                connection.Length = characteristicConnection.Length;
                                connection.ConduitLength = characteristicConnection.ConduitLength;
                                connection.ConduitType = characteristicConnection.ConduitType;
                            }
                            else
                            {
                                foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                                {
                                    if (system.Controllers.Contains(controller))
                                    {
                                        var connection = controller.AddSubScope(subScopeToConnect);
                                        connection.Length = characteristicConnection.Length;
                                        connection.ConduitLength = characteristicConnection.ConduitLength;
                                        connection.ConduitType = characteristicConnection.ConduitType;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (targetObject is TECController && referenceObject is TECPanel)
            {
                var characteristicController = targetObject as TECController;
                var characteristicPanel = referenceObject as TECPanel;
                if (CharactersticInstances.ContainsKey(characteristicPanel) && CharactersticInstances.ContainsKey(characteristicController))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECController controllerToConnect = null;
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            if (system.Controllers.Contains(controller))
                            {
                                controllerToConnect = controller;
                                break;
                            }
                        }
                        if (controllerToConnect != null)
                        {
                            foreach (TECPanel panel in CharactersticInstances.GetInstances(characteristicPanel))
                            {
                                if (system.Panels.Contains(panel))
                                {
                                    panel.Controllers.Add(controllerToConnect);
                                }
                            }
                        }

                    }
                }
            }
            else if (targetObject is TECCost && referenceObject is TECScope && !(targetObject is TECMisc)
                && !(targetObject is TECController) && !(targetObject is TECDevice))
            {
                if(referenceObject is TECSystem)
                {
                    foreach(TECSystem system in SystemInstances)
                    {
                        system.AssociatedCosts.Add(targetObject as TECCost);
                    }
                }else
                {
                    var characteristicScope = referenceObject as TECScope;
                    var cost = targetObject as TECCost;
                    if (CharactersticInstances.ContainsKey(characteristicScope))
                    {
                        foreach (TECScope scope in CharactersticInstances.GetInstances(characteristicScope))
                        {
                            scope.AssociatedCosts.Add(cost);
                        }
                    }
                }
                
            }
        }
        private void handleRemove(object targetObject, object referenceObject)
        {
            if (referenceObject is TECSystem)
            {
                if ((referenceObject as TECSystem).SystemInstances.Count == 0)
                {
                    return;
                }
            }
            if (targetObject is TECController && referenceObject is TECSystem)
            {
                var characteristicController = targetObject as TECController;
                foreach (TECSystem system in SystemInstances)
                {
                    var controllersToRemove = new List<TECController>();
                    foreach (TECController controller in system.Controllers)
                    {
                        if (CharactersticInstances.GetInstances(characteristicController).Contains(controller))
                        {
                            controllersToRemove.Add(controller);
                            CharactersticInstances.RemoveItem(characteristicController, controller);
                        }
                    }
                    foreach (TECController controller in controllersToRemove)
                    {
                        system.Controllers.Remove(controller);
                    }
                }
            }
            else if (targetObject is TECPanel && referenceObject is TECSystem)
            {
                var characteristicPanel = targetObject as TECPanel;
                foreach (TECSystem system in SystemInstances)
                {
                    var panelsToRemove = new List<TECPanel>();
                    foreach (TECPanel panel in system.Panels)
                    {
                        if (CharactersticInstances.GetInstances(characteristicPanel).Contains(panel))
                        {
                            panelsToRemove.Add(panel);
                            CharactersticInstances.RemoveItem(characteristicPanel, panel);
                        }
                    }
                    foreach (TECPanel panel in panelsToRemove)
                    {
                        system.Panels.Remove(panel);
                    }
                }
            }
            else if (targetObject is TECEquipment && referenceObject is TECSystem)
            {
                var characteristicEquipment = targetObject as TECEquipment;
                foreach (TECSystem system in SystemInstances)
                {
                    var equipmentToRemove = new List<TECEquipment>();
                    foreach (TECEquipment equipment in system.Equipment)
                    {
                        if (CharactersticInstances.GetInstances(characteristicEquipment).Contains(equipment))
                        {
                            equipmentToRemove.Add(equipment);
                            CharactersticInstances.RemoveItem(characteristicEquipment, equipment);
                        }
                    }
                    foreach (TECEquipment equipment in equipmentToRemove)
                    {
                        system.Equipment.Remove(equipment);
                    }
                }
            }
            else if (targetObject is TECSubScope && referenceObject is TECEquipment)
            {
                var characteristicEquipment = referenceObject as TECEquipment;
                var characteristicSubScope = targetObject as TECSubScope;
                if (CharactersticInstances.ContainsKey(characteristicEquipment))
                {
                    foreach (TECEquipment equipment in CharactersticInstances.GetInstances(characteristicEquipment))
                    {
                        var subScopeToRemove = new List<TECSubScope>();
                        foreach (TECSubScope subScope in equipment.SubScope)
                        {
                            if (CharactersticInstances.GetInstances(characteristicSubScope).Contains(subScope))
                            {
                                subScopeToRemove.Add(subScope);
                                CharactersticInstances.RemoveItem(characteristicSubScope, subScope);

                            }
                        }
                        foreach (TECSubScope subScope in subScopeToRemove)
                        {
                            equipment.SubScope.Remove(subScope);
                        }
                    }
                }
            }
            else if (targetObject is TECDevice && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var device = targetObject as TECDevice;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        subScope.Devices.Remove(device);
                    }
                }
            }
            else if (targetObject is TECPoint && referenceObject is TECSubScope)
            {
                var characteristicSubScope = referenceObject as TECSubScope;
                var characteristicPoint = targetObject as TECPoint;
                if (CharactersticInstances.ContainsKey(characteristicSubScope))
                {
                    foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                    {
                        var pointsToRemove = new List<TECPoint>();
                        foreach (TECPoint point in subScope.Points)
                        {
                            if (CharactersticInstances.GetInstances(characteristicPoint).Contains(point))
                            {
                                pointsToRemove.Add(point);
                                CharactersticInstances.RemoveItem(characteristicPoint, point);
                            }
                        }
                        foreach (TECPoint point in pointsToRemove)
                        {
                            subScope.Points.Remove(point);
                        }
                    }
                }
            }
            else if (targetObject is TECSubScopeConnection && referenceObject is TECController)
            {
                var characteristicConnection = targetObject as TECSubScopeConnection;
                var characteristicSubScope = (targetObject as TECSubScopeConnection).SubScope;
                var characteristicController = (referenceObject as TECController);
                if (CharactersticInstances.ContainsKey(characteristicSubScope) && (CharactersticInstances.ContainsKey(characteristicController) || characteristicController.IsGlobal))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECSubScope subScopeToRemove = null;
                        foreach (TECSubScope subScope in CharactersticInstances.GetInstances(characteristicSubScope))
                        {
                            foreach (TECEquipment equipment in system.Equipment)
                            {
                                if (equipment.SubScope.Contains(subScope))
                                {
                                    subScopeToRemove = subScope;
                                    break;
                                }
                            }
                        }
                        if (subScopeToRemove != null)
                        {
                            if (characteristicController.IsGlobal)
                            {
                                characteristicController.RemoveSubScope(subScopeToRemove);
                            }
                            else
                            {
                                foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                                {
                                    if (system.Controllers.Contains(controller))
                                    {
                                        controller.RemoveSubScope(subScopeToRemove);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (targetObject is TECController && referenceObject is TECPanel)
            {
                var characteristicController = targetObject as TECController;
                var characteristicPanel = referenceObject as TECPanel;
                if (CharactersticInstances.ContainsKey(characteristicController) && CharactersticInstances.ContainsKey(characteristicPanel))
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        TECController controllerToRemove = null;
                        foreach (TECController controller in CharactersticInstances.GetInstances(characteristicController))
                        {
                            foreach (TECPanel panel in system.Panels)
                            {
                                if (panel.Controllers.Contains(controller))
                                {
                                    controllerToRemove = controller;
                                    break;
                                }
                            }
                        }
                        if (controllerToRemove != null)
                        {
                            foreach (TECPanel panel in CharactersticInstances.GetInstances(characteristicPanel))
                            {
                                if (system.Panels.Contains(panel))
                                {
                                    panel.Controllers.Remove(controllerToRemove);
                                    break;
                                }
                            }
                        }

                    }
                }
            }
            else if (targetObject is TECCost && referenceObject is TECScope && !(targetObject is TECMisc)
                && !(targetObject is TECController) && !(targetObject is TECDevice))
            {
                if (referenceObject is TECSystem)
                {
                    foreach (TECSystem system in SystemInstances)
                    {
                        system.AssociatedCosts.Remove(targetObject as TECCost);
                    }
                }
                else
                {
                    var characteristicScope = referenceObject as TECScope;
                    var cost = targetObject as TECCost;
                    if (CharactersticInstances.ContainsKey(characteristicScope))
                    {
                        foreach (TECScope scope in CharactersticInstances.GetInstances(characteristicScope))
                        {
                            scope.AssociatedCosts.Remove(cost);
                        }
                    }
                }
            }
        }

        private void handleSystemSubScopeRemoval(TECSystem system)
        {
            foreach (TECEquipment equipment in system.Equipment)
            {
                handleEquipmentSubScopeRemoval(equipment);
            }
        }
        private void handleEquipmentSubScopeRemoval(TECEquipment equipment)
        {
            foreach (TECSubScope subScope in equipment.SubScope)
            {
                handleSubScopeRemovalInConnections(subScope);
            }
        }
        private void handleSubScopeRemovalInConnections(TECSubScope subScope)
        {
            foreach (TECController controller in Controllers)
            {
                ObservableCollection<TECSubScope> subScopeToRemove = new ObservableCollection<TECSubScope>();
                foreach (TECSubScopeConnection connection in controller.ChildrenConnections)
                {
                    if (connection.SubScope == subScope)
                    {
                        subScopeToRemove.Add(subScope as TECSubScope);
                    }
                }
                foreach (TECSubScope sub in subScopeToRemove)
                {
                    controller.RemoveSubScope(sub);
                }
            }
        }

        public void RefreshReferences()
        {
            watcher.Changed -= Object_PropertyChanged;
            watcher = new ChangeWatcher(this);
            watcher.Changed += Object_PropertyChanged;
        }
        public TECSystem AddInstance(TECBid bid)
        {
            Dictionary<Guid, Guid> guidDictionary = new Dictionary<Guid, Guid>();
            var newSystem = new TECSystem();
            newSystem.Name = Name;
            newSystem.Description = Description;
            foreach (TECEquipment equipment in Equipment)
            {
                var toAdd = new TECEquipment(equipment, guidDictionary, CharactersticInstances);
                CharactersticInstances.AddItem(equipment, toAdd);
                newSystem.Equipment.Add(toAdd);
            }
            foreach (TECController controller in Controllers)
            {
                var toAdd = new TECController(controller, guidDictionary);
                CharactersticInstances.AddItem(controller, toAdd);
                newSystem.Controllers.Add(toAdd);
            }
            foreach (TECPanel panel in Panels)
            {
                var toAdd = new TECPanel(panel, guidDictionary);
                CharactersticInstances.AddItem(panel, toAdd);
                newSystem.Panels.Add(toAdd);
            }
            foreach(TECCost cost in AssociatedCosts)
            {
                newSystem.AssociatedCosts.Add(cost);
            }
            ModelLinkingHelper.LinkSystem(newSystem, bid, guidDictionary);
            var newSubScope = newSystem.SubScope;
            foreach(TECSubScope subScope in SubScope)
            {
                var instances = CharactersticInstances.GetInstances(subScope);
                foreach(TECSubScope subInstance in instances)
                {
                    if (newSubScope.Contains(subInstance))
                    {
                        if(subScope.Connection != null && subScope.Connection.ParentController.IsGlobal)
                        {
                            TECSubScopeConnection instanceSSConnect = subScope.Connection.ParentController.AddSubScope(subInstance);
                            instanceSSConnect.Length = subScope.Connection.Length;
                            instanceSSConnect.ConduitLength = subScope.Connection.ConduitLength;
                            instanceSSConnect.ConduitType = subScope.Connection.ConduitType;
                        }
                    }
                }
            }
            SystemInstances.Add(newSystem);
            return (newSystem);
        }
        #endregion
    }
}
