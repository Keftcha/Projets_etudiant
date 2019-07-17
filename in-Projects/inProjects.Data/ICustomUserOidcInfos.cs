using CK.DB.User.UserOidc;
using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data
{
    public interface ICustomUserOidcInfos : IUserOidcInfo
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
