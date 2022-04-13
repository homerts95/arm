using System.ComponentModel.DataAnnotations;

namespace armAPI.Models
{
  public record UserLogin
  {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
  }
}
