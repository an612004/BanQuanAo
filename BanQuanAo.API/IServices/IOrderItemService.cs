using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;

namespace BanQuanAo.API.IServices
{
    public interface IOrderItemService
    {
        public Task<bool> Create(OrderItemVM item);

        public Task<bool> Delete(Guid id);

        public Task<List<OrderItem>> GetAll();

        public Task<OrderItem> GetItem(Guid id);

        public Task<bool> Update(Guid id,  OrderItemVM item);
    }
}
