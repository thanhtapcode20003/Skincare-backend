﻿// OrderRepository.cs
using Microsoft.EntityFrameworkCore;
using SkinCare_Data.Data;
using SkinCare_Data.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkinCare_Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SkinCare_DBContext _context;

        public OrderRepository(SkinCare_DBContext context)
        {
            _context = context;
        }

        public async Task<Order> GetCartByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderStatus == "Cart");
        }

        public async Task<Order> GetByIdAsync(string orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> GetOrderCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<int> GetOrderDetailCountAsync()
        {
            return await _context.OrderDetails.CountAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        public Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public SkinCare_DBContext GetContext()
        {
            return _context;
        }
    }
}