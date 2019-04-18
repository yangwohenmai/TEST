using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Net.Caching;
using Dapper.Net.Extensions;
using Dapper.Net.Patterns.Chainable;
using Dapper.Net.Patterns.Chainable.Syntax.Clause;
using Dapper.Net.Patterns.Chainable.Syntax.Statement;

namespace Dapper.Net {

    public interface IDapperPatterns<TModel, TKey> {
        // Get a single entity by primary key
        TModel Get(TKey id);

        // Get a single entity by primary key or throw
        TModel GetSingle(TKey id);

        // Get multiple entities by condition set matches
        IEnumerable<TModel> Get(SqlWhere clause);

        // Upsert resource
        TModel Put(TModel entity);

        // Create resource
        TKey Post(TModel entity);

        // Delete resource
        bool Delete(TKey id);
    }

    /// <summary>
    /// Class in charge of formatting SQL strings
    /// </summary>
    /// <typeparam name="TModel">Type of model.</typeparam>
    public class SqlStrings<TModel> {
        private readonly TModel model;

        /*
        public string Select(SqlArgs whereMap, params string[] columns)
        {
            return string.Format("SELECT {0} FROM {1} [NOLOCK] WHERE {2}"
                , FormatColumns()
                , _entity.EntityName);
        }
        */

        /*
        public string FormatColumns(string[] columns = null)
        {
            return columns == null ? _entity.FormatColumns() : _entity.FormatColumns(columns);
        }
        */
    }

    public class DapperPatterns<TModel, TKey> : IDapperPatterns<TModel, TKey> {
        private readonly DapperService _service;
        private readonly SqlStrings<TModel> _sqlStrings;

        public DapperPatterns(DapperService service) {
            _service = service;
            _sqlStrings = new SqlStrings<TModel>();
        }

        public TModel Get(TKey id) {
            throw new NotImplementedException();
        }

        public TModel GetSingle(TKey id) {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> Get(SqlWhere clause) {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> Get(ArgsMap constraints) {
            throw new NotImplementedException();
        }

        public TModel Put(TModel entity) {
            throw new NotImplementedException();
        }

        public TKey Post(TModel entity) {
            throw new NotImplementedException();
        }

        public bool Delete(TKey id) {
            throw new NotImplementedException();
        }
    }






    /// <summary>
    /// Wrapper for dapper service calls.
    /// </summary>
    /// <remarks>
    /// Supports sync/async versions of Query, CacheOrQuery, Execute, CacheOrExecute, ExecuteScalar, CacheOrExecuteScalar, QueryMultiple, CacheOrQueryMultiple and includes built-in support for reading SQL from enums and text files for each of the methods.
    /// </remarks>
    public abstract class DapperService {
        protected const int DefaultCacheTime = 360;

        protected ICacheProvider CacheProvider { get; }
        protected string ConnectionString { get; }

        protected DapperService(string connectionString, ICacheProvider cacheProvider = null) {
            CacheProvider = cacheProvider ?? DefaultCacheProvider.LazyCache.Value;
            ConnectionString = connectionString;
        }

        protected SqlConnection GetConnection() => new SqlConnection(ConnectionString);

        #region Sync Wrappers

        // Query Start
        protected IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        protected IEnumerable<T> Query<T>(ISqlStatement statement, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
            => Query<T>(statement.ToSql(), statement.GetParam(), transaction, buffered, commandTimeout, commandType);

        protected IEnumerable<T> Query<T>(Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
            => Query<T>(sql.GetDescription(), param, transaction, buffered, commandTimeout, commandType);

        protected IEnumerable<T> QueryFile<T>(string path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
            => Query<T>(path.ReadFileContents(), param, transaction, buffered, commandTimeout, commandType);

        protected IEnumerable<T> QueryFile<T>(Enum path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
            => QueryFile<T>(path.GetDescription(), param, transaction, buffered, commandTimeout, commandType);

        protected IEnumerable<T> QuerySProc<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
            => Query<T>(sql, param, transaction, buffered, commandTimeout, CommandType.StoredProcedure);

        protected IEnumerable<T> QuerySProc<T>(ISqlStatement statement, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
            => Query<T>(statement.ToSql(), statement.GetParam(), transaction, buffered, commandTimeout, CommandType.StoredProcedure);

        protected IEnumerable<T> QuerySProc<T>(Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
            => QuerySProc<T>(sql.GetDescription(), param, transaction, buffered, commandTimeout);

        // Query End

        // CacheOrQuery Start
        protected IEnumerable<T> CacheOrQuery<T>(string key, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheProvider.GetWithRefresh(key, () => Query<T>(sql, param, transaction, buffered, commandTimeout, commandType), cacheTime);

        protected IEnumerable<T> CacheOrQuery<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheProvider.GetWithRefresh(key, () => Query<T>(statement.ToSql(), statement.GetParam(), transaction, buffered, commandTimeout, commandType), cacheTime);

        protected IEnumerable<T> CacheOrQuery<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrQuery<T>(key, sql.GetDescription(), param, transaction, buffered, commandTimeout, commandType, cacheTime);

        protected IEnumerable<T> CacheOrQueryFile<T>(string key, string path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrQuery<T>(key, path.ReadFileContents(), param, transaction, buffered, commandTimeout, commandType, cacheTime);

        protected IEnumerable<T> CacheOrQueryFile<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrQueryFile<T>(key, path.GetDescription(), param, transaction, buffered, commandTimeout, commandType, cacheTime);

        protected IEnumerable<T> CacheOrQuerySProc<T>(string key, string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrQuery<T>(key, sql, param, transaction, buffered, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected IEnumerable<T> CacheOrQuerySProc<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrQuery<T>(key, statement.ToSql(), statement.GetParam(), transaction, buffered, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected IEnumerable<T> CacheOrQuerySProc<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrQuerySProc<T>(key, sql.GetDescription(), param, transaction, buffered, commandTimeout, cacheTime);

        // CacheOrQuery End

        // Execute Start
        protected int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return connection.Execute(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected int Execute(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => Execute(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected int Execute(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => Execute(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected int ExecuteFile(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => Execute(path.ReadFileContents(), param, transaction, commandTimeout, commandType);

        protected int ExecuteFile(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => ExecuteFile(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected int ExecuteSProc(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => Execute(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected int ExecuteSProc(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => Execute(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected int ExecuteSProc(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => ExecuteSProc(sql.GetDescription(), param, transaction, commandTimeout);

        // Execute End

        // Execute (with return param) Start
        protected T Execute<T>(string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue) {
            var parameters = new DynamicParameters(param);
            parameters.Add(returnParam, dbType: ConvertTypeToDbType<T>(), direction: returnParamDirection);
            Execute(sql, parameters, transaction, commandTimeout, commandType);
            return parameters.Get<T>(returnParam);
        }

        protected T Execute<T>(ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => Execute<T>(statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, commandType, returnParamDirection);

        protected T Execute<T>(Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => Execute<T>(sql.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection);

        protected T ExecuteFile<T>(string path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => Execute<T>(path.ReadFileContents(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection);

        protected T ExecuteFile<T>(Enum path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => ExecuteFile<T>(path.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection);

        protected T ExecuteSProc<T>(string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => Execute<T>(sql, returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection);

        protected T ExecuteSProc<T>(ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => Execute<T>(statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection);

        protected T ExecuteSProc<T>(Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => Execute<T>(sql.GetDescription(), returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection);

        // Execute (with return param) End

        // CacheOrExecute (with return param) Start
        protected T CacheOrExecute<T>(string key, string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheProvider.GetWithRefresh(key, () => Execute<T>(sql, returnParam, param, transaction, commandTimeout, commandType, returnParamDirection), cacheTime);

        protected T CacheOrExecute<T>(string key, ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheProvider.GetWithRefresh(key, () => Execute<T>(statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, commandType, returnParamDirection), cacheTime);

        protected T CacheOrExecute<T>(string key, Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecute<T>(key, sql.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected T CacheOrExecuteFile<T>(string key, string path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecute<T>(key, path.ReadFileContents(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected T CacheOrExecuteFile<T>(string key, Enum path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteFile<T>(key, path.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected T CacheOrExecuteSProc<T>(string key, string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecute<T>(key, sql, returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection, cacheTime);

        protected T CacheOrExecuteSProc<T>(string key, ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecute<T>(key, statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection, cacheTime);

        protected T CacheOrExecuteSProc<T>(string key, Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecute<T>(key, sql.GetDescription(), returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection, cacheTime);

        // CacheOrExecute (with return param) End

        // ExecuteScalar Start
        protected T ExecuteScalar<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return connection.ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected T ExecuteScalar<T>(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => ExecuteScalar<T>(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected T ExecuteScalar<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => ExecuteScalar<T>(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected T ExecuteScalarFile<T>(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => ExecuteScalar<T>(path.ReadFileContents(), param, transaction, commandTimeout, commandType);

        protected T ExecuteScalarFile<T>(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => ExecuteScalarFile<T>(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected T ExecuteScalarSProc<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => ExecuteScalar<T>(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected T ExecuteScalarSProc<T>(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => ExecuteScalar<T>(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected T ExecuteScalarSProc<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => ExecuteScalarSProc<T>(sql.GetDescription(), param, transaction, commandTimeout);

        // ExecuteScalar End

        // CacheOrExecuteScalar Start
        protected T CacheOrExecuteScalar<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheProvider.GetWithRefresh(key, () => ExecuteScalar<T>(sql, param, transaction, commandTimeout, commandType), cacheTime);

        protected T CacheOrExecuteScalar<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalar<T>(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType, cacheTime);

        protected T CacheOrExecuteScalar<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalar<T>(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected T CacheOrExecuteScalarFile<T>(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalar<T>(key, path.ReadFileContents(), param, transaction, commandTimeout, commandType, cacheTime);

        protected T CacheOrExecuteScalarFile<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalarFile<T>(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected T CacheOrExecuteScalarSProc<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalar<T>(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected T CacheOrExecuteScalarSProc<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalar<T>(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected T CacheOrExecuteScalarSProc<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => CacheOrExecuteScalarSProc<T>(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);

        // CacheOrExecuteScalar End

        // QueryMultiple Start
        protected SqlMapper.GridReader QueryMultiple(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return connection.QueryMultiple(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected SqlMapper.GridReader QueryMultiple(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => QueryMultiple(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected SqlMapper.GridReader QueryMultiple(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => QueryMultiple(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected SqlMapper.GridReader QueryMultipleFile(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => QueryMultiple(path.ReadFileContents(), param, transaction, commandTimeout, commandType);

        protected SqlMapper.GridReader QueryMultipleFile(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => QueryMultipleFile(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected SqlMapper.GridReader QueryMultipleSProc(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => QueryMultiple(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected SqlMapper.GridReader QueryMultipleSProc(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => QueryMultiple(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected SqlMapper.GridReader QueryMultipleSProc(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => QueryMultipleSProc(sql.GetDescription(), param, transaction, commandTimeout);

        // QueryMultiple End

        // CacheOrQueryMultiple Start
        protected SqlMapper.GridReader CacheOrQueryMultiple(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => CacheProvider.GetWithRefresh(key, () => QueryMultiple(sql, param, transaction, commandTimeout, commandType), cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultiple(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultiple(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType, cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultiple(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultiple(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultipleFile(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultiple(key, path.ReadFileContents(), param, transaction, commandTimeout, commandType, cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultipleFile(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultipleFile(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultipleSProc(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultiple(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultipleSProc(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultiple(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected SqlMapper.GridReader CacheOrQueryMultipleSProc(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
            => CacheOrQueryMultipleSProc(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);

        // CacheOrQueryMultiple End

        #endregion

        #region Async wrappers

        // QueryAsync Start
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return await connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<IEnumerable<T>> QueryAsync<T>(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryAsync<T>(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected async Task<IEnumerable<T>> QueryAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryAsync<T>(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<IEnumerable<T>> QueryFileAsync<T>(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryAsync<T>(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);

        protected async Task<IEnumerable<T>> QueryFileAsync<T>(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryFileAsync<T>(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<IEnumerable<T>> QuerySProcAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await QueryAsync<T>(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<IEnumerable<T>> QuerySProcAsync<T>(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => await QueryAsync<T>(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<IEnumerable<T>> QuerySProcAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await QuerySProcAsync<T>(sql.GetDescription(), param, transaction, commandTimeout);

        // QueryAsync End

        // CacheOrQueryAsync Start
        protected async Task<IEnumerable<T>> CacheOrQueryAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheProvider.GetWithRefreshAsync(key, async () => await QueryAsync<T>(sql, param, transaction, commandTimeout, commandType), cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQueryAsync<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQueryAsync<T>(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType, cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQueryAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQueryAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQueryFileAsync<T>(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQueryAsync<T>(key, await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQueryFileAsync<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQueryFileAsync<T>(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQueryAsync<T>(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQueryAsync<T>(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected async Task<IEnumerable<T>> CacheOrQuerySProcAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrQuerySProcAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);

        // CacheOrQueryAsync End

        // ExecuteAsync Start
        protected async Task<int> ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return await connection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<int> ExecuteAsync(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteAsync(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected async Task<int> ExecuteAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteAsync(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<int> ExecuteFileAsync(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteAsync(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);

        protected async Task<int> ExecuteFileAsync(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteFileAsync(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<int> ExecuteSProcAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await ExecuteAsync(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<int> ExecuteSProcAsync(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => await ExecuteAsync(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<int> ExecuteSProcAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await ExecuteSProcAsync(sql.GetDescription(), param, transaction, commandTimeout);

        // ExecuteAsync End

        // ExecuteAsync (with return param) Start
        protected async Task<T> ExecuteAsync<T>(string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue) {
            var parameters = new DynamicParameters(param);
            parameters.Add(returnParam, dbType: ConvertTypeToDbType<T>(), direction: returnParamDirection);
            await ExecuteAsync(sql, parameters, transaction, commandTimeout, commandType);
            return parameters.Get<T>(returnParam);
        }

        protected async Task<T> ExecuteAsync<T>(ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteAsync<T>(statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, commandType, returnParamDirection);

        protected async Task<T> ExecuteAsync<T>(Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteAsync<T>(sql.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection);

        protected async Task<T> ExecuteFileAsync<T>(string path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteAsync<T>(path.ReadFileContents(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection);

        protected async Task<T> ExecuteFileAsync<T>(Enum path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteFileAsync<T>(path.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection);

        protected async Task<T> ExecuteSProcAsync<T>(string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteAsync<T>(sql, returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection);

        protected async Task<T> ExecuteSProcAsync<T>(ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteAsync<T>(statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection);

        protected async Task<T> ExecuteSProcAsync<T>(Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue)
            => await ExecuteAsync<T>(sql.GetDescription(), returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection);

        // ExecuteAsync (with return param) End

        // CacheOrExecuteAsync (with return param) Start
        protected async Task<T> CacheOrExecuteAsync<T>(string key, string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheProvider.GetWithRefreshAsync(key, async () => await ExecuteAsync<T>(sql, returnParam, param, transaction, commandTimeout, commandType, returnParamDirection), cacheTime);

        protected async Task<T> CacheOrExecuteAsync<T>(string key, ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteAsync<T>(key, statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected async Task<T> CacheOrExecuteAsync<T>(string key, Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteAsync<T>(key, sql.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected async Task<T> CacheOrExecuteFileAsync<T>(string key, string path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteAsync<T>(key, path.ReadFileContents(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected async Task<T> CacheOrExecuteFileAsync<T>(string key, Enum path, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteFileAsync<T>(key, path.GetDescription(), returnParam, param, transaction, commandTimeout, commandType, returnParamDirection, cacheTime);

        protected async Task<T> CacheOrExecuteSProcAsync<T>(string key, string sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteAsync<T>(key, sql, returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection, cacheTime);

        protected async Task<T> CacheOrExecuteSProcAsync<T>(string key, ISqlStatement statement, string returnParam, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteAsync<T>(key, statement.ToSql(), returnParam, statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection, cacheTime);

        protected async Task<T> CacheOrExecuteSProcAsync<T>(string key, Enum sql, string returnParam, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, ParameterDirection returnParamDirection = ParameterDirection.ReturnValue, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteAsync<T>(key, sql.GetDescription(), returnParam, param, transaction, commandTimeout, CommandType.StoredProcedure, returnParamDirection, cacheTime);

        // CacheOrExecuteAsync (with return param) End

        // ExecuteScalarAsync Start
        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return await connection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<T> ExecuteScalarAsync<T>(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteScalarAsync<T>(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected async Task<T> ExecuteScalarAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteScalarAsync<T>(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<T> ExecuteScalarFileAsync<T>(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteScalarAsync<T>(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);

        protected async Task<T> ExecuteScalarFileAsync<T>(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await ExecuteScalarFileAsync<T>(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<T> ExecuteScalarSProcAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<T> ExecuteScalarSProcAsync<T>(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => await ExecuteScalarAsync<T>(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<T> ExecuteScalarSProcAsync<T>(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await ExecuteScalarSProcAsync<T>(sql.GetDescription(), param, transaction, commandTimeout);

        // ExecuteScalarAsync End

        // CacheOrExecuteScalarAsync Start
        protected async Task<T> CacheOrExecuteScalarAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheProvider.GetWithRefreshAsync(key, async () => await ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType), cacheTime);

        protected async Task<T> CacheOrExecuteScalarAsync<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarAsync<T>(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType, cacheTime);

        protected async Task<T> CacheOrExecuteScalarAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<T> CacheOrExecuteScalarFileAsync<T>(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarAsync<T>(key, await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<T> CacheOrExecuteScalarFileAsync<T>(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarFileAsync<T>(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<T> CacheOrExecuteScalarSProcAsync<T>(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarAsync<T>(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected async Task<T> CacheOrExecuteScalarSProcAsync<T>(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarAsync<T>(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected async Task<T> CacheOrExecuteScalarSProcAsync<T>(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime) where T : class
            => await CacheOrExecuteScalarSProcAsync<T>(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);

        // CacheOrExecuteScalarAsync End

        // QueryMultipleAsync Start
        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) {
            using (var connection = GetConnection()) {
                return await connection.QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType);
            }
        }

        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryMultipleAsync(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType);

        protected async Task<SqlMapper.GridReader> QueryMultipleAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryMultipleAsync(sql.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<SqlMapper.GridReader> QueryMultipleFileAsync(string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryMultipleAsync(await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType);

        protected async Task<SqlMapper.GridReader> QueryMultipleFileAsync(Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            => await QueryMultipleFileAsync(path.GetDescription(), param, transaction, commandTimeout, commandType);

        protected async Task<SqlMapper.GridReader> QueryMultipleSProcAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await QueryMultipleAsync(sql, param, transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<SqlMapper.GridReader> QueryMultipleSProcAsync(ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null)
            => await QueryMultipleAsync(statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure);

        protected async Task<SqlMapper.GridReader> QueryMultipleSProcAsync(Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null)
            => await QueryMultipleSProcAsync(sql.GetDescription(), param, transaction, commandTimeout);

        // QueryMultipleAsync End

        // CacheOrQueryMultipleAsync Start
        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleAsync(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => await CacheProvider.GetWithRefreshAsync(key, async () => await QueryMultipleAsync(sql, param, transaction, commandTimeout, commandType), cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleAsync(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleAsync(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, commandType, cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleAsync(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleAsync(key, sql.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleFileAsync(string key, string path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleAsync(key, await path.ReadFileContentsAsync(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleFileAsync(string key, Enum path, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleFileAsync(key, path.GetDescription(), param, transaction, commandTimeout, commandType, cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleSProcAsync(string key, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleAsync(key, sql, param, transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleSProcAsync(string key, ISqlStatement statement, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleAsync(key, statement.ToSql(), statement.GetParam(), transaction, commandTimeout, CommandType.StoredProcedure, cacheTime);

        protected async Task<SqlMapper.GridReader> CacheOrQueryMultipleSProcAsync(string key, Enum sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, int cacheTime = DefaultCacheTime)
            => await CacheOrQueryMultipleSProcAsync(key, sql.GetDescription(), param, transaction, commandTimeout, cacheTime);

        // CacheOrQueryMultipleAsync End

        #endregion

        public static Type ConvertDbTypeToType(DbType type) => GetTypeFromTypeCode(ConvertDbTypeToTypeCode(type));
        public static Type GetTypeFromTypeCode(TypeCode typeCode) => typeCode == TypeCode.Empty ? null : Type.GetType("System." + typeCode);

        public static TypeCode ConvertDbTypeToTypeCode(DbType dbType) {
            switch (dbType) {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return TypeCode.String;
                case DbType.Boolean:
                    return TypeCode.Boolean;
                case DbType.Byte:
                    return TypeCode.Byte;
                case DbType.VarNumeric: // ???
                case DbType.Currency:
                case DbType.Decimal:
                    return TypeCode.Decimal;
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2: // new Katmai type
                case DbType.Time: // new Katmai type - no TypeCode for TimeSpan
                    return TypeCode.DateTime;
                case DbType.Double:
                    return TypeCode.Double;
                case DbType.Int16:
                    return TypeCode.Int16;
                case DbType.Int32:
                    return TypeCode.Int32;
                case DbType.Int64:
                    return TypeCode.Int64;
                case DbType.SByte:
                    return TypeCode.SByte;
                case DbType.Single:
                    return TypeCode.Single;
                case DbType.UInt16:
                    return TypeCode.UInt16;
                case DbType.UInt32:
                    return TypeCode.UInt32;
                case DbType.UInt64:
                    return TypeCode.UInt64;
                case DbType.Guid: // ???
                case DbType.Binary:
                case DbType.Object:
                case DbType.DateTimeOffset: // new Katmai type - no TypeCode for DateTimeOffset
                default:
                    return TypeCode.Object;
            }
        }

        public static DbType ConvertTypeToDbType<T>() {
            return ConvertTypeCodeToDbType(Type.GetTypeCode(typeof (T)));
        }

        public static DbType ConvertTypeCodeToDbType(TypeCode typeCode) {
            // no TypeCode equivalent for TimeSpan or DateTimeOffset
            switch (typeCode) {
                case TypeCode.Boolean:
                    return DbType.Boolean;
                case TypeCode.Byte:
                    return DbType.Byte;
                case TypeCode.Char:
                    return DbType.StringFixedLength; // ???
                case TypeCode.DateTime: // Used for Date, DateTime and DateTime2 DbTypes
                    return DbType.DateTime;
                case TypeCode.Decimal:
                    return DbType.Decimal;
                case TypeCode.Double:
                    return DbType.Double;
                case TypeCode.Int16:
                    return DbType.Int16;
                case TypeCode.Int32:
                    return DbType.Int32;
                case TypeCode.Int64:
                    return DbType.Int64;
                case TypeCode.SByte:
                    return DbType.SByte;
                case TypeCode.Single:
                    return DbType.Single;
                case TypeCode.String:
                    return DbType.String;
                case TypeCode.UInt16:
                    return DbType.UInt16;
                case TypeCode.UInt32:
                    return DbType.UInt32;
                case TypeCode.UInt64:
                    return DbType.UInt64;
                case TypeCode.DBNull:
                case TypeCode.Empty:
                case TypeCode.Object:
                default:
                    return DbType.Object;
            }
        }
    }

}