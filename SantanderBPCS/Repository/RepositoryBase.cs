using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using AutoMapper;
using Dapper;

namespace SantanderBPCS.Repository
{
    public abstract class RepositoryBase : IRepositoryBase
    {
        protected string _connectionString;

        public async Task<int> Execute(string query, object param = null)
        {
            try
            {
                _connectionString = ConfigurationManager.ConnectionStrings["BpcsConnectionString"].ConnectionString;

                using (var conn = ObterConexaoBase())
                {
                    try
                    {

                        var result = await conn.ExecuteAsync(query, param);
                        return result;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Erro: {e.Message}");
            }
        }

        public IDbConnection ObterConexaoBase() => new OleDbConnection(_connectionString);

        
    }
}
