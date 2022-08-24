using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using SimpleDataManagement.Model;

namespace prj4
{

    class Program
    {

 
        public class myClass11
        {
            static public int a=4068;
               
        }


        static void Main(string[] args)
        {


             // LOAD THE DATA FILES INTO LISTS VIA DESERIALIZATION

             List<Address> AddrList = new List<Address>();

             using (StreamReader r = new StreamReader(".\\DataSources\\Addresses_20220824_00.json"))
             {
                 string json = r.ReadToEnd();
                 AddrList = JsonSerializer.Deserialize<List<Address>>(json);
             }

             List<Person> PersonList = new List<Person>();
 
             using (StreamReader r = new StreamReader(".\\DataSources\\Persons_20220824_00.json"))
             {
                 string json = r.ReadToEnd();
                 PersonList = JsonSerializer.Deserialize<List<Person>>(json);
             }
                         
              List<Vehicle> VehicleList = new List<Vehicle>();
 
             using (StreamReader r = new StreamReader(".\\DataSources\\Vehicles_20220824_00.json"))
             {
                 string json = r.ReadToEnd();
                 VehicleList = JsonSerializer.Deserialize<List<Vehicle>>(json);
             }
            

             List<Organization> OrgList = new List<Organization>();
 
             using (StreamReader r = new StreamReader(".\\DataSources\\Organizations_20220824_00.json"))
             {
                 string json = r.ReadToEnd();
                 OrgList = JsonSerializer.Deserialize<List<Organization>>(json);
             }
            

           //  1. Do all files have entities? (true/false)
           int addrCount = AddrList.Count();
           int personCount = PersonList.Count();
           int orgCount = OrgList.Count();
           int vehicleCount = VehicleList.Count();
           
           if ((addrCount == 0) || (personCount == 0) || (orgCount == 0) || (vehicleCount == 0))
               Console.WriteLine("Do all files have entities?  FALSE");
           else 
               Console.WriteLine("Do all files have entities?  TRUE");

           // 2. What is the total count for all entities?
           int totalRecsCount = addrCount + personCount + orgCount + vehicleCount;

           Console.WriteLine("Total count for all entities =  {0} " , totalRecsCount);

           // 3. What is the count for each type of Entity?  
           Console.WriteLine("Total Address Count ={0} ",addrCount);
           Console.WriteLine("Total Person Count ={0} ",personCount);
           Console.WriteLine("Total Organization Count ={0} ",orgCount);
           Console.WriteLine("Total Vehicle Count ={0} ",vehicleCount);
                
           //  4. Provide a breakdown of entities which have associations  
           //  a. Per Entity count
           //  b. Total count  

         //  var xx =
         //      OrgList.GroupBy(s => new { s.Score, s.StreamId}).Select(g => new { EntityID = g.Key, TotalCount = g.Count() });

           // 5. Provide the vehicle detail that is associated to the address
           //  "4976 Penelope
           //   Via South Franztown, NH 71024"?  
 
           // retrieve the address entityId
           var address4976EntityID = AddrList
                    .Where(v => v.StreetAddress == "4976 Penelope Via")
                    .Where(v => v.City == "South Franztown")
                    .Where(v => v.State == "NH")
                    .Where(v => v.ZipCode == "71024")
                    .Select(v => v.EntityId).ToArray();

           // Add exception handler for no address found           
           Console.WriteLine("4976 Penelope Via South Franztown, NH 71024 entityID= {0}", address4976EntityID);
           

           // now get associated vehicles for that address entityId.  need to drill into asociations
     /*      var vehToAddrList = VehicleList
                    .Where(v => v.Associations.SelectMany().Where(x =>  ) = address4976EntityID)
                    .Where(v => v.Associations.Select(x => x.EntityId = address4976EntityID.ToString())

                    .Where(v => v.Associations.Count > 0)
                    .Select(v => v);
*/
     // print out the vehicle details for each vehicle in the  vehToAddrList
     //      foreach (var item in vehToAddrList)
     //          Console.WriteLine(item.Make + ", " + item.Model);



           // 6. What person(s) are associated to the organization "Thiel and Sons"?

           // 7. How many people have the same first and middle name?

           // 8. Provide a breakdown of entities where the id contains "B3" in the following manor:
           //       a. Total count by type of Entity
           //       b. Total count of all entities






        }
    }
}
