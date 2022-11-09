using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsLibrary;

namespace DesktopApplication.Contracts.Services;
public interface ISessionService
{
    void CreateSession(User user);
    void DestroySession();

    int GetSessionUserId();
}
