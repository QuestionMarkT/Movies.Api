namespace Movies.Api;

public static class ApiEndpoints
{
    const string ApiBase = "api";

    public static class Movies
    {
        const string MoviesBase = $"{ApiBase}/movies";

        public const string GetId = $"{MoviesBase}/{{idOrSlug}}";
        public const string GetAll = MoviesBase;
    }
}
