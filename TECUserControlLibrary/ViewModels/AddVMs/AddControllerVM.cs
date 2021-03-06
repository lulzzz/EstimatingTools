﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using GalaSoft.MvvmLight.CommandWpf;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace TECUserControlLibrary.ViewModels.AddVMs
{
    public class AddControllerVM : AddVM
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private TECSystem parent;
        private TECController toAdd;
        private int quantity;
        private Action<TECController> add;
        private TECControllerType noneControllerType;
        private string _hintText;
        private TECControllerType _selectedType;
        private bool isTypical = false;
        private bool isFBO = false;
        
        public TECController ToAdd
        {
            get { return toAdd; }
            private set
            {
                toAdd = value;
                RaisePropertyChanged("ToAdd");
            }
        }
        public int Quantity
        {
            get { return quantity; }
            set
            {
                quantity = value;
                RaisePropertyChanged("Quantity");
            }
        }
        public string HintText
        {
            get { return _hintText; }
            set
            {
                _hintText = value;
                RaisePropertyChanged("HintText");
            }
        }
        public TECControllerType SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                RaisePropertyChanged("SelectedType");
                if (value != null && ToAdd is TECProvidedController provided)
                {
                    if (!provided.ChangeType(SelectedType))
                    {
                        logger.Error("Limbo controller ToAdd could not have its type set to the SelectedType.");
                    }
                }
            }
        }
        public bool IsFBO
        {
            get { return isFBO; }
            set
            {
                isFBO = value;
                RaisePropertyChanged("IsFBO");
                TECController newAdd;
                if (value)
                {
                    newAdd = new TECFBOController(PropertiesVM.Catalogs);
                }
                else
                {
                    newAdd = new TECProvidedController(SelectedType);
                }
                newAdd.Name = ToAdd.Name;
                newAdd.Description = ToAdd.Description;
                newAdd.Tags.AddRange(ToAdd.Tags);
                newAdd.AssociatedCosts.AddRange(ToAdd.AssociatedCosts);
                ToAdd = newAdd;
            }
        }

        public List<TECControllerType> ControllerTypes { get; private set; }

        public AddControllerVM(TECSystem parentSystem, IEnumerable<TECControllerType> controllerTypes, TECScopeManager scopeManager) : base(scopeManager)
        {
            setup(controllerTypes);
            parent = parentSystem;
            isTypical = parent.IsTypical;
            add = controller =>
            {
                parent.AddController(controller);
            };

        }

        public AddControllerVM(Action<TECController> addMethod, IEnumerable<TECControllerType> controllerTypes, TECScopeManager scopeManager) : base(scopeManager)
        {
            setup(controllerTypes);
            add = addMethod;
        }

        public void SetTemplate(TECProvidedController template)
        {
            ToAdd = new TECProvidedController(template);
        }
        
        private void setup(IEnumerable<TECControllerType> controllerTypes)
        {
            Quantity = 1;
            noneControllerType = new TECControllerType(new TECManufacturer());
            noneControllerType.Name = "Select Controller Type";
            toAdd = new TECProvidedController(noneControllerType);
            ControllerTypes = new List<TECControllerType>(controllerTypes);
            ControllerTypes.Insert(0, noneControllerType);
            AddCommand = new RelayCommand(addExecute, addCanExecute);
            SelectedType = noneControllerType;
            //PropertiesVM = new PropertiesVM()

        }
        
        private bool addCanExecute()
        {
            bool canAdd = (ToAdd is TECProvidedController provided && provided.Type != noneControllerType) || ToAdd is TECFBOController;
            if (canAdd)
            {
                HintText = null;
                return true;
            } else
            {
                HintText = "Select a Controller Type";
                return false;
            }
        }
        private void addExecute()
        {
            for(int x = 0; x < Quantity; x++)
            {
                var controller = ToAdd;
                if (!AsReference)
                {
                    if(ToAdd is TECProvidedController provided)
                    {
                        controller = new TECProvidedController(provided);
                    }
                    else
                    {
                        controller = new TECFBOController((TECFBOController)ToAdd);
                    }
                }
                add(controller);
                Added?.Invoke(controller);
            }
        }
    }
}
