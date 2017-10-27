﻿using EstimatingLibrary;
using System.IO;
using System;
using EstimatingUtilitiesLibrary;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using TECUserControlLibrary.ViewModels;
using DebugLibrary;
using TECUserControlLibrary.Utilities;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary.Database;
using GalaSoft.MvvmLight;
using TECUserControlLibrary.Debug;
using EstimatingUtilitiesLibrary.Exports;
using TECUserControlLibrary.ViewModels.SummaryVMs;

namespace EstimateBuilder.MVVM
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : BuilderViewModel
    {
        #region Constants
        const string SPLASH_TITLE = "Welcome to Estimate Builder";
        const string SPLASH_SUBTITLE = "Please select object templates and select and existing file or create a new bid";
        #endregion
        #region Fields

        private DatabaseManager templatesDB;
        private bool _templatesLoaded;
        private TECEstimator _estimate;
        private TECTemplates _templates;

        #endregion
        #region Constructors
        public MainViewModel() : base(SPLASH_TITLE, SPLASH_SUBTITLE, true)
        {
            
        }

        #endregion
        #region Events
        #endregion
        #region Properties
        public ScopeEditorVM ScopeEditorVM { get; set; }
        public LaborVM LaborVM { get; set; }
        public ReviewVM ReviewVM { get; set; }
        public ProposalVM ProposalVM { get; set; }
        public ElectricalVM ElectricalVM { get; set; }
        public NetworkVM NetworkVM { get; set; }
        public ItemizedSummaryVM ItemizedSummaryVM { get; private set; }
        public MaterialSummaryVM MaterialSummaryVM { get; private set; }
        public ICommand ToggleTemplatesCommand { get; private set; }
        public ICommand DocumentCommand { get; private set; }
        public ICommand LoadTemplatesCommand { get; private set; }
        public ICommand CSVExportCommand { get; private set; }
        public ICommand BudgetCommand { get; private set; }
        public ICommand ExcelExportCommand { get; private set; }
        public ICommand EngineeringExportCommand { get; private set; }
        public ICommand RefreshTemplatesCommand { get; private set; }
        public ICommand RefreshBidCommand { get; private set; }
        public TECBid Bid
        {
            get { return workingScopeManager as TECBid; }
            set
            {
                workingScopeManager = value;
                Estimate = new TECEstimator(Bid, watcher);
            }
        }
        public TECTemplates Templates
        {
            get { return _templates; }
            set
            {
                _templates = value;
                RaisePropertyChanged("Templates");
            }
        }
        public TECEstimator Estimate
        {
            get { return _estimate; }
            set
            {
                _estimate = value;
                RaisePropertyChanged("Estimate");
            }
        }
        public override Visibility TemplatesVisibility
        {
            get
            {
                return ScopeEditorVM.TemplatesVisibility;
            }
            set
            {
                ScopeEditorVM.TemplatesVisibility = value;
                RaisePropertyChanged("TemplatesVisibility");
            }
        }

        protected override bool TemplatesHidden
        {
            get
            {
                return Properties.Settings.Default.TemplatesHidden;
            }
            set
            {
                if (Properties.Settings.Default.TemplatesHidden != value)
                {
                    Properties.Settings.Default.TemplatesHidden = value;
                    RaisePropertyChanged("TemplatesHidden");
                    TemplatesHiddenChanged();
                    Properties.Settings.Default.Save();
                }
            }
        }
        protected override string ScopeDirectoryPath
        {
            get { return Properties.Settings.Default.ScopeDirectoryPath; }
            set
            {
                Properties.Settings.Default.ScopeDirectoryPath = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string TemplatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                if (Properties.Settings.Default.TemplatesFilePath != value)
                {
                    Properties.Settings.Default.TemplatesFilePath = value;
                    Properties.Settings.Default.Save();
                    SettingsVM.TemplatesLoadPath = TemplatesFilePath;
                }
            }
        }
        protected override string startupFilePath
        {
            get
            {
                return Properties.Settings.Default.StartupFile;
            }
            set
            {
                Properties.Settings.Default.StartupFile = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string defaultDirectory
        {
            get
            {
                return (Properties.Settings.Default.DefaultDirectory);
            }
            set
            {
                Properties.Settings.Default.DefaultDirectory = value;
                Properties.Settings.Default.Save();
            }
        }
        protected override string defaultSaveFileName
        {
            get
            {
                return (Bid.BidNumber + " " + Bid.Name);
            }
        }
        protected override TECScopeManager workingScopeManager
        {
            get
            { return base.workingScopeManager; }
            set
            {
                base.workingScopeManager = value;
                RaisePropertyChanged("Bid");
                updateBidWithTemplates();
                refresh();
            }
        }
        
        private bool templatesLoaded
        {
            get { return _templatesLoaded; }
            set
            {
                _templatesLoaded = value;
                if (LaborVM != null)
                {
                    LaborVM.TemplatesLoaded = templatesLoaded;
                }
            }
        }
        #endregion
        #region Methods
        
        protected override void setupExtensions(MenuType menuType)
        {
            base.setupExtensions(menuType);
            setupScopeEditorVM(Bid, Templates);
            setupLaborVM(Bid, Templates);
            setupReviewVM(Bid);
            setupProposalVM(Bid);
            setupElectricalVM(Bid);
            setupNetworkVM(Bid, watcher);
            setupItemizedSummaryVM(Bid, watcher);
            setupMaterialSummaryVM(Bid, watcher);
        }
        protected override void setupCommands()
        {
            base.setupCommands();
            DocumentCommand = new RelayCommand(documentExecute);
            CSVExportCommand = new RelayCommand(csvExportExecute);
            BudgetCommand = new RelayCommand(budgetExecute);
            LoadTemplatesCommand = new RelayCommand(LoadTemplatesExecute);
            ExcelExportCommand = new RelayCommand(excelExportExecute);
            EngineeringExportCommand = new RelayCommand(engineeringExportExecute);
            RefreshTemplatesCommand = new RelayCommand(refreshTemplatesExecute);
            RefreshBidCommand = new RelayCommand(refreshBidExecute, refreshBidCanExecute);
            ToggleTemplatesCommand = new RelayCommand(toggleTemplatesExecute);
        }
        
        protected override void setupMenu(MenuType menuType)
        {
            MenuVM = new MenuVM(MenuType.EB);

            MenuVM.NewCommand = NewCommand;
            MenuVM.LoadCommand = LoadCommand;
            MenuVM.SaveCommand = SaveCommand;
            MenuVM.SaveAsCommand = SaveAsCommand;
            MenuVM.ExportProposalCommand = DocumentCommand;
            MenuVM.ExportEngineeringCommand = EngineeringExportCommand;
            MenuVM.LoadTemplatesCommand = LoadTemplatesCommand;
            MenuVM.ExportPointsListCommand = CSVExportCommand;
            MenuVM.UndoCommand = UndoCommand;
            MenuVM.RedoCommand = RedoCommand;
            //MenuVM.ExportExcelCommand = ExcelExportCommand;

            MenuVM.RefreshTemplatesCommand = RefreshTemplatesCommand;
            MenuVM.RefreshBidCommand = RefreshBidCommand;

            MenuVM.TemplatesHidden = TemplatesHidden;
            MenuVM.ToggleTemplatesCommand = ToggleTemplatesCommand;

            MenuVM.DebugWindowCommand = new RelayCommand(debugMenuExecute);
        }
        protected void LoadTemplatesExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            //User choose path
            string path = UIHelpers.GetLoadPath(UIHelpers.TemplatesFileParameters, defaultDirectory);
            if (path != null)
            {
                TemplatesFilePath = path;
                loadTemplates(TemplatesFilePath);
            }
        }
        protected void buildTitleString(string filePath)
        {
            //string bidName = "";
            //if (Bid != null)
            //{
            //    bidName = Bid.Name;
            //}
            string title = Path.GetFileNameWithoutExtension(filePath);
            TitleString = title + " - Estimate Builder";
        }
        protected override TECScopeManager NewScopeManager()
        {
            return new TECBid();
        }
        protected override void startUp(string bidPath, string templatesPath)
        {
            if (bidPath == "")
            {
                isNew = true;
                setupData(templatesPath);
            }
            else
            {
                isNew = false;
                setupData(templatesPath, bidPath);

            }
            
            buildTitleString(bidPath);
            setupCommands();
            setupExtensions(MenuType.EB);
            TemplatesFilePath = templatesPath;

            workingFileParameters = UIHelpers.EstimateFileParameters;
            CurrentVM = this;
        }
        protected override void startFromFile()
        {
            throw new NotImplementedException();
        }

        private void setupScopeEditorVM(TECBid bid, TECTemplates templates)
        {
            ScopeEditorVM = new ScopeEditorVM(bid, templates);
            ScopeEditorVM.PropertyChanged += scopeEditorVM_PropertyChanged;
            if (TemplatesHidden)
            { ScopeEditorVM.TemplatesVisibility = Visibility.Hidden; }
            else
            { ScopeEditorVM.TemplatesVisibility = Visibility.Visible; }
        }
        private void setupLaborVM(TECBid bid, TECTemplates templates)
        {
            LaborVM = new LaborVM();
            LaborVM.Bid = bid;
            LaborVM.Templates = templates;
            LaborVM.LoadTemplates += LoadTemplatesExecute;
            LaborVM.TemplatesLoaded = templatesLoaded;
        }
        private void setupReviewVM(TECBid bid)
        {
            ReviewVM = new ReviewVM();
            ReviewVM.Refresh(Estimate, bid);
        }
        private void setupProposalVM(TECBid bid)
        {
            ProposalVM = new ProposalVM(bid);
        }
        private void setupElectricalVM(TECBid bid)
        {
            ElectricalVM = new ElectricalVM(bid);
        }
        private void setupNetworkVM(TECBid bid, ChangeWatcher cw)
        {
            NetworkVM = new NetworkVM(bid, cw);
        }
        private void setupItemizedSummaryVM(TECBid bid, ChangeWatcher cw)
        {
            ItemizedSummaryVM = new ItemizedSummaryVM(bid, cw);
        }
        private void setupMaterialSummaryVM(TECBid bid, ChangeWatcher cw)
        {
            MaterialSummaryVM = new MaterialSummaryVM(bid, cw);
        }
        private void setupData(string templatesPath, string bidPath = "")
        {
            if (isNew)
            {
                Bid = new TECBid();
            }
            else
            {
                load(false, bidPath);
            }
            loadTemplates(templatesPath, false);
            updateBidWithTemplates();
        }
        private void loadTemplates(string TemplatesFilePath, bool async = true)
        {
            SetBusyStatus("Loading templates from file: " + TemplatesFilePath, false);
            templatesDB = new DatabaseManager(TemplatesFilePath);
            if (async)
            {
                templatesDB.LoadComplete += (scope) =>
                {
                    if (scope != null)
                    {
                        Templates = scope as TECTemplates;
                    }
                    ResetStatus();
                };
                templatesDB.AsyncLoad();
            } else
            {
                Templates = templatesDB.Load() as TECTemplates;
                ResetStatus();
            }
            
        }
        private void toggleTemplatesExecute()
        {
            if (TemplatesHidden)
            {
                TemplatesHidden = false;
            }
            else
            {
                TemplatesHidden = true;
            }
        }
        private void documentExecute()
        {
            string path = UIHelpers.GetSavePath(UIHelpers.WordDocumentFileParameters, defaultSaveFileName,
                defaultDirectory, ScopeDirectoryPath, isNew);

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    //ScopeDocumentBuilder.CreateScopeDocument(Bid, path, isEstimate);
                    var builder = new ScopeWordDocumentBuilder();
                    builder.CreateScopeWordDocument(Bid, Estimate, path);
                    DebugHandler.LogDebugMessage("Scope saved to document.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void csvExportExecute()
        {
            //User choose path
            string path = UIHelpers.GetSavePath(UIHelpers.CSVFileParameters, defaultSaveFileName, defaultDirectory, ScopeDirectoryPath, isNew);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BidPointsToCSV(Bid);
                    DebugHandler.LogDebugMessage("Points saved to csv.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void budgetExecute()
        {
            //new View.BudgetWindow();
            //MessengerInstance.Send<GenericMessage<ObservableCollection<TECSystem>>>(new GenericMessage<ObservableCollection<TECSystem>>(Bid.Systems));
        }
        private void excelExportExecute()
        {
            //User choose path
            string path = UIHelpers.GetSavePath(UIHelpers.CSVFileParameters, defaultSaveFileName, defaultDirectory, ScopeDirectoryPath, isNew);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    EstimateSpreadsheetExporter.Export(Bid, path);
                    DebugHandler.LogDebugMessage("Exported to estimating spreadhseet.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void engineeringExportExecute()
        {
            //User choose path
            string path = UIHelpers.GetSavePath(UIHelpers.WordDocumentFileParameters, defaultSaveFileName, defaultDirectory, ScopeDirectoryPath, isNew);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    TurnoverExporter.GenerateEngineeringExport(path, Bid, Estimate);
                    DebugHandler.LogDebugMessage("Exported to engineering turnover document.");
                }
                else
                {
                    DebugHandler.LogError("Could not open file " + path + " File is open elsewhere.");
                }
            }
        }
        private void refreshTemplatesExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (TemplatesFilePath != null)
            {
                loadTemplates(TemplatesFilePath);
            }
        }
        private void refreshBidExecute()
        {
            if (!IsReady)
            {
                MessageBox.Show("Program is busy. Please wait for current processes to stop.");
                return;
            }
            if (loadedStackLength > 0)
            {
                string message = "Would you like to save your changes before reloading?";
                MessageBoxResult result = MessageBox.Show(message, "Create new", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    SetBusyStatus("Saving...", false);
                    if (saveDelta(false))
                    {
                        SetBusyStatus("Loading...", false);
                        DebugHandler.LogDebugMessage("Reloading bid.");
                        refresh();
                    }
                    else
                    {
                        DebugHandler.LogError("Save unsuccessful. Reload bid did not occur..");
                    }
                    ResetStatus();
                }
                else if (result == MessageBoxResult.No)
                {
                    SetBusyStatus("Loading...", false);
                    DebugHandler.LogDebugMessage("Reloading bid.");
                    //Bid = load(saveFilePath) as TECBid;
                    ResetStatus();
                }
                else
                {
                    return;
                }
            }
            else
            {
                SetBusyStatus("Loading...", false);
                DebugHandler.LogDebugMessage("Reloading bid.");
                //Bid = load(saveFilePath) as TECBid;
                ResetStatus();
            }
        }
        private bool refreshBidCanExecute()
        {
            return (!isNew);
        }
        private void refresh()
        {
            if (Bid != null && Templates != null)
            {
                ScopeEditorVM.Refresh(Bid, Templates);
                LaborVM.Refresh(Bid, Estimate, Templates);
                ReviewVM.Refresh(Estimate, Bid);
                ProposalVM.Refresh(Bid);
                ElectricalVM.Refresh(Bid);
                NetworkVM.Refresh(Bid, watcher);
                ItemizedSummaryVM.Refresh(Bid, watcher);
            }
        }
        private void updateBidWithTemplates()
        {
            if (Templates != null && Bid != null)
            {
                UtilitiesMethods.UnionizeCatalogs(Bid.Catalogs, Templates.Catalogs);
                ModelLinkingHelper.LinkBidToCatalogs(Bid);
                if (isNew && Templates.Parameters.Count > 0)
                {
                    Bid.Parameters.UpdateConstants(Templates.Parameters[0]);
                }
            }
        }
        private void settingsVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesHidden")
            {
                TemplatesHidden = SettingsVM.TemplatesHidden;
            }
            else if (e.PropertyName == "TemplatesLoadPath")
            {
                TemplatesFilePath = SettingsVM.TemplatesLoadPath;
            }
        }
        private void scopeEditorVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TemplatesVisibility")
            {
                if (ScopeEditorVM.TemplatesVisibility == Visibility.Visible)
                {
                    TemplatesHidden = false;
                }
                else if (ScopeEditorVM.TemplatesVisibility == Visibility.Hidden)
                {
                    TemplatesHidden = true;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private void debugMenuExecute()
        {
            EBDebugWindow debug = new EBDebugWindow(Bid);
            debug.Show();
        }
        #endregion
        
    }
}