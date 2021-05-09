using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactApi.Models;
using ImpactApi.Services;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponsController : ControllerBase
    {
        ImpactDatabaseService _databaseService;

        public WeaponsController(ImpactDatabaseService databaseService)
        {
            this._databaseService = databaseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Weapon>> Get(string id, string expand = "")
        {
            return await _databaseService.GetWeapon(id, expand);
        }

        [HttpGet("{id}/stats")]
        public async Task<ActionResult<List<WeaponStat>>> GetWeaponStats(string id)
        {
            List<WeaponStat> weaponStats = await _databaseService.GetWeaponStats(id);
            if (weaponStats.Count != 0)
                return weaponStats;
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<Weapon>>> GetAllWeapons(string expand = "")
        {
            return await _databaseService.GetAllWeapons(expand);
        }

        [HttpGet("{id}/all")]
        public async Task<ActionResult<Weapon>> GetAllWeaponStats(string id)
        {
            return await _databaseService.GetWeapon(id, "stats");
        }
    }
}