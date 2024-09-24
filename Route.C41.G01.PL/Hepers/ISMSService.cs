using Twilio.Rest.Api.V2010.Account;
using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.PL.Hepers
{
	public interface ISMSService
	{
		public void SendSMS(SMSMessage sms);
	}
}
