﻿using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using EstimatingLibrary;
using EstimatingLibrary.Interfaces;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EstimatingUtilitiesLibrary.Exports
{
    internal static class Turnover
    {
        const int MAX_SHEETNAME_CHARACTERS = 31;

        internal static string accountingFormat = "_($* #,##0.00_);_($* (#,##0.00);_($* \" - \"??_);_(@_)";

        internal static void GenerateTurnoverExport(string path, TECBid bid, TECEstimator estimate, bool openOnComplete = true)
        {
            XLWorkbook workbook = new XLWorkbook();

            createSummarySheet(workbook, bid, estimate);
            MaterialSummaryExport.AddControllersSheet(workbook, bid, estimate, "Controller Hardware");
            MaterialSummaryExport.AddPanelsSheet(workbook, bid, "Panel Hardware");
            MaterialSummaryExport.AddDevicesSheet(workbook, bid);
            MaterialSummaryExport.AddValvesSheet(workbook, bid);
            MaterialSummaryExport.AddElectricalMaterialSheet(workbook, bid);
            MaterialSummaryExport.AddMiscCostsSheet(workbook, bid);
            workbook.SaveAs(path);
            
            if (openOnComplete)
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        internal static void GenerateSummaryExport(string path, TECBid bid, TECEstimator estimate, bool openOnComplete = true)
        {
            using (WordprocessingDocument package = WordprocessingDocument.Create(path, 
                DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                package.AddMainDocumentPart();
                Document document = new Document();
                Body body = new Body();
                document.Append(body);
                package.MainDocumentPart.Document = document;

                body.Append(introParagraph(bid));
                body.Append(laborSummary(estimate));
                body.Append(costSummary(estimate));
                body.Append(saleSummary(estimate));

                package.MainDocumentPart.Document.Save();
            }
            if (openOnComplete)
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        internal static void GenerateBOM(string path, TECBid bid, bool openOnComplete = true)
        {
            XLWorkbook workbook = new XLWorkbook();
            createBomSheets(workbook, bid);
            workbook.SaveAs(path);
            if (openOnComplete)
            {
                System.Diagnostics.Process.Start(path);
            }
        }

        private static void createBomSheets(XLWorkbook workbook, TECBid bid)
        {
            int postfix = 1;
            List<String> sheetNames = new List<string>();
            foreach (TECTypical typical in bid.Systems.Where(typ => typ.Instances.Count > 0))
            {
                List<ICost> associatedCosts = new List<ICost>();
                string sheetName = typical.Instances.Count > 1 ? typical.Name : typical.Instances[0].Name;
                if (sheetName == "")
                {
                    sheetName = "Untitled";
                }

                int postFixChars = 5;
                int maxChars = MAX_SHEETNAME_CHARACTERS - postFixChars;

                if (sheetName.Count() > maxChars)
                {
                    sheetName = sheetName.Substring(0, maxChars);
                }
                if (sheetNames.Contains(sheetName))
                {
                    sheetName = sheetName + "(" + postfix + ")";
                    postfix++;
                }
                sheetNames.Add(sheetName);

                IXLWorksheet worksheet = workbook.Worksheets.Add(sheetName);
                worksheet.Cell(1, 4).Value = sheetName;
                worksheet.Cell(1, 4).Style.Font.SetBold();
                if (typical.Instances.Count > 1)
                {
                    worksheet.Cell(1, 5).Value = "(Quantity: " + typical.Instances.Count + ")";
                }
                int x = 3;
                worksheet.Cell(x, 4).Value = "FIELD MATERIAL";
                x++;
                worksheet.Cell(x, 1).Value = "TAG";
                worksheet.Cell(x, 1).Style.Font.SetBold();
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "QTY.";
                worksheet.Cell(x, 2).Style.Font.SetBold();
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = "MANUFACTURER";
                worksheet.Cell(x, 3).Style.Font.SetBold();
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 4).Value = "MODEL NO.";
                worksheet.Cell(x, 4).Style.Font.SetBold();
                worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 5).Value = "DESCRIPTION";
                worksheet.Cell(x, 5).Style.Font.SetBold();
                worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 6).Value = "REMARKS";
                worksheet.Cell(x, 6).Style.Font.SetBold();
                worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
                List<IEndDevice> devices = new List<IEndDevice>();
                associatedCosts.AddRange(typical.AssociatedCosts);
                foreach (TECEquipment equipment in typical.Equipment)
                {
                    associatedCosts.AddRange(equipment.AssociatedCosts);
                    foreach (TECSubScope subScope in equipment.SubScope)
                    {
                        associatedCosts.AddRange(subScope.AssociatedCosts);
                        devices.AddRange(subScope.Devices);
                    }
                }
                foreach (IEndDevice device in devices.Distinct())
                {
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = devices.Count(item => item == device);
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    if (device is TECHardware hardware) { worksheet.Cell(x, 3).Value = hardware.Manufacturer.Label; }
                    else{ worksheet.Cell(x, 3).Value = "NA"; }
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 4).Value = device.Name;
                    worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 5).Value = device.Description;
                    worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                foreach (TECController controller in typical.Controllers)
                {
                    associatedCosts.AddRange(controller.AssociatedCosts);
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = "1";
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = controller is TECProvidedController provided ? provided.Type.Manufacturer.Label : "FBO";
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 4).Value = controller is TECProvidedController provided1 ? provided1.Type.Name : "FBO";
                    worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 5).Value = controller is TECProvidedController provided2 ? provided2.Type.Description : "FBO";
                    worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                foreach (TECPanel panel in typical.Panels)
                {
                    associatedCosts.AddRange(panel.AssociatedCosts);
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = "1";
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = panel.Type.Manufacturer.Label;
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 4).Value = panel.Type.Name;
                    worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 5).Value = panel.Type.Description;
                    worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                x++;
                worksheet.Cell(x, 4).Value = "OTHER MATERIAL";
                x++;
                worksheet.Cell(x, 1).Value = "Name";
                worksheet.Cell(x, 1).Style.Font.SetBold();
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "QTY.";
                worksheet.Cell(x, 2).Style.Font.SetBold();
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = "DESCRIPTION";
                worksheet.Cell(x, 3).Style.Font.SetBold();
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
                foreach (TECCost cost in associatedCosts.Distinct())
                {
                    worksheet.Cell(x, 1).Value = cost.Name;
                    worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 2).Value = associatedCosts.Count(item => item == cost);
                    worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(x, 3).Value = cost.Description;
                    worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    x++;
                }
                worksheet.Columns().AdjustToContents();

            }

            createMiscBOMSheet(workbook, bid);

        }

        private static void createMiscBOMSheet(XLWorkbook workbook, TECBid bid)
        {
            List<ICost> associatedCosts = new List<ICost>();
            IXLWorksheet worksheet = workbook.Worksheets.Add("Misc.");
            worksheet.Cell(1, 4).Value = "Misc.";
            worksheet.Cell(1, 4).Style.Font.SetBold();
            int x = 3;
            worksheet.Cell(x, 4).Value = "FIELD MATERIAL";
            x++;
            worksheet.Cell(x, 1).Value = "TAG";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 2).Value = "QTY.";
            worksheet.Cell(x, 2).Style.Font.SetBold();
            worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 3).Value = "MANUFACTURER";
            worksheet.Cell(x, 3).Style.Font.SetBold();
            worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 4).Value = "MODEL NO.";
            worksheet.Cell(x, 4).Style.Font.SetBold();
            worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 5).Value = "DESCRIPTION";
            worksheet.Cell(x, 5).Style.Font.SetBold();
            worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 6).Value = "REMARKS";
            worksheet.Cell(x, 6).Style.Font.SetBold();
            worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;
            
            foreach (TECController controller in bid.Controllers)
            {
                associatedCosts.AddRange(controller.AssociatedCosts);
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "1";
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = controller is TECProvidedController provided ? provided.Type.Manufacturer.Label : "FBO";
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 4).Value = controller is TECProvidedController provided1 ? provided1.Type.Name : "FBO";
                worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 5).Value = controller is TECProvidedController provided2 ? provided2.Type.Description : "FBO";
                worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
            }
            foreach (TECPanel panel in bid.Panels)
            {
                associatedCosts.AddRange(panel.AssociatedCosts);
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = "1";
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = panel.Type.Manufacturer.Label;
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 4).Value = panel.Type.Name;
                worksheet.Cell(x, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 5).Value = panel.Type.Description;
                worksheet.Cell(x, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
            }
            x++;
            worksheet.Cell(x, 4).Value = "OTHER MATERIAL";
            x++;
            worksheet.Cell(x, 1).Value = "Name";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 2).Value = "QTY.";
            worksheet.Cell(x, 2).Style.Font.SetBold();
            worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Cell(x, 3).Value = "DESCRIPTION";
            worksheet.Cell(x, 3).Style.Font.SetBold();
            worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;
            foreach (TECCost cost in associatedCosts.Distinct())
            {
                worksheet.Cell(x, 1).Value = cost.Name;
                worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 2).Value = associatedCosts.Count(item => item == cost);
                worksheet.Cell(x, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(x, 3).Value = cost.Description;
                worksheet.Cell(x, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                x++;
            }
            worksheet.Columns().AdjustToContents();

        }

        private static void createSummarySheet(XLWorkbook workbook, TECBid bid, TECEstimator estimate)
        {
            IXLWorksheet worksheet = workbook.Worksheets.Add("Summary");

            createProjectInfoSection(worksheet, bid, 1);
            createCostSummarySection(worksheet, estimate, 7);
            createLaborSummarySection(worksheet, estimate, 20, 1);
            createSalesSummarySection(worksheet, estimate, 20, 4);
            
            var image = worksheet.AddPicture(createPlotImage(estimate));
            image.MoveTo(worksheet.Cell(28, 1).Address);
            //image.Scale(.7);

            worksheet.Columns().AdjustToContents();
            worksheet.PageSetup.PageOrientation = XLPageOrientation.Portrait;
            worksheet.PageSetup.FitToPages(1, 1);
        }

        private static void createProjectInfoSection(IXLWorksheet worksheet, TECBid bid, int startRow)
        {
            int x = startRow;
            worksheet.Cell(x, 1).Value = "Project Information";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;

            worksheet.Cell(x, 1).Value = "Name";
            worksheet.Cell(x, 2).Value = bid.Name;
            x++;
            worksheet.Cell(x, 1).Value = "Bid Number";
            worksheet.Cell(x, 2).Value = bid.BidNumber;
            x++;
            worksheet.Cell(x, 1).Value = "Salesperson";
            worksheet.Cell(x, 2).Value = bid.Salesperson;
            x++;
            worksheet.Cell(x, 1).Value = "Estimator";
            worksheet.Cell(x, 2).Value = bid.Estimator;
            x++;

            x = startRow + 1;
            worksheet.Cell(x, 4).Value = "Tax Exempt";
            worksheet.Cell(x, 5).Value = bid.Parameters.IsTaxExempt ? "Yes" : "No";
            x++;
            worksheet.Cell(x, 4).Value = "Bond Required";
            worksheet.Cell(x, 5).Value = bid.Parameters.RequiresBond ? "Yes" : "No";

        }

        private static void createCostSummarySection(IXLWorksheet worksheet, TECEstimator estimate, int startRow)
        {
            int x = startRow;
            worksheet.Cell(x, 1).Value = "Costs Summary";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            worksheet.Cell(x, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;
            worksheet.Cell(x, 1).Value = "TEC";
            worksheet.Cell(x, 1).Style.Font.SetBold();
            x++;
            worksheet.Cell(x, 1).Value = "Material Cost";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.TECMaterialCost);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 1).Value = "Tax";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.Tax);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 1).Value = "Shipping";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.TECShipping);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 1).Value = "Warranty";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.TECWarranty);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 1).Value = "Labor";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.TECLaborCost);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 1).Value = "Escalation";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.Escalation);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 1).Value = "Total Cost";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.TECCost);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x += 2;
            worksheet.Cell(x, 1).Value = "Subtotal";
            worksheet.Cell(x, 2).Value = String.Format("{0}", estimate.TECSubtotal);
            worksheet.Cell(x, 2).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 2).DataType = XLCellValues.Number;
            x++;

            x = startRow + 1;
            worksheet.Cell(x, 4).Value = "Subcontractor";
            worksheet.Cell(x, 4).Style.Font.SetBold();
            x++;

            worksheet.Cell(x, 4).Value = "Material Cost";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.ElectricalMaterialCost);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 4).Value = "Shipping";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.ElectricalShipping);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 4).Value = "Warranty";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.ElectricalWarranty);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 4).Value = "Labor";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.SubcontractorLaborCost);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 4).Value = "Total Cost";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.SubcontractorCost);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
            x += 2;
            worksheet.Cell(x, 4).Value = "Escalation";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.ElectricalEscalation);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, 4).Value = "Subtotal";
            worksheet.Cell(x, 5).Value = String.Format("{0}", estimate.SubcontractorSubtotal);
            worksheet.Cell(x, 5).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, 5).DataType = XLCellValues.Number;
        }

        private static void createLaborSummarySection(IXLWorksheet worksheet, TECEstimator estimate, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            int yPrime = y + 1;

            worksheet.Cell(x, y).Value = "Labor Summary";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;

            worksheet.Cell(x, y).Value = "Project Management (Hours)";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.PMLaborHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, y).Value = "Engineering (Hours)";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.ENGLaborHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, y).Value = "Software (Hours)";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.SoftLaborHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, y).Value = "Commissioning (Hours)";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.CommLaborHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, y).Value = "Graphics (Hours)";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.GraphLaborHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;
            worksheet.Cell(x, y).Value = "Field (Hours)";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.TECFieldHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;

            worksheet.Cell(x, y).Value = "Total Hours";
            worksheet.Cell(x, y).Style.Font.SetBold();

            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.TECLaborHours);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            worksheet.Cell(x, yPrime).Style.Font.SetBold();
            
        }

        private static void createSalesSummarySection(IXLWorksheet worksheet, TECEstimator estimate, int startRow, int startColumn)
        {
            int x = startRow;
            int y = startColumn;
            int yPrime = y + 1;

            worksheet.Cell(x, y).Value = "Sale Summary";
            worksheet.Cell(x, y).Style.Font.SetBold();
            worksheet.Cell(x, y).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            x++;

            worksheet.Cell(x, y).Value = "Price";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", estimate.TotalPrice);
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = accountingFormat;
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
            x++;

            worksheet.Cell(x, y).Value = "Margin";
            worksheet.Cell(x, yPrime).Value = String.Format("{0}", (estimate.Margin / 100));
            worksheet.Cell(x, yPrime).Style.NumberFormat.Format = "0.00%";
            worksheet.Cell(x, yPrime).DataType = XLCellValues.Number;
        }

        private static string createPlotImage(TECEstimator estimate)
        {
            string path = Path.GetTempFileName();
            var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
            PlotModel plotModel = new PlotModel { Title = "Cost Distribution" };
            OxyPlot.Series.PieSeries pieSeries = new OxyPlot.Series.PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            pieSeries.Slices.Add(new PieSlice("Material Cost", estimate.TECMaterialCost) { IsExploded = false });
            pieSeries.Slices.Add(new PieSlice("Labor Cost", estimate.TECLaborCost) { IsExploded = false });
            pieSeries.Slices.Add(new PieSlice("Sub. Labor Cost", estimate.SubcontractorLaborCost) { IsExploded = false });
            pieSeries.Slices.Add(new PieSlice("Sub. Material Cost", estimate.ElectricalMaterialCost) { IsExploded = false });
            plotModel.Series.Add(pieSeries);

            pngExporter.ExportToFile(plotModel, path);
            return path;
        }

        private static Paragraph introParagraph(TECBid bid)
        {
            Paragraph par = new Paragraph();
            Text nameText = new Text(String.Format("Bid: {0}", bid.Name));
            Text numberText = new Text(String.Format("Number: {0}", bid.BidNumber));
            Text salespersonText = new Text(String.Format("Salesperson: {0}", bid.Salesperson));
            Text estimatorText = new Text(String.Format("Estimator: {0}", bid.Estimator));
            par.Append(new Run(nameText));
            par.Append(new Break());
            par.Append(new Run(numberText));
            par.Append(new Break());
            par.Append(new Run(salespersonText));
            par.Append(new Break());
            par.Append(new Run(estimatorText));
            par.Append(new Break());
            return par;
        }

        private static Paragraph laborSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text pmLaborText = new Text(String.Format("Project Management Labor: {0:F} hours", estimate.PMLaborHours));
            Text engLaborText = new Text(String.Format("Engineering Labor: {0:F} hours", estimate.ENGLaborHours));
            Text softLaborText = new Text(String.Format("Software Labor: {0:F} hours", estimate.SoftLaborHours));
            Text commLaborText = new Text(String.Format("Commissioning Labor: {0:F} hours", estimate.CommLaborHours));
            Text graphLaborText = new Text(String.Format("Graphics Labor: {0:F} hours", estimate.GraphLaborHours));
        
            par.Append(new Run(pmLaborText));
            par.Append(new Break());
            par.Append(new Run(engLaborText));
            par.Append(new Break());
            par.Append(new Run(softLaborText));
            par.Append(new Break());
            par.Append(new Run(commLaborText));
            par.Append(new Break());
            par.Append(new Run(graphLaborText));

            return par;
        }

        private static Paragraph costSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text materialText = new Text(String.Format("Material Cost: {0:C}", estimate.TECMaterialCost));
            Text subLaborText = new Text(String.Format("Subcontractor Labor Hours: {0}", estimate.SubcontractorLaborHours));
            Text subLaborCostText = new Text(String.Format("Subcontractor Labor Cost: {0:C}", estimate.SubcontractorLaborCost));
            Text subMaterialText = new Text(String.Format("Subcontractor Material: {0:C}", estimate.ElectricalMaterialCost));
            
            par.Append(new Break());
            par.Append(new Run(materialText));
            par.Append(new Break());
            par.Append(new Run(subLaborText));
            par.Append(new Break());
            par.Append(new Run(subLaborCostText));
            par.Append(new Break());
            par.Append(new Run(subMaterialText));

            return par;
        }

        private static Paragraph saleSummary(TECEstimator estimate)
        {
            Paragraph par = new Paragraph();
            Text saleText = new Text(String.Format("Sale Price: {0:C}", estimate.TotalPrice));
            Text marginText = new Text(String.Format("Margin: %{0:F2}", estimate.Margin));

            par.Append(new Break());
            par.Append(new Run(saleText));
            par.Append(new Break());
            par.Append(new Run(marginText));

            return par;
        }
    }
}
