using System.Reflection;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Hello Cosmos!");

var uri = "https://projectzen.documents.azure.com:443/";
var key = GetSecret("CosmosDBKey");

var client = new CosmosClient(uri, key);

//option 1
DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync("employees");
Database db = databaseResponse.Database;



// //option 2  - When you KNOW it will succeed. (which isn't often)
// Database database = await client.CreateDatabaseIfNotExistsAsync("employees");

var containerResponse = await db.CreateContainerIfNotExistsAsync("emps", "/address/zip");
var container = containerResponse.Container;

// //create some employees
// var emp1 = new { id = "1", name = "John", age=50, address = new { street = "123 main", city = "monroe", state = "mi", zip = "48162" } };
// var emp2 = new { id = "2", name = "Jane", age=40, address = new { street = "456 main", city = "monroe", state = "mi", zip = "48162" } };
// var emp3 = new { id = "3", name = "Joe", age=30, address = new { street = "789 main", city = "Columbus", state = "OH", zip = "50123" } };

// //insert them
// var emp1Response = await container.UpsertItemAsync(emp1);
// Console.WriteLine($"Inserted {emp1.name} | RUs: {emp1Response.RequestCharge}");

// var emp2Response = await container.UpsertItemAsync(emp2);
// Console.WriteLine($"Inserted {emp2.name} | RUs: {emp2Response.RequestCharge}");

// var emp3Response = await container.UpsertItemAsync(emp3);
// Console.WriteLine($"Inserted {emp3.name} | RUs: {emp3Response.RequestCharge}");


//query them
var query = "SELECT * FROM emps e WHERE e.address.zip = '48162'";

var queryDefinition = new QueryDefinition(query);

var queryResultSetIterator = container.GetItemQueryIterator<Employee>(queryDefinition);

while (queryResultSetIterator.HasMoreResults)
{
    var currentResultSet = await queryResultSetIterator.ReadNextAsync();
    foreach (var emp in currentResultSet)
    {
        Console.WriteLine($"{emp.name} lives in {emp.address.city}");
    }
}




static string GetSecret(string secretName)
{
        // Get the user secrets
    var configBuilder = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly(), true);
    var config = configBuilder.Build();
    // Get the storage account key from the user secrets
    var storageAccountKey = config[secretName] ?? throw new Exception($"{secretName} is not in user-secrets");;
    
    return storageAccountKey;
}

class Employee
{
    public string id { get; set; }
    public string name { get; set; }
    public int age { get; set; }
    public Address address { get; set; }
}
class Address
{
    public string street { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
}




