using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
namespace ShipAnyWhere
{
    public static class UpdateInventory
    {
        [FunctionName("UpdateInventory")]
            public async static void Run([ServiceBusTrigger("orders", Connection = "ShipAnyWhere_SERVICEBUS")]PurchaseOrder order,

                              [CosmosDB(databaseName: "inventory",
                               collectionName:"Products",
                               ConnectionStringSetting = "dbConnectionString",
                               Id = "{Sku}")]Product product ,                              

                               [CosmosDB(databaseName:"Sales", 
                               collectionName:"Orders", 
                               ConnectionStringSetting ="dbConnectionString")]
                               IAsyncCollector<PurchaseOrder> writeResultsToCosmos,  

                               ILogger log)
                               
        {
              log.LogInformation($"C# ServiceBus queue trigger function processed message: {order.Sku}");
           if( product!=null && product.AvailableQuantity >= order.Quantity)
            
            {
              order.Status = "Accepted";
            }
            else
            order.Status ="Rejected";
           await writeResultsToCosmos.AddAsync(order);
          
        }
    }
}
