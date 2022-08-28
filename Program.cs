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

        public class GeneralStringStruct {public string a;public string b;public string c; public string d;public string e;
                                          public string EntityId;public string AssocEntityId; public string AssocEntityType; }


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
           
           Console.WriteLine("\nQuestion #1");

           if ((addrCount == 0) || (personCount == 0) || (orgCount == 0) || (vehicleCount == 0))
               Console.WriteLine("Do all files have entities?  FALSE");
           else 
               Console.WriteLine("Do all files have entities?  TRUE");

           // 2. What is the total count for all entities?
           Console.WriteLine("\nQuestion #2");
           int totalRecsCount = addrCount + personCount + orgCount + vehicleCount;


           Console.WriteLine("Total count for all entities =  {0} " , totalRecsCount);

           // 3. What is the count for each type of Entity? 
           Console.WriteLine("\nQuestion #3");
 
           Console.WriteLine("Total Person Count ={0} ",personCount);
           Console.WriteLine("Total Organization Count ={0} ",orgCount);
           Console.WriteLine("Total Vehicle Count ={0} ",vehicleCount);
           Console.WriteLine("Total Address Count ={0} ",addrCount);
                
           //  4. Provide a breakdown of entities which have associations  
           //  a. Per Entity count
           //  b. Total count  
           Console.WriteLine("\nQuestion #4");
 
           // get the list items that have at least 1 association
           var myPersonAssociationList = PersonList
                    .Where(v => v.Associations.Count > 0)
                    .Select(v => v).ToList();

           var myOrgAssociationList = OrgList
                    .Where(v => v.Associations.Count > 0)
                    .Select(v => v).ToList();

           var myVehAssociationList = VehicleList
                    .Where(v => v.Associations.Count > 0)
                    .Select(v => v).ToList();

           var myAddrAssociationList = AddrList
                    .Where(v => v.Associations.Count > 0)
                    .Select(v => v).ToList();


           // Per entity association counts
           Console.WriteLine("Per Entity Count: ");
           Console.WriteLine("Person: {0}",myPersonAssociationList.Count);
           Console.WriteLine("Organization: {0}",myOrgAssociationList.Count);
           Console.WriteLine("Vehicle: {0}",myVehAssociationList.Count);
           Console.WriteLine("Address: {0}",myAddrAssociationList.Count);
           Console.WriteLine("Total Count: {0} \n", (myPersonAssociationList.Count + myOrgAssociationList.Count + myVehAssociationList.Count + myAddrAssociationList.Count ));
           
           // 5. Provide the vehicle detail that is associated to the address
           //  "4976 Penelope
           //   Via South Franztown, NH 71024"?  
           Console.WriteLine("Question #5");
 
           List<GeneralStringStruct>  myPersAssnList = new List<GeneralStringStruct>();
           List<GeneralStringStruct>  myOrgAssnList = new List<GeneralStringStruct>();
           List<GeneralStringStruct>  myVehAssnList = new List<GeneralStringStruct>();
           List<GeneralStringStruct>  myAddrAssnList = new List<GeneralStringStruct>();

           // retrieve the address entityId
          var address4976EntityIDlist =   (from p in AddrList
                                        where p.StreetAddress.ToUpper() == "4976 PENELOPE VIA"
                                        where p.City.ToUpper() == "SOUTH FRANZTOWN"
                                        where p.State.ToUpper() == "NH"
                                        where p.ZipCode == "71024"                        
                                       select p).ToList();

           string address4976EntityId = "4976 Penelope Via -- EntityID not FOUND";  // default
           if (address4976EntityIDlist.Count() > 0)  // populate if exists
               address4976EntityId = address4976EntityIDlist[0].EntityId;

           Console.WriteLine("4976 Penelope Via ---> EntityID = {0}", address4976EntityId);      
 
           ////////////////////////////
           // now, essentially join the "child list" to the associated "parent" elements
           // couldn't find a way to do with linq, so doing it this way instead
           //
           // For Address
           //
           ///////////////////////////
           
           foreach (var item in AddrList)
           {
               GeneralStringStruct oneRec = new GeneralStringStruct();

              // only grab the records that contain associations 
              foreach (var aitem in item.Associations)  
              {
                 // Add association data to new list, essentially joining then
                 oneRec.a = item.StreetAddress;
                 oneRec.b = item.City;
                 oneRec.EntityId = item.EntityId;
                 oneRec.AssocEntityId = aitem.EntityId;
                 oneRec.AssocEntityType = aitem.EntityType;

                 myAddrAssnList.Add(oneRec);

              }
           }

           //Console.WriteLine("Address Associations:");
           //foreach (var item in myAddrAssnList)
           //{
           //  Console.WriteLine("Entity Id = {0} AssocEntityId = {1}, AssocEntityIdType = {2}" ,item.EntityId, item.AssocEntityId,  item.AssocEntityType.ToUpper()  );
           //}

           ////////////////////////////
           // now, essentially join the "child list" to the associated "parent" elements
           // not sure how to do currently with linq, so doing it this way instead
           //
           // For Vehicle
           //
           ///////////////////////////

           foreach (var item in VehicleList)
           {
               GeneralStringStruct oneRec = new GeneralStringStruct();

              // only grab the records that contain associations
              foreach (var aitem in item.Associations)  
              {
                 // Add association data to new list, essentially joining then
                 oneRec.a = item.Make;
                 oneRec.b = item.Model;
                 oneRec.c = item.Year.ToString();
                 oneRec.d = item.PlateNumber;
                 oneRec.e = item.State;
                 oneRec.EntityId = item.EntityId;
                 oneRec.AssocEntityId = aitem.EntityId;
                 oneRec.AssocEntityType = aitem.EntityType;

                 myVehAssnList.Add(oneRec);

              }
           }

           ////////////////////////////
           // now, essentially join the "child list" to the associated "parent" elements
           // not sure how to do currently with linq, so doing it this way instead
           //
           // For Organization
           //
           ///////////////////////////

           foreach (var item in OrgList)
           {
               GeneralStringStruct oneRec = new GeneralStringStruct();

              // only grab the records that contain associations
              foreach (var aitem in item.Associations)  
              {
                 // Add association data to new list, essentially joining then
                 oneRec.a = item.Name;
                 oneRec.b = item.Type;
                 oneRec.EntityId = item.EntityId;
                 oneRec.AssocEntityId = aitem.EntityId;
                 oneRec.AssocEntityType = aitem.EntityType;

                 myOrgAssnList.Add(oneRec);

              }
           }


           // display  each vehicle's address links
           var vehAddressesList = myVehAssnList
                    .Where(v => v.AssocEntityId == address4976EntityId)
                    .Where(v => v.AssocEntityType.ToUpper() == "ADDRESS")
                       .Select(v => v);


           // display  each address' vehicle links
           var addrVehiclesList = myAddrAssnList
                    .Where(v => v.EntityId == address4976EntityId)
                    .Where(v => v.AssocEntityType.ToUpper() == "VEHICLE")
                    .Select(v => v);

           if (addrVehiclesList.Count() > 0)
           {
              foreach (var item in addrVehiclesList)  // get each vehicle's data
              {
                 // display  each address' vehicle links
                 var vehDataList = VehicleList
                    .Where(v => v.EntityId == item.AssocEntityId)
                    .Select(v => v);   // should only be 1

                 if (vehDataList.Count() > 0)   // should only be 1
                 {
                   foreach (var vitem in vehDataList)  // get each vehicle's data
                   {                     
                     Console.WriteLine("Make = {0} , Model = {1} , Plate Number = {2} , State = {3} , Year = {4} ", vitem.Make, vitem.Model, vitem.PlateNumber, vitem.State, vitem.Year);
                     Console.WriteLine("      EntityId = {0} ", vitem.EntityId);
                   }
 
                 }
              }      
           }
           else if (vehAddressesList.Count() > 0)
           {
                // should only be one vehicle in this case
                 // a=make, b=model
               Console.WriteLine("Make = {0} , Model = {1} ", vehAddressesList.First().a, vehAddressesList.First().b);

           }
           else
               Console.WriteLine("4976 Penelope Via ---> NO VEHICLES FOUND");



           // 6. What person(s) are associated to the organization "Thiel and Sons"?

           Console.WriteLine("\nQuestion #6");
           // retrieve the Organization entityId
           var thielEntityID = OrgList
                    .Where(v => v.Name == "Thiel and Sons")
                    .Select(v => v.EntityId).ToArray();

           if (thielEntityID.Count() > 0)  // couldn't find Thiel and Son
               Console.WriteLine("No associations exist for Thiel and Sons");
           else  // We found "Thiel and Sons". See if the association exists within the Person table
           {
               // find Thiel and Son associated entries in the Person Association list
               var persToOrgList = myPersAssnList
                    .Where(v => v.AssocEntityId == thielEntityID[0].ToString())
                    .Where(v => v.AssocEntityType.ToUpper() == "ADDRESS")
                    .Select(v => v);
               if (persToOrgList.Count() > 0)  // found an association
               {   
                    Console.WriteLine("The following persons are related to Thiel and Sons:");

                    foreach (var item in persToOrgList)              
                    {
                        Console.WriteLine("Name = {0} ", item.a);
                    }
               }
               else
                    Console.WriteLine("No associations exist for Thiel and Sons");        
       
           }  


           // 7. How many people have the same first and middle name?
           Console.WriteLine("\nQuestion #7");

           int sameFirstMiddleCount =   (from p in PersonList
                                            where p.FirstName.ToUpper() == p.MiddleName.ToUpper()
                                            select p).Count();
           Console.WriteLine("Same First and Middle Name count = {0}", sameFirstMiddleCount);        
                                           

 
           // 8. Provide a breakdown of entities where the id contains "B3" in the following manor:
           //       a. Total count by type of Entity
           //       b. Total count of all entities
           Console.WriteLine("\nQuestion #8");
 

           int personSubStrCount =   (from p in PersonList
                                      where p.EntityId.ToUpper().Contains("B3")
                                       select p).Count();
           Console.WriteLine("Person 'B3' count = {0}", personSubStrCount);    

                     
           int VehSubStrCount =   (from p in VehicleList
                                      where p.EntityId.ToUpper().Contains("B3")
                                       select p).Count();
           Console.WriteLine("Vehicle 'B3' count = {0}", VehSubStrCount);        
              
          
           int OrgSubStrCount =   (from p in OrgList
                                      where p.EntityId.ToUpper().Contains("B3")
                                       select p).Count();
           Console.WriteLine("Organization 'B3' count = {0}", OrgSubStrCount);        
          
           int AddrSubStrCount =   (from p in AddrList
                                      where p.EntityId.ToUpper().Contains("B3")
                                       select p).Count();
           Console.WriteLine("Address 'B3' count = {0}\n", AddrSubStrCount);        



        }
    }
}
