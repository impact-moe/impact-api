using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponsController : ControllerBase
    {
        ImpactDbContext _dbContext;

        public WeaponsController(ImpactDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Weapon>> Get(string id)
        {
            Weapon weapon = await _dbContext.Weapons.FindAsync(id);

            if (weapon != null)
                return weapon;

            return NoContent();
        }

        [HttpGet("{id}/stats")]
        public async Task<ActionResult<List<WeaponStat>>> GetWeaponStats(string id)
        {
            List<WeaponStat> weaponStats = await _dbContext.WeaponStats.Where(o => o.WeaponId == id).ToListAsync();

            if (weaponStats.Count != 0)
                return weaponStats;
                
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<Weapon>>> GetAllWeapons()
        {
            return await _dbContext.Weapons.ToListAsync();
        }

        [HttpGet("{id}/all")]
        public async Task<ActionResult<Weapon>> GetWeaponDetails(string id)
        {
            return await _dbContext.Weapons.Include(o => o.WeaponStats).SingleOrDefaultAsync(o => o.Id == id);
        }
    }
} 