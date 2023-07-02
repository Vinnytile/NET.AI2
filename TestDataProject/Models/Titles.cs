namespace TestDataProject.Models;

public class Titles {
  public int Id { get; set; }
  public string? Title { get; set; }
  public string? Description { get; set; }
  public int ReleaseYear { get; set; }
  public string? AgeCertification { get; set; }
  public int RunTime { get; set; }
  public List<string>? Genres { get; set; }
  public string? ProductionCountry { get; set; }
  public int Seasons { get; set; }
}
