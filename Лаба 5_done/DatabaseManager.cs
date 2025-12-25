using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
namespace Лаба_4
{
    public class DatabaseManager : IDisposable{
        private string connectionString;
        private SQLiteConnection connection;
        public DatabaseManager(){
            var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.db");
            connectionString = $"Data Source={databasePath};Version=3;";
            InitializeDatabase();
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            Console.WriteLine("DatabaseManager инициализирован с SQLite");
        }
        private void InitializeDatabase(){
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.db"))){
                SQLiteConnection.CreateFile("clients.db");
            }
            using (var tempConnection = new SQLiteConnection(connectionString)){
                tempConnection.Open();
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Clients (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ClientType TEXT NOT NULL,
                    BaseCost REAL NOT NULL,
                    PricingStrategyType TEXT NOT NULL,
                    DiscountValue REAL DEFAULT 0,
                    AdditionalInfo TEXT,
                    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
                )";
                using (var command = new SQLiteCommand(createTableQuery, tempConnection)){
                    command.ExecuteNonQuery();
                }
            }
        }
        private object ToParameters(IClient client){
            return new{
                Name = client.Name,
                ClientType = client.ClientType,
                BaseCost = client.BaseCost,
                PricingStrategyType = client.PricingStrategyType,
                DiscountValue = client.DiscountValue,
                AdditionalInfo = client.AdditionalInfo
            };
        }
        public void AddClient(IClient client){
            string query = @"
            INSERT INTO Clients (Name, ClientType, BaseCost, PricingStrategyType, DiscountValue, AdditionalInfo)
            VALUES (@Name, @ClientType, @BaseCost, @PricingStrategyType, @DiscountValue, @AdditionalInfo);
            SELECT last_insert_rowid();";
            var newId = connection.ExecuteScalar<int>(query, ToParameters(client));
            client.Id = newId;
        }
        public void UpdateClient(IClient client){
            string query = @"
            UPDATE Clients 
            SET Name = @Name, 
                ClientType = @ClientType, 
                BaseCost = @BaseCost, 
                PricingStrategyType = @PricingStrategyType,
                DiscountValue = @DiscountValue,
                AdditionalInfo = @AdditionalInfo
            WHERE Id = @Id";
            var parameters = new{
                Id = client.Id,
                Name = client.Name,
                ClientType = client.ClientType,
                BaseCost = client.BaseCost,
                PricingStrategyType = client.PricingStrategyType,
                DiscountValue = client.DiscountValue,
                AdditionalInfo = client.AdditionalInfo
            };
            connection.Execute(query, parameters);
        }
        public void DeleteClient(int id){
            string query = "DELETE FROM Clients WHERE Id = @Id";
            connection.Execute(query, new { Id = id });
        }
        public List<IClient> GetAllClients(){
            var clientsData = connection.Query<ClientData>("SELECT * FROM Clients ORDER BY Name").ToList();
            var clients = new List<IClient>();
            foreach (var data in clientsData){
                clients.Add(CreateClientFromData(data));
            }
            return clients;
        }
        public IClient GetClientById(int id){
            var data = connection.QueryFirstOrDefault<ClientData>(
                "SELECT * FROM Clients WHERE Id = @Id", new { Id = id });
            return data != null ? CreateClientFromData(data) : null;
        }
        private IClient CreateClientFromData(ClientData data){
            IClient client;
            switch (data.ClientType){
                case "VIP":
                    client = new VIPClient();
                    break;
                case "Corporate":
                    client = new CorporateClient();
                    break;
                default:
                    client = new RegularClient();
                    break;
            }
            client.Id = data.Id;
            client.Name = data.Name;
            client.ClientType = data.ClientType;
            client.BaseCost = data.BaseCost;
            client.PricingStrategyType = data.PricingStrategyType;
            client.DiscountValue = data.DiscountValue;
            client.AdditionalInfo = data.AdditionalInfo;
            return client;
        }
        private class ClientData{
            public int Id { get; set; }
            public string Name { get; set; }
            public string ClientType { get; set; }
            public double BaseCost { get; set; }
            public string PricingStrategyType { get; set; }
            public double DiscountValue { get; set; }
            public string AdditionalInfo { get; set; }
        }
        public void Dispose(){
            connection?.Close();
            connection?.Dispose();
        }
    }
}