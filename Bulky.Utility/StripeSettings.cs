using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
	public class StripeSettings
	{
		public string SecretKey { get; set; }       //Stripe Secret Key
		public string PublishableKey { get; set; }    //Stripe Publishable Key
	}
}
