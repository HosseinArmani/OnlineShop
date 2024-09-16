using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Domain.ViewModels.Order;
using Shop.Domain.ViewModels.Paging;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositores
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopDbContext _context;
        public OrderRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        public async Task<OrderDetail> CheckOrderDetail(long orderId, long productId)
        {
            return await _context.OrderDetails.AsQueryable().FirstOrDefaultAsync(d => d.OrderId == orderId && d.ProductId == productId);
        }

        public async Task<Order> CheckUserOrder(long userId)
        {
            return await _context.Orders.AsQueryable().FirstOrDefaultAsync(o => o.UserId == userId && !o.IsFinaly);
        }

        public async Task<Order> GetOrderById(long orderId)
        {
            return await _context.Orders.SingleOrDefaultAsync(o => o.Id == orderId);
        }
        public async Task<Order> GetOrderById(long orderId, long userId)
        {
            return await _context.Orders.SingleOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }
        public async Task<Order> GetUserBasketForUserPanel(long orderId, long userId)
        {
            return await _context.Orders.AsQueryable().Include(o => o.OrderDetails).ThenInclude(od=>od.Product).FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);


        }
        public async Task<Order> GetShowBasket(long userId)
        {
            return await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Product).AsQueryable()
                .Where(o => o.UserId == userId  && !o.IsFinaly)
                .FirstOrDefaultAsync();
        }
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<int> SumOrder(long orderId)
        {
            return await _context.OrderDetails.AsQueryable().Where(o => o.OrderId == orderId && !o.IsDelete).SumAsync(o => o.Price*o.Count);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }
  

        public async Task<Transportation> GetTransportations()
        {

            return await _context.Transportations.FirstOrDefaultAsync();
        }

        public async Task<Discount> GetDiscountByCode(string code)
        {
            return await _context.Discounts.SingleOrDefaultAsync(d => d.DisCountCode == code);
        }

        public  void UpdateDisccount(Discount discount)
        {
             _context.Discounts.Update(discount);
        }

        public async Task<bool> GetUserUsed(long userId, long discountId)
        {
            return await _context.UserDiscounts.AnyAsync(d => d.UserId == userId && d.DisCountId == discountId);
        }

        public async Task AddUserDiscount(long userId, long discountId)
        {
            await _context.UserDiscounts.AddAsync(new UserDiscount
            {
                 UserId=userId,
                 DisCountId=discountId

            });
        }

        public async Task AddDiscount(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
        }

        public async Task<Discount> GetDiscountById(long discountId)
        {
           return await _context.Discounts.FindAsync(discountId);
        }

        public async Task<List<Discount>> GetAllDiscount()
        {
            return await _context.Discounts.ToListAsync();
        }

        public async Task<ResultOrderStateViewModel> GetResultOrder()
        {
            return new ResultOrderStateViewModel
            {
                CancelCount = await _context.Orders.AsQueryable().Where(c => c.OrderState == OrderState.Cancel && c.CreateDate.Day == DateTime.Now.Day).CountAsync(),
                RequestCount = await _context.Orders.AsQueryable().Where(c => c.OrderState == OrderState.Requested).CountAsync(),
                ProcessingCount = await _context.Orders.AsQueryable().Where(c => c.OrderState == OrderState.Processing && c.CreateDate.Day == DateTime.Now.Day).CountAsync(),
                SentCount = await _context.Orders.AsQueryable().Where(c => c.OrderState == OrderState.Sent).CountAsync()
            };
        }

        public async Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders)
        {
            var query = _context.Orders.Include(c => c.OrderDetails).Include(c => c.User).OrderByDescending(o => o.CreateDate).AsQueryable();

            #region state
            switch (filterOrders.OrderStateFilter)
            {
                case OrderStateFilter.All:
                    break;
                case OrderStateFilter.Requested:
                    query = query.Where(c => c.OrderState == OrderState.Requested);
                    break;
                case OrderStateFilter.Processing:
                    query = query.Where(c => c.OrderState == OrderState.Processing);

                    break;
                case OrderStateFilter.Sent:
                    query = query.Where(c => c.OrderState == OrderState.Sent);

                    break;
                case OrderStateFilter.Cancel:
                    query = query.Where(c => c.OrderState == OrderState.Cancel);

                    break;
            }
            #endregion


            var pager = Pager.Build(filterOrders.PageId, await query.CountAsync(), filterOrders.TakeEntity, filterOrders.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();
            return filterOrders.SetPaging(pager).SetOrders(allData);
        }

        public async Task<Order> GetOrderDetail(long orderId)
        {
            return await _context.Orders.Include(o=>o.OrderDetails).ThenInclude(o=>o.Product).Include(o=>o.User).ThenInclude(o=>o.addresses)
                .AsQueryable().Where(o => o.Id == orderId).SingleOrDefaultAsync();
        }

        public async Task<FilterOrderForUserPanel> FilterOrdersForUserPanel(FilterOrderForUserPanel filterOrders)
        {
            var query =  _context.Orders.Include(o => o.OrderDetails).Include(o => o.User).Where(u => u.UserId == filterOrders.UserId).OrderByDescending(o=>o.CreateDate).AsQueryable();

            var pager = Pager.Build(filterOrders.PageId, await query.CountAsync(), filterOrders.TakeEntity, filterOrders.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();
            return filterOrders.SetPaging(pager).SetOrders(allData);
        }

        public async Task<OrderDetail> GetOrderDetailById(long id)
        {
            return await _context.OrderDetails.FindAsync(id);
        }

        public void RemoveOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
        }

    

        public async Task<FilterExpiredOrders> FilterExpiredOrders(FilterExpiredOrders filterExpired)
        {
            var query =  _context.Orders.AsQueryable();
            query = query.Where(o => !o.IsFinaly);

            var pager = Pager.Build(filterExpired.PageId, await query.CountAsync(), filterExpired.TakeEntity, filterExpired.CountForShowAfterAndBefor);
            var allData = await query.Paging(pager).ToListAsync();
            return filterExpired.SetPaging(pager).SetOrders(allData);

        }

        public async Task RemoveOrderExpiredForAdmin(long orderId)
        {
            var order = await GetOrderById(orderId);

            var orderDetail =await _context.OrderDetails.Where(d => d.OrderId == order.Id).ToListAsync();

            foreach (var detail in orderDetail)
            {
                _context.OrderDetails.RemoveRange(orderDetail);
            }
            _context.Orders.Remove(order);

            _context.SaveChanges();
        }

        public async Task<Transportation> GetTransportationById(long id)
        {
            return await _context.Transportations.FindAsync(id);
        }

        public void UpdateTransportation(Transportation transportation)
        {
            _context.Transportations.Update(transportation);
        }
    }
}
