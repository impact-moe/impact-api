using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ImpactApi.Entities;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly ImpactDbContext _dbContext;

        public CharactersController(ImpactDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> Get(string id)
        {
            return await this._dbContext.Characters.FindAsync(id);
        }

        [HttpGet("{id}/all")]
        public async Task<ActionResult<Character>> GetDetails(string id)
        {
            Character character = await this._dbContext.Characters
                .Include(o => o.ArtifactPriorities)
                .Include(o => o.MainStatPriorities)
                .Include(o => o.Roles)
                .Include(o => o.SubStatPriorities)
                .Include(o => o.WeaponPriorities)
                .SingleOrDefaultAsync(o => o.Id == id);

            if (character != null)
                return character;

            return NoContent();
        }

        [HttpGet("{id}/talents")]
        public async Task<ActionResult<List<Talent>>> GetTalents(string id)
        {
            List<Talent> talents = await this._dbContext.Talents.Where(o => o.CharacterId == id).ToListAsync();

            if (talents.Count != 0)
                return talents;

            return NoContent();
        }

        [HttpGet("{id}/constellations")]
        public async Task<ActionResult<List<Constellation>>> GetConstellations(string id)
        {
            List<Constellation> constellations = await this._dbContext.Constellations.Where(o => o.CharacterId == id).ToListAsync();

            if (constellations.Count != 0)
                return constellations;

            return NoContent();
        }

        [HttpGet("{id}/overview")]
        public async Task<ActionResult<CharacterOverview>> GetCharacterOverview(string id)
        {
            CharacterOverview overview = await this._dbContext.CharacterOverviews.FindAsync(id);

            if (overview != null)
                return overview;

            return NoContent();
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<List<Role>>> GetCharacterRoles(string id, string expand = "")
        {
            List<Role> roles = await this._dbContext.RoleNotes.Where(o => o.CharacterId == id).ToListAsync();

            if (roles.Count != 0)
                return roles;

            return NoContent();
        }

        [HttpGet("{id}/weapon-priorities")]
        public async Task<ActionResult<List<WeaponPriority>>> GetWeaponPriorities(string id)
        {
            List<WeaponPriority> weaponPriorities = await this._dbContext.WeaponPriorities.Where(o => o.CharacterId == id).ToListAsync();

            if (weaponPriorities.Count != 0)
                return weaponPriorities;

            return NoContent();
        }

        [HttpGet("{id}/artifact-priorities")]
        public async Task<ActionResult<List<ArtifactPriority>>> GetArtifactPriorities(string id)
        {
            List<ArtifactPriority> artifactPriorities = await this._dbContext.ArtifactPriorities.Where(o => o.CharacterId == id).ToListAsync();

            if (artifactPriorities.Count != 0)
                return artifactPriorities;

            return NoContent();
        }

        [HttpGet("{id}/main-stat-priorities")]
        public async Task<ActionResult<List<MainStatPriority>>> GetMainStatPriorities(string id)
        {
            List<MainStatPriority> mainStatPriorities = await this._dbContext.MainStatPriorities.Where(o => o.CharacterId == id).ToListAsync();

            if (mainStatPriorities.Count != 0)
                return mainStatPriorities;

            return NoContent();
        } 

        [HttpGet("{id}/sub-stat-priorities")]
        public async Task<ActionResult<List<SubStatPriority>>> GetSubStatPriorities(string id)
        {
            List<SubStatPriority> subStatPriorities = await this._dbContext.SubStatPriorities.Where(o => o.CharacterId == id).ToListAsync();

            if (subStatPriorities.Count != 0)
                return subStatPriorities;
                
            return NoContent();
        }
 
        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetAllCharacters()
        {
            return await this._dbContext.Characters.ToListAsync();
        }
    }
}
