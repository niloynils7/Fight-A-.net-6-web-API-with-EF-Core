using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(
            AddCharacterDto newCharacter
        )
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            response.Data = await _context.Characters
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter(int userId)
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters
                .Where(c => c.User.Id == userId)
                .ToListAsync();
            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
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
                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(
                    c => c.Id == updatedCharacter.Id
                );
                if (dbCharacter == null)
                {
                    response.Message = "Data not exists";
                    response.Success = false;
                    return response;
                }
                dbCharacter.Name = updatedCharacter.Name;
                dbCharacter.HitPoints = updatedCharacter.HitPoints;
                dbCharacter.Skill = updatedCharacter.Skill;
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
            var dbCharacter = await _context.Characters.FirstAsync(c => c.Id == id);
            _context.Characters.Remove(dbCharacter);
            await _context.SaveChangesAsync();
            response.Data = await _context.Characters
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
            return response;
        }
    }
}
