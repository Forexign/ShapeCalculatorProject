using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.FormulaModels
{
    public class FormulaInputGridViewModel
    {
        public int FormulaID { get; set; }
        public string FormulaName { get; set; }
        public List<FormulaParameter> Parameters { get; set; }
    }
}
