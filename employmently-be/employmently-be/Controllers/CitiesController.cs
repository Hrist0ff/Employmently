﻿using employmently_be.Models;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CityDto>> GetCities() {
            var cities = CitiesDataStore.Current.Cities;

            return Ok(cities);
        }
        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if (cityToReturn == null) {
                return NotFound();
            }

            return Ok(cityToReturn);
        }

    }
}