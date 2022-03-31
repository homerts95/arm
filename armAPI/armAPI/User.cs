namespace armAPI
{
  public class User
  {
    public int id { get; set; }
    public string Lastname { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty ;
    public bool IsAdmin { get; set; }
    public bool IsTrainer { get; set; }
    public string Email { get; set; } = string.Empty ;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty ;
    public DateTime? Modifiedon { get; set; } = DateTime.Now;

  }
}
