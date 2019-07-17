using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.Event
{
    public readonly struct EventStruct
    {
        readonly public int Status;

        public EventStruct(int status )
        {
            Status = status;
        }
    }
}
