using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.Hotel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IMapper _mapper;

    public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
    {
        _hotelsRepository = hotelsRepository;
        _mapper = mapper;
    }

    // GET: api/Hotels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels()
    {
        var hotels = await _hotelsRepository.GetAllAsync();
        var hotelDtos = _mapper.Map<IList<HotelDto>>(hotels);

        return Ok(hotelDtos);
    }

    // GET: api/Hotels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDto>> GetHotel(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var hotel = await _hotelsRepository.GetAsync(id);

        if (hotel == null)
        {
            return NotFound();
        }

        var getHotelDto = _mapper.Map<HotelDto>(hotel);

        return Ok(getHotelDto);
    }

    // PUT: api/Hotels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutHotel(int id, UpdateHotelDto updateHotelDto)
    {
        if (id != updateHotelDto.Id)
        {
            return BadRequest();
        }

        var hotel = await _hotelsRepository.GetAsync(updateHotelDto.Id);

        if (hotel == null)
        {
            return NotFound();
        }

        _mapper.Map(updateHotelDto, hotel);

        try
        {
            await _hotelsRepository.UpdateAsync(hotel);
        }
        catch (DbUpdateConcurrencyException)
        {
            var isExists = await HotelExistsAsync(id);
            if (!isExists)
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

    // POST: api/Hotels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<HotelDto>> PostHotel(CreateHotelDto createHotelDto)
    {
        if (createHotelDto is null)
        {
            return BadRequest();
        }

        var hotel = _mapper.Map<Hotel>(createHotelDto);

        await _hotelsRepository.AddAsync(hotel);

        var hotelDto = _mapper.Map<HotelDto>(hotel);

        return CreatedAtAction("GetHotel", new { id = hotelDto.Id }, hotelDto);
    }

    // DELETE: api/Hotels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        var hotel = await _hotelsRepository.GetAsync(id);
        if (hotel == null)
        {
            return NotFound();
        }

        await _hotelsRepository.DeleteAsync(id);

        return NoContent();
    }

    private async Task<bool> HotelExistsAsync(int id)
    {
        return await _hotelsRepository.IsExists(id);
    }
}
