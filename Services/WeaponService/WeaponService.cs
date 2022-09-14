using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.DTOs.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponService(
            DataContext context,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponse<GetCharacterDto> serviceResponse =
                new ServiceResponse<GetCharacterDto>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == newWeapon.CharacterId
                        && c.User.Id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                );
                if (character == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found";
                    return serviceResponse;
                }
                Weapon weapon = new Weapon {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
