using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    public static class StaticDetails
    {
        //All Roles Constents
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

		//All Status Level Order Summary Constents    (Mainly Two Roles 'Customer' and 'Company')
		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProgress = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelld = "Cancelled";
		public const string StatusRefund = "Refunded";


		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejected = "Rejected";		
	}
}
