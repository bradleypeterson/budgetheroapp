using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;

namespace DesktopApplication.Services;
public class SessionService : ISessionService
{
    private User? sessionUser;

    public SessionService()
    {
        sessionUser = null;
    }

    public void CreateSession(User user)
    {
        sessionUser = user;
    }
    public void DestroySession()
    {
        sessionUser = null;
    }
    public int GetSessionUserId()
    {
        if (sessionUser is not null)
        {
            return sessionUser.UserId;
        }
        else
        {
            return -1;
        }
    }
}
