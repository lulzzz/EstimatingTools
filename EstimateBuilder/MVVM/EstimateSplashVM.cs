﻿using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;
using TECUserControlLibrary.ViewModels;

namespace EstimateBuilder.MVVM
{
    public class EstimateSplashVM : SplashVM
    {
        const string SPLASH_TITLE = "Welcome to Estimate Builder";
        const string SPLASH_SUBTITLE = "Please select object templates and select and existing file or create a new bid";

        private List<string> fileExtensions = new List<string> { ".edb", ".tdb" };

        private string defaultTemplatesDirectory;

        private string _bidPath = "";

        public string BidPath
        {
            get { return _bidPath; }
            set
            {
                _bidPath = value;
                RaisePropertyChanged("BidPath");
            }
        }

        public ICommand GetBidPathCommand { get; private set; }
        public ICommand ClearBidPathCommand { get; private set; }

        public string FirstRecentBid
        {
            get { return EBSettings.FirstRecentBid; }
        }
        public string SecondRecentBid
        {
            get { return EBSettings.SecondRecentBid; }
        }
        public string ThirdRecentBid
        {
            get { return EBSettings.ThirdRecentBid; }
        }
        public string FourthRecentBid
        {
            get { return EBSettings.FourthRecentBid; }
        }
        public string FifthRecentBid
        {
            get { return EBSettings.FifthRecentBid; }
        }

        public override string FirstRecentTemplates
        {
            get { return EBSettings.FirstRecentTemplates; }
        }
        public override string SecondRecentTemplates
        {
            get { return EBSettings.SecondRecentTemplates; }
        }
        public override string ThirdRecentTemplates
        {
            get { return EBSettings.ThirdRecentTemplates; }
        }
        public override string FourthRecentTemplates
        {
            get { return EBSettings.FourthRecentTemplates; }
        }
        public override string FifthRecentTemplates
        {
            get { return EBSettings.FifthRecentTemplates; }
        }

        public ICommand ChooseRecentBidCommand { get; }
        
        public event Action<string, string> EditorStarted;

        public EstimateSplashVM(string templatesPath, string defaultDirectory, string defaultTemplatesDirectory) :
            base(SPLASH_TITLE, SPLASH_SUBTITLE, defaultDirectory)
        {
            this.defaultTemplatesDirectory = defaultTemplatesDirectory; 

            TemplatesPath = templatesPath;
            
            GetBidPathCommand = new RelayCommand(getBidPathExecute);
            ClearBidPathCommand = new RelayCommand(clearBidPathExecute);
            GetTemplatesPathCommand = new RelayCommand(getTemplatesPathExecute);
            ClearTemplatesPathCommand = new RelayCommand(clearTemplatesPathExecute);
            OpenExistingCommand = new RelayCommand(openExistingExecute, openExistingCanExecute);
            CreateNewCommand = new RelayCommand(createNewExecute, createNewCanExecute);

            ChooseRecentBidCommand = new RelayCommand<string>(chooseRecentBidExecute, chooseRecentCanExecute);
        }
        
        private void getBidPathExecute()
        {
            string path = getPath(FileDialogParameters.EstimateFileParameters, defaultDirectory);
            if(path != null)
            {
                BidPath = path;
            }
        }
        private void clearBidPathExecute()
        {
            BidPath = "";
        }
        private void getTemplatesPathExecute()
        {
            string path = getPath(FileDialogParameters.TemplatesFileParameters, defaultTemplatesDirectory);
            if(path != null)
            {
                TemplatesPath = path;
            }
        }
        private void clearTemplatesPathExecute()
        {
            TemplatesPath = "";
        }

        private void openExistingExecute()
        {
            LoadingText = "Loading...";
            if (!File.Exists(BidPath))
            {
                MessageBox.Show("Bid file no longer exists at that path.");
                LoadingText = "";
            }
            else if (TemplatesPath == "" || TemplatesPath == null)
            {
                MessageBoxResult result = MessageBox.Show("No templates have been selected. Would you like to continue?", "Continue?", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        EditorStarted?.Invoke(BidPath, "");
                        break;
                    default:
                        LoadingText = "";
                        break;
                }
            }
            else if (!File.Exists(TemplatesPath))
            {
                MessageBox.Show("Templates file no longer exist at that path.");
                LoadingText = "";
            }
            else
            {
                EditorStarted?.Invoke(BidPath, TemplatesPath);
            }
        }
        private bool openExistingCanExecute()
        {
            return (BidPath != "" && BidPath != null);
        }
    
        private void createNewExecute()
        {
            LoadingText = "Loading...";
            if (TemplatesPath == "" || TemplatesPath == null)
            {
                MessageBoxResult result = MessageBox.Show("No templates have been selected. Would you like to continue?", "Continue?", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        EditorStarted?.Invoke("", "");
                        break;
                    default:
                        LoadingText = "";
                        break;
                }
            }
            else if (!File.Exists(TemplatesPath))
            {
                MessageBox.Show("Templates file no longer exist at that path.");
                LoadingText = "";
            }
            else
            {
                EditorStarted?.Invoke("", TemplatesPath);
            }
        }
        private bool createNewCanExecute()
        {
            return true;
        }

        private void chooseRecentBidExecute(string path)
        {
            BidPath = path;
        }

        public override void DragOver(IDropInfo dropInfo)
        {
            UIHelpers.FileDragOver(dropInfo, fileExtensions);
        }

        public override void Drop(IDropInfo dropInfo)
        {
            string path = UIHelpers.FileDrop(dropInfo, fileExtensions);
            if(path != null)
            {
                string ext = Path.GetExtension(path);
                switch(ext)
                {
                    case ".edb":
                        BidPath = path;
                        break;
                    case ".tdb":
                        TemplatesPath = path;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
