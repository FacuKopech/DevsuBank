using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEntityAsync(Cuenta entity)
        {
            Cuenta? accountFound = await this.GetEntityAsync(entity.Id);
            if (accountFound == null)
            {
                await _context.Cuentas.AddAsync(entity);
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }

        public async Task<bool> DeleteEntityAsync(Guid Id)
        {
            Cuenta? accountToDelete = await this.GetEntityAsync(Id);
            if (accountToDelete != null)
            {
                _context.Cuentas.Remove(accountToDelete);
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }

        public async Task<ICollection<Cuenta>> GetAllAsync()
        {
            return await _context.Cuentas.Include(c => c.Cliente).ToListAsync();
        }

        public async Task<Cuenta?> GetEntityAsync(Guid Id)
        {
            return await _context.Cuentas.Where(c => c.Id == Id)
                .Include(mo => mo.Movimientos)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ModifyEntityAsync(Cuenta entity)
        {
            Cuenta? accountToModify = await this.GetEntityAsync(entity.Id);
            if (accountToModify != null)
            {
                accountToModify.TipoCuenta = entity.TipoCuenta;
                accountToModify.Estado = entity.Estado;

                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }
    }
}
