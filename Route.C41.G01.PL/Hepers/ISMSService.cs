using Route.C41.G01.DAL.Models;
using Twilio.Rest.Api.V2010.Account;

namespace Route.C41.G01.PL.Hepers
{
    public interface ISMSService
    {
        public MessageResource SendSMS(SMSMessage sms);
    }
}
