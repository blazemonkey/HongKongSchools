using HongKongSchools.Models;
using HongKongSchools.Services.SqlLiteService;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.Services.SqlLiteService
{
    public class SqlLiteService : ISqlLiteService
    {
        private readonly SQLiteAsyncConnection _conn;

        public SQLiteAsyncConnection Conn
        {
            get { return _conn; }
        }

        public SqlLiteService()
        {
            _conn = new SQLiteAsyncConnection("hkschools.db");
        }

        public async Task InitDb()
        {
            var createTasks = new Task[]
            {
                _conn.CreateTableAsync<Address>(),
                _conn.CreateTableAsync<Category>(),
                _conn.CreateTableAsync<District>(),
                _conn.CreateTableAsync<FinanceType>(),
                _conn.CreateTableAsync<Gender>(),
                _conn.CreateTableAsync<Level>(),
                _conn.CreateTableAsync<Religion>(),
                _conn.CreateTableAsync<School>(),
            };

            Task.WaitAll(createTasks);
            await InsertDataAsync();
        }

        private async Task InsertDataAsync()
        {

        }

        public async Task ClearLocalDb()
        {
            await _conn.DropTableAsync<Address>();
            await _conn.DropTableAsync<Category>();
            await _conn.DropTableAsync<District>();
            await _conn.DropTableAsync<FinanceType>();
            await _conn.DropTableAsync<Gender>();
            await _conn.DropTableAsync<Level>();
            await _conn.DropTableAsync<Religion>();
            await _conn.DropTableAsync<School>();
            await InitDb();
        }
    }
}
