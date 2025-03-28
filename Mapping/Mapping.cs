using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Movies.Api.Mapping;

public static class Mapping
{
    public static Movie ToMovie(this CreateMovieRequest request) => new()
    {
        Id = Guid.NewGuid(),
        Title = request.Title,
        YearOfRelease = request.YearOfRelease,
        Genres = [.. request.Genres]
    };

    public static Movie ToMovie(this UpdateMovieRequest request, Guid id) => new()
    {
        Id = id,
        Title = request.Title,
        YearOfRelease = request.YearOfRelease,
        Genres = [.. request.Genres]
    };

    public static MovieResponse ToMovieResponse(this Movie movie) => new()
    {
        Id = movie.Id,
        Title = movie.Title,
        Slug = movie.Slug,
        YearOfRelease = movie.YearOfRelease,
        Genres = movie.Genres
    };

    public static MoviesResponse ToMoviesResponse(this IEnumerable<Movie> movies) => new()
    {
        Movies = movies.Select(ToMovieResponse)
    };
}