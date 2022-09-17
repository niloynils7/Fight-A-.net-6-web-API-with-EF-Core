using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.DTOs.Fight
{
    public class FightRequestDto
    {
        public List<int> CharacterIds { get; set; }
    }
}