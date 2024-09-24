using Microsoft.Extensions.Options;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Services.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Route.C41.G01.PL.Hepers
{
	public class SMSService : ISMSService
	{
		private TwilioSettings _options;

		public SMSService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }
        public void SendSMS(SMSMessage sms)
		{
			TwilioClient.Init(_options.AccountSID, _options.AuthToken);

			var Result = MessageResource.Create(
				body: sms.Body,
				from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
				to: sms.PhoneNumber
				);
		}
	}
}
