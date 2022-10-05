using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cazino.DB
{
    public partial class CazinoEntities
    {
        private static CazinoEntities cazinoEntities;

        public static CazinoEntities GetContext()
        {
            if (cazinoEntities == null)
                cazinoEntities = new CazinoEntities();
            return cazinoEntities;
        }
    }
}
