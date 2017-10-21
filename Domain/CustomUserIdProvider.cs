using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace Domain
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            var userId = request.User.Identity.GetUserId();
            return userId;
        }
    }
}
