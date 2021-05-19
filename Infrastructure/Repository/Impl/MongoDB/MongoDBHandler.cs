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
            
            if(_mongoUrl != null){
                _client = new MongoClient(_mongoUrl);
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
