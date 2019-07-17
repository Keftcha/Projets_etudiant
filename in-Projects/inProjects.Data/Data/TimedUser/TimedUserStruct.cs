using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.TimedUser
{
    public readonly struct TimedUserStruct
    {
       readonly public int TimedUserId;
       readonly public int Status;

        public TimedUserStruct(int timedUserId, int status)
        {
            TimedUserId = timedUserId;
            Status = status;
        }
    }
}
