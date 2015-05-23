using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Services.SqlLiteService
{
    public interface ISqlLiteService
    {
        SQLiteAsyncConnection Conn { get; }
    }
}
