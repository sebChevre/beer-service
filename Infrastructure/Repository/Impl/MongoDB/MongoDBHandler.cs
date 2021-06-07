using System;
using System.Collections.Generic;
using MongoDB.Driver;

using BeerApi.Infrastructure.Configuration;

namespace BeerApi.Infrastructure.Repository.Impl.MongoDB
{
    public class MongoDBHandler
    {
    
        private readonly MongoClient _client;
        IBeerstoreDatabaseSettings _settings;
        IMongoDatabase _dataBase;

        string _mongoUrl;
        public MongoDBHandler(IBeerstoreDatabaseSettings settings)
        {
            _mongoUrl =  Environment.GetEnvironmentVariable("MONGODB_URL");
            var mongoUsername =  Environment.GetEnvironmentVariable("MONGODB_USERNAME");
            var mongoPass =  Environment.GetEnvironmentVariable("MONGODB_PASS");

            var connectionString = String.Format("mongodb://{0}:{1}@{2}",mongoUsername,mongoPass,_mongoUrl);
            
            
            if(_mongoUrl != null){
                _client = new MongoClient(connectionString);
                _settings = settings;
                _dataBase = _client.GetDatabase(settings.DatabaseName);
            }

        }

        public IMongoDatabase GetDataBase(){
            return this._dataBase;
        }

        public  List<string> GetListDatabaseName () {
            
            var dataBaseNames = _client.ListDatabaseNames();
            return getDatabaseNamesAsList(dataBaseNames);
        }

        private List<string> getDatabaseNamesAsList(IAsyncCursor<string> dbNames) {

            List<string> databaseNames = new List<string>();

            dbNames.ForEachAsync<string>(element => { databaseNames.Add(element);return; });

            return databaseNames;
        }
        
    }
}
