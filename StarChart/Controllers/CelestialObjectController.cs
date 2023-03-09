using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.OData.Metadata;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
       public  CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            if (!_context.CelestialObjects.Where(d => d.Id == id).Any())
            {
                return NotFound();
            }
            CelestialObject obj = new CelestialObject();
            obj = _context.CelestialObjects.Where(d => d.Id == id).FirstOrDefault();
            obj.Satellites = _context.CelestialObjects.Where(d => d.Id == id).ToList();
            return Ok(obj);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            if (!_context.CelestialObjects.Where(d => d.Name == name).Any())
            {
                return NotFound();
            }
            List<CelestialObject> obj = new List<CelestialObject>();
            obj = _context.CelestialObjects.Where(d => d.Name == name).ToList();
            obj.ForEach(d =>
            {
                d.Satellites= _context.CelestialObjects.Where(e => e.Id == d.Id).ToList();
            });
            return Ok(obj);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> obj = new List<CelestialObject>();
            List<CelestialObject> data = new List<CelestialObject>();
            data = _context.CelestialObjects.ToList();
            obj = data;
            obj.ForEach(d =>
            {
                d.Satellites = data.Where(e=>e.Id!=d.Id).ToList();
            });
            return Ok(obj);
        }
    }
}
