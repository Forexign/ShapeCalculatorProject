using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.FormulaModels
{
    public class FormulaParameter
    {
        public int FormulaParameterID { get; set; }
        public int FormulaID { get; set; }
        public string ParameterName { get; set; }
        public int Order { get; set; }
    }
}
