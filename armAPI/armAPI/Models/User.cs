namespace armAPI
{
  public class User
  {
    public int Id { get; set; }
    public string? Lastname { get; set; } = string.Empty;
    public string? Firstname { get; set; } = string.Empty ;
    public string? Authlevel { get; set; } = string.Empty ;
    public string? Role { get; set; } = string.Empty ;
    public string? Email { get; set; } = string.Empty ;
    public string? Password { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty ;
    public DateTime? Modifiedon { get; set; } = DateTime.Now;

  }
}
