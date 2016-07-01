using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamoDbTutorial
{
    class Program
    {
        /// <summary>
        /// Tutorials
        /// http://docs.aws.amazon.com/amazondynamodb/latest/gettingstartedguide/GettingStarted.NET.01.html
        /// https://dotnetcodr.com/2015/01/26/using-amazon-dynamodb-with-the-aws-net-api-part-2-code-beginnings/
        /// https://dotnetcodr.com/2015/01/29/using-amazon-dynamodb-with-the-aws-net-api-part-3-table-operations/
        /// https://dotnetcodr.com/2015/02/02/using-amazon-dynamodb-with-the-aws-net-api-part-4-record-insertion/
        /// https://dotnetcodr.com/2015/02/05/using-amazon-dynamodb-with-the-aws-net-api-part-5-updating-and-deleting-records/
        /// 
        /// </summary>
        public Program()
        {
            AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            ddbConfig.ServiceURL = "http://dynamodb.us-west-2.amazonaws.com";
            ddbConfig.Validate();
            AmazonDynamoDBClient client;
            try
            {
                client = new AmazonDynamoDBClient(ddbConfig);

                // Create
                //CreateTable(client);

                // Load
                //LoadTable(client);

                // Read
                //ReadItems(client, 2016, "The Big Movie");

                // Update
                //UpdateItems(client);

                // Delete
                DeleteItem(client, 2014, "Bohemian Rhapsody!");

                // Delete table
                //DeleteTable(client, "Movies");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create a DynamoDB client; " + ex.Message);
            }

            Console.Write("\n\n ...Press any key to continue");
            Console.ReadKey();
            Console.WriteLine();
        }

        void CreateTable(AmazonDynamoDBClient client)
        {
            // Build a 'CreateTableRequest' for the new table
            CreateTableRequest createRequest = new CreateTableRequest
            {
                TableName = "Movies",
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                  new AttributeDefinition
                  {
                    AttributeName = "year",
                    AttributeType = "N"
                  },
                  new AttributeDefinition
                  {
                    AttributeName = "title",
                    AttributeType = "S"
                  }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                  new KeySchemaElement
                  {
                    AttributeName = "year",
                    KeyType = "HASH"
                  },
                  new KeySchemaElement
                  {
                    AttributeName = "title",
                    KeyType = "RANGE"
                  }
                },
            };

            // Provisioned-throughput settings are required even though
            // the local test version of DynamoDB ignores them
            createRequest.ProvisionedThroughput = new ProvisionedThroughput(1, 1);
            CreateTableResponse createResponse;
            try
            {
                createResponse = client.CreateTable(createRequest);
                Console.WriteLine("\n\n Created the \"Movies\" table successfully!\n    Status of the new table: '{0}'", createResponse.TableDescription.TableStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create the new table; " + ex.Message);
            }
        }

        void LoadTable(AmazonDynamoDBClient client)
        {
            try
            {
                var table = GetTableObject(client, "Movies");
                if (table == null)
                    return;

                Document movie = new Document();
                movie["year"] = 2012;
                movie["title"] = "Bohemian Rhapsody!";

                Document info = new Document();
                info["directors"] = new List<string> { "Alice Smith", "Bob Jones" };
                info["image_url"] = new DynamoDBNull();

                movie.Add("info", info);

                table.PutItem(movie);
                Console.WriteLine("\n   Data loaded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to load data in table; " + ex.Message);
            }
        }

        void ReadItems(AmazonDynamoDBClient client, int year, string title)
        {
            try
            {
                var table = GetTableObject(client, "Movies");
                if (table == null)
                    return;

                Document document = table.GetItem(year, title);
                if (document != null)
                    Console.WriteLine("\nGetItem succeeded: \n" + document.ToJsonPretty());
                else
                    Console.WriteLine("\nGetItem succeeded, but the item was not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to read data; " + ex.Message);
            }
        }

        void UpdateItems(AmazonDynamoDBClient client)
        {
            try
            {
                var table = GetTableObject(client, "Movies");
                if (table == null)
                    return;

                DynamoDBContext context = new DynamoDBContext(client);
                var movie = new Movies();
                movie.year = 2016;
                movie.title = "The Big Movie";
                movie.info = new Info();
                movie.info.actors = new List<string> { "Larry", "Moe", "Curly" };
                movie.info.rating = 5.5;

                context.Save<Movies>(movie);
                Console.WriteLine("\nItem updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to update data; " + ex.Message);
            }
        }

        void DeleteItem(AmazonDynamoDBClient client, int year, string title)
        {
            try
            {
                var table = GetTableObject(client, "Movies");
                if (table == null)
                    return;

                DynamoDBContext context = new DynamoDBContext(client);
                var movie = new Movies();
                movie.year = year;
                movie.title = title;               

                context.Delete<Movies>(movie);
                Console.WriteLine("\nItem Deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to delete data; " + ex.Message);
            }
        }

        void DeleteTable(AmazonDynamoDBClient client,string tableName)
        {
            try
            {
                client.DeleteTable(tableName);

                Console.WriteLine("\nTable Deleted");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to delete table; " + ex.Message);
            }
        }

        Table GetTableObject(AmazonDynamoDBClient client, string tableName)
        {
            // Now, create a Table object for the specified table
            try
            {
                return Table.LoadTable(client, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to load the 'Movies' table; " + ex.Message);
                return null;
            }
        }

        static void Main(string[] args)
        {
            new Program();
        }
    }
}
