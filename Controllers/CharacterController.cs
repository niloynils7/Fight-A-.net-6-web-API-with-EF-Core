using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotnet_rpg.DTOs.Character;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    // Authorize attribute age na dile kaj kore nai swagger e
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacter());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetCharacterById(int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost("SaveCharacter")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(
            AddCharacterDto character
        )
        {
            return Ok(await _characterService.AddCharacter(character));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> UpdateCharacter(
            UpdateCharacterDto updateCharacter
        )
        {
            return Ok(await _characterService.UpdateCharacter(updateCharacter));
        }

        [HttpDelete]
        public async Task<ActionResult<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            return Ok(await _characterService.DeleteCharacter(id));
        }
    }
}
