using MenuStripGUI.Model;
using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Handler
{
    class ExplicitTypeHandler : BaseFormalLanguageHandler
    {

        public ExplicitTypeHandler(string functionInforLine, string preConditionLine, string postConditionLine)
        {
            this.typeID = FormalLanguageType.EXPLICIT;
            this.functionInforLine = functionInforLine;
            this.preConditionLine = preConditionLine;
            this.postConditionLine = postConditionLine;

            base.GetFunctionInformation();
        }

        public override List<string> GenerateInputFunction()
        {
            var list = new List<String>();

            //Function name
            string functionDeclaration = string.Format("\t\tpublic| void |{0}_{1}",
                Const.Input,
                GetFunctionNameFormat(OutputFunctionFormatType.REF_FUNCTION_DECLARATION, this.inputMap));

            list.Add(functionDeclaration);
            list.Add("\t\t{");

            //Function body
            //Get array size parameter first
            var arraySizeVariable = "";
            foreach (KeyValuePair<string, string> pair in this.inputMap)
            {
                string type = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                if (type == "int" || type == "double")
                {
                    arraySizeVariable = pair.Key;
                    string function = string.Format("\t\t\tConsole|.|WriteLine|(|\"Enter input {0}: \"|);", pair.Key);
                    list.Add(function);

                    function = string.Format("\t\t\t{0} = {1}.|Parse|(|Console|.|ReadLine|());", pair.Key, type);
                    list.Add(function);
                }                
            }
            //Get array parameter
            foreach (KeyValuePair<string, string> pair in this.inputMap)
            {
                string type = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                if (type == "int[]" || type == "double[]")
                {
                    string function = string.Format("\t\t\tConsole|.|WriteLine|(|\"Enter elements for input {0}\"|);", pair.Key);
                    list.Add(function);

                    list.Add(string.Format("\t\t\t{0} = new {1}[{2}];", pair.Key, 
                        FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value).Replace("[", "").Replace("]", ""), 
                        arraySizeVariable));

                    string loopInputInformation = string.Format("\t\t\tfor| (|int| i = 0; i < {0}; i++)", arraySizeVariable);
                    list.Add(loopInputInformation);
                    list.Add("\t\t\t{");
                    list.Add("\t\t\t\tConsole|.|Write|(|\"Enter element: \"|);");
                    list.Add(string.Format("\t\t\t\t{0}[i] = {1}.|Parse|(|Console|.|ReadLine|());", pair.Key, type.Replace("[", String.Empty).Replace("]", String.Empty)));
                    list.Add("\t\t\t}");
                }
            }
            list.Add("\t\t}");

            return list;
        }

        public override List<string> GenerateCheckFunction()
        {
            List<string> scriptList = new List<string>();
            string functionDeclarationLine = string.Format("\t\tpublic| int |{0}_{1}",
                Const.Check,
                GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_DECLARATION, this.inputMap));
            scriptList.Add(functionDeclarationLine);
            scriptList.Add("\t\t{");//open bracket of CheckFunction function
            scriptList.Add("\t\t\treturn| 1;");
            scriptList.Add("\t\t}");
            return scriptList;
        }

        public override List<string> GenerateProcessFunction()
        {
            List<string> scriptList = new List<string>();

            List<string> valueParameterList = inputMap.Values.ToList();
            ExplicitPostBaseModel explicitPostModel = ExplicitPostBaseModel.GetExplicitPostModel(postConditionLine,
                GetParameterKeyFromValue(valueParameterList[0]),
                GetParameterKeyFromValue(valueParameterList[1]));

            string functionDeclarationLine = string.Format("\t\tpublic| bool |{0}_{1}",
                Const.Process,
                GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_DECLARATION, this.inputMap));
            scriptList.Add(functionDeclarationLine);
            scriptList.Add("\t\t{");//open bracket of CheckFunction function

            foreach (string script in explicitPostModel.GenerateLoopCode())
            {
                scriptList.Add(script);
            }

            scriptList.Add("\t\t}");//close bracket of CheckFunction function

            return scriptList;
        }

        private string GetParameterKeyFromValue(string value)
        {
            foreach (KeyValuePair<string, string> input in inputMap)
            {
                if (value == input.Value)
                    return input.Key;
            }
            Console.Write("Not found value for input map");
            return "";
        }
    }
}
