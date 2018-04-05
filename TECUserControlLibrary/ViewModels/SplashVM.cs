﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using System.Windows.Input;
using TECUserControlLibrary.Models;

namespace TECUserControlLibrary.ViewModels
{
    public abstract class SplashVM : ViewModelBase, IDropTarget
    {
        protected string defaultDirectory;

        private string _titleText;
        private string _subtitleText;
        private string _loadingText;
        private string _templatesPath;

        public string TitleText
        {
            get { return _titleText; }
            set
            {
                _titleText = value;
                RaisePropertyChanged("TitleText");
            }
        }
        public string SubtitleText
        {
            get { return _subtitleText; }
            set
            {
                _subtitleText = value;
                RaisePropertyChanged("SubtitleText");
            }
        }
        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                RaisePropertyChanged("LoadingText");
            }
        }
        public string Version { get; set; }
        public string TemplatesPath
        {
            get { return _templatesPath; }
            set
            {
                _templatesPath = value;
                RaisePropertyChanged("TemplatesPath");
            }
        }

        public ICommand GetTemplatesPathCommand { get; protected set; }
        public ICommand ClearTemplatesPathCommand { get; protected set; }
        public ICommand OpenExistingCommand { get; protected set; }
        public ICommand CreateNewCommand { get; protected set; }

        #region Recent Files Properties
        public abstract string FirstRecentTemplates { get; }
        public abstract string SecondRecentTemplates { get; }
        public abstract string ThirdRecentTemplates { get; }
        public abstract string FourthRecentTemplates { get; }
        public abstract string FifthRecentTemplates { get; }

        public ICommand ChooseRecentTemplatesCommand { get; }
        #endregion

        public SplashVM(string titleText, string subtitleText, string defaultDirectory)
        {
            LoadingText = "";
            this.defaultDirectory = defaultDirectory;
            _titleText = titleText;
            _subtitleText = subtitleText;

            ChooseRecentTemplatesCommand = new RelayCommand<string>(chooseRecentTemplatesExecute, chooseRecentCanExecute);
        }
        
        protected string getPath(FileDialogParameters fileParams, string initialDirectory = null)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = fileParams.Filter;
            openFileDialog.DefaultExt = fileParams.DefaultExtension;
            openFileDialog.AddExtension = true;
            
            string savePath = null;
            if (openFileDialog.ShowDialog() == true)
            {
                savePath = openFileDialog.FileName;
                if(savePath == "")
                {
                    savePath = null;
                }
            }
            return savePath;
        }

        public abstract void DragOver(IDropInfo dropInfo);
        public abstract void Drop(IDropInfo dropInfo);

        private void chooseRecentTemplatesExecute(string path)
        {
            TemplatesPath = path;
        }
        protected bool chooseRecentCanExecute(string path)
        {
            return (path != null && path != "");
        }
    }
}