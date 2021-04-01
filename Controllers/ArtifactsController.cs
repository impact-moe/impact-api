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
    public class ArtifactsController : ControllerBase
    {
        ImpactDatabaseService _databaseService;

        public ArtifactsController(ImpactDatabaseService databaseService)
        {
            this._databaseService = databaseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artifact>> GetArtifact(string id)
        {
            try {
                Artifact artifact = await _databaseService.GetArtifact(id);
                if (artifact != null)
                    return artifact;
                return NotFound();
            }
            catch (Exception e) {
                return StatusCode(500, e);
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Artifact>>> GetAllArtifacts()
        {
            try
            {
                return await _databaseService.GetAllArtifacts();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }

        [HttpGet("{id}/artifact-set")]
        public async Task<ActionResult<ArtifactSet>> GetArtifactSet(string setId)
        {
            try
            {
                ArtifactSet artifactSet = await _databaseService.GetArtifactSet(setId);
                if (artifactSet != null)
                    return artifactSet;
                return NotFound();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}
