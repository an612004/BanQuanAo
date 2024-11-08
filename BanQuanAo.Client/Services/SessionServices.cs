using BanQuanAo.Share.Models;
using Newtonsoft.Json;

namespace BanQuanAo.Client.Services
{
    public static class SessionServices
    {

        // Lấy dữ liệu từ session trả về 1 list sản phẩm
        public static List<CartItem> GetCartItemFromSession(ISession session, string key)
        {
            // Bước 1: Lấy string data từ session ở dạng json
            string jsonData = session.GetString(key);
            if (jsonData == null) return new List<CartItem>();
            // Nếu dữ liệu null thì tạo mới 1 list rỗng
            // bước 2: Convert về List
            var CartItems = JsonConvert.DeserializeObject<List<CartItem>>(jsonData);
            return CartItems;
        }
        // Ghi dữ liệu từ 1 list vào session
        public static void SetCartItemToSession(ISession session, string key, object values)
        {
            var jsonData = JsonConvert.SerializeObject(values);
            session.SetString(key, jsonData);
        }
        public static bool CheckExistProduct(Guid id, List<CartItem> CartItems)
        {
            return CartItems.Any(x => x.Id == id);
        }
        // Kiểm tra sự tồn tại của sp trong List
    }
}
