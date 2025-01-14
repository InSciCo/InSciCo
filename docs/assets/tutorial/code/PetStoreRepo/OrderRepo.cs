using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using LazyStackDynamoDBRepo;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using PetStoreSchema.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace PetStoreRepo.Models
{
    public class OrderEnvelope : DataEnvelope<Order>
    {

        protected override void SetDbRecordFromEnvelopeInstance()
        {
            // Set the Envelope Key fields from the EntityInstance data
            TypeName = "Order.v1.0.0";
            // Primary Key is PartitionKey + SortKey 
            PK = "Order:"; // Partition key
            SK = EntityInstance.Id}; // sort/range key

            // The base method copies information from the envelope keys into the dbRecord
            base.SetDbRecordFromEnvelopeInstance();
        }
    }

    public interface IOrderRepo : IDYDBRepository<OrderEnvelope, Order> 
    {
        Task<ActionResult<Order>> PlaceOrderAsync(Order body);
        Task<ActionResult<Order>> GetOrderByIdAsync(long orderId);
        Task<StatusCodeResult> DeleteOrderAsync(long orderId);
    }

    public class OrderRepo : DYDBRepository<OrderEnvelope, Order>, IOrderRepo
    {
        public OrderRepo(
            IAmazonDynamoDB client,
            IPetRepo petRepo
            ) : base(client, envVarTableName: "TABLE_NAME")
        {
            this.petRepo = petRepo;
        }


        IPetRepo petRepo;

        // This dictionary allows us to use class attribute names that happen to also be
        // DynamoDB reserved words in our ProjectionExpressions
        Dictionary<string, string> _ExpressionAttributeNames = new Dictionary<string, string>()
        {
            {"#Data", "Data" },
            {"#Status", "Status" },
            {"#General", "General" }
        };


        public async Task<ActionResult<Order>> PlaceOrderAsync(Order body)
        {
            return await CreateAsync(body as Order);
        }

        public async Task<ActionResult<Order>> GetOrderByIdAsync(long orderId)
        {
            return await ReadAsync(
                 pK: "Orders:",
                 sK: "Order:" + orderId.ToString());
        }

        public async Task<StatusCodeResult> DeleteOrderAsync(long orderId)
        {
            return await DeleteAsync(
                pK: "Orders:",
                sK: "Order:" + orderId.ToString());
        }
    }
}

