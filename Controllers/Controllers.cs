using System;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using System.Threading.Tasks;
using Movies.Contracts.Responses;
using Movies.Api.Mapping;
using System.Collections.Generic;

namespace Movies.Api.Controllers;

[ApiController]
//[Route("api/[controller]")]
public class MoviesController(IMovieRepository __movieRepository) : ControllerBase
{
    readonly IMovieRepository _movieRepository = __movieRepository;

    [HttpPost(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> Create(CreateMovieRequest request)
    {
        Movie movie = request.ToMovie();
        await _movieRepository.CreateMovie(movie);
        
        MovieResponse movieResponse = movie.ToMovieResponse();
        return CreatedAtAction(
            nameof(Get),
            new
            {
                idOrSlug = movieResponse.Id
            },
            movieResponse);
        //return Created($"/{ApiEndpoints.Movies.GetAll}/{movieResponse.Id}", movieResponse); (almost) EQUIVALENT OF THE ABOVE
    }

    [HttpGet(ApiEndpoints.Movies.GetId)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug)
    {
        // Movie? movie = await _movieRepository.GetById(id);

        Movie? movie = Guid.TryParse(idOrSlug, out Guid guidId) ?
            await _movieRepository.GetById(guidId) :
            await _movieRepository.GetBySlug(idOrSlug);

        if(movie is null)
            return NotFound();
        
        return Ok(movie.ToMovieResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        Queue<Movie> movies = [];

        await foreach(Movie movie in _movieRepository.GetAll())
            movies.Enqueue(movie);
        
        return Ok(movies.ToMoviesResponse());
    }

    [HttpPut(ApiEndpoints.Movies.GetId)]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateMovieRequest request)
    {
        Movie? movie = request.ToMovie(id);

        if(!await _movieRepository.Update(movie))
            return NotFound();
        
        return Ok(movie.ToMovieResponse());
    }

    [HttpDelete(ApiEndpoints.Movies.GetId)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        if(!await _movieRepository.DeleteById(id))
            return NotFound();

        return NoContent();
    }
}