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
    public class WeaponsController : ControllerBase
    {
        ImpactDatabaseService _databaseService;

        public WeaponsController(ImpactDatabaseService databaseService)
        {
            this._databaseService = databaseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Weapon>> Get(string id, bool stats)
        {
            try
            {
                Weapon weaponObj = await _databaseService.GetWeapon(id, stats);
                if (weaponObj != null)
                    return weaponObj;
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/stats")]
        public async Task<ActionResult<List<WeaponStat>>> GetWeaponStats(string id)
        {
            try
            {
                List<WeaponStat> weaponStats = await _databaseService.GetWeaponStats(id);
                if (weaponStats != null)
                    return weaponStats;
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Weapon>>> GetAllWeapons(bool stats)
        {
            try
            {
                return await _databaseService.GetAllWeapons(stats);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/all")]
        public async Task<ActionResult<Weapon>> GetAllWeaponStats(string id)
        {
            try
            {
                return await _databaseService.GetWeapon(id, true);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}