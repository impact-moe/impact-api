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
        public async Task<ActionResult<Weapon>> Get(string id, string expand = "")
        {
            try
            {
                Weapon weaponObj = await _databaseService.GetWeapon(id, expand);
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

        [HttpGet]
        public async Task<ActionResult<List<Weapon>>> GetAllWeapons(string expand = "")
        {
            try
            {
                return await _databaseService.GetAllWeapons(expand);
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
                return await _databaseService.GetWeapon(id, "stats");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}