using System;
using System.Collections.Generic;

namespace Dapper.Example.Repository
{
    public interface IDapperGenericRepository
    {
        string ConnectionString { get; set; }
        void Execute(string command);
        void Execute(string command, object param);
        List<T> List<T>(string command);
        List<T> List<T>(string command, int id);
        List<T> List<T>(string command, object param);
        Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>> List<T1, T2, T3>(string command, object param);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string command, object param);
        void QueryExecute(string command);
        void QueryExecute(string command, object param);
        T Single<T>(string command, int id);
        T Single<T>(string command, object param);
    }
}
