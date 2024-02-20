using Domain.Shared.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Shared.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly MainDbContext _context;


        public BaseRepository(MainDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual T? Find(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual IEnumerable<T> FindAll()
        {
            return _context.Set<T>().ToList();
        }

        public virtual async Task<T> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

    }
}
