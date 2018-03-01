﻿using EstimatingLibrary;
using EstimatingLibrary.Utilities;
using EstimatingUtilitiesLibrary;
using EstimatingUtilitiesLibrary.Database;
using EstimatingUtilitiesLibrary.Exports;
using NLog;
using System;
using System.Windows;
using TECUserControlLibrary.BaseVMs;
using TECUserControlLibrary.Debug;
using TECUserControlLibrary.Models;
using TECUserControlLibrary.Utilities;

namespace EstimateBuilder.MVVM
{
    public class EstimateManager : AppManager<TECBid>
    {
        #region Fields and Properties
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private TECBid bid;
        private TECTemplates templates;
        private TECEstimator estimate;

        private DatabaseManager<TECTemplates> templatesDatabaseManager;

        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateMenuVM menuVM
        {
            get { return MenuVM as EstimateMenuVM; }
        }
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateEditorVM editorVM
        {
            get { return EditorVM as EstimateEditorVM; }
        }
        /// <summary>
        /// Estimate-typed splash vm for manipulation
        /// </summary>
        private EstimateSplashVM splashVM
        {
            get { return SplashVM as EstimateSplashVM; }
        }

        override protected FileDialogParameters workingFileParameters
        {
            get
            {
                return FileDialogParameters.EstimateFileParameters;
            }
        }
        override protected string defaultDirectory
        {
            get
            {
                return Properties.Settings.Default.DefaultDirectory;
            }
            set
            {
                Properties.Settings.Default.DefaultDirectory = value;
                Properties.Settings.Default.Save();
            }
        }
        override protected string defaultFileName
        {
            get
            {
                string fileName = "";
                if (bid.Name != null && bid.Name != "")
                {
                    fileName += bid.Name;
                    if (bid.BidNumber != null && bid.BidNumber != "")
                    {
                        fileName += (" - " + bid.BidNumber);
                    }
                }
                return fileName;
            }
        }
        
        override protected string templatesFilePath
        {
            get { return Properties.Settings.Default.TemplatesFilePath; }
            set
            {
                Properties.Settings.Default.TemplatesFilePath = value;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        public EstimateManager() : base("Estimate Builder", 
            new EstimateSplashVM(Properties.Settings.Default.TemplatesFilePath, Properties.Settings.Default.DefaultDirectory), new EstimateMenuVM())
        {
            splashVM.BidPath = getStartUpFilePath();
            splashVM.EditorStarted += userStartedEditorHandler;
            TitleString = "Estimate Builder";
            setupCommands();
        }
        
        private void userStartedEditorHandler(string bidFilePath, string templatesFilePath)
        {
            this.templatesFilePath = templatesFilePath;
            buildTitleString(bidFilePath, "Estimate Builder");
            if(templatesFilePath != "")
            {
                templatesDatabaseManager = new DatabaseManager<TECTemplates>(templatesFilePath);
                templatesDatabaseManager.LoadComplete += assignData;
                ViewEnabled = false;
                templatesDatabaseManager.AsyncLoad();
            }
            else
            {
                assignData(new TECTemplates());
            }

            void assignData(TECTemplates loadedTemplates)
            {
                if (templatesDatabaseManager != null)
                {
                    templatesDatabaseManager.LoadComplete -= assignData;
                }

                templates = loadedTemplates;
                if (bidFilePath != "")
                {
                    ViewEnabled = false;
                    databaseManager = new DatabaseManager<TECBid>(bidFilePath);
                    databaseManager.LoadComplete += handleLoaded;
                    databaseManager.AsyncLoad();
                }
                else
                {
                    handleLoaded(getNewWorkingScope());
                }
            }
        }

        protected override void handleLoaded(TECBid loadedBid)
        {
            if (loadedBid != null && templates != null)
            {
                bid = loadedBid;
                watcher = new ChangeWatcher(bid);
                doStack = new DoStacker(watcher);
                deltaStack = new DeltaStacker(watcher, bid);
                bid.Catalogs.Fill(templates.Catalogs);
                ModelLinkingHelper.LinkBidToCatalogs(bid);

                estimate = new TECEstimator(bid, watcher);

                EditorVM = new EstimateEditorVM(bid, templates, watcher, estimate);
                CurrentVM = EditorVM;
            }
            else
            {
                this.splashVM.LoadingText = "";
            }
            ViewEnabled = true;
        }
        private void handleLoadedTemplates(TECTemplates templates)
        {
            this.templates = templates;
            bid.Catalogs.Fill(templates.Catalogs);
            ModelLinkingHelper.LinkBidToCatalogs(bid);
            estimate = new TECEstimator(bid, watcher);
            editorVM.Refresh(bid, this.templates, watcher, estimate);
        }
        
        #region Menu Commands Methods
        private void setupCommands()
        {
            menuVM.SetLoadTemplatesCommand(loadTemplatesExecute, canLoadTemplates);
            menuVM.SetRefreshBidCommand(refreshExecute, canRefresh);
            menuVM.SetRefreshTemplatesCommand(refreshTemplatesExecute, canRefreshTemplates);
            menuVM.SetExportProposalCommand(exportProposalExecute, canExportProposal);
            menuVM.SetExportTurnoverCommand(exportTurnoverExecute, canExportTurnover);
            menuVM.SetExportPointsListExcelCommand(exportPointsListExcelExecute, canExportPointsListExcel);
            menuVM.SetExportPointsListCSVCommand(exportPointsListCSVExecute, canExportPointsListCSV);
            menuVM.SetExportSummaryCommand(exportSummaryExecute, canExportSummary);
            menuVM.SetExportBudgetCommand(exportBudgetExecute, canExportBudget);
            menuVM.SetExportBOMCommand(exportBOMExecute, canExportBOM);
            menuVM.SetDebugWindowCommand(debugWindowExecute, canDebugWindow);
            menuVM.SetUpdateCatalogsCommand(updateCatalogsExecute, canUpdateCatalogs);
        }
        
        //Load Templates
        private void loadTemplatesExecute()
        {
            string message = "Would you like to save your changes before loading new templates?";

            checkForChanges(message, loadTemplates);

            void loadTemplates()
            {
                string loadFilePath = UIHelpers.GetLoadPath(FileDialogParameters.TemplatesFileParameters, defaultDirectory);
                if (loadFilePath != null)
                {
                    ViewEnabled = false;
                    StatusBarVM.CurrentStatusText = "Loading Templates...";
                    templatesDatabaseManager = new DatabaseManager<TECTemplates>(loadFilePath);
                    templatesDatabaseManager.LoadComplete += handleTemplatesLoadComplete;
                    templatesDatabaseManager.AsyncLoad();
                }
            }
        }
        private bool canLoadTemplates()
        {
            return databaseReady();
        }
        private void handleTemplatesLoadComplete(TECTemplates templates)
        {
            handleLoadedTemplates(templates);
            StatusBarVM.CurrentStatusText = "Ready";
            ViewEnabled = true;
        }
        //Refresh Templates
        private void refreshTemplatesExecute()
        {
            string message = "Would you like to save your changes before refreshing?";
            checkForChanges(message, refreshTemplates, () => { ViewEnabled = true; });

            void refreshTemplates()
            {
                ViewEnabled = false;
                StatusBarVM.CurrentStatusText = "Loading...";
                templatesDatabaseManager.LoadComplete += handleTemplatesLoadComplete;
                templatesDatabaseManager.AsyncLoad();
            }
        }
        private bool canRefreshTemplates()
        {
            return templatesDatabaseManager != null && databaseReady();
        }
        //Update Catalogs
        private void updateCatalogsExecute()
        {
            string message = "Updating catalogs could change pricing. Continue?";
            MessageBoxResult result = MessageBox.Show(message, "Save", MessageBoxButton.YesNo,
                    MessageBoxImage.Exclamation);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    bid.Catalogs.Unionize(templates.Catalogs);
                    ModelLinkingHelper.LinkBidToCatalogs(bid);
                    estimate = new TECEstimator(bid, watcher);
                    editorVM.Refresh(bid, this.templates, watcher, estimate);
                    break;
                case MessageBoxResult.No:
                    return;
                default:
                    return;
            }

        }
        private bool canUpdateCatalogs()
        {
            return templates != null;
        }
        //Export Proposal
        private void exportProposalExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.WordDocumentFileParameters, 
                defaultFileName, defaultDirectory, workingFileDirectory);

            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    ScopeWordDocumentBuilder.CreateScopeWordDocument(bid, estimate, path);
                    logger.Info("Scope saved to document.");
                }
                else
                {
                    notifyFileLocked(path);
                }
            }
        }
        private bool canExportProposal()
        {
            return true;
        }
        //Export Turnover Sheet
        private void exportTurnoverExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Turnover.GenerateTurnoverExport(path, bid, estimate);
                    logger.Info("Exported to turnover document.");
                }
                else
                {
                    notifyFileLocked(path);
                }
            }
        }
        private bool canExportTurnover()
        {
            return true;
        }
        //Export Points List (Excel)
        private void exportPointsListExcelExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                            defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    PointsList.ExportPointsList(path, bid);
                    logger.Info("Points saved to Excel.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportPointsListExcel()
        {
            return true;
        }
        //Export Points List (CSV)
        private void exportPointsListCSVExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.CSVFileParameters,
                            defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    CSVWriter writer = new CSVWriter(path);
                    writer.BidPointsToCSV(bid);
                    logger.Info("Points saved to csv.");
                }
                else
                {
                    logger.Warn("Could not open file {0}. File is open elsewhere.", path);
                }
            }
        }
        private bool canExportPointsListCSV()
        {
            return true;
        }
        //Export Summary
        private void exportSummaryExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.WordDocumentFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Turnover.GenerateSummaryExport(path, bid, estimate);
                    logger.Info("Exported to summary turnover document.");
                }
                else
                {
                    notifyFileLocked(path);
                }
            }
        }
        private bool canExportSummary()
        {
            return true; 
        }
        //Export Budget
        private void exportBudgetExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Budget.GenerateReport(path, bid);
                    logger.Info("Exported to budget document.");
                }
                else
                {
                    notifyFileLocked(path);
                }
            }
        }
        private bool canExportBudget()
        {
            return true;
        }
        //Export Budget
        private void exportBOMExecute()
        {
            string path = UIHelpers.GetSavePath(FileDialogParameters.ExcelFileParameters,
                                        defaultFileName, defaultDirectory, workingFileDirectory);
            if (path != null)
            {
                if (!UtilitiesMethods.IsFileLocked(path))
                {
                    Turnover.GenerateBOM(path, bid);
                    logger.Info("Exported to BOM document.");
                }
                else
                {
                    notifyFileLocked(path);
                }
            }
        }
        private bool canExportBOM()
        {
            return true;
        }
        //Debug Window
        private void debugWindowExecute()
        {
            var dbWindow = new EBDebugWindow(bid);
            dbWindow.Show();
        }
        private bool canDebugWindow()
        {
            return true;
        }
        #endregion
        
        private string getStartUpFilePath()
        {
            string startUpFilePath = Properties.Settings.Default.StartUpFilePath;
            Properties.Settings.Default.StartUpFilePath = null;
            Properties.Settings.Default.Save();
            return startUpFilePath;
        }
        protected override TECBid getWorkingScope()
        {
            return bid;
        }
        protected override TECBid getNewWorkingScope()
        {
            TECBid outBid = new TECBid();
            if(templates!= null && templates.Parameters.Count > 0)
            {
                outBid.Parameters = templates.Parameters[0];
            }
            return outBid;
        }
    }
}
