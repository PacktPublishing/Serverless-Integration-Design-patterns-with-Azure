using System;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using System.IO;
using Newtonsoft.Json;

using System.Collections;
using System.Linq;


namespace ShipAnyWhere_FaceAPITest
{
 
    class Program
    {
        static string _faceAPIKey = "0be7e081f204437990ad877011103d23";
         const string faceEndpoint =
            "https://northeurope.api.cognitive.microsoft.com";
         const string personGroupId = "myfriends";
         const string friendImageDir= @"C:\Git\ShipAnyWhere-FaceAPITest\Data\PersonGroup\AnnaImages";
        static  void Main(string[] args)
        {
           
           var faceClient = new FaceClient(new ApiKeyServiceClientCredentials(_faceAPIKey),
                                           new System.Net.Http.DelegatingHandler[]{});
           faceClient.Endpoint = faceEndpoint;
           //Create a new PersonGroup
            var faceId = faceClient.PersonGroup.GetAsync("myfriends")
                        .GetAwaiter()
                        .GetResult();
            if(faceId == null )
            {
                    faceClient.PersonGroup.CreateAsync("myfriends","MyFriends")
                    .GetAwaiter()
                    .GetResult();
            }

            //Creating a friend 
            var friendAnna = faceClient.PersonGroupPerson.CreateAsync(personGroupId,"anna")
                            .GetAwaiter()
                            .GetResult();
            
            foreach(string image in Directory.GetFiles(friendImageDir, "*.jpg"))
            {
                using(Stream imageStream = File.OpenRead(image))
                {
                    faceClient.PersonGroupPerson.AddFaceFromStreamAsync(personGroupId,friendAnna.PersonId,imageStream)
                    .GetAwaiter()
                    .GetResult();
                }
            }

            faceClient.PersonGroup.TrainAsync(personGroupId)
            .GetAwaiter()
            .GetResult();

            string testImage = @"C:\Git\ShipAnyWhere-FaceAPITest\Data\PersonGroup\Family.jpg";
            using (Stream imageStream = File.OpenRead(testImage))
            {
                var faces = faceClient.Face.DetectWithStreamAsync(imageStream,true)
                            .GetAwaiter()
                            .GetResult();
                var faceids = faces.Select(e=>(Guid)e.FaceId).ToList();

                var identifyResults = faceClient.Face.IdentifyAsync(faceids,personGroupId)
                .GetAwaiter()
                .GetResult();;
                foreach(var result in identifyResults)
                {      
                    if(result.Candidates.Count == 0)              
                    Console.WriteLine("No one identified");

                    else
                    {
                        var candidateId = result.Candidates[0].PersonId;
                        var person =  faceClient.PersonGroupPerson.GetAsync(personGroupId, candidateId)
                        .GetAwaiter().GetResult();
                         Console.WriteLine("Identified as {0}", person.Name);
                    }
                    
                }
            }
            
        }
    }
}
