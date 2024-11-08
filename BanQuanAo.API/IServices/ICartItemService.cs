using BanQuanAo.Share.Models;

namespace BanQuanAo.API.IServices
{
    public interface ICartItemService
    {
        public Task<List<CartItem>> GetAll();
        public Task<bool> AddCartItem(CartItem item);

        // Read (Lấy danh sách các mục trong CartItem bởi CartId)
        public Task<CartItem> GetCartItemsByCartIds(Guid ?cartItemId);
        public Task<IEnumerable<CartItem>> GetAllCartItemsByCartId(Guid CartId);

        // Update (Cập nhật thông tin một mục trong CartItem)
        public Task<bool> UpdateCartItem(Guid cartItemId, int? newquantity, int? newPrice);
        public Task<bool> UpdateQuantity(Guid cartItemId, int? newquantity);
        public Task<bool> UpdateQuantityCartItem(CartItem cartItem);

        // Delete (Xóa một mục trong CartItem)
        public Task<bool> DeleteCartItem(Guid CartId);
        public  Task<bool> Delete1Item(Guid CartItemId);
    }
}
