using System.Collections;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using SimpleJSON;

public class WorkMogoDb : MonoBehaviour
{ 

    public static MongoClient client;
    public static IMongoDatabase database;
    private static bool isConnection = false;
    public static bool Connection()
    {
        try
        {
            if (!isConnection)
            {
                client = new MongoClient("mongodb+srv://admin:0987654321@gameclaster.zzp3f.mongodb.net/GameClaster?retryWrites=true&w=majority");
                database = client.GetDatabase("TestDatabase");
                isConnection = true;
            }
        }
        catch
        {
            isConnection = false;
        }
        return isConnection;
    }


    public static string GetData(string name, string path)
    {
        try
        {
            return GetJson(name);
        }
        catch
        {
            return null;
        }
    }

    public static string GetJson(string name)
    {
        var collection = database.GetCollection<BsonDocument>("data_json");
        var filter = new BsonDocument() { { "name", name } };
        var jsonFormat = collection.Find(filter).FirstOrDefault();
        return jsonFormat is null ? null : jsonFormat.GetValue(2).ToString();
    }

    public static void GetImages(string name, string path)
    {
        var gridFS = new GridFSBucket(database);
        var collection = database.GetCollection<BsonDocument>("data_image");
        var filter = new BsonDocument() { { "father_name", name } };
        var nameImages = new List<string>();
        collection.Find(filter).ForEachAsync(x => nameImages.Add(x.GetValue(2).AsString));
        foreach (var nameImage in nameImages)
        {
            var file = new FileStream(path + "\\" + nameImage, FileMode.OpenOrCreate);
            gridFS.DownloadToStreamByNameAsync(nameImage, file);
        }
    }

    [System.Obsolete]
    public static bool SigIn(string name, string password)
    {
        var players = database.GetCollection<BsonDocument>("players");
        var filter = new BsonDocument()
            {
                {"name", name },
                {"password", password}
            };
        return players.Find(filter).Count() == 1 ? true : false;
    }

    [System.Obsolete]
    public static bool CheckIn(string name, string password)
    {
        var players = database.GetCollection<BsonDocument>("players");
        var filter = new BsonDocument()
            {
                {"name",name}
            };
        if (players.Find(filter).Count() != 0)
            return false;
        var newPlayer = new BsonDocument()
            {
                {"name", name },
                {"password", password}
            };
        players.InsertOneAsync(newPlayer);
        return true;
    }
    public static Dictionary<string, int> GetPlayerData(string name, Dictionary<string, int> data)
    {
        var dataPlayers = database.GetCollection<BsonDocument>("players_data");
        var filter = new BsonDocument()
            {
                {"name",name}
            };
        var result = new Dictionary<string, int>();
        var allPlayerData = dataPlayers.Find(filter).FirstOrDefault();
        foreach (var key in data.Keys)
        {
            result[key] = (int)allPlayerData[key];
        }
        return result;
    }

    public static void SetPlayerData(string name, Dictionary<string, int> data)
    {
        var dataPlayers = database.GetCollection<BsonDocument>("players_data");
        var filter = new BsonDocument()
            {
                {"name",name}
            };
        var allPlayerData = dataPlayers.Find(filter).FirstOrDefault();
        BsonDocument updata = UpdataPlayerData(name, data);
        if (allPlayerData is null)
        {
            dataPlayers.InsertOne(updata);
        }
        else
        {
            dataPlayers.FindOneAndUpdate(filter, updata);
        }
    }

    private static BsonDocument UpdataPlayerData(string name, Dictionary<string, int> data)
    {
        var result = new BsonDocument()
            {
                {"name", name }
            };
        result.Add(data);
        return result;
    }
}
