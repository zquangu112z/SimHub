using System;
using Org.Apache.Http.Authentication;

namespace Sample
{
	public class User
	{
		//7
		public string userName{ get; set;}
		public string userAge{ get; set;}
		public string userPhone{ get; set;}
		public string userAddress{ get; set;} 
		public string userSign{ get; set;} //chu ky
		public string userDateJoined { get; set;} //ngay tham gia
		public bool userStatus { get; set;}//co quyen mua ban

		public User (string name, string age, string phone, string address, string sign, string datejoned, bool status)
		{
			userAddress = address;
			userAge = age;
			userDateJoined = datejoned;
			userName = name;
			userPhone = phone;
			userSign = sign;
			userStatus = status;
		}
	}
}

