using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace src
{
    public class ProcessJsonFiles
    {
        public string Path { get; set; }
        public Arguments ArgsParams { get; set; }
        private string Result { get; set; }

        public ProcessJsonFiles(string path, Arguments argParams)
        {
            this.Path = path;
            this.ArgsParams = argParams;
        }
        public void GetResult(){

            /**Get Users Json File Paths**/
            string[] userJsonFilePaths = GetJsonUserFilePaths();
            
            /**Loop over each user file**/
            foreach (string filePath in userJsonFilePaths){

                /**Get Json Files content from specified path**/
                string userJsonContent = GetJsonFileContent(filePath);
                
                /**Parse Json Files content **/
                JObject parsedJObject = ParseJsonContent(userJsonContent);

                /**Getting UserId from JsonFile Path **/
                string userId = filePath.Split("/")[2].Split(".")[0];

                /**Users total meal count**/
                int totalOrderedMeals = 0;

                /**Daywise Id dictionary**/
                var dayWiseIdDict = new Dictionary<DateTime, int>();
                
                /**Day Id wise meals dictionary**/
                var orderedMealsDict = new Dictionary<string, int>();

                /**Select specific Json Object Token**/
                var calenderJsonObject = parsedJObject.SelectToken("calendar");
                var dayWiseIdJson = calenderJsonObject.Value<JObject>("dateToDayId").Properties();
                var orderedMealsJson = calenderJsonObject.Value<JObject>("mealIdToDayId").Properties();
                
                /**Convert Json Object Tokens to dictionary**/
                dayWiseIdDict = dayWiseIdJson
                                .ToDictionary(
                                    k => Convert.ToDateTime(k.Name),
                                    v => int.Parse(v.Value.ToString()));

                orderedMealsDict = orderedMealsJson
                                    .ToDictionary(
                                        k => k.Name,
                                        v => int.Parse(v.Value.ToString()));

                /** Filter Day Ids within given arguments date range**/
                var filteredDayIds =  dayWiseIdDict
                                        .Where(x=> x.Key >= ArgsParams.FromDate 
                                                && x.Key <= ArgsParams.ToDate)
                                        .Select(X=> X.Value).ToArray();

                /**Count day id wise meals**/
                foreach (int dayId in filteredDayIds)
                {
                    totalOrderedMeals += orderedMealsDict.Where(x=> x.Value == dayId).Count();
                }

                /**Active users list string**/
                if(ArgsParams.Action == "active" && (totalOrderedMeals >=5 && totalOrderedMeals<10)){
                    Result += userId+',';
                }

                /**Superactive users list string**/
                if(ArgsParams.Action == "superactive" && totalOrderedMeals >10){
                    Result += userId+',';
                }
                
                /**Bored users list string**/
                if(ArgsParams.Action == "bored" && totalOrderedMeals < 5){
                    Result += userId+',';
                }

            }
            /**Final Result**/    
            Console.WriteLine(Result.Remove(Result.Length - 1, 1));
        }

        
        private string[] GetJsonUserFilePaths(){
            
            string[] jsonFilePaths = Directory.GetFiles(@Path, "*.json",SearchOption.AllDirectories);

            return jsonFilePaths;
        }

        private string GetJsonFileContent(string jsonFilePath){
            
            string userJsonFileContent = File.ReadAllText(jsonFilePath);
            
            return userJsonFileContent;
        }

        private JObject ParseJsonContent(string jsonContent){
            
            JObject parsedJsonContent = JObject.Parse(jsonContent);
            
            return parsedJsonContent;
        }


    }
}