using armAPI.Models;

namespace armAPI.Services
{
  public interface IUserService
  {
    public User Get(UserLogin userLogin);
  }
}
