namespace Intersect.Server.Web.RestApi.Types.User;

public partial struct UserInfoRequestBody
{
    public string Username { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }
}
