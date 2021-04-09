using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactApi.Models;
using ImpactApi.Services;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        ImpactDatabaseService _databaseService;

        public CharactersController(ImpactDatabaseService databaseService)
        {
            this._databaseService = databaseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> Get(string id, string expand = "")
        {
            try
            {
                Character character = await _databaseService.GetCharacter(id, expand);
                if (character != null)
                    return character;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/all")]
        public async Task<ActionResult<Character>> GetDetails(string id)
        {
            try
            {
                Character character = await _databaseService.GetCharacter(id, "talents,constellations,overview");
                if (character != null)
                    return character;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/talents")]
        public async Task<ActionResult<List<Talent>>> GetTalents(string id)
        {
            try
            {
                List<Talent> talents = await _databaseService.GetTalents(id);
                if (talents.Count != 0)
                    return talents;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/constellations")]
        public async Task<ActionResult<List<Constellation>>> GetConstellations(string id)
        {
            try
            {
                List<Constellation> constellations = await _databaseService.GetConstellations(id);
                if (constellations.Count != 0)
                    return constellations;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/overview")]
        public async Task<ActionResult<CharacterOverview>> GetCharacterOverview(string id)
        {
            try
            {
                CharacterOverview overview = await _databaseService.GetCharacterOverview(id);
                if (overview.RecommendedRole != string.Empty)
                    return overview;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<List<Role>>> GetCharacterRoles(string id, string expand = "")
        {
            try
            {
                List<Role> roles = await _databaseService.GetCharacterRoles(id, expand);
                if (roles.Count != 0)
                    return roles;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/weapon-priorities")]
        public async Task<ActionResult<List<WeaponPriority>>> GetWeaponPriorities(string id, string expand = "")
        {
            try
            {
                List<WeaponPriority> weaponPriorities = await _databaseService.GetWeaponPriorities(id, expand);
                if (weaponPriorities.Count != 0)
                    return weaponPriorities;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/artifact-priorities")]
        public async Task<ActionResult<List<ArtifactPriority>>> GetArtifactPriorities(string id, string expand = "")
        {
            try
            {
                List<ArtifactPriority> artifactPriorities = await _databaseService.GetArtifactPriorities(id, expand);
                if (artifactPriorities.Count != 0)
                    return artifactPriorities;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/main-stat-priorities")]
        public async Task<ActionResult<List<MainStatPriority>>> GetMainStatPriorities(string id)
        {
            try
            {
                List<MainStatPriority> mainStatPriorities = await _databaseService.GetMainStatPriorities(id);
                if (mainStatPriorities.Count != 0)
                    return mainStatPriorities;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/sub-stat-priorities")]
        public async Task<ActionResult<List<SubStatPriority>>> GetSubStatPriorities(string id)
        {
            try
            {
                List<SubStatPriority> subStatPriorities = await _databaseService.GetSubStatPriorities(id);
                if (subStatPriorities.Count != 0)
                    return subStatPriorities;
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetAllCharacters(string expand = "")
        {
            try
            {
                return await _databaseService.GetAllCharacters(expand);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}
