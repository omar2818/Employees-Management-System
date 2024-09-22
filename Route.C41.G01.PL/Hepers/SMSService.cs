using Microsoft.Extensions.Options;
using Route.C41.G01.DAL.Models;
using Route.C41.G01.PL.Services.Settings;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Route.C41.G01.PL.Hepers
{
    public class SMSService : ISMSService
    {
        private TwilioSettings _Options;

        public SMSService(IOptions<TwilioSettings> options)
        {
            _Options = options.Value;
        }
        public MessageResource SendSMS(SMSMessage sms)
        {
            TwilioClient.Init(_Options.AccountSID, _Options.AuthToken);

            var Result = MessageResource.Create(
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_Options.TwilioPhoneNumber),
                to: sms.PhoneNumber
                );

            return Result;
        }
    }
}
