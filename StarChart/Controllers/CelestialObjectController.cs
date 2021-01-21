using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var CelestailObject = _context.CelestialObjects.Where(x => x.Id == id).SingleOrDefault();
                if(CelestailObject == null)
                {
                    return NotFound();
                }

                var OrbitedObjects = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestailObject.Id).ToList();
                CelestailObject.Satellites.AddRange(OrbitedObjects);

                return Ok(CelestailObject);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var CelestailObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
                if (CelestailObjects.Count == 0)
                {
                    return NotFound();
                }

                foreach(var CelestailObject in CelestailObjects)
                {
                    var OrbitedObjects = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestailObject.Id).ToList();
                    CelestailObject.Satellites.AddRange(OrbitedObjects);
                }

                return Ok(CelestailObjects);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var CelestailObjects = _context.CelestialObjects.ToList();

                foreach (var CelestailObject in CelestailObjects)
                {
                    var OrbitedObjects = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestailObject.Id).ToList();
                    CelestailObject.Satellites.AddRange(OrbitedObjects);
                }

                return Ok(CelestailObjects);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
