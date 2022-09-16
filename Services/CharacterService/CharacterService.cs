using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;
using dotnet_rpg.DTOs.Skill;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(
            IMapper mapper,
            DataContext context,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() =>
            int.Parse(
                _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            );

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(
            AddCharacterDto newCharacter
        )
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            response.Data = await _context.Characters
                .Where(x => x.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User.Id == GetUserId())
                .ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            response.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(
            UpdateCharacterDto updatedCharacter
        )
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var dbCharacter = await _context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);
                if (dbCharacter == null)
                {
                    response.Message = "Data not exists";
                    response.Success = false;
                    return response;
                }
                if (dbCharacter.User.Id != GetUserId())
                {
                    response.Message = "Character Not Found";
                    response.Success = false;
                    return response;
                }
                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Strength = updatedCharacter.Strength;
                dbCharacter.Defense = updatedCharacter.Defense;
                dbCharacter.Intelligence = updatedCharacter.Intelligence;
                dbCharacter.Class = updatedCharacter.Class;
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(
                c => c.Id == id && c.User.Id == GetUserId()
            );
            if (dbCharacter != null)
            {
                _context.Characters.Remove(dbCharacter);
                await _context.SaveChangesAsync();
                response.Data = await _context.Characters
                    .Where(c => c.User.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c))
                    .ToListAsync();
            }
            else
            {
                response.Success = false;
                response.Message = "Character not found";
            }
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(
            AddCharacterSkillDto newCharacterSkill
        )
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(s => s.Skills)
                    .FirstOrDefaultAsync(
                        c => c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId()
                    );

                if (character == null)
                {
                    response.Success = false;
                    response.Message = "Character Not Found";
                    return response;
                }
                var skill = await _context.Skills.FirstOrDefaultAsync(
                    s => s.Id == newCharacterSkill.SkillId
                );
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = "Skill Not Found";
                    return response;
                }

                character.Skills.Add(skill);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
