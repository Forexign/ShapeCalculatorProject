using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.FormulaModels
{
    public class FormulaInput
    {
        public int FormulaInputID { get; set; }
        public int FormulaID { get; set; }
        public bool IsCalculated { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }

    }
}
