using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Domain.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IWalletRepository _walletRepository;
        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IWalletRepository walletRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _walletRepository = walletRepository;
        }

        public async Task<long> AddOrder(long userId, long productId)
        {
            var product = await _productRepository.GetProductById(productId);
            product.Count -= 1;
            var order = await _orderRepository.CheckUserOrder(userId);
            int prcent = 0;
            if (product.DicPercent != null)
            {
                prcent = (product.Price * (int)product.DicPercent) / 100;

            }
            if (order == null)
            {
                order = new Order
                {                  
                    UserId = userId,
                    IsFinaly = false,
                    OrderSum = ((product.DicPercent != null) ? (product.Price - prcent) : product.Price),
                    OrderState = OrderState.Processing,

                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail
                        {
                              ProductId=productId,
                               Count=1,
                                Price=((product.DicPercent!=null)?(product.Price-prcent):product.Price),

                        }
                    }

                };
                await _orderRepository.AddOrder(order);
                await _orderRepository.SaveChange();


            }


            else
            {
                var detail = await _orderRepository.CheckOrderDetail(order.Id, productId);
                if (detail != null)
                {
                    detail.Count += 1;
                    _orderRepository.UpdateOrderDetail(detail);

                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId = order.Id,
                        Count = 1,
                        ProductId = product.Id,
                        Price = ((product.DicPercent != null) ? (product.Price - prcent) : product.Price)
                    };
                    await _orderRepository.AddOrderDetail(detail);
                }
                await _orderRepository.SaveChange();
                await UpdatePriceOrder(order.Id);
            }

             _productRepository.UpdateProduct(product);

            return order.Id;
        }

        public async Task UpdatePriceOrder(long orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            order.OrderSum = await _orderRepository.SumOrder(orderId);

            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChange();
        }

        public async Task<Order> GetUserBasketForUserPanel(long orderId, long userId)
        {
            return await _orderRepository.GetUserBasketForUserPanel(orderId, userId);
        }


        public async Task<FinallyOrderResult> FinallyOrder(FinallyOrderViewzModel finallyOrder, long userId)
        {
            if (finallyOrder.UserId != userId)
            {
                return FinallyOrderResult.HasNotUser;
            }
            var order = await _orderRepository.GetOrderById(finallyOrder.OrderId, userId);
            if (order == null || order.IsFinaly == true)
            {
                return FinallyOrderResult.NotFound;
            }
            var transportation = await _orderRepository.GetTransportations();

            if (order.OrderSum <= transportation.minBuy)
            {
                order.OrderSum += transportation.Amount;
            }
            if (await _walletRepository.BalanceWallet(userId) >= order.OrderSum)
            {

                order.IsFinaly = true;
                order.OrderState = OrderState.Requested;

                var walet = new UserWallet
                {
                    Amount = order.OrderSum,
                    IsPay = true,
                    Description = $"فاکتور شما {order.Id}",
                    UserId = userId,
                    WalletType = WalletType.Bardasht

                };
                await _walletRepository.AddWallet(walet);
                _orderRepository.UpdateOrder(order);
                await _orderRepository.SaveChange();
                return FinallyOrderResult.Success;
            }
            return FinallyOrderResult.Errore;
        }

        public async Task<Transportation> GetTransportations()
        {
            return await _orderRepository.GetTransportations();
        }

        public async Task<DiscountUseType> UseDiscount(long orderId, string code)
        {
            var discount = await _orderRepository.GetDiscountByCode(code);

            if (discount == null) return DiscountUseType.NotFound;

            if (discount.StartDate != null && discount.StartDate < DateTime.Now) return DiscountUseType.ExpirDate;

            if (discount.EndDate != null && discount.EndDate >= DateTime.Now) return DiscountUseType.ExpirDate;

            if (discount.UsabaleCount != null && discount.UsabaleCount < 1) return DiscountUseType.Finished;

            var order = await _orderRepository.GetOrderById(orderId);
            if (await _orderRepository.GetUserUsed(order.UserId, discount.DisCountId)) return DiscountUseType.UserUsed;


            int persent = (order.OrderSum * discount.Percent) / 100;

            order.OrderSum = order.OrderSum - persent;

            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChange();
            if (discount.UsabaleCount != null)
            {
                discount.UsabaleCount -= 1;
            }

            _orderRepository.UpdateDisccount(discount);

            await _orderRepository.AddUserDiscount(order.UserId, discount.DisCountId);

            await _orderRepository.SaveChange();

            return DiscountUseType.Success;


        }

        public async Task<CreateDiscountCodeResult> CreateDiscount(CreateDiscountCodeViewModel create)
        {
            var discount = new Discount
            {
                DisCountCode = create.DisCountCode,
                Percent = create.Percent,
                UsabaleCount = create.UsabaleCount

            };

            if (create.StDate != null)
            {
                string[] std = create.StDate.Split("/");
                discount.StartDate = new DateTime(int.Parse(std[0]),
                    int.Parse(std[1]),
                    int.Parse(std[2]),
                    new PersianCalendar()
                    );
            }
            if (create.EdDate != null)
            {
                string[] edd = create.EdDate.Split("/");
                discount.EndDate = new DateTime(int.Parse(edd[0]),
                    int.Parse(edd[1]),
                    int.Parse(edd[2]),
                    new PersianCalendar()
                    );
            }
            await _orderRepository.AddDiscount(discount);
            await _orderRepository.SaveChange();
            return CreateDiscountCodeResult.Sucesss;
        }

        public async Task<EditDiscountCodeViewModel> GetEditDiscount(long discountId)
        {
            var discount = await _orderRepository.GetDiscountById(discountId);
            return new EditDiscountCodeViewModel
            {
                DiscountId = discount.DisCountId,
                DisCountCode = discount.DisCountCode,
                Percent = discount.Percent,
                UsabaleCount = discount.UsabaleCount,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate



            };
        }

        public async Task<EditDiscountCodeResult> EditDiscount(EditDiscountCodeViewModel edit)
        {
            var discount = await _orderRepository.GetDiscountById(edit.DiscountId);
            if (discount == null) return EditDiscountCodeResult.NotFound;

            discount.DisCountCode = edit.DisCountCode;
            discount.Percent = edit.Percent;
            discount.UsabaleCount = edit.UsabaleCount;

            if (edit.StDate != null)
            {
                string[] std = edit.StDate.Split("/");
                discount.StartDate = new DateTime(int.Parse(std[0]),
                    int.Parse(std[1]),
                    int.Parse(std[2]),
                    new PersianCalendar()
                    );
            }
            if (edit.EdDate != null)
            {
                string[] edd = edit.EdDate.Split("/");
                discount.EndDate = new DateTime(int.Parse(edd[0]),
                    int.Parse(edd[1]),
                    int.Parse(edd[2]),
                    new PersianCalendar()
                    );
            }
            _orderRepository.UpdateDisccount(discount);
            await _orderRepository.SaveChange();

            return EditDiscountCodeResult.Success;
        }

        public async Task<List<Discount>> GetAllDiscount()
        {
            return await _orderRepository.GetAllDiscount();
        }

        public async Task<Order> GetOrderById(long orderId)
        {
            return await _orderRepository.GetOrderById(orderId);
        }

        public async Task ChangeIsFinalyOrder(long orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            order.IsFinaly = true;
            order.OrderState = OrderState.Requested;
            _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChange();
        }

        public async Task<ResultOrderStateViewModel> GetResultOrder()
        {
            return await _orderRepository.GetResultOrder();
        }

        public async Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders)
        {
            return await _orderRepository.FilterOrders(filterOrders);
        }

        public async Task<bool> ChangeStateToSend(long orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);
            if (order != null)
            {
                order.OrderState = OrderState.Sent;
                _orderRepository.UpdateOrder(order);
                await _orderRepository.SaveChange();

                return true;
            }
            return false;
        }

        public async Task<Order> GetOrderDetail(long orderId)
        {
            return await _orderRepository.GetOrderDetail(orderId);
        }

        public async Task<FilterOrderForUserPanel> FilterOrdersForUserPanel(FilterOrderForUserPanel filterOrders)
        {
            return await _orderRepository.FilterOrdersForUserPanel(filterOrders);
        }

        public async Task<long> RemoveOrderDetail(long detailId)
        {
            var orderDetail = await _orderRepository.GetOrderDetailById(detailId);
            var order = await _orderRepository.GetOrderById(orderDetail.OrderId);
            var product = await _productRepository.GetProductById(orderDetail.ProductId);
            product.Count += 1;

            if (orderDetail.Count != 1)
            {
                orderDetail.Count -= 1;
                order.OrderSum = (await _orderRepository.SumOrder(order.Id) - orderDetail.Price);
                _orderRepository.UpdateOrder(order);
                await _orderRepository.SaveChange();
            }
            else
            {
                _orderRepository.RemoveOrderDetail(orderDetail);
                order.OrderSum = (await _orderRepository.SumOrder(order.Id) - orderDetail.Price);
                _orderRepository.UpdateOrder(order);
                await _orderRepository.SaveChange();
            }


            return order.Id;


        }

        public async Task<Order> GetShowBasket(long userId)
        {
            return await _orderRepository.GetShowBasket(userId);
        }

        public async Task<FilterExpiredOrders> FilterExpiredOrders(FilterExpiredOrders filterExpired)
        {
            return await _orderRepository.FilterExpiredOrders(filterExpired);
        }

        public async Task<bool> RemoveOrderExpiredForAdmin(long orderId)
        {
            await _orderRepository.RemoveOrderExpiredForAdmin(orderId);
            return true;
        }

        public async Task<EditTransportationViewModel> GetEditTransportation(long id)
        {
            var transportation = await _orderRepository.GetTransportationById(id);
            if (transportation != null)
            {
                return new EditTransportationViewModel
                {
                    Id = transportation.Id,
                    Title = transportation.Title,
                    Amount = transportation.Amount,
                    minBuy = transportation.minBuy,
                    IsActive = transportation.IsActive
                };
            }
            return null;
        }

        public async Task<EditTransportationResult> EditTransportation(EditTransportationViewModel edit)
        {
            var transportation = await _orderRepository.GetTransportationById(edit.Id);
            if (transportation == null) return EditTransportationResult.NotFound;

            transportation.Title = edit.Title;
            transportation.Amount = edit.Amount;
            transportation.minBuy = edit.minBuy;
            transportation.IsActive = edit.IsActive;
            
            _orderRepository.UpdateTransportation(transportation);
            await _orderRepository.SaveChange();

            return EditTransportationResult.Success;


        }
    }
}
