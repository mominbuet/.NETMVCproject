using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;

namespace OG_SLN.Models
{
    public class DcrPublishViewModel
    {
        public DcrExpenseModel expenses { get; set; }
        public DcrSpecimenModel specimens { get; set; }

        public List<DcrDetailsModel> details { get; set; }
    }

    public class DcrExpenseModel
    {
        public SEC_USERS user { get; set; }

        public DateTime? date { get; set; }

        public List<DCR_PUB_EXP_GET_Result> expense { get; set; }
    }

    public class DcrSpecimenModel
    {
        public SEC_USERS user { get; set; }

        public DateTime? date { get; set; }

        public List<DCR_PUB_SPECIMEN_GET_Result> specimen { get; set; }
    }

    public class DcrDetailsModel
    {
        public DateTime? date { get; set; }

        public List<TRN_EXPENSE_APPROVAL_GET_Result> expense { get; set; }

        public List<DCR_PUB_DCR_DETAIL_GET_Result> detail { get; set; }
    }
}