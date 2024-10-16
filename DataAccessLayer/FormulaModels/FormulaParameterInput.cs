using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.FormulaModels
{
    public class FormulaParameterInput
    {
        public int FormulaParameterInputID { get; set; }
        public int FormulaInputID { get; set; }
        public int FormulaParameterID { get; set; }
        public int ParameterValue { get; set; }
    }
}
