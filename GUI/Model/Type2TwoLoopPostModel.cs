using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Model
{
    public class Type2TwoLoopPostModel : Type2PostBaseModel
    {
        public string startRangeSecondLoop;
        public string endRangeSecondLoop;
        public string indexVariableNameSecondLoop;

        public Type2TwoLoopPostModel(string postValue, string arrayName, string parameterName)
        {
            this.arrayName = arrayName;
            this.parameterName = parameterName;
            SplitStringAndGetProperty(postValue);
        }

        public override void SplitStringAndGetProperty(string postValue)
        {
            string[] splitedPostValue = postValue.Split(new[] { "." }, StringSplitOptions.None);
            string firstLoopInformation = splitedPostValue[0];
            string secondLoopInformation = splitedPostValue[1];

            this.expression = splitedPostValue[2];
            this.loopType = GetLoopType(firstLoopInformation, secondLoopInformation);

            this.indexVariableName = Type2PostBaseModel.GetIndexVariableNameFromClause(
                firstLoopInformation
                .Split(new[] { "TH" }, StringSplitOptions.None)[0]
                .Split(new[] { "(" }, StringSplitOptions.None)[1]);
            var firstLoopRange = GetLoopInformation(firstLoopInformation);
            this.startRange = firstLoopRange[0];
            this.endRange = firstLoopRange[1];

            this.indexVariableNameSecondLoop = Type2PostBaseModel.GetIndexVariableNameFromClause(
                secondLoopInformation
                .Split(new[] { "TH" }, StringSplitOptions.None)[0]);
            var secondLoopRange = GetLoopInformation(secondLoopInformation);
            this.startRangeSecondLoop = secondLoopRange[0];
            this.endRangeSecondLoop = secondLoopRange[1];
        }

        public override List<string> GenerateLoopCode()
        {
            var list = new List<String>();

            string firstForLine = string.Format("\t\t\tfor| (|int| {0} = {1} - 1; {0} <= {2} - 1; {0}++)", this.indexVariableName, startRange, endRange);
            list.Add(firstForLine);
            list.Add("\t\t\t{");

            string codeExpression = expression.Replace("(", "[").Replace(")", "]");

            switch (loopType)
            {
                case PostConditionType.VM_VM:
                    AppendLoopCodeTypeVM_VM(ref list, codeExpression);
                    break;
                case PostConditionType.VM_TT:
                    AppendLoopCodeTypeVM_TT(ref list, codeExpression);
                    break;
                case PostConditionType.TT_TT:
                    AppendLoopCodeTypeTT_TT(ref list, codeExpression);
                    break;
                case PostConditionType.TT_VM:
                    AppendLoopCodeTypeTT_VM(ref list, codeExpression);
                    break;
            }

            return list;
        }

        private PostConditionType GetLoopType(string firstLoopInformation, string secondLoopInformation)
        {
            if (firstLoopInformation.Contains("VM"))
            {
                if (secondLoopInformation.Contains("TT"))
                    return PostConditionType.VM_TT;
                else
                    return PostConditionType.VM_VM;
            }                
            else
            {
                if (secondLoopInformation.Contains("VM"))
                    return PostConditionType.TT_VM;
                else
                    return PostConditionType.TT_TT;
            }
        }

        private string[] GetLoopInformation(string loopInformation)
        {
            string range = loopInformation.Split(new[] { "TH" }, StringSplitOptions.None)[1];
            range = range.Replace("{", String.Empty);
            range = range.Replace("}", String.Empty);

            string[] loopRange = { range.Split(new[] { "_" }, StringSplitOptions.None)[0],
            range.Split(new[] { "_" }, StringSplitOptions.None)[1] };

            return loopRange;
        }

        private void AppendLoopCodeTypeVM_VM(ref List<string> scriptList, string codeExpression)
        {
            string secondForLine = string.Format("\t\t\t\tfor| (|int| {0} = {1}; {0} <= {2} - 1; {0}++)", this.indexVariableNameSecondLoop, startRangeSecondLoop, endRangeSecondLoop);
            scriptList.Add(secondForLine);

            scriptList.Add("\t\t\t\t{");
            scriptList.Add(string.Format("\t\t\t\t\tif| ({0})", codeExpression) + " { }");
            scriptList.Add("\t\t\t\t\telse| return| false;");
            scriptList.Add("\t\t\t\t}");

            scriptList.Add("\t\t\t}");
            scriptList.Add("\t\t\treturn| true;");
        }

        private void AppendLoopCodeTypeVM_TT(ref List<string> scriptList, string codeExpression) {
            string flagVariable = "check_" + arrayName;
            scriptList.Add(string.Format("\t\t\t\tbool| {0} = false;", flagVariable));

            string secondForLine = string.Format("\t\t\t\tfor| (|int| {0} = {1}; {0} <= {2} - 1; {0}++)", this.indexVariableNameSecondLoop, startRangeSecondLoop, endRangeSecondLoop);
            scriptList.Add(secondForLine);

            scriptList.Add("\t\t\t\t{");
            scriptList.Add(string.Format("\t\t\t\t\tif| ({0}) ", codeExpression));
            scriptList.Add(string.Format("\t\t\t\t\t\t{0} = true;", flagVariable));
            scriptList.Add("\t\t\t\t}");
            scriptList.Add(string.Format("\t\t\t\tif| (({0}) == false)", flagVariable));
            scriptList.Add("\t\t\t\t\treturn| false;");

            scriptList.Add("\t\t\t}");
            scriptList.Add("\t\t\treturn| true;");
        }
        private void AppendLoopCodeTypeTT_TT(ref List<string> scriptList, string codeExpression)
        {
            string secondForLine = string.Format("\t\t\t\tfor| (|int| {0} = {1}; {0} <= {2} - 1; {0}++)", this.indexVariableNameSecondLoop, startRangeSecondLoop, endRangeSecondLoop);
            scriptList.Add(secondForLine);

            scriptList.Add("\t\t\t\t{");
            scriptList.Add(string.Format("\t\t\t\t\tif| ({0})", codeExpression));
            scriptList.Add("\t\t\t\t\t\treturn| true;");
            scriptList.Add("\t\t\t\t}");

            scriptList.Add("\t\t\t}");
            scriptList.Add("\t\t\treturn| false;");
        }
        private void AppendLoopCodeTypeTT_VM(ref List<string> scriptList, string codeExpression)
        {
            string secondForLine = string.Format("\t\t\t\tfor| (|int| {0} = {1}; {0} <= {2} - 1; {0}++)", this.indexVariableNameSecondLoop, startRangeSecondLoop, endRangeSecondLoop);
            scriptList.Add(secondForLine);

            scriptList.Add("\t\t\t\t{");
            scriptList.Add(string.Format("\t\t\t\t\tif| (!({0}))", codeExpression) + " break;");
            scriptList.Add(string.Format("\t\t\t\t\tif| ({0} == {1} - 1)| return| true;", this.indexVariableNameSecondLoop, endRangeSecondLoop));
            scriptList.Add("\t\t\t\t}");

            scriptList.Add("\t\t\t}");
            scriptList.Add("\t\t\treturn| false;");
        }
    }
}
