using System;
using Microsoft.AspNetCore.Mvc;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using System.Threading.Tasks;
using System.Linq;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMovieRepository __movieRepository) : ControllerBase
{
    readonly IMovieRepository _movieRepository = __movieRepository;

    [HttpPost]
    public async Task<IActionResult> Create(CreateMovieRequest request)
    {
        Movie movie = new()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = [.. request.Genres]
        };
        await _movieRepository.CreateMovie(movie);

        MovieResponse movieResponse = new()
        {
            Id = movie.Id,
            Title = movie.Title,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres
        };

        return Created($"/api/movies/{movieResponse.Id}", movieResponse);
    }
}