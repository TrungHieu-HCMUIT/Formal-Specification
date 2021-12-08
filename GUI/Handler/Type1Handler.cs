using MenuStripGUI.Model;
using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Handler
{
    class Type1Handler: BaseFormalLanguageHandler
    {
        public List<Type1PostExpressionModel> implicitPostExpressionModelList;

        public Type1Handler(string functionInforLine, string preConditionLine, string postConditionLine)
        {
            this.typeID = FormalLanguageType.TYPE_1;
            this.functionInforLine = functionInforLine;
            this.preConditionLine = preConditionLine;
            this.postConditionLine = postConditionLine;

            this.implicitPostExpressionModelList = new List<Type1PostExpressionModel>();

            base.GetFunctionInformation();
        }

        public override List<string> GenerateCheckFunction()
        {
            var list = new List<String>();

            string functionDeclarationLine = string.Format("\t\tpublic| int| {0}_{1}", 
                Const.Check, 
                GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_DECLARATION, this.inputMap));
            list.Add(functionDeclarationLine);

            list.Add("\t\t{");//open bracket of CheckFunction function
            if (preConditionLine == "")
            {
                list.Add("\t\t\treturn| 1;");
            }
            else
            {
                string condition = string.Format("\t\t\tif| ({0})", preConditionLine);
                list.Add(condition);
                list.Add("\t\t\t\treturn| 1;");
                list.Add("\t\t\telse");
                list.Add("\t\t\t\treturn| 0;");
            }
            list.Add("\t\t}");//close bracket of CheckFunction function

            return list;
        }

        public override List<string> GenerateProcessFunction()
        {
            HandleImplicitPostExpressionModelList();
            var list = new List<String>();

            string functionDeclarationLine = string.Format("\t\tpublic| {0} |Process_{1}", 
                FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(this.outputMap.First().Value),
                GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_DECLARATION, this.inputMap));
            list.Add(functionDeclarationLine);

            list.Add("\t\t{");//open bracket of CheckFunction function

            string outputVarDeclarationLine = string.Format("\t\t\t|{0}| {1} = ",
                FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(outputMap.First().Value),
                outputMap.First().Key);
            switch (FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(outputMap.First().Value))
            {
                case "string":
                    outputVarDeclarationLine += "\"\";";
                    break;
                case "bool":
                    outputVarDeclarationLine += "false;";
                    break;
                default:
                    outputVarDeclarationLine += "0;";
                    break;
            }
            list.Add(outputVarDeclarationLine);

            //Case there is just one execution expression
            //Ex: post x=-b/a
            if (implicitPostExpressionModelList.Count == 1)
            {
                if (implicitPostExpressionModelList[0].conditionExpressionList.Count == 0)
                {
                    list.Add(string.Format("\t\t\t{0}", implicitPostExpressionModelList[0].GenerateExecutionScript()));
                }
            }
            else
            {
                foreach (Type1PostExpressionModel model in implicitPostExpressionModelList)
                {
                    list.Add(string.Format("\t\t\t{0}", model.GenerateConditionScript()));
                    list.Add(string.Format("\t\t\t\t{0}", model.GenerateExecutionScript()));
                }
            }
            list.Add(string.Format("\t\t\treturn| {0};", outputMap.First().Key));

            list.Add("\t\t}");//close bracket of CheckFunction function

            return list;
        }

        private void HandleImplicitPostExpressionModelList()
        {
            //Split the entire expression to && expression pair
            //and create list of ImplicitPostExpressionModel
            string[] implicitPostExpressionStringList = postConditionLine.Split(new[] { "||" }, StringSplitOptions.None);
            foreach (string implicitPostExpressionString in implicitPostExpressionStringList)
            {
                var newImplicitPostExpressionString = StringHelper.ClearParenthese(implicitPostExpressionString);
                string[] expressionList = newImplicitPostExpressionString.Split(new[] { "&&" }, StringSplitOptions.None);

                string execution = "";
                List<string> conditionList = new List<string>();
                foreach (string expression in expressionList)
                {
                    if (expression.Contains(this.outputMap.First().Key)
                        && !expression.Contains(">")
                        && !expression.Contains("<")
                        && !expression.Contains("!"))
                        execution = expression;
                    else
                        conditionList.Add(expression);
                }
                this.implicitPostExpressionModelList.Add(new Type1PostExpressionModel(execution, conditionList));
            }
        }
    }
}
