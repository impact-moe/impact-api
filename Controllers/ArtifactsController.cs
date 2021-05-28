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
        ImpactDbContext _dbContext;

        public ArtifactsController(ImpactDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Artifact>> GetArtifact(int id)
        {
            Artifact artifact = await this._dbContext.Artifacts.FindAsync(id);

            if (artifact != null)
                return artifact;

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<List<Artifact>>> GetAllArtifacts()
        {
            return await this._dbContext.Artifacts.Include(o => o.ArtifactSet).ToListAsync();
        }

        [HttpGet("artifact-set/{setId}")]
        public async Task<ActionResult<ArtifactSet>> GetArtifactSet(string setId)
        {
            ArtifactSet artifactSet = await this._dbContext.ArtifactSets.FindAsync(setId);

            if (artifactSet != null)
                return artifactSet;
                
            return NoContent();
        }
    }
}
