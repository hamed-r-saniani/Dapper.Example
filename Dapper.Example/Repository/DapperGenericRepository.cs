using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dapper.Example.Repository
{
    public class DapperGenericRepository : IDapperGenericRepository
    {
        private IConfiguration _configuration { get; set; }

        public DapperGenericRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DapperExample");
        }

        public string ConnectionString { get; set; }

        public void Execute(string command)
        {
            Execute(command, null);
        }

        public void Execute(string command, object param)
        {
            using (var connection = new SqlConnection(connectionString: ConnectionString))
            {
                connection.Execute(command, param, commandType: CommandType.StoredProcedure);
            }
        }

        public List<T> List<T>(string command)
        {
            return List<T>(command, null);
        }

        public List<T> List<T>(string command, int id)
        {
            return List<T>(command, new { id });
        }

        public List<T> List<T>(string command, object param)
        {
            using (var connection = new SqlConnection(connectionString: ConnectionString))
            {
                var result = connection.Query<T>(command, param, commandType: CommandType.StoredProcedure);
                if(result != null)
                    return result.ToList();
            }
            return default(List<T>);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string command, object param)
        {
            using (var connection = new SqlConnection(connectionString: ConnectionString))
            {
                var result = connection.QueryMultiple(command, param, commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();
                var item3 = result.Read<T3>().ToList();

                if (item1 != null && item2 != null && item3 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(new List<T1>(),new List<T2>(),new List<T3>());
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string command, object param)
        {
            using (var connection = new SqlConnection(connectionString: ConnectionString))
            {
                var result = connection.QueryMultiple(command, param, commandType: CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1 != null && item2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public void QueryExecute(string command)
        {
            QueryExecute(command, null);
        }

        public void QueryExecute(string command, object param)
        {
            using (var connection = new SqlConnection(connectionString: ConnectionString))
            {
                connection.Execute(command, param,commandType:CommandType.StoredProcedure);
            }
        }

        public T Single<T>(string command, int id)
        {
            return Single<T>(command, new { id });
        }

        public T Single<T>(string command, object param)
        {
            using (var connection = new SqlConnection(connectionString: ConnectionString))
            {
                var result = connection.Query<T>(command, param, commandType: CommandType.StoredProcedure);
                if (result != null)
                    return result.FirstOrDefault();
            }
            return default(T);
        }
    }
}
