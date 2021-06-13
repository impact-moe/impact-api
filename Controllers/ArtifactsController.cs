using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImpactApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImpactApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtifactsController : ControllerBase
    {
        private readonly ImpactDbContext _dbContext;

        public ArtifactsController(ImpactDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artifact>> GetArtifact(int id, string expand)
        {
            Artifact artifact = await _dbContext.Artifacts.FindAsync(id);

            if (artifact == null)
                return NoContent();

            if (expand != null)
            {
                expand = expand.ToLower();
                if (expand == "artifact-set")
                    await _dbContext.Entry(artifact).Reference(o => o.ArtifactSet).LoadAsync();
            }

            return artifact;
        }

        [HttpGet]
        public async Task<ActionResult<List<Artifact>>> GetAllArtifacts()
        {
            return await _dbContext.Artifacts.Include(o => o.ArtifactSet).ToListAsync();
        }

        [HttpGet("artifact-set/{setId}")]
        public async Task<ActionResult<ArtifactSet>> GetArtifactSet(string setId)
        {
            ArtifactSet artifactSet = await _dbContext.ArtifactSets.FindAsync(setId);

            if (artifactSet == null)
                return NoContent();
                
            return artifactSet;
        }
    }
}
