using GroceryStoreEF6.Model.ResponseModel;

namespace GroceryStoreEF6.Services
{
    public interface IEmailService
    {
        public void SendEmail(MessageResponse message);
    }
}
