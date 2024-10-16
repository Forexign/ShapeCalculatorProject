using DevExpress.Web.Mvc;
using DataAccessLayer.FormulaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace PortalMVC2.Controllers
{
    public class FormulaController : BaseController
    {
        public ActionResult Index()
        {
            var model = db.Formulas.ToList();
            return View(model);
        }
        public ActionResult CreateFormula()
        {
            var model = new FormulaParameterCreateViewModel
            {
                ParameterCount = new SelectList(new List<int> { 1, 2, 3, 4, 5 })
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateFormula(FormulaParameterCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Formula formula = new Formula
                {
                    FormulaName = model.FormulaName,
                    FormulaText = model.FormulaText
                };
                db.Formulas.Add(formula);
                db.SaveChanges();
                FormulaParameter formulaParameter1 = new FormulaParameter
                {
                    FormulaID = formula.FormulaID,
                    ParameterName = model.ParameterName1,
                    Order = 1
                };
                db.FormulaParameters.Add(formulaParameter1);

                if (model.ParameterName2 != null)
                {
                    FormulaParameter formulaParameter2 = new FormulaParameter
                    {
                        FormulaID = formula.FormulaID,
                        ParameterName = model.ParameterName2,
                        Order = 2
                    };
                    db.FormulaParameters.Add(formulaParameter2);
                }
                if (model.ParameterName3 != null)
                {
                    FormulaParameter formulaParameter3 = new FormulaParameter
                    {
                        FormulaID = formula.FormulaID,
                        ParameterName = model.ParameterName3,
                        Order = 3
                    };
                    db.FormulaParameters.Add(formulaParameter3);
                }
                if (model.ParameterName4 != null)
                {
                    FormulaParameter formulaParameter4 = new FormulaParameter
                    {
                        FormulaID = formula.FormulaID,
                        ParameterName = model.ParameterName4,
                        Order = 4
                    };
                    db.FormulaParameters.Add(formulaParameter4);
                }
                if (model.ParameterName5 != null)
                {
                    FormulaParameter formulaParameter5 = new FormulaParameter
                    {
                        FormulaID = formula.FormulaID,
                        ParameterName = model.ParameterName5,
                        Order = 5
                    };
                    db.FormulaParameters.Add(formulaParameter5);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        DataAccessLayer.DbContext.ApplicationDbContext db = new DataAccessLayer.DbContext.ApplicationDbContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {

            var model = db.Database.SqlQuery<FormulaInputGridViewModel>("\r\nDECLARE @cols AS NVARCHAR(MAX),\r\n    " +
                "    @query AS NVARCHAR(MAX);\r\nSET @cols = STUFF((SELECT DISTINCT ',' + QUOTENAME(ParameterName) \r\n" +
                "                   FROM FormulaParameters\r\n             " +
                "      FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '');\r\n\r\n" +
                "SET @query = '\r\nSELECT FormulaName, ' + @cols + '\r\nFROM \r\n(\r\n   " +
                " SELECT f.FormulaName, fp.ParameterName, fpi.ParameterValue\r\n   " +
                " FROM Formulae f\r\n   " +
                " JOIN FormulaParameters fp ON f.FormulaID = fp.FormulaID\r\n\t" +
                "left outer join FormulaParameterInputs fpi on fpi.FormulaParameterID = fp.FormulaParameterID\r\n) AS SourceTable\r\n" +
                "PIVOT\r\n(\r\n   " +
                " MAX(ParameterValue)\r\n    FOR ParameterName IN (' + @cols + ')\r\n) AS PivotTable';\r\nEXEC sp_executesql @query;\r\n")
                .AsQueryable();

            var columnNames = model.Where(i => i.FormulaName == "YeniFormül").FirstOrDefault();
            var gercekcolumnnames = db.FormulaParameters.Where(i => i.FormulaID == 2).ToList();
            columnNames.Parameters = gercekcolumnnames;

            DataTable dataTable = new DataTable();           
            dataTable.Columns.Add("FormulaName", typeof(string));
            foreach (var item in columnNames.Parameters)
            {
                dataTable.Columns.Add(item.ParameterName, typeof(string));
            }

            return PartialView("~/Views/Formula/_GridViewPartial.cshtml", dataTable);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] DataAccessLayer.FormulaModels.FormulaParameterInput item)
        {
            var model = db.FormulaParameterInputs;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Formula/_GridViewPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] DataAccessLayer.FormulaModels.FormulaParameterInput item)
        {
            var model = db.FormulaParameterInputs;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.FormulaParameterInputID == item.FormulaParameterInputID);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("~/Views/Formula/_GridViewPartial.cshtml", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 FormulaParameterInputID)
        {
            var model = db.FormulaParameterInputs;
            if (FormulaParameterInputID >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.FormulaParameterInputID == FormulaParameterInputID);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("~/Views/Formula/_GridViewPartial.cshtml", model.ToList());
        }
    }
}