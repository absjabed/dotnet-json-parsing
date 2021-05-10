using System;
using System.Linq;

namespace src
{
    public class Arguments
    {
        public string Action { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public string[] DefaultActions { get; } = new string[]{ "active", "superactive", "bored" };

        public Arguments(string[] arguments)
        {
            /**Checks for invalid number of arguments**/
            if(arguments.Length == 3){

                /**Checks if action command is valid**/
                if(this.DefaultActions.Contains(arguments[0], StringComparer.OrdinalIgnoreCase)){

                    this.Action = arguments[0];
                    this.FromDate = Convert.ToDateTime(arguments[1]);
                    this.ToDate = Convert.ToDateTime(arguments[2]);

                    /*Swap Dates if from date is greater than to date*/
                    if(this.FromDate > this.ToDate){
                        DateTime tmpDate = this.FromDate;
                        this.FromDate = this.ToDate;
                        this.ToDate = tmpDate;
                    }

                }else{
                    throw new Exception("Invalid action command, Try with 'active' or 'superactive' or 'bored'");
                }

            }else{
                 throw new Exception("Invalid Arguments, 3 arguments expected.");
            }
        }
    }
}