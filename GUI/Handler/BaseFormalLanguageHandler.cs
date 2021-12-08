using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Handler
{
    public enum FormalLanguageType { TYPE_1, TYPE_2 }
    public enum OutputFunctionFormatType { REF_FUNCTION_DECLARATION, VALUE_FUNCTION_DECLARATION, REF_FUNCTION_CALL, VALUE_FUNCTION_CALL }
    public abstract class BaseFormalLanguageHandler
    {
        public FormalLanguageType typeID;
        public string functionInforLine;
        public string preConditionLine;
        public string postConditionLine;

        public string functionName;
        public Dictionary<string, string> inputMap = new Dictionary<string, string>();
        public Dictionary<string, string> outputMap = new Dictionary<string, string>();

        public abstract List<string> GenerateCheckFunction();
        public abstract List<string> GenerateProcessFunction();

        public void GetFunctionInformation()
        {
            string[] lines = functionInforLine.Split(new[] { "(", ")" }, StringSplitOptions.None);
            functionName = lines[0];
            InitInputMap(lines[1]);
            InitOutputMap(lines[2]);
        }

        private void InitInputMap(string inputString)
        {
            string[] splitedInputString = inputString.Split(new[] { ":", "," }, StringSplitOptions.None);
            for (int i = 0; i < splitedInputString.Length; i+= 2)
            {
                this.inputMap.Add(splitedInputString[i], splitedInputString[i + 1]);
            }
        }

        private void InitOutputMap(string outputString)
        {
            string[] splitedOutputString = outputString.Split(new[] { ":", "," }, StringSplitOptions.None);
            this.outputMap.Add(splitedOutputString[0], splitedOutputString[1]);
        }

        public virtual List<string> GenerateInputFunction()
        {
            var list = new List<String>();

            //Function name
            string functionDeclaration = string.Format("\t\tpublic| void |{0}_{1}",
                Const.Input,
                GetFunctionNameFormat(OutputFunctionFormatType.REF_FUNCTION_DECLARATION, this.inputMap));

            list.Add(functionDeclaration);
            list.Add("\t\t{");

            //Function body
            foreach (KeyValuePair<string, string> pair in this.inputMap)
            {
                string type = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                string function = string.Format("\t\t\tConsole|.|WriteLine|(|\"Enter input {0}: \"|);", pair.Key);
                list.Add(function);

                function = string.Format("\t\t\t{0} = {1}.|Parse|(|Console|.|ReadLine|());", pair.Key, type);
                list.Add(function);
            }
            list.Add("\t\t}");


            return list;
        }

        public List<string> GenerateOutputFunction()
        {
            var list = new List<String>();
            //Function name

            string funtionDeclaration = string.Format("\t\tpublic| void |{0}_{1}", 
                Const.Output, 
                GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_DECLARATION, this.outputMap));
            list.Add(funtionDeclaration);
            list.Add("\t\t{");

            //Function body
            string function = "\t\t\tConsole|.WriteLine|(|\"The result is: {0}\"|,";
            string functionTail = string.Format(" {0});", this.outputMap.First().Key);
            list.Add(function + functionTail);
            list.Add("\t\t}");
                
            return list;
        }

        public string GetFunctionNameFormat(OutputFunctionFormatType formatType, Dictionary<string, string> parameterMaps)
        {
            string result = functionName + "|(";
            // Function parameter content
            switch (formatType)
            {
                case OutputFunctionFormatType.REF_FUNCTION_DECLARATION:
                    int countCase1 = 0;
                    int numberOfInputParameterCase1 = parameterMaps.Count;
                    foreach (KeyValuePair<string, string> pair in parameterMaps)
                    {
                        countCase1++;
                        string primitiveType = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                        string line = string.Format("|ref| |{0}| {1}", primitiveType, pair.Key);
                        if (countCase1 < numberOfInputParameterCase1)
                            line = string.Concat(line, ", ");
                        result += line;
                    }
                    break;
                case OutputFunctionFormatType.VALUE_FUNCTION_DECLARATION:
                    int countCase2 = 0;
                    int numberOfInputParameterCase2 = parameterMaps.Count;
                    foreach (KeyValuePair<string, string> pair in parameterMaps)
                    {
                        countCase2++;
                        string primitiveType = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                        string line = string.Format("|{0}| {1}", primitiveType, pair.Key);
                        if (countCase2 < numberOfInputParameterCase2)
                            line = string.Concat(line, ", ");
                        result += line;
                    }
                    break;
                case OutputFunctionFormatType.REF_FUNCTION_CALL:
                    int countCase3 = 0;
                    int numberOfInputParameterCase3 = parameterMaps.Count;
                    foreach (KeyValuePair<string, string> pair in parameterMaps)
                    {
                        countCase3++;
                        string primitiveType = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                        string line = string.Format("|ref| {0}", pair.Key);
                        if (countCase3 < numberOfInputParameterCase3)
                            line = string.Concat(line, ", ");
                        result += line;
                    }
                    break;
                case OutputFunctionFormatType.VALUE_FUNCTION_CALL:
                    int countCase4 = 0;
                    int numberOfInputParameterCase4 = parameterMaps.Count;
                    foreach (KeyValuePair<string, string> pair in parameterMaps)
                    {
                        countCase4++;
                        string primitiveType = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                        string line = string.Format("{0}", pair.Key);
                        if (countCase4 < numberOfInputParameterCase4)
                            line = string.Concat(line, ", ");
                        result += line;
                    }
                    break;
            }
            result += ")";

            return result;
        }
    }
}
