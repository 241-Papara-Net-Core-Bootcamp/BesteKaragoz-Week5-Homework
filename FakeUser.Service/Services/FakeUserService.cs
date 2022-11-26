using FakeUser.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeUser.Infrastructure.Dtos;
using AutoMapper;
using FakeUser.Data.Abstracts;
using FakeUser.Domain.Entities;
using Hangfire;

namespace FakeUser.Service.Services
{
    public class FakeUserService : IFakeUserService
    {
        private readonly IRepository<FakeUserEntity> _repository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        const string userCacheKey = "allmembers";
        public FakeUserService(IRepository<FakeUserEntity> repository, IMapper mapper, ICacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task RefreshCache()
        {
            _cacheService.Remove(userCacheKey);
            var cachedList = await _repository.GetAll();
            _cacheService.Set(userCacheKey, cachedList);
        }
        public async Task<IEnumerable<FakeUserDto>> GetAll()
        {
            var isExist = _cacheService.TryGet(userCacheKey, out List<FakeUserDto> usersDto);
            if (!isExist)
            {
                var users = await _repository.GetAll();
                usersDto = _mapper.Map<List<FakeUserDto>>(users);
                _cacheService.Set(userCacheKey, usersDto);
            }

            return usersDto;
        }
        public async Task<FakeUserDto> GetById(int id)
        {
            var user = await _repository.GetById(id);
            return _mapper.Map<FakeUserDto>(user);
        }

        public async Task Add(FakeUserDto userDto)
        {
            var user = _mapper.Map<FakeUserEntity>(userDto);
            await _repository.Add(user);
            BackgroundJob.Enqueue(() => RefreshCache());

        }
        public async Task Delete(int id)
        {
            var user = await _repository.GetById(id);
            await _repository.Delete(user);
            BackgroundJob.Enqueue(() => RefreshCache());
        }
        public async Task Update(FakeUserDto userDto, int id)
        {
            var updatedUser = _mapper.Map<FakeUserEntity>(userDto);
            updatedUser.Id = id;
            await _repository.Update(updatedUser);
            BackgroundJob.Enqueue(() => RefreshCache());
        }

       
    }
}
