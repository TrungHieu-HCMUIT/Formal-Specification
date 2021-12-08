using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Model
{
    public class Type1PostExpressionModel
    {
        public string executionExpression;
        public List<string> conditionExpressionList;

        public Type1PostExpressionModel(string execution, List<string> conditionList)
        {
            this.executionExpression = ChangeBooleanTextValue(execution);
            conditionExpressionList = conditionList;
        }

        public string GenerateExecutionScript()
        {
            return string.Format("|{0}|;", executionExpression);
        }

        public string GenerateConditionScript()
        {
            int numberOfConditionExpression = conditionExpressionList.Count;

            string line = "if| ";
            if (numberOfConditionExpression > 1) line += "(";

            for(int i = 0; i < numberOfConditionExpression; i++)
            {
                //If condition expression contains = operator, replace it to == so that it can be execute logically
                if (conditionExpressionList[i].Contains("=")
                    && !conditionExpressionList[i].Contains(">")
                    && !conditionExpressionList[i].Contains("<")
                    && !conditionExpressionList[i].Contains("!"))
                    conditionExpressionList[i] = conditionExpressionList[i].Replace("=", "==");
                if (i == 0)
                    line += "(" + conditionExpressionList[i] + ")";
                else 
                    line += " && (" + conditionExpressionList[i] + ")";
            }
            if (numberOfConditionExpression > 1) line += ")";

            return line;
        }

        private string ChangeBooleanTextValue(string text)
        {
            text = text.Replace("TRUE", "true");
            text = text.Replace("FALSE", "false");
            return text;
        }
    }
}
