namespace FW.WPF.Identity.Interfaces;

public interface IUserIdentity
{
    string UserName { get; set; }
    string Password { get; set; }
}