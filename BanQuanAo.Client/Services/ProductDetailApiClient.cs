using BanQuanAo.Client.IServices;
using BanQuanAo.Share.Models;
using BanQuanAo.Share.ViewModels;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Drawing;
using System.Net.Http.Headers;
using System.Security.Policy;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;

namespace BanQuanAo.Client.Services
{
    public class ProductDetailApiClient : IproductDetailApiClient
    {
        private HttpClient _httpClient;

        public ProductDetailApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //làm thừa rồi huhu :(
        ////loại bỏ thẻ html
        //private string RemoveHtmlTags(string html)
        //{
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(html);

        //    return doc.DocumentNode.InnerText;
        //}
        //public string ReplaceUnicodeCharacters(string input)
        //{
        //    if (string.IsNullOrEmpty(input))
        //    {
        //        return input;
        //    }

        //    string normalized = input.Normalize(NormalizationForm.FormKD);
        //    Encoding removal = Encoding.GetEncoding(Encoding.UTF8.CodePage,
        //                                            new EncoderReplacementFallback(""),
        //                                            new DecoderReplacementFallback(""));
        //    byte[] bytes = removal.GetBytes(normalized);
        //    string asciiString = Encoding.UTF8.GetString(bytes);

        //    // Giải mã ký tự HTML
        //    return HttpUtility.HtmlDecode(asciiString);
        //}
        public async Task<bool> CreateProduct(ProductDetailVM request, Guid productId, Guid sizeId, Guid colorId)
        {
            string apiUrl = "https://localhost:7007/api/productDetail/create-productdetail";
            var requestContent = new MultipartFormDataContent();
            request.ProductId = productId;
            request.SizeId = sizeId;
            request.ColorId = colorId;


            requestContent.Add(new StringContent(request.ProductId.ToString()), "productid");
            requestContent.Add(new StringContent(request.ColorId.ToString()), "colorid");
            requestContent.Add(new StringContent(request.SizeId.ToString()), "sizeid");
            requestContent.Add(new StringContent(request.Quantity.ToString()), "quantity");
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.PriceSale.ToString()), "pricesale");
            requestContent.Add(new StringContent(request.Create_At.ToString()), "create_At");
            requestContent.Add(new StringContent(request.Update_At.ToString()), "update_At");
            requestContent.Add(new StringContent(request.Description), "description");
            requestContent.Add(new StringContent(request.Status.ToString()), "status");
            foreach (var file in request.filecollection)
            {
                requestContent.Add(new StreamContent(file.OpenReadStream())
                {
                    Headers =
                     {
                       ContentLength = file.Length,
                       ContentType = new MediaTypeHeaderValue(file.ContentType)
                     }
                }, "filecollection", file.FileName);
            }
            var response = await _httpClient.PostAsync(apiUrl, requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProducImage(Guid request)
        {
        
            string apiURL = $"https://localhost:7007/api/productimage/delete-productimage-{request}";
            var response = await _httpClient.DeleteAsync(apiURL);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductDetail(Guid request)
        {
            string apiURL = $"https://localhost:7007/api/productDetail/delete-productdetail-{request}";
            var response = await _httpClient.DeleteAsync(apiURL);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ProductDetailVM>> GetAllProductDetail()
        {
            string apiUrl = "https://localhost:7007/api/productDetail/get-all-productdetail";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string apiData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ProductDetailVM>>(apiData);
            return result;
        }

        public async Task<ProductDetailVM> GetByIdProductDetail(Guid productDetailId)
        {
            string apiURL = $"https://localhost:7007/api/productDetail/get/{productDetailId}";
            var response = await _httpClient.GetAsync(apiURL);
            response.EnsureSuccessStatusCode();
            string apiData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDetailVM>(apiData);
            return result;
        }

        public async Task<List<Colors>> GetListColor()
        {
            string apiUrl = "https://localhost:7007/api/color/get-all-Color";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string apiData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<Colors>>(apiData);
            return result;
        }

        public async Task<List<ProductVM>> GetListProduct()
        {
            string apiUrl = "https://localhost:7007/api/product/get-all-product";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string apiData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ProductVM>>(apiData);
            return result;
        }

        public async Task<List<ProductImage>> GetListProI()
        {
            string apiUrl = "https://localhost:7007/api/productimage/get-all-productimage";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string apiData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<ProductImage>>(apiData);
            return result;
        }

        public async Task<List<SizeVM>> GetListSize()
        {
            string apiUrl = "https://localhost:7007/api/size/get-all-size";
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string apiData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<SizeVM>>(apiData);
            return result;
        }

        public async Task<bool> UpdateProduct(ProductDetailVM request, Guid sizeId, Guid colorId)
        {
            string apiUrl = $"https://localhost:7007/api/productDetail/update-productdetail-{request.Id}";
            var requestContent = new MultipartFormDataContent();
            request.SizeId = sizeId;
            request.ColorId = colorId;
            requestContent.Add(new StringContent(request.ColorId.ToString()), "colorid");
            requestContent.Add(new StringContent(request.SizeId.ToString()), "sizeid");
            requestContent.Add(new StringContent(request.Quantity.ToString()), "quantity");
            requestContent.Add(new StringContent(request.PriceSale.ToString()), "pricesale");
            requestContent.Add(new StringContent(request.Update_At.ToString()), "Update_At");
            requestContent.Add(new StringContent(request.Status.ToString()), "Status");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            if (request.filecollection != null)
            {
                foreach (var file in request.filecollection)
                {
                    requestContent.Add(new StreamContent(file.OpenReadStream())
                    {
                        Headers =
                        {
                            ContentLength = file.Length,
                            ContentType = new MediaTypeHeaderValue(file.ContentType)
                        }
                    }, "filecollection", file.FileName);
                }
            }

            var response = await _httpClient.PutAsync(apiUrl, requestContent);
            return response.IsSuccessStatusCode;
        }


    }
}
