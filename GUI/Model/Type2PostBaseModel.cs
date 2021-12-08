using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Model
{
    public enum PostConditionType { VM, TT, VM_TT, VM_VM, TT_VM, TT_TT }
    public abstract class Type2PostBaseModel
    {
        public PostConditionType loopType;
        public string arrayName;
        public string parameterName;
        public string expression;
        public string startRange;
        public string endRange;
        public string indexVariableName;

        public abstract List<string> GenerateLoopCode();
        public abstract void SplitStringAndGetProperty(string postValue);

        public static Type2PostBaseModel GetExplicitPostModel(string postValue, string arrayName, string parameterName)
        {
            postValue = ReplaceTwoDot(postValue);

            int numberOfLoop = postValue.Count(underscore => underscore == '_');
            if (numberOfLoop == 1)
                return new Type2OneLoopPostModel(postValue, arrayName, parameterName);
            else
                return new Type2TwoLoopPostModel(postValue, arrayName, parameterName);
        }


        private static string ReplaceTwoDot(string text)
        {
            return text.Replace("..", "_");
        }

        //Clause format
        //Ex: VM {indexVariableName} TH
        public static string GetIndexVariableNameFromClause(string clause)
        {
            return clause.Replace("VM", String.Empty)
                .Replace("TT", String.Empty)
                .Replace("TH", String.Empty)
                .Trim();
        }
    }
}
