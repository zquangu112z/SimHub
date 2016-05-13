using System;
using System.Runtime.Serialization;

namespace Sample
{
//	public class Sim
//	{
//		//public string simID { get; set;}
//		public string simNumber { get; set; }
//		public string simMade { get; set; }
//		public string simOwner { get; set; }
//		public string simPrice { get; set;}
//		public Sim (string number, string price, string owner, string made)
//		{
//			//simID = id;
//			simNumber = number;
//			simPrice = price;
//			simOwner = owner;
//			simMade = made;
//		}
//	}

	public class RootObject
	{
		public RootObject(){
			
		}
		public int Id { get; set; }
		public string Number { get; set; }
		public string Price { get; set; }
		public string Made { get; set; }
		public string Type { get; set; }
		public int OwnerId { get; set; }
		public SimHubOwner.RootObject Owner { get; set; }
	}
}

