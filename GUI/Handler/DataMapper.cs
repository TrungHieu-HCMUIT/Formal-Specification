using MenuStripGUI.Handler;
using MenuStripGUI.Model;
using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace DataMapper
{
    public class DataMapper
    {
        
        private static DataMapper instance;
        public BaseFormalLanguageHandler handlerModel;
        public string exception = "";

        public static DataMapper Instance
        {
            get { if (instance == null) instance = new DataMapper(); return instance; }
            private set { instance = value; }
        }
        private DataMapper() { }

        //Generate output source code
        public List<string> GenerateOutputSource(ref string flagException, string className, string input)
        {
            var outputScripts = new List<string>();
            input = input.Replace(" ", String.Empty);

            string[] formalLanguageLines = input.Split(new[] {"pre", "post" }, StringSplitOptions.None);
            var functionInformationLine = formalLanguageLines[0];
            var preConditionLine = formalLanguageLines[1];
            var postConditionLine = formalLanguageLines[2];

            switch (FormalLanguageHelper.GetFormalLanguageType(preConditionLine, postConditionLine))
            {
                case FormalLanguageType.IMPLICIT:
                    handlerModel = new ImplicitTypeHandler(functionInformationLine, preConditionLine, postConditionLine);
                    break;
                case FormalLanguageType.EXPLICIT:
                    handlerModel = new ExplicitTypeHandler(functionInformationLine, preConditionLine, postConditionLine);
                    break;
            }

            string[] splitedFunctionInformationLine = functionInformationLine.Split(new[] { "(", ")" }, StringSplitOptions.None);

            //ten ham
            try
            {
                outputScripts.Add("using| System;");
                outputScripts.Add("namespace| FormalSpecification");
                outputScripts.Add("{"); //open bracket for name space body
                outputScripts.Add(string.Format("\tpublic| class| {0}", className));
                outputScripts.Add("\t{"); //open bracket for class body

                MapToIOFunction(ref outputScripts);
                MapToCheckFunction(ref outputScripts);
                MapToProcessFunction(ref outputScripts);
                MapToMainFunction(ref outputScripts, className);

                outputScripts.Add("\t}");//close bracket for class program body
                outputScripts.Add("}");//close bracket for name space body

                flagException = exception;
                return outputScripts;
            }
            catch
            {
                flagException = "Invalid Input";
                return outputScripts;
            }
            
        }

        //IO function generation
        public void MapToIOFunction(ref List<string> outputScripts)
        {
                var listInputFunctionScript = handlerModel.GenerateInputFunction();
                foreach(string scriptLine in listInputFunctionScript) {
                    outputScripts.Add(scriptLine);
                }

                var listOutputFunctionScript = handlerModel.GenerateOutputFunction();
            foreach (string scriptLine in listOutputFunctionScript)
            {
                outputScripts.Add(scriptLine);
            }
        }

        //Pre handler: Generate check function
        public void MapToCheckFunction(ref List<string> outputScripts)
        {
            ExceptionPreHandler(ref handlerModel.preConditionLine);
            var listCheckFunctionScript = handlerModel.GenerateCheckFunction();
            foreach (string scriptLine in listCheckFunctionScript)
            {
                outputScripts.Add(scriptLine);
            }
        }

        public void MapToProcessFunction(ref List<string> outputScripts)
        {
            if (handlerModel.typeID == FormalLanguageType.IMPLICIT) ExceptionPostHandler(ref handlerModel.postConditionLine);
            else
            {
                handlerModel.postConditionLine = handlerModel.postConditionLine.Remove(handlerModel.postConditionLine.Length - 1, 1);
            }
            var listProcessFunctionScript = handlerModel.GenerateProcessFunction();
            foreach (string scriptLine in listProcessFunctionScript)
            {
                outputScripts.Add(scriptLine);
            }
        }

        public void MapToMainFunction(ref List<string> list, string class_name)
        {
            try
            {
                list.Add("\t\tpublic| static| void| Main|(|string[]| args)");
                list.Add("\t\t{"); //open bracket for main function
                GenerateVariableInitializationCode(ref list, handlerModel.inputMap);
                GenerateVariableInitializationCode(ref list, handlerModel.outputMap);

                list.Add(string.Format("\t\t\t{0}| fs| = |new |{0}|();", class_name));
                list.Add(string.Format("\t\t\tfs|.|{0}_{1}|;", Const.Input, handlerModel.GetFunctionNameFormat(OutputFunctionFormatType.REF_FUNCTION_CALL, handlerModel.inputMap)));
                list.Add(string.Format("\t\t\tif |(|fs|.|{0}_{1}| == 1)", Const.Check, handlerModel.GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_CALL, handlerModel.inputMap)));
                list.Add("\t\t\t{"); //open bracket for if condition
                list.Add(string.Format("\t\t\t\t{0} = |fs|.|{1}_{2}|;", handlerModel.outputMap.First().Key, 
                    Const.Process, 
                    handlerModel.GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_CALL, handlerModel.inputMap)));


                list.Add(string.Format("\t\t\t\t|fs|.|{0}_{1}|;", 
                    Const.Output, 
                    handlerModel.GetFunctionNameFormat(OutputFunctionFormatType.VALUE_FUNCTION_CALL, handlerModel.outputMap)));

                list.Add("\t\t\t}");//close bracket for if condition
                list.Add("\t\t\telse");
                list.Add("\t\t\t\tConsole|.WriteLine|(|\"Invalid input!\"|);");
                list.Add("\t\t\tConsole|.ReadLine|();");
                list.Add("\t\t}"); //close bracket for main function
            }
            catch
            {
                exception = "Invalid Main";
                Console.WriteLine("Failed: MainHandle");
            }
        }

        private void GenerateVariableInitializationCode(ref List<string> list, Dictionary<string, string> variableMap)
        {
            foreach (KeyValuePair<string, string> pair in variableMap)
            {
                string primitiveType = FormalLanguageHelper.MapFormalLanguageTypeToPrimitiveType(pair.Value);
                switch (primitiveType)
                {
                    case "string":
                        list.Add(string.Format("\t\t\t|{0}| {1} = \"\";", primitiveType, pair.Key));
                        break;
                    case "bool":
                        list.Add(string.Format("\t\t\t|{0}| {1} = false;", primitiveType, pair.Key));
                        break;
                    case "double[]":
                        list.Add(string.Format("\t\t\t|{0}| {1} = ", primitiveType, pair.Key) + "{ };");
                        break;
                    case "int[]":
                        list.Add(string.Format("\t\t\t|{0}| {1} = ", primitiveType, pair.Key) + "{ };");
                        break;
                    default:
                        list.Add(string.Format("\t\t\t|{0}| {1} = 0;", primitiveType, pair.Key));
                        break;
                }
            }
        }

        //Add or remove parenthese, avoid exception for pre condition
        public void ExceptionPreHandler(ref string preStringValue)
        {
            preStringValue = StringHelper.ClearParenthese(preStringValue);
            if (preStringValue == "")
                return;

            string[] conditionList = preStringValue.Split(new[] { "&&" }, StringSplitOptions.None);

            if (conditionList.Length == 1)
            {
                preStringValue = string.Format("({0})", conditionList[0]);
            }
            else
            {
                preStringValue = "(";
                for (int i = 0; i < conditionList.Length; i++)
                {
                    if (i == 0)
                        preStringValue += string.Format("({0})", conditionList[0]);
                    else
                        preStringValue += string.Format(" && ({0})", conditionList[i]);
                }
                preStringValue += ")";
            }
        }

        //Add or remove parenthese, avoid exception for post condition
        public void ExceptionPostHandler(ref string postValue)
        {
            postValue = StringHelper.ClearParenthese(postValue);
            string[] expressionAndPair = postValue.Split(new[] { "||" }, StringSplitOptions.None);
            //Case there is only one expression
            if (expressionAndPair.Length == 1)
            {
                postValue = string.Format("({0})", expressionAndPair[0]);
                return;
            }
            //Case there is more than one expression
            else
            {
                //Run for each && expression pair
                postValue = "(";
                for (int i = 0; i < expressionAndPair.Length; i++)
                {
                    if (i == 0)
                    {   //Run for each mini expression
                        string[] singleExpression = expressionAndPair[i].Split(new[] { "&&" }, StringSplitOptions.None);
                        for (int j=0; j< singleExpression.Length;j++)
                        {
                            if (j==0)
                                postValue += string.Format("({0})", singleExpression[j]);
                            else
                                postValue += string.Format("( && {0})", singleExpression[j]);
                        }    
                    }
                    else
                    {
                        postValue += "||";
                        string[] singleExpression = expressionAndPair[i].Split(new[] { "&&" }, StringSplitOptions.None);
                        for (int j = 0; j < singleExpression.Length; j++)
                        {
                            if (j == 0)
                                postValue += string.Format("({0})", singleExpression[j]);
                            else
                                postValue += string.Format("( && {0})", singleExpression[j]);
                        }
                    }
                }
                postValue += ")";
                return;
            }
        }
    }
}
