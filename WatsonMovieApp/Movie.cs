using System;
using System.Collections.Generic;

namespace WatsonMovieApp
{
    internal class Movie
    {
        internal List<string> genres;
        internal int id;
        internal string overview;
        internal string posterPath;
        internal DateTime? releaseDate;
        internal string title;

        internal List<string> getGenres(List<int> genreIds)
        {
            List<string> temp = new List<string>();

            foreach (int i in genreIds)
            {
                switch (i)
                {
                    case 28:
                        temp.Add("Action");
                        break;
                    case 12:
                        temp.Add("Adventure");
                        break;
                    case 16:
                        temp.Add("Animation");
                        break;
                    case 35:
                        temp.Add("Comedy");
                        break;
                    case 80:
                        temp.Add("Crime");
                        break;
                    case 99:
                        temp.Add("Documentay");
                        break;
                    case 18:
                        temp.Add("Drama");
                        break;
                    case 10751:
                        temp.Add("Family");
                        break;
                    case 14:
                        temp.Add("Fantasy");
                        break;
                    case 36:
                        temp.Add("History");
                        break;
                    case 27:
                        temp.Add("Horror");
                        break;
                    case 9648:
                        temp.Add("Mystery");
                        break;
                    case 10749:
                        temp.Add("Romance");
                        break;
                    case 878:
                        temp.Add("Science Fiction");
                        break;
                    case 10770:
                        temp.Add("TV Movie");
                        break;
                    case 53:
                        temp.Add("Thriller");
                        break;
                    case 10752:
                        temp.Add("War");
                        break;
                    case 37:
                        temp.Add("Western");
                        break;
                }

            }

            return temp;
        }
    }
}
