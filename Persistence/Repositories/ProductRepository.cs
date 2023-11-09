using System.Linq.Expressions;
using Application.Abstractions.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context):base(context)
    {
        
    }
    public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
             return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(a =>a.IsDeleted == false)
            .ToListAsync();
        }

        public async Task<Product> GetAsync(string id)
        {
            return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .SingleOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
        }

        public async Task<Product> GetAsync(Expression<Func<Product, bool>> expression)
        {
            return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(a => a.IsDeleted == false && a.IsAvailable == true)
            .SingleOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<Product>> GetByPriceAsync(decimal price)
        {
             return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(a => a.Price == price && a.IsDeleted == false&& a.IsAvailable == true)
            .ToListAsync();
        }

        public Task<Product> GetProductByBrandAsync(string brandId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryId)
        {
             return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(a => a.CategoryId == categoryId && a.IsDeleted == false)
            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetSelectedAsync(Expression<Func<Product, bool>> expression)
        {
             return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(a =>  a.IsDeleted == false)
            .ToListAsync();
        }
}

