using Route.C41.G01.DAL.Models;

namespace Route.C41.G01.PL.Hepers
{
    public interface IMailSettings
    {
        public void SendEmail(Email email);
    }
}
