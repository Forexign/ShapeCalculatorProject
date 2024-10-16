using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccessLayer.FormulaModels
{
    public class FormulaParameterCreateViewModel
    {
        public string FormulaName { get; set; }
        public string FormulaText { get; set; }
        public SelectList ParameterCount{ get; set; }
        public string ParameterName1 { get; set; }
        public int Order1 { get; set; } = 1;
        public string ParameterName2 { get; set; }
        public int Order2 { get; set; } = 2;
        public string ParameterName3 { get; set; }
        public int Order3 { get; set; } = 3;
        public string ParameterName4 { get; set; }
        public int Order4 { get; set; } = 4;
        public string ParameterName5 { get; set; }
        public int Order5 { get; set; } = 5;
    }
}
