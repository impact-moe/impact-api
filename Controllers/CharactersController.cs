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
        public async Task<ActionResult<Character>> Get(string id, string expand)
        {
            Character character = await _dbContext.Characters.FindAsync(id);
            char[] delimiterChars = {','};
            string[] parameters;

            if (character == null)
                return NoContent();
            
            if (expand != null)
            {
                expand = expand.ToLower();
                parameters = expand.Split(delimiterChars);

                foreach (string parameter in parameters)
                {
                    switch (parameter)
                    {
                        case "talents":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Talents)
                                .LoadAsync();
                            break;
                        case "constellations":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Constellations)
                                .LoadAsync();
                            break;
                        case "artifact-priorities":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .Query()
                                .Include(p => p.ArtifactPriorities)
                                .LoadAsync();
                            break;
                        case "main-stat-priorities":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .Query()
                                .Include(p => p.MainStatPriorities)
                                .LoadAsync();
                            break;
                        case "sub-stat-priorities":
                            await _dbContext.Entry(character)
                                .Collection(o => o.SubStatPriorities)
                                .LoadAsync();
                            break;
                        case "weapon-priorities":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .Query()
                                .Include(p => p.WeaponPriorities)
                                .LoadAsync();
                            break;
                        case "roles":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .LoadAsync();
                            break;
                        case "overview":
                            await _dbContext.Entry(character)
                                .Reference(o => o.CharacterOverview)
                                .LoadAsync();
                            break;
                        case "artifact-set":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .Query()
                                .Include(p => p.ArtifactPriorities)
                                .ThenInclude(q => q.ArtifactSet)
                                .LoadAsync();
                            break;
                        case "weapon":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .Query()
                                .Include(p => p.WeaponPriorities)
                                .ThenInclude(q => q.Weapon)
                                .LoadAsync();
                            break;
                        case "stats":
                            await _dbContext.Entry(character)
                                .Collection(o => o.Roles)
                                .Query()
                                .Include(p => p.WeaponPriorities)
                                .ThenInclude(q => q.Weapon)
                                .ThenInclude(r => r.WeaponStats)
                                .LoadAsync();
                            break;
                        case "roles.weaponpriorities.weapon":
                            goto case "weapon";
                        case "roles.weaponpriorities.weapon.stats":
                            goto case "stats";
                        case "roles.weaponpriorities":
                            goto case "weapon-priorities";
                        case "roles.artifactpriorities":
                            goto case "artifact-priorities";
                        case "roles.artifactpriorities.artifactset":
                            goto case "artifact-set";
                        case "roles.mainstatpriorities":
                            goto case "main-stat-priorities";
                        case "roles.substatpriorities":
                            goto case "sub-stat-priorities";
                    }
                }
            }

            return character;
        }

        [HttpGet("{id}/all")]
        public async Task<ActionResult<Character>> GetDetails(string id)
        {
            Character character = await _dbContext.Characters.FindAsync(id);

            if (character == null)
                return NoContent();
            
            await _dbContext.Entry(character).Collection(o => o.Talents).LoadAsync();
            await _dbContext.Entry(character).Collection(o => o.Constellations).LoadAsync();
            await _dbContext.Entry(character).Collection(o => o.ArtifactPriorities).LoadAsync();
            await _dbContext.Entry(character).Collection(o => o.MainStatPriorities).LoadAsync();
            await _dbContext.Entry(character).Collection(o => o.SubStatPriorities).LoadAsync();
            await _dbContext.Entry(character).Collection(o => o.WeaponPriorities).LoadAsync();
            await _dbContext.Entry(character).Collection(o => o.Roles).LoadAsync();
            await _dbContext.Entry(character).Reference(o => o.CharacterOverview).LoadAsync();

            return character;
        }

        [HttpGet("{id}/talents")]
        public async Task<ActionResult<List<Talent>>> GetTalents(string id)
        {
            return await _dbContext.Talents.Where(o => o.CharacterId == id).ToListAsync();
        }

        [HttpGet("{id}/constellations")]
        public async Task<ActionResult<List<Constellation>>> GetConstellations(string id)
        {
            return await _dbContext.Constellations.Where(o => o.CharacterId == id).ToListAsync();
        }

        [HttpGet("{id}/overview")]
        public async Task<ActionResult<CharacterOverview>> GetCharacterOverview(string id)
        {
            return await _dbContext.CharacterOverviews.SingleOrDefaultAsync(o => o.CharacterId == id);
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<List<Role>>> GetCharacterRoles(string id, string expand)
        {
            List<Role> roles = await _dbContext.Roles.Where(o => o.CharacterId == id).ToListAsync();
            char[] delimiterChars = {','};
            string[] parameters;

            if (roles.Count == 0)
                return NoContent();

            if (expand != null)
            {
                expand = expand.ToLower();
                parameters = expand.Split(delimiterChars);
                foreach (string parameter in parameters)
                {
                    switch (parameter)
                    {
                        case "artifact-priorities":
                            foreach (Role role in roles)
                                await _dbContext.Entry(role)
                                .Collection(o => o.ArtifactPriorities)
                                .Query()
                                .Include(p => p.ArtifactSet)
                                .LoadAsync();
                            break;
                        case "main-stat-priorities":
                            foreach (Role role in roles)
                                await _dbContext.Entry(role)
                                    .Collection(o => o.MainStatPriorities)
                                    .LoadAsync();
                            break;
                        case "sub-stat-priorities":
                            foreach (Role role in roles)
                                await _dbContext.Entry(role)
                                    .Collection(o => o.SubStatPriorities)
                                    .LoadAsync();
                            break;
                        case "weapon-priorities":
                            foreach (Role role in roles)
                                await _dbContext.Entry(role)
                                    .Collection(o => o.WeaponPriorities)
                                    .Query()
                                    .Include(p => p.Weapon)
                                    .LoadAsync();
                            break;
                    }
                }                
            }

            return roles;
        }

        [HttpGet("{id}/roles/all")]
        public async Task<ActionResult<List<Role>>> GetRoleDetails(string id)
        {
            List<Role> roles = await _dbContext.Roles
                .Where(o => o.CharacterId == id)
                .Include(p => p.ArtifactPriorities)
                .ThenInclude(q => q.ArtifactSet)
                .AsSplitQuery()
                .Include(r => r.MainStatPriorities)
                .Include(s => s.SubStatPriorities)
                .Include(t => t.WeaponPriorities)
                .ThenInclude(u => u.Weapon)
                .AsSplitQuery()
                .ToListAsync();
            
            if (roles.Count == 0)
                return NoContent();
            
            return roles;
        }

        [HttpGet("{id}/weapon-priorities")]
        public async Task<ActionResult<List<WeaponPriority>>> GetWeaponPriorities(string id)
        {
            return await _dbContext.WeaponPriorities.Where(o => o.CharacterId == id).Include(p => p.Weapon).ToListAsync();
        }

        [HttpGet("{id}/artifact-priorities")]
        public async Task<ActionResult<List<ArtifactPriority>>> GetArtifactPriorities(string id)
        {
            return await _dbContext.ArtifactPriorities.Where(o => o.CharacterId == id).Include(p => p.ArtifactSet).ToListAsync();
        }

        [HttpGet("{id}/main-stat-priorities")]
        public async Task<ActionResult<List<MainStatPriority>>> GetMainStatPriorities(string id)
        {
            return await _dbContext.MainStatPriorities.Where(o => o.CharacterId == id).ToListAsync();
        } 

        [HttpGet("{id}/sub-stat-priorities")]
        public async Task<ActionResult<List<SubStatPriority>>> GetSubStatPriorities(string id)
        {
            return await _dbContext.SubStatPriorities.Where(o => o.CharacterId == id).ToListAsync();
        }
 
        [HttpGet]
        public async Task<ActionResult<List<Character>>> GetAllCharacters()
        {
            return await _dbContext.Characters.ToListAsync();
        }
    }
}
