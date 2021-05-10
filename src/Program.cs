using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace src
{
    class Program
    {
        static void Main(string[] args)
        {
            /**Checks for invalid arguments**/
             if(args.Length == 3){
                 
                 string action = args[0], fromDate = args[1], toDate = args[2];
                 string[] actionCommands = new string[]{"active","superactive","bored"};
                 string userListString = String.Empty;

                try
                {
                    if(actionCommands.Contains(action, StringComparer.OrdinalIgnoreCase)){
                        
                        string[] filePaths = Directory.GetFiles(@"../data/", "*.json",SearchOption.AllDirectories);
                        DateTime fDate = Convert.ToDateTime(fromDate), tDate = Convert.ToDateTime(toDate);

                        /*Swap Dates if from date is greater than to date*/
                        if(fDate > tDate){
                            DateTime tmpDate = fDate;
                            fDate = tDate;
                            tDate = tmpDate;
                        }

                        foreach (string path in filePaths)
                        {
                            var userJsonString = File.ReadAllText(path);
                            var userJObject = JObject.Parse(userJsonString);

                            string userId = path.Split("/")[2].Split(".")[0];
                            int totalOrderedMeals = 0;
                            var dayWiseIdDict = new Dictionary<DateTime, int>();
                            var orderedMealsDict = new Dictionary<string, int>();

                            var calenderJsonObject = userJObject.SelectToken("calendar");
                            var dayWiseIdJson = calenderJsonObject.Value<JObject>("dateToDayId").Properties();
                            var orderedMealsJson = calenderJsonObject.Value<JObject>("mealIdToDayId").Properties();
                            
                           
                                dayWiseIdDict = dayWiseIdJson
                                                .ToDictionary(
                                                    k => Convert.ToDateTime(k.Name),
                                                    v => int.Parse(v.Value.ToString()));

                                orderedMealsDict = orderedMealsJson
                                                .ToDictionary(
                                                    k => k.Name,
                                                    v => int.Parse(v.Value.ToString()));

                            var filtered =  dayWiseIdDict.Where(x=> x.Key >= fDate && x.Key <= tDate).Select(X=> X.Value).ToArray();

                            foreach (int dayId in filtered)
                            {
                                totalOrderedMeals += orderedMealsDict.Where(x=> x.Value == dayId).Count();
                            }

                            if(action == "active" && (totalOrderedMeals >=5 && totalOrderedMeals<10)){
                                userListString += userId+',';
                            }

                            if(action == "superactive" && totalOrderedMeals >10){
                                userListString += userId+',';
                            }

                            if(action == "bored" && totalOrderedMeals < 5){
                                userListString += userId+',';
                            }

                        }
                
                        Console.WriteLine(action+" -> "+userListString.Remove(userListString.Length - 1, 1));

                    }else{
                        throw new Exception("Invalid action command, Try with 'active' or 'superactive' or 'bored'");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
             }else{
               Console.WriteLine("Invalid Arguments, 3 arguments expected.");  
             }
        }
    }
}
