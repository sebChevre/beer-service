using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeerApi.Services;
using BeerApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace BeerApi.Controllers
{
    [ApiController]
    [Route("/api/beer")]
    public class BeerController : ControllerBase
    {
        public const string API_PATH = "/api/beer";
        private readonly BeerService _beerService;

        private readonly ILogger<BeerController> _logger;

        public BeerController(BeerService beerService,ILogger<BeerController> logger)
        {
            _beerService = beerService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "reader")]
        public List<Beer> GetAllBeers()
        {
            _logger.LogInformation("GetAllBeers called");
            return _beerService.GetAllBeers();
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "reader")]
        public IActionResult GetBeerById(string id)
        {
            _logger.LogInformation("GetBeerById called, with id: " + id);
            Beer beer = _beerService.GetBeer(id);

            if(beer == null){
                return NotFound(API_PATH + id);
            }

            return Ok(beer);
        }

        [HttpPost]
        [Authorize(Policy = "contributor")]
        public IActionResult CreateBeer(Beer beer){
            _logger.LogInformation("CreateBeer called, with object: {}",beer);
            _beerService.CreateBeer(beer);
            return Created(API_PATH + beer.Id,"");
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = "contributor")]
        public IActionResult UpdateBeer(Beer beer, String id){
            _logger.LogInformation("UpdateBeer called, with object: {} for id: {}",beer,id);
            _beerService.UpdateBeer(beer,id);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "contributor")]
        public IActionResult  DeleteBeer(String id){
            _logger.LogInformation("DeletBeer called, for id: {}",id);
            _beerService.DeleteBeer(id);
            return Ok("Entity with id : " + id + "sucessfully deleted!");
        }
    }
}
