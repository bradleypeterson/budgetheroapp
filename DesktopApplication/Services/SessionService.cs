using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApplication.Contracts.Services;
using ModelsLibrary;

namespace DesktopApplication.Services;
public class SessionService : ISessionService
{
    public event EventHandler? OnSessionCreated;
    public event EventHandler? OnSessionDestroyed;

    private User? sessionUser;

    public SessionService()
    {
        sessionUser = null;
    }

    public void CreateSession(User user)
    {
        sessionUser = user;
        OnSessionCreated?.Invoke(this, EventArgs.Empty);
    }
    public void DestroySession()
    {
        sessionUser = null;
        OnSessionDestroyed?.Invoke(this, EventArgs.Empty);
    }
    public Guid GetSessionUserId()
    {
        if (sessionUser is not null)
            return sessionUser.UserId;

        throw new Exception("A session must be created before calling this method.");
    }

    public string GetSessionUsername()
    {
        if (sessionUser is not null)
        {
            if (sessionUser.Username is not null)
                return sessionUser.Username;
            else
                throw new Exception("Session Service | Session username was never set.");
        }
        else
            throw new Exception("Session Service | Session user was never stored.");
    }
}
