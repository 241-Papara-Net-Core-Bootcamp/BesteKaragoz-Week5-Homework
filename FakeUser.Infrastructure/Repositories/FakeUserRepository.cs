using FakeUser.Infrastructure.Context;
using FakeUser.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FakeUser.Core.Interfaces;

namespace FakeUser.Infrastructure.Repositories
{
    public class FakeUserRepository : Repository<FakeUserEntity>, IFakeUserRepository
    {
        private readonly DbSet<FakeUserEntity> _fakeusers;
        public FakeUserRepository(FakeUserDbContext context) :base(context) 
        {
            _fakeusers = context.Set<FakeUserEntity>();
        }
    }
}
