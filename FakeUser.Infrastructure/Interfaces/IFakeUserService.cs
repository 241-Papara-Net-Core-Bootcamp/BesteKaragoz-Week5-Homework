

using FakeUser.Infrastructure.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeUser.Infrastructure.Interfaces
{
    public interface IFakeUserService
    {
        Task<IEnumerable<FakeUserDto>> GetAll();
        Task<FakeUserDto> GetById(int id);
        Task Add(FakeUserDto userDto);
        Task Delete(int id);
        Task Update(FakeUserDto userDto, int id);
       
    }
}
