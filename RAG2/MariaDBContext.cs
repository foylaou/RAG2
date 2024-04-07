using Microsoft.EntityFrameworkCore;
using RAG2.Entities; // 确保这里正确引用了您的 User 实体所在的命名空间

namespace RAG2;

public class MariaDBContext : DbContext
{
    public DbSet<User> Users { get; set; } // 在 EF Core 中使用 Microsoft.EntityFrameworkCore.DbSet

    public MariaDBContext(DbContextOptions<MariaDBContext> options) : base(options)
    {
    }
    
}