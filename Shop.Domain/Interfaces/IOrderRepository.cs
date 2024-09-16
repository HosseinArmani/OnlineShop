using Shop.Domain.Models.Account;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
   public interface IOrderRepository
    {
        Task<Order> CheckUserOrder(long userId);
        Task AddOrder(Order order);
        void UpdateOrder(Order order);
        Task<Order> GetOrderById(long orderId);
        Task<Order> GetOrderById(long orderId,long userId);
        Task<OrderDetail> CheckOrderDetail(long orderId, long productId);
        void UpdateOrderDetail(OrderDetail orderDetail);
        Task AddOrderDetail(OrderDetail orderDetail);
        Task<int> SumOrder(long orderId);
        Task<Order> GetUserBasketForUserPanel(long orderId,long userId);
        Task<Order> GetShowBasket( long userId);
        Task<Transportation> GetTransportations();
        Task<Discount> GetDiscountByCode(string code);
        void UpdateDisccount(Discount discount);
        Task<bool> GetUserUsed(long userId, long discountId);
        Task AddUserDiscount(long userId,long discountId);
        Task<FilterOrderForUserPanel> FilterOrdersForUserPanel(FilterOrderForUserPanel filterOrders);
        #region Admin
        Task AddDiscount(Discount discount);
        Task<Discount> GetDiscountById(long discountId);
        Task<List<Discount>> GetAllDiscount();
        Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders);
        Task<ResultOrderStateViewModel> GetResultOrder();
        Task<Order> GetOrderDetail(long orderId);
        Task<OrderDetail> GetOrderDetailById(long id);
        void RemoveOrderDetail(OrderDetail orderDetail);
        Task<FilterExpiredOrders> FilterExpiredOrders(FilterExpiredOrders filterExpired);
        Task RemoveOrderExpiredForAdmin(long orderId);
        Task<Transportation> GetTransportationById(long id);
        void UpdateTransportation(Transportation transportation);
        #endregion

        Task SaveChange();
    }
}
