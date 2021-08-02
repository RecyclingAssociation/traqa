// TRAQA API TESTING EXAMPLE

namespace TraqaApiTesting
{
  using System;
	using Newtonsoft.Json.Linq;
	using Newtonsoft.Json;
  using RestSharp;
	using System.Collections.Generic;
	
	/**
	Working Example from the view point of a weigh bridge to demonstrate some interaction with Traqa
	
	1.  Authentication
	2.  GET some Planned Loads (Future Loads for the load site in question)
	3.  POST a completed load back to Traqa (done when truck is on the second weigh before leaving site)
	
	For illustration only we use the open load data from the GET, add some data that would come from the weighbridge software and POST back to Traqa
	Data Model classes are contained at the bottom for this example.
	
	**/
    public class Program
    {	
		
		public static void Main()
        {
			
			string username = "username";  // Replace with your Username / Email from Traqa
			string password = "password"; // Replace with your Password from Traqa
            string basePath = "https://uat.api.traqa.io/dev";  //THE UAT Environment for Testing only.
			
			// RETRIEVE ID TOKEN
			string IdToken = TraqaApiTesting.Program.GetIdToken(username, password, basePath);
			//Console.WriteLine(IdToken);
			
			// GET OPEN LOADS FROM TRAQA
			dynamic OpenLoads = GetOpenLoads(basePath, IdToken);
			//Console.WriteLine(OpenLoads);
			//Console.WriteLine(OpenLoads[0]);
			
			// GET COMPLETED LOADS FROM TRAQA 
			//dynamic CompletedLoads = GetCompletedLoads(basePath, IdToken);
			//Console.WriteLine(CompletedLoads);
			
			//COMPLETE THE OPEN LOAD AND SEND TO TRAQA Using the Data from Open Load and values captured from weighbridge software.
			dynamic CompletedLoadJson = CreateCompletedLoadplan(
				ShipperDetails: OpenLoads[0].shipper,
				CarrierName: (string)OpenLoads[0].carrier.name,
				CarrierId: (string)OpenLoads[0].carrier.organizationId,
				HaulierName:"Haulier Co Ltd",
				HaulierId:"CBDU209018",
				shippersIdentifyingNumberForShipment: (string)OpenLoads[0].references.shippersIdentifyingNumberForShipment,
				pickupReferenceNumber: (string)OpenLoads[0].references.pickupReferenceNumber,
				despatchNoteNumber:"211449",
				vehicleIdentificationNumber:"NV18NLD",
				driverName:"STEVEN",
				actualArrivalTransportEvent:new System.DateTime(2021, 12, 28, 10, 45, 30),
				actualDepartureTransportEvent:new System.DateTime(2021, 12, 28, 10, 45, 30),
				verifiedWeightValue:28500,
				weightVerificationMethod: "1",
				weighingDate : new System.DateTime(2021, 12, 28, 10, 45, 30),
				equipmentNumber:"MSKU9070323",
				equipmentType:"42G0",
				equipmentTareWeightValue:3000,
				sealnumber:"0859498",
				grossWeightValue:25500,
				quantity:24,
				harmonizeSystemCode: (string)OpenLoads[0].tradeItem.harmonizeSystemCode,
				description: (string)OpenLoads[0].tradeItem.description
			);
			//Console.WriteLine(CompletedLoadJson);
			
			// POST COMPLETED LOAD TO TRAQA API
			dynamic PostCompletedLoadResponse = PostCompletedLoad(basePath, IdToken, CompletedLoadJson);
			Console.WriteLine(PostCompletedLoadResponse);
			
        }
		
		/**
		FUNCTIONS BELOW ARE TO POST a Load, Get Loads (Open nand Closed) as well as data model classes used in this example
		**/
		public static dynamic PostCompletedLoad(string basePath, string IdToken, dynamic CompletedLoadJson)
		{	
			
			// QUERY LOADS ENDPOINT FOR STATUS COMPLETED LOADS 
			var client = new RestClient(basePath);
			var PostCompletedLoad = new RestRequest("/weights", Method.POST);
			PostCompletedLoad.AddHeader("Authorization", IdToken);
			PostCompletedLoad.AddParameter("application/json", CompletedLoadJson, ParameterType.RequestBody);
			var PostCompletedResponse = client.Execute(PostCompletedLoad);
			dynamic PostCompletedResponseJson = JsonConvert.DeserializeObject(PostCompletedResponse.Content);
			return PostCompletedResponseJson;
		}
		
		public static dynamic GetCompletedLoads(string basePath, string IdToken)
		{	
			// QUERY LOADS ENDPOINT FOR STATUS COMPLETED LOADS 
			var client = new RestClient(basePath);
			var RequestCompletedLoads = new RestRequest("/loads?status=COMPLETED&order=asc", Method.GET);
			RequestCompletedLoads.AddHeader("Authorization", IdToken);
			var OpenCompletedResponse = client.Execute(RequestCompletedLoads);
			dynamic OpenCompletedJson = JsonConvert.DeserializeObject(OpenCompletedResponse.Content);
			return OpenCompletedJson;
		}
		
		public static dynamic GetOpenLoads(string basePath, string IdToken)
		{	
			// QUERY LOADS ENDPOINT FOR STATUS OPEN LOADS 
			var client = new RestClient(basePath);
			var RequestOpenLoads = new RestRequest("/loads?status=OPEN&order=desc", Method.GET);
			RequestOpenLoads.AddHeader("Authorization", IdToken);
			var OpenLoadResponse = client.Execute(RequestOpenLoads);
			dynamic OpenLoadJson = JsonConvert.DeserializeObject(OpenLoadResponse.Content);
			return OpenLoadJson;
		}
		
		public static string GetIdToken(string username, string password, string basePath)
		{
			// QUERY TRAQA WITH USERNAME AND PASSWORD
			var client = new RestClient(basePath);
			var request_token = new RestRequest("/getToken", Method.POST);
			client.Authenticator = new HttpBasicAuthenticator(username, password);
			var AuthResponse = client.Execute(request_token);	
			//Console.WriteLine("Retrieving Token...");
			string tokenResponse = AuthResponse.Content;
			
			//PARSE RESPONSE FOR ID TOKEN
			dynamic data = JObject.Parse(tokenResponse);
			string IdToken = data.IdToken;
			return IdToken;
		}
		
		public static dynamic CreateCompletedLoadplan(
			dynamic ShipperDetails,
			
			string CarrierName,
			string CarrierId,
			
			string HaulierName,
			string HaulierId,
			
			string shippersIdentifyingNumberForShipment,
			string pickupReferenceNumber,
			string despatchNoteNumber,
			string vehicleIdentificationNumber,
			string driverName,
			
			DateTime actualArrivalTransportEvent,
			DateTime actualDepartureTransportEvent,
			
			int verifiedWeightValue,
			string weightVerificationMethod,
			DateTime weighingDate,
			
			string equipmentNumber,
			string equipmentType,
			int equipmentTareWeightValue,
			string sealnumber,
			int grossWeightValue,
			int quantity,
			string harmonizeSystemCode,
			string description
		)
		{	
			// SHIPPER DETAILS
			Address shipperaddress = new Address()
			{
				street = ShipperDetails.address.street,
				city = ShipperDetails.address.city,
				state = ShipperDetails.address.state,
				country = ShipperDetails.address.country,
				postcode = ShipperDetails.address.postcode
			};
			
			// SHIPPER DETAILS
			Shipper shipperdetails = new Shipper()
			{	
				name = ShipperDetails.name,
				address = shipperaddress,
				organizationId = ShipperDetails.organizationId
			};

			// LOADINGSITE DETAILS (YOUR DETAILS)
			Address loadingSiteaddress = new Address()
			{
				street = "1 Old Hall Street",
				city = "Liverpool",
				state = "Merseyside",
				country = "GB",
				postcode = "L3 9HG"
			};

			LoadingSite loadingSitedetails = new LoadingSite()
			{
				name = "Recycling Facility Limited",
				address = loadingSiteaddress,
				organizationId = "GBLIVRFAC"
			};

			// CARRIER DETAILS
			// Id is a valid SCAC code
			Carrier carrierdetails = new Carrier()
			{
				name = CarrierName,
				organizationId = CarrierId
			};

			// HAULIER DETAILS
			// Id is a valid haulier waste registration code
			Haulier haulierdetails = new Haulier()
			{
				name = HaulierName,
				organizationId = HaulierId
			};

			// REFERENCES
			// pickup ref is a loading reference

			References references = new References()
			{
				shippersIdentifyingNumberForShipment= shippersIdentifyingNumberForShipment,
				pickupReferenceNumber = pickupReferenceNumber,
				despatchNoteNumber = despatchNoteNumber,
				vehicleIdentificationNumber = vehicleIdentificationNumber,
				driverName = driverName
			};

			// DATES
			Dates dates = new Dates()
			{
				actualArrivalTransportEvent = actualArrivalTransportEvent,
				actualDepartureTransportEvent = actualDepartureTransportEvent
			};

			// VGM
			VerifiedWeight verifiedWeight = new VerifiedWeight()	
			{
				value = verifiedWeightValue,
				unit = "KGM",
				weightVerificationMethod = weightVerificationMethod,
				weighingDate = weighingDate
			};

			// EQUIPMENT TARE WEIGHT
			EquipmentTareWeight equipmentTareWeight = new EquipmentTareWeight()	
			{
				value = equipmentTareWeightValue,
				unit  = "KGM"
			};

			// TRANSPORT EQUIPMENT
			// equipment number will likely be a container number
			// type will be container type code
			TransportEquipment transportEquipment = new TransportEquipment()	
			{
				equipmentNumber = equipmentNumber,
				equipmentType = equipmentType,
				seal = new List<Seal>() { 
					new Seal(){ identifier = sealnumber}
				},
				verifiedWeight = verifiedWeight,
				equipmentTareWeight = equipmentTareWeight
			};

			// GROSS WEIGHT
			GrossWeight grossWeight = new GrossWeight()	
			{
				value = grossWeightValue,
				unit  = "KGM"
			};

			// TRADE ITEM
			TradeItem tradeItem = new TradeItem()	
			{
				quantity = quantity,
				harmonizeSystemCode = harmonizeSystemCode,
				description = description,
				grossWeight = grossWeight
			};

			// COMPLETED LOADPLAN
			TraqaCompletedLoadplan traqaCompletedLoadplan = new TraqaCompletedLoadplan()
			{
				shipper = shipperdetails,
				carrier = carrierdetails,
				haulier = haulierdetails,
				loadingSite = loadingSitedetails,
				references= references,
				dates = dates,
				transportEquipment= transportEquipment,
				tradeItem = tradeItem
			};

			string traqaCompletedLoadplanjson = JsonConvert.SerializeObject(traqaCompletedLoadplan);
			return traqaCompletedLoadplanjson;
		}
		

        
		
    }
	
	public class Address
    {
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postcode { get; set; }
    }

    public class Shipper
    {
        public string name { get; set; }
        public Address address { get; set; }
        public string organizationId { get; set; }
    }

    public class Carrier
    {
        public string name { get; set; }
        public string organizationId { get; set; }
    }

    public class Haulier
    {
        public string name { get; set; }
        public string organizationId { get; set; }
    }

    public class LoadingSite
    {
        public string name { get; set; }
        public Address address { get; set; }
        public string organizationId { get; set; }
    }

    public class References
    {
        public string shippersIdentifyingNumberForShipment { get; set; }
        public string pickupReferenceNumber { get; set; }
        public string despatchNoteNumber { get; set; }
        public string vehicleIdentificationNumber { get; set; }
        public string driverName { get; set; }
    }

    public class Dates
    {
        public DateTime actualArrivalTransportEvent { get; set; }
        public DateTime actualDepartureTransportEvent { get; set; }
    }

    public class Seal
    {
        public string identifier { get; set; }
    }

    public class VerifiedWeight
    {
        public int value { get; set; }
        public string unit { get; set; }
        public string weightVerificationMethod { get; set; }
        public DateTime weighingDate { get; set; }
    }

    public class EquipmentTareWeight
    {
        public int value { get; set; }
        public string unit { get; set; }
    }

    public class TransportEquipment
    {
        public string equipmentNumber { get; set; }
        public string equipmentType { get; set; }
        public List<Seal> seal { get; set; }
        public VerifiedWeight verifiedWeight { get; set; }
        public EquipmentTareWeight equipmentTareWeight { get; set; }
    }

    public class GrossWeight
    {
        public int value { get; set; }
        public string unit { get; set; }
    }

    public class TradeItem
    {
        public int quantity { get; set; }
        public string harmonizeSystemCode { get; set; }
        public string description { get; set; }
        public GrossWeight grossWeight { get; set; }
    }

    public class TraqaCompletedLoadplan
    {
        public Shipper shipper { get; set; }
        public Carrier carrier { get; set; }
        public Haulier haulier { get; set; }
        public LoadingSite loadingSite { get; set; }
        public References references { get; set; }
        public Dates dates { get; set; }
        public TransportEquipment transportEquipment { get; set; }
        public TradeItem tradeItem { get; set; }
    }
}
