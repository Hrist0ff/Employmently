using employmently_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace employmently_be.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var pots = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (pots == null)
            {
                return NotFound();
            }

            return Ok(pots.PointsOfInterest);

        }
        [HttpGet("{potId}", Name = "GetPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId,int potId)
        {
            var pots = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (pots == null)
            {
                return NotFound();
            }
            if (pots.PointsOfInterest.FirstOrDefault(c => c.Id == potId) == null)
            {
                return NotFound();
            }
            return Ok(pots.PointsOfInterest.FirstOrDefault(c => c.Id == potId));

        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

      
            var newId = city.PointsOfInterest.Last().Id;

            var newPoint = new PointOfInterestDto()
            {
                Id = ++newId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterest.Add(newPoint);
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, potId = newPoint.Id }, newPoint);


        }

        [HttpPut("{potId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int potId, PointOfInterestForUpdateDto updateDto)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            var point = city.PointsOfInterest.FirstOrDefault(c => c.Id == potId);
            if (point == null)
            {
                return NotFound();
            }
            point.Name = updateDto.Name;
            point.Description = updateDto.Description;
            return NoContent();



        }

    }


}
