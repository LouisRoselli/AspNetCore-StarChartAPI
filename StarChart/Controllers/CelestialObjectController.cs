using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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
                if (CelestailObjects.Any())
                {
                    foreach (var CelestailObject in CelestailObjects)
                    {
                        var OrbitedObjects = _context.CelestialObjects.Where(x => x.OrbitedObjectId == CelestailObject.Id).ToList();
                        CelestailObject.Satellites.AddRange(OrbitedObjects);
                    }

                    return Ok(CelestailObjects);
                }

                return NotFound();
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(celestialObject);
                    _context.SaveChangesAsync();

                    return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject celestialObject)
        {
            try
            {
                var UpdatedCelestailObject = _context.CelestialObjects.Where(x => x.Id == id).SingleOrDefault();
                if (UpdatedCelestailObject == null)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    UpdatedCelestailObject.Name = celestialObject.Name;
                    UpdatedCelestailObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
                    UpdatedCelestailObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

                    _context.Update(UpdatedCelestailObject);
                    _context.SaveChangesAsync();

                    return NoContent();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return BadRequest();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            try
            {
                var UpdatedCelestailObject = _context.CelestialObjects.Where(x => x.Id == id).SingleOrDefault();
                if (UpdatedCelestailObject == null)
                {
                    return NotFound();
                }

                UpdatedCelestailObject.Name = name;

                _context.Update(UpdatedCelestailObject);
                _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var UpdatedCelestailObjects = _context.CelestialObjects.Where(x => x.Id == id || x.OrbitedObjectId == id).ToList();
                if (UpdatedCelestailObjects.Any())
                {
                    _context.RemoveRange(UpdatedCelestailObjects);
                    _context.SaveChangesAsync();

                    return NoContent();
                }

                return NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
