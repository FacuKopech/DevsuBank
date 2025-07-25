using Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddEntityAsync(Cliente entity)
        {
            Cliente? clientFound = await this.GetEntityAsync(entity.Id);
            if (clientFound == null)
            {
                await _context.Clientes.AddAsync(entity);
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }

        public async Task<bool> DeleteEntityAsync(Guid Id)
        {
            Cliente? clientToDelete = await this.GetEntityAsync(Id);
            if (clientToDelete != null)
            {
                _context.Clientes.Remove(clientToDelete);
                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }

        public async Task<ICollection<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Include(cu => cu.Cuentas)
                .ThenInclude(mo => mo.Movimientos)
                .ToListAsync();
        }

        public async Task<Cliente?> GetEntityAsync(Guid Id)
        {
            return await _context.Clientes.Where(c => c.Id == Id)
                .Include(cu => cu.Cuentas)
                .ThenInclude(mo => mo.Movimientos)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ModifyEntityAsync(Cliente entity)
        {
            Cliente? clientToModify = await this.GetEntityAsync(entity.Id);
            if (clientToModify != null)
            {
                clientToModify.Nombre = entity.Nombre;
                clientToModify.Edad = entity.Edad;
                clientToModify.Direccion = entity.Direccion;
                clientToModify.Telefono = entity.Telefono;
                clientToModify.Contraseña = entity.Contraseña;
                clientToModify.Estado = entity.Estado;

                var changes = await _context.SaveChangesAsync();
                return changes > 0;
            }
            return false;
        }
    }
}
