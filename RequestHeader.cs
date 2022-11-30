using System;

public class RequestHeader
{
	public RequestHeader()
	{
		public string hfidABDM { get; set; }
		public string hfidHMIS { get; set; }
		public DateTime fromDate { get; set; }
		public DateTime toDate { get; set; }
		public string syncRequestType { get; set; }
		public string txn_id { get; set; }
		public string module_code { get; set; }
	    public string Authorization { get; set; }
   }
}
