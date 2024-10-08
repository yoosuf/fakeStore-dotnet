public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public IGenericRepository<Product> Products { get; private set; }
    public IGenericRepository<Category> Categories { get; private set; }
    public IGenericRepository<User> Users { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Products = new GenericRepository<Product>(_context);
        Categories = new GenericRepository<Category>(_context);
        Users = new GenericRepository<User>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}