﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TECUserControlLibrary.Interfaces;

namespace EstimateBuilder.MVVM
{
    public class EstimateSettingsVM : ViewModelBase
    {
        private bool settingsChanged = false;

        public string DefaultBidDirectory
        {
            get { return EBSettings.BidDirectory; }
            set
            {
                EBSettings.BidDirectory = value;
                settingsChanged = true;
            }
        }
        public string DefaultTemplatesDirectory
        {
            get { return EBSettings.TemplatesDirectory; }
            set
            {
                EBSettings.TemplatesDirectory = value;
                settingsChanged = true;
            }
        }
        public bool OpenOnExport
        {
            get { return EBSettings.OpenFileOnExport; }
            set
            {
                EBSettings.OpenFileOnExport = value;
                settingsChanged = true;
            }
        }

        public ICommand ChooseBidDirectoryCommand { get; }
        public ICommand ChooseTemplatesDirectoryCommand { get; }
        public ICommand ApplyCommand { get; }

        public EstimateSettingsVM()
        {
            ApplyCommand = new RelayCommand(applySettingsExecute, applySettingsCanExecute);
        }

        private void chooseBidDirectoryExecute()
        {
            throw new NotImplementedException();
        }

        private void chooseTemplatesDirectoryExecute()
        {
            throw new NotImplementedException();
        }

        private void applySettingsExecute()
        {
            EBSettings.Save();
            settingsChanged = false;
        }
        private bool applySettingsCanExecute()
        {
            return settingsChanged;
        }
    }
}
