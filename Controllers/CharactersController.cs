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
        public async Task<ActionResult<Character>> Get(string id, string expand)
        {
            try
            {
                Character character = await _databaseService.GetCharacter(id, expand);
                if (character != null)
                    return character;
                return NotFound();
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
                return NotFound();
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
                if (talents != null)
                    return talents;
                return NotFound();
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
                if (constellations != null)
                    return constellations;
                return NotFound();
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
                if (overview != null)
                    return overview;
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/weapon-priorities")]
        public async Task<ActionResult<List<WeaponPriority>>> GetWeaponPriorities(string id, bool details)
        {
            try
            {
                List<WeaponPriority> weaponPriorities = await _databaseService.GetWeaponPriorities(id, details);
                if (weaponPriorities != null)
                    return weaponPriorities;
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/artifact-priorities")]
        public async Task<ActionResult<List<ArtifactPriority>>> GetArtifactPriorities(string id, bool details)
        {
            try
            {
                List<ArtifactPriority> artifactPriorities = await _databaseService.GetArtifactPriorities(id, details);
                if (artifactPriorities != null)
                    return artifactPriorities;
                return NotFound();
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
                if (mainStatPriorities != null)
                    return mainStatPriorities;
                return NotFound();
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
                if (subStatPriorities != null)
                    return subStatPriorities;
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetAllCharacters(string expand)
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
