using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Model
{
    public class Type2OneLoopPostModel : Type2PostBaseModel
    {
        public Type2OneLoopPostModel(string postValue, string arrayName, string parameterName)
        {
            this.arrayName = arrayName;
            this.parameterName = parameterName;
            SplitStringAndGetProperty(postValue);
        }

        public override void SplitStringAndGetProperty(string postValue)
        {
            string[] splitedPostValue = postValue.Split(new[] { "." }, StringSplitOptions.None);
            string loopInformation = splitedPostValue[0];

            this.expression = splitedPostValue[1];
            this.loopType = loopInformation.Contains("VM") ? PostConditionType.VM : PostConditionType.TT;

            this.indexVariableName = Type2PostBaseModel.GetIndexVariableNameFromClause(
                loopInformation
                .Split(new[] { "TH" }, StringSplitOptions.None)[0]
                .Split(new[] { "(" }, StringSplitOptions.None)[1]);
                
            string range = loopInformation.Split(new[] { "TH" }, StringSplitOptions.None)[1];
            range = range.Replace("{", String.Empty);
            range = range.Replace("}", String.Empty);

            this.startRange = range.Split(new[] { "_" }, StringSplitOptions.None)[0];
            this.endRange = range.Split(new[] { "_" }, StringSplitOptions.None)[1];
        }

        public override List<string> GenerateLoopCode()
        {
            var list = new List<String>();

            string forLine = string.Format("\t\t\tfor| (|int| {0} = {1} - 1; {0} <= {2} - 1; {0}++)", this.indexVariableName,startRange, endRange);
            list.Add(forLine);
            list.Add("\t\t\t{");

            string codeExpression = expression.Replace("(", "[").Replace(")","]");

            if (loopType == PostConditionType.VM)
            {
                list.Add(string.Format("\t\t\t\tif| ({0})", codeExpression) + " { }");
                list.Add("\t\t\t\telse| return| false;");
            }
            else if (loopType == PostConditionType.TT)
            {
                list.Add(string.Format("\t\t\t\tif| ({0})|", codeExpression) + " return| true;");
            }

            list.Add("\t\t\t}");

            if (loopType == PostConditionType.VM)
            {
                list.Add("\t\t\treturn| true;");
            }
            else if (loopType == PostConditionType.TT)
            {
                list.Add("\t\t\treturn| false;");
            }

            return list;
        }

        
    }
}
