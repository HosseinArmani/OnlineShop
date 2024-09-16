using Shop.Application.Utils;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Interfaces
{
   public interface IOrderService
    {
        Task<long> AddOrder(long userId, long productId);
        Task UpdatePriceOrder(long orderId);
        Task<Order> GetUserBasketForUserPanel(long orderId, long userId);
        Task<Order> GetShowBasket(long userId);
        Task<FinallyOrderResult> FinallyOrder(FinallyOrderViewzModel finallyOrder, long userId);
        Task<Transportation> GetTransportations();
        Task<DiscountUseType> UseDiscount(long orderId, string code);
        Task<Order> GetOrderById(long orderId);
        Task<FilterOrderForUserPanel> FilterOrdersForUserPanel(FilterOrderForUserPanel filterOrders);
        #region Admin
        Task<CreateDiscountCodeResult> CreateDiscount(CreateDiscountCodeViewModel create);
        Task<EditDiscountCodeViewModel> GetEditDiscount(long discountId);
        Task<EditDiscountCodeResult> EditDiscount(EditDiscountCodeViewModel edit);
        Task<List<Discount>> GetAllDiscount();
        Task ChangeIsFinalyOrder(long orderId);
        Task<ResultOrderStateViewModel> GetResultOrder();
        Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders);
        Task<bool> ChangeStateToSend(long orderId);
        Task<Order> GetOrderDetail(long orderId);
        Task<long> RemoveOrderDetail(long DetailId);
        Task<FilterExpiredOrders> FilterExpiredOrders(FilterExpiredOrders filterExpired);

        Task<bool> RemoveOrderExpiredForAdmin(long orderId);
        Task<EditTransportationViewModel> GetEditTransportation(long id);
        Task<EditTransportationResult> EditTransportation(EditTransportationViewModel edit);


        #endregion

    }
}
