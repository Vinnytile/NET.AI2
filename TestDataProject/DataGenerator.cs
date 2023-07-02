using System.Text;
using Bogus;
using TestDataProject.Models;

namespace TestDataProject;

public static class DataGenerator {
  public static void GenerateData() {
    // initialize random generator
    var random = new Random();

    // ---GENERATE TITLES---
    var titlesNumber = 3; // number of dataset object

    // age certification dataset
    var ageCertification = new[] {
      "G", "PG", "PG - 13", "R", "NC - 17", "U", "U / A", "A", "S", "AL", "6", "9", "12", "12A", "15", "18", "18R", "R18", "R21", "M", "MA15 +",
      "R16", "R18 +", "X18", "T", "E", "E10 +", "EC", "C", "CA", "GP", "M / PG", "TV - Y", "TV - Y7", "TV - G", "TV - PG", "TV - 14", "TV - M"
    };
    // genres dataset
    var genres = new[] {
      "Action", "Comedy", "Documentary", "Drama", "Fantasy", "Horror", "Musical", "Mystery", "Romance", "Science Fiction", "Thriller", "Western"
    };

    // initial title id
    var titleIds = 0;
    // release start date
    var releaseYearStart = DateTime.Parse("01/01/1950");
    // release end date
    var releaseYearEnd = DateTime.Now;
    
    // test data
    var testTitles = new Faker<Titles>()
      .StrictMode(true)
      .RuleFor(t => t.Id, f => titleIds++)
      .RuleFor(t => t.Title, f => f.Lorem.Word())
      .RuleFor(t => t.Description, f => f.Lorem.Word())
      .RuleFor(t => t.ReleaseYear, f => f.Date.Between(releaseYearStart, releaseYearEnd).Year)
      .RuleFor(t => t.AgeCertification, f => f.PickRandom(ageCertification))
      .RuleFor(t => t.RunTime, f => f.Random.Number(30, 200))
      .RuleFor(t => t.Genres, f => {
        var genreIndex1 = random.Next(genres.Length);
        var genreIndex2 = random.Next(genres.Length);
        return new List<string> {
          genres[genreIndex1], genres[genreIndex2]
        };
      })
      .RuleFor(t => t.ProductionCountry, f => f.Address.CountryCode())
      .RuleFor(t => t.Seasons, f => f.Random.Number(0, 10));
    
    // generate titles dataset
    var titles = testTitles.Generate(titlesNumber);

    // show title dataset
    ShowTitles(titles);

    // save title to the csv file
    SaveTitlesToCSV(titles);

    // ---GENERATE CREDITS---
    var creditsNumber = 3; // number of dataset object

    //active role dataset
    var activeRole = new[] {
      "Actor", "Actress"
    };
    //non-active role dataset
    var nonActiveRole = new[] {
      "Director", "Producer", "Screenwriter", "Cinematographer", "Film Editor", "Production Designer", "Costume Designer", "Music Composer"
    };
    // entire role dataset
    var role = activeRole.Concat(nonActiveRole).ToArray();

    // initial credit id
    var creditIds = 0;

    // test data
    var testCredits = new Faker<Credits>()
    .StrictMode(true)
    .RuleFor(c => c.Id, f => creditIds++)
    .RuleFor(c => c.TitleId, (f) => {
      var index = random.Next(titles.Count);
      return titles[index].Id;
    })
    .RuleFor(c => c.RealName, f => f.Person.FullName)
    .RuleFor(c => c.Role, f => f.PickRandom(role))
    .RuleFor(c => c.CharacterName, (f, c) => {
      if (activeRole.Contains(c.Role)) {
        return f.Person.UserName;
      }
      else {
        return "";
      }
    });

    // generate titles dataset
    var credits = testCredits.Generate(creditsNumber);

    // show credit dataset
    ShowCredits(credits);

    // save credits to csv file
    SaveCreditsToCSV(credits);
  }

  public static void ShowTitles(List<Titles> titles) {
    foreach (var title in titles) {
      Console.WriteLine("Id: " + title.Id);
      Console.WriteLine("Title: " + title.Title);
      Console.WriteLine("Description: " + title.Description);
      Console.WriteLine("ReleaseYear: " + title.ReleaseYear);
      Console.WriteLine("AgeCertification: " + title.AgeCertification);
      Console.WriteLine("RunTime: " + title.RunTime);
      Console.Write("Genres: ");
      foreach (var genre in title.Genres!) {
        Console.Write(genre + ",");
      }
      Console.WriteLine();
      Console.WriteLine("ProductionCountry: " + title.ProductionCountry);
      Console.WriteLine("Seasons: " + title.Seasons);
      Console.WriteLine("\n");
    }
  }

  public static void SaveTitlesToCSV(List<Titles> titles) {
    string file = "../../../../Titles.csv";
    string separator = ",";
    StringBuilder output = new();
    string[] headings = { "Id", "Title", "Description", "ReleaseYear", "AgeCertification", "RunTime", "Genres", "ProductionCountry", "Seasons" };
    output.AppendLine(string.Join(separator, headings));

    foreach (var title in titles) {
      var genres = String.Join("-", title.Genres!);
      string[] newLine = {
        title.Id.ToString(), title.Title!, title.Description!, title.ReleaseYear.ToString(), title.AgeCertification!, title.RunTime.ToString(),
        genres, title.ProductionCountry!, title.Seasons.ToString()
      };
      output.AppendLine(string.Join(separator, newLine));
    }

    try {
      File.WriteAllText(file, output.ToString());
      Console.WriteLine("The data has been successfully saved to the CSV file");
    }
    catch {
      Console.WriteLine("Data could not be written to the CSV file.");
      return;
    }
  }

  public static void ShowCredits(List<Credits> credits) {
    foreach (var credit in credits) {
      Console.WriteLine("Id: " + credit.Id);
      Console.WriteLine("TitleId: " + credit.TitleId);
      Console.WriteLine("RealName: " + credit.RealName);
      Console.WriteLine("CharacterName: " + credit.CharacterName);
      Console.WriteLine("Role: " + credit.Role);
      Console.WriteLine("\n");
    }
  }

  public static void SaveCreditsToCSV(List<Credits> titles) {
    string file = "../../../../Credits.csv";
    string separator = ",";
    StringBuilder output = new();
    string[] headings = { "Id", "TitleId", "RealName", "CharacterName", "Role" };
    output.AppendLine(string.Join(separator, headings));

    foreach (var title in titles) {
      string[] newLine = {
        title.Id.ToString(), title.TitleId.ToString(), title.RealName!, title.CharacterName!.ToString(), title.Role!
      };
      output.AppendLine(string.Join(separator, newLine));
    }

    try {
      File.WriteAllText(file, output.ToString());
      Console.WriteLine("The data has been successfully saved to the CSV file");
    }
    catch {
      Console.WriteLine("Data could not be written to the CSV file.");
      return;
    }
  }
}
