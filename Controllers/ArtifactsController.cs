using Microsoft.AspNetCore.Mvc;
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
            Artifact artifact = await _databaseService.GetArtifact(id);
            if (artifact != null)
                return artifact;
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<Artifact>>> GetAllArtifacts()
        {
            return await _databaseService.GetAllArtifacts();
        }

        [HttpGet("artifact-set/{setId}")]
        public async Task<ActionResult<ArtifactSet>> GetArtifactSet(string setId)
        {
            ArtifactSet artifactSet = await _databaseService.GetArtifactSet(setId);
            if (artifactSet != null)
                return artifactSet;
            return NoContent();
        }
    }
}
