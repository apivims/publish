using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model_hims
{
    public class RequestHeader
    {

		public string hfidABDM { get; set; }
		public string hfidHMIS { get; set; }
		public string fromDate { get; set; }
		public string toDate { get; set; }
		public string syncRequestType { get; set; }
		public string txn_id { get; set; }
		public string module_code { get; set; }
		public string Authorization { get; set; }
		public string msg { get; set; }



	}
}
