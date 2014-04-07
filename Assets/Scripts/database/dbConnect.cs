using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

using System.Collections.Specialized;
using System.Net;
using System.IO;


namespace dbConnect {

	public class dbClass{

		private ArrayList parameterList;
		private ArrayList valueList;
		private string function;
		private string url;
		private string jsonReturn;

		//instantiate
		public dbClass()
		{
			parameterList = new ArrayList();
			valueList = new ArrayList();
			function = "";
			url = "http://www.sgi-singapore.com/projects/ORILE/jsonConnect.php";
			jsonReturn = "";
		}

		//values to be added to the backend
		public void addValues(string parameter, string value)
		{
			if (!System.String.IsNullOrEmpty(parameter) && !System.String.IsNullOrEmpty(parameter))
			{
				//Debug.Log ("added");
				parameterList.Add(parameter);
				valueList.Add(value);
			}
		}

		public void addFunction(string function)
		{
			//encode MD5?
			this.function = function;
		}

		public string connectToDb()
		{
			//make sure all the paramter has something
			if (!System.String.IsNullOrEmpty(function) && !System.String.IsNullOrEmpty(url) && parameterList.Count > 0 && valueList.Count > 0)
			{
				WebClient webClient = new WebClient();
				NameValueCollection formData = new NameValueCollection();
				var sendJSON = new JSONClass();

				//get the values to be sent to backend
				for (int i=0; i < parameterList.Count;i++)
				{
					string key = (string)parameterList[i];
					sendJSON["info"][key] = (string)valueList[i];
				}

				//send to backend
				sendJSON["function"] = this.function;
				formData["send"] = sendJSON.ToString();
				byte[] responseBytes = webClient.UploadValues(this.url, "POST", formData);
				string responsefromserver = Encoding.UTF8.GetString(responseBytes);
				Debug.Log(responsefromserver);

				//parse the return values
				var returnValue = JSONNode.Parse(responsefromserver);
				int error = Convert.ToInt32(returnValue["error"]);
				                
				if (error != -1)
				{
					//if got error, parse it and return the appropriate error message
					string errorString = this.parseError(error);
					return errorString;

				} else {

					//if no error, return "SUCCESS NO RETURN" if only "TRUE" is returned by backend
					this.jsonReturn = returnValue["result"][0].ToString();
					if (String.Compare(this.jsonReturn, "\"TRUE\"") == 0)
					{
						return "SUCCESS NO RETURN";
					} else {
						return "SUCCESS";
					}
				}

				//return "";

			}
		
			return "you did not insert a function/parameter/value";
		}

		//return a value based on the paramter (string)
		public string getReturnValue(string parameter)
		{
			var returnArray = JSONNode.Parse(this.jsonReturn);
			return returnArray[parameter];
		}

		//return a value based on the paramter (int)
		public int getReturnValueInt(string parameter)
		{
			var returnArray = JSONNode.Parse(this.jsonReturn);
			int returnValue = Convert.ToInt32(returnArray[parameter]);
			return returnValue;
		}

		private string parseError(int error)
		{
			String errorString = "";

			switch (error) 
			{
				case 400:
					errorString = "Internal error: variable(s) has/have empty value";
					break;

				case 401:
					errorString = "Internal error: variable(s) is not a number when it should be";
					break;

				case 402:
					errorString = "Internal error: variable(s) not passed to backend";
					break;

				case 500:
					errorString = "Internal error: cannot connect to db";
					break;

				case 501:
					errorString = "Internal error: create table error";
					break;

				case 502:
					errorString = "Internal error: insert table error";
					break;

				case 503:
					errorString = "Internal error: select table error";
					break;

				case 504:
					errorString = "Internal error: update table error";
					break;

				case 505:
					errorString = "Internal error: delete table error";
					break;

				case 506:
					errorString = "Internal error: no result from table";
					break;

				case 600:
					errorString = "Player name exists in table; cannot create player";
					break;

				case 601:
					errorString = "Player name does not exist in table";
					break;

				case 602:
					errorString = "password is wrong";
					break;
					
			}
			return errorString;
		}
	}

}
