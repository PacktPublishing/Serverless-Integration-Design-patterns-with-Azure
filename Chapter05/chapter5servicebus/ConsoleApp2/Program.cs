using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace ConsoleApp1
{
    class Program
    {
        private static string sbconnection = "Endpoint=sb://contosointegration.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=StuWmIS1lrv4QmlGefa1Q3rVyuRuPPVOxdri59sEvLM=";
        static async Task Main(string[] args)
        {
            Program obj = new Program();
            //await obj.CreatetopicprioritySubscription("sample01");
            await obj.SendMessageToTopicAsync();
        }

        public async Task<object> CreateSbQueues(string queuename)
        {
            var client = new ManagementClient(sbconnection);
            bool queueExists = await client.QueueExistsAsync(queuename).ConfigureAwait(false);
            if (!queueExists)
            {
                QueueDescription queueName = new QueueDescription(queuename);
                queueName.MaxSizeInMB = 1024;
                queueName.DefaultMessageTimeToLive = new TimeSpan(2, 0, 0, 0);
                var result = await client.CreateQueueAsync(queueName).ConfigureAwait(false);
                return result;
            }
            else
            {
                return "Queue already exsits!!";
            }
        }

        public async Task<object> CreateSbtopic(string topicname ,string subscriptionName)
        {
            var client = new ManagementClient(sbconnection);
            bool topicExists = await client.TopicExistsAsync(topicname).ConfigureAwait(false);
            if (!topicExists)
            {
                TopicDescription topicName = new TopicDescription(topicname);
                topicName.MaxSizeInMB = 1024;
                topicName.DefaultMessageTimeToLive = new TimeSpan(2, 0, 0, 0);
                dynamic result = await client.CreateTopicAsync(topicname).ConfigureAwait(false);
                if (result.Path != null)
                {
                    SubscriptionDescription subName = new SubscriptionDescription(result.Path, subscriptionName);
                    subName.Status = 0;
                    var result01 = await client.CreateSubscriptionAsync(subName).ConfigureAwait(false);
                    return result;
                }
                return result;
            }
            else
            {
                return "Topic already exsits!!";
            }
        }

        public async Task<object> CreatetopicMatchAllSubscription(string topicname)
        {
            var client = new ManagementClient(sbconnection);
            bool topicExists = await client.TopicExistsAsync(topicname).ConfigureAwait(false);
            if (topicExists)
            {
                    SubscriptionDescription subName = new SubscriptionDescription(topicname, "AllMessages");
                    subName.Status = 0;
                    var result01 = await client.CreateSubscriptionAsync(subName).ConfigureAwait(false);
                    return result01;
             }
            else
            {
                return "Unable to create MatchAll Subscription!!";
            }
        }

        public async Task CreatetopicprioritySubscription(string topicname)
        {
            var client = new ManagementClient(sbconnection);
            bool topicExists = await client.TopicExistsAsync(topicname).ConfigureAwait(false);
            string[] subscriptionarray = new string[] { "highprioritysubscription", "lowprioritysubscription" };
            if (topicExists)
            {
                foreach (var item in subscriptionarray)
                {
                    if (item == "highprioritysubscription")
                    {
                        SubscriptionDescription subName = new SubscriptionDescription(topicname, item);
                        subName.Status = 0;
                        RuleDescription subscriptionRule = new RuleDescription();
                        subscriptionRule.Filter = new SqlFilter("Priority >= 10");
                        var result01 = await client.CreateSubscriptionAsync(subName, subscriptionRule).ConfigureAwait(false);
                    }
                    else
                    {
                        SubscriptionDescription subName = new SubscriptionDescription(topicname, item);
                        subName.Status = 0;
                        RuleDescription subscriptionRule = new RuleDescription();
                        subscriptionRule.Filter = new SqlFilter("Priority < 10");
                        var result01 = await client.CreateSubscriptionAsync(subName, subscriptionRule).ConfigureAwait(false);
                    }
                }
            }
        }


        public async Task<object> CreatetopicsqlFilterSubscription(string topicname)
        {
            var client = new ManagementClient(sbconnection);
            bool topicExists = await client.TopicExistsAsync(topicname).ConfigureAwait(false);
            if (topicExists)
            {
                SubscriptionDescription subName = new SubscriptionDescription(topicname, "SQLFilterSubscription");
                subName.Status = 0;
                RuleDescription subscriptionRule = new RuleDescription();
                var result01 = await client.CreateSubscriptionAsync(subName, subscriptionRule).ConfigureAwait(false);
                return result01;
            }
            else
            {
                return "Unable to create sqlfilter Subscription!!";
            }
        }
        public async Task<object> CreatetopicCorrelationFilterSubscription(string topicname)
        {
            var client = new ManagementClient(sbconnection);
            bool topicExists = await client.TopicExistsAsync(topicname).ConfigureAwait(false);
            if (topicExists)
            {
                SubscriptionDescription subName = new SubscriptionDescription(topicname, "CorrelationFilterSubscription");
                subName.Status = 0;
                RuleDescription subscriptionRule = new RuleDescription();
                subscriptionRule.Filter = new CorrelationFilter
                {
                       Label= "Correlationfiltersample",
                       ReplyTo = "x",
                       ContentType="Finanical",
                };
                var result01 = await client.CreateSubscriptionAsync(subName, subscriptionRule).ConfigureAwait(false);
                return result01;
            }
            else
            {
                return "Unable to create CorrelationFilter Subscription!!";
            }
        }


        public async Task SendMessageTopriorityQueue()
        {

            QueueClient highpriorityqueueClient = new QueueClient(sbconnection, "highpriorityqueue");
            QueueClient lowpriorityqueueClient = new QueueClient(sbconnection, "lowpriorityqueue");

            for (int i = 0; i < 10; i++)
            {
                string messageBody = $"priorityqueue Message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                await highpriorityqueueClient.SendAsync(message);
            }

            for (int i = 0; i < 10; i++)
            {
                string messageBody = $"lowpriorityqueue Message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                await lowpriorityqueueClient.SendAsync(message);
            }
        }


        public async Task SendMessageToTopicAsync()
        {

            TopicClient priorityClient = new TopicClient(sbconnection, "sample01");
            for (int i = 0; i < 15; i++)
            {
                string messageBody = $"priorityTopic Message {i}";
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                message.UserProperties.Add("Priority", i);
                await priorityClient.SendAsync(message);
            }
        }
    }
}
