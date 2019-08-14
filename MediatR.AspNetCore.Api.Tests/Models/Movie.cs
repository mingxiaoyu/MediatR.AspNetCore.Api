using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR.AspNetCore.Api.Tests.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FakesMovie
    {
        public static List<Movie> GetMovies()
        {
            return new List<Movie>()
            {
                new Movie()
                {
                    Id =1,
                    Name ="Angel"
                },
                new Movie()
                {
                    Id =2,
                    Name ="Baby"
                }
            };
        }

        public static Movie GetMovie(int Id)
        {
            var list = GetMovies();
            return list.FirstOrDefault(x => x.Id == Id);
        }
    }
}
