using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.FormulaModels
{
    public class FormulaResult
    {
        public int FormulaResultID { get; set; }
        public int FormulaInputID { get; set; }
        public double Result { get; set; }
        public DateTime CalculatedDate { get; set; }
        public Guid UserId { get; set; }
    }
}
