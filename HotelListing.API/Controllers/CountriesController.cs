using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly HotelListingDbContext _context;
    private readonly IMapper _mapper;

    public CountriesController(HotelListingDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountryDto>>> GetCountries()
    {
        if (_context.Countries is null)
        {
            return NotFound();
        }

        var countries = await _context.Countries.ToListAsync();
        var countriesDto = _mapper.Map<IEnumerable<GetCountryDto>>(countries);

        return Ok(countriesDto);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Country>> GetCountry(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        if (_context.Countries is null)
        {
            return NotFound();
        }

        var country = await _context.Countries
            .Include(c => c.Hotels)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (country is null)
        {
            return NotFound();
        }

        var countryDto = _mapper.Map<CountryDto>(country);

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

        if (_context.Countries is null)
        {
            return NotFound();
        }

        var country = await _context.Countries.FindAsync(id);

        if (country is null)
        {
            return NotFound();
        }

        _mapper.Map(updateCountryDto, country);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(id))
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
            throw new ArgumentNullException(nameof(createCountry));
        }

        if (_context.Countries is null)
        {
            return Problem("Entity set 'HotelListingDbContext.Countries'  is null.");
        }

        var country = _mapper.Map<Country>(createCountry);

        _context.Countries.Add(country);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCountry", new { id = country.Id }, country);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (_context.Countries is null)
        {
            return NotFound();
        }
        var country = await _context.Countries.FindAsync(id);
        if (country is null)
        {
            return NotFound();
        }

        _context.Countries.Remove(country);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CountryExists(int id)
    {
        return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
