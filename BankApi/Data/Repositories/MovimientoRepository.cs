using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repositories
{
    public class MovimientoRepository : IMovimientoRepository

    {
        private readonly ApplicationDbContext _context;

        public MovimientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEntityAsync(Movimiento entity)
        {
            Movimiento? transactionFound = await this.GetEntityAsync(entity.Id);
            if (transactionFound == null)
            {
                await _context.Movimientos.AddAsync(entity);
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }

        public async Task<bool> DeleteEntityAsync(Guid Id)
        {
            Movimiento? transactionToDelete = await this.GetEntityAsync(Id);
            if (transactionToDelete != null)
            {
                _context.Movimientos.Remove(transactionToDelete);
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }

        public async Task<ICollection<Movimiento>> GetAllAsync()
        {
            return await _context.Movimientos.Include(c => c.Cuenta).ToListAsync();
        }

        public async Task<Movimiento?> GetEntityAsync(Guid Id)
        {
            return await _context.Movimientos.Where(c => c.Id == Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ModifyEntityAsync(Movimiento entity)
        {
            Movimiento? transactionToModify = await this.GetEntityAsync(entity.Id);
            if (transactionToModify != null)
            {
                transactionToModify.TipoMovimiento = entity.TipoMovimiento;

                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }
    }
}
