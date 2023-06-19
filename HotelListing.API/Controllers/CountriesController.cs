using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesRepository _countriesRepository;
    private readonly IMapper _mapper;

    public CountriesController(ICountriesRepository countriesRepository, IMapper mapper)
    {
        _countriesRepository = countriesRepository;
        _mapper = mapper;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        var countries = await _countriesRepository.GetAllAsync();
        var countryDtos = _mapper.Map<IEnumerable<GetCountryDto>>(countries);

        return Ok(countryDtos);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var country = await _countriesRepository.GetDetailsAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        var countryDto = _mapper.Map<GetCountryDto>(country);

        return Ok(countryDto);
    }

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
    {
        if (id != updateCountryDto.Id)
        {
            return BadRequest();
        }

        var country = await _countriesRepository.GetAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        _mapper.Map(updateCountryDto, country);

        try
        {
            await _countriesRepository.UpdateAsync(country);
        }
        catch (DbUpdateConcurrencyException)
        {
            var isExists = await CountryExists(id);
            if (isExists)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountry)
    {
        if (createCountry is null)
        {
            return BadRequest();
        }

        var country = _mapper.Map<Country>(createCountry);

        await _countriesRepository.AddAsync(country);

        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var country = await _countriesRepository.GetAsync(id);
        if (country is null)
        {
            return NotFound();
        }

        await _countriesRepository.DeleteAsync(country.Id);

        return NoContent();
    }

    private async Task<bool> CountryExists(int id)
    {
        return await _countriesRepository.IsExists(id);
    }
}
