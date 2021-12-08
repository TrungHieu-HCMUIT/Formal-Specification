using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuStripGUI.Handler
{
    class InputHandler
    {
        private static InputHandler instance;

        public BaseFormalLanguageHandler handlerModel;

        public static InputHandler Instance
        {
            get { if (instance == null) instance = new InputHandler(); return instance; }
            private set { instance = value; }
        }
        private InputHandler() { }

        //Format and recolor for pre condition in input
        public string PreHandleForInput(string pre)
        {
            pre = StringHelper.ClearParenthese(pre);
            if (pre == "")
                return pre;

            string[] temp_pre = pre.Split(new[] { "&&" }, StringSplitOptions.None);
            if (temp_pre.Length > 1)
            {
                pre = "(";
                for (int i = 0; i < temp_pre.Length; i++)
                {
                    if (i == 0)
                        pre += string.Format("({0})", temp_pre[i]);
                    else
                        pre += string.Format(" ~&&~ ({0})", temp_pre[i]);
                }
                pre += ")";
            }
            else
            {
                pre = string.Format("({0})", temp_pre[0]);
            }
            return pre;

        }

        //Format and recolor for pre condition in input
        public string PostHandleForInput(string post)
        {
            if (handlerModel.typeID == FormalLanguageType.TYPE_1)
            {
                post = StringHelper.ClearParenthese(post);
                string[] expressions = post.Split(new[] { "||" }, StringSplitOptions.None);
                //If there is just one expression
                if (expressions.Length == 1)
                {
                    post = string.Format("({0})", expressions[0]);
                }
                //If there is more than one expression
                else
                {
                    //Run for each expression
                    post = "(";
                    for (int i = 0; i < expressions.Length; i++)
                    {

                        if (i == 0)
                        {   //Split into many expression and handle like PreHandleForOutput
                            post += PreHandleForInput(expressions[i]);
                        }
                        else
                        {
                            post += "\r\n       ~||~ " + PreHandleForInput(expressions[i]);
                        }
                    }
                    post += ")";
                }
            }
            return post;
        }

        public void RecolorOutput(RichTextBox txbOutput, List<string> outputScript, string className)
        {
            txbOutput.Text = "";
            for (int i = 0; i < outputScript.Count; i++)
            {
                string[] splitString = outputScript[i].Split(new char[] { '|' });
                for (int j = 0; j < splitString.Length; j++)
                {
                    if (splitString[j].Contains("using") ||
                        splitString[j].Contains("namespace") ||
                        splitString[j].Contains("public") ||
                        splitString[j].Contains("class") ||
                        splitString[j].Contains("void") ||
                        splitString[j].Contains("ref") ||
                        splitString[j].Contains("float") ||
                        splitString[j].Contains("int") ||
                        splitString[j].Contains("string") ||
                        splitString[j].Contains("bool") ||
                        splitString[j].Contains("new") ||
                        splitString[j].Contains("static")
                        )
                        txbOutput.SelectionColor = Color.Blue;
                    else if (splitString[j].Contains("FormalSpecification"))
                        txbOutput.SelectionColor = Color.Black;
                    else if (splitString[j].Contains("\""))
                        txbOutput.SelectionColor = Color.Maroon;
                    else if (splitString[j].Contains("Main") ||
                        splitString[j].Contains("Parse") ||
                        splitString[j].Contains(Const.Input) ||
                        splitString[j].Contains(Const.Output) ||
                        splitString[j].Contains(Const.Check) ||
                        splitString[j].Contains(Const.Process) ||
                        splitString[j].Contains("ReadLine") ||
                        splitString[j].Contains("WriteLine"))
                        txbOutput.SelectionColor = Color.Peru;
                    else if (splitString[j].Contains("if") ||
                        splitString[j].Contains("else") ||
                        splitString[j].Contains("return"))
                        txbOutput.SelectionColor = Color.Purple;
                    else if (splitString[j].Contains(className) ||
                        splitString[j].Contains("fs") ||
                        splitString[j].Contains("Console"))
                        txbOutput.SelectionColor = Color.DarkCyan;
                    else
                        txbOutput.SelectionColor = Color.Black;
                    txbOutput.AppendText(splitString[j]);
                }
                txbOutput.AppendText("\r\n");
            }
        }

        public void RecolorInput(RichTextBox txbInput, string input)
        {
            txbInput.Text = "";
            //Split input to 3 lines
            string[] splitedInput = input.Split(new[] { "pre", "post" }, StringSplitOptions.None);

            //Title line
            //Title name
            string[] titleSplit = splitedInput[0].Split(new[] { "(", ")" }, StringSplitOptions.None);
            txbInput.SelectionColor = Color.DarkCyan;
            txbInput.AppendText(titleSplit[0]);
            //Title parametter
            string[] title_split_para = titleSplit[1].Split(new[] { ":", "," }, StringSplitOptions.None);
            for (int i = 0; i < title_split_para.Length; i += 2)
            {
                if (i == 0)
                {
                    txbInput.SelectionColor = Color.Black;
                    txbInput.AppendText("(" + title_split_para[i] + " : ");
                    txbInput.SelectionColor = Color.Red;
                    txbInput.AppendText(title_split_para[i + 1]);
                }
                else
                {
                    txbInput.SelectionColor = Color.Black;
                    txbInput.AppendText(", " + title_split_para[i] + " : ");
                    txbInput.SelectionColor = Color.Red;
                    txbInput.AppendText(title_split_para[i + 1]);
                }
            }
            txbInput.AppendText(") ");
            //Title result
            string[] titleSplitedResult = titleSplit[2].Split(new[] { ":", "," }, StringSplitOptions.None);
            txbInput.SelectionColor = Color.Black;
            txbInput.AppendText(titleSplitedResult[0] + " : ");
            txbInput.SelectionColor = Color.Red;
            txbInput.AppendText(titleSplitedResult[1] + "\r\n");
            //Pre line
            txbInput.SelectionColor = Color.Blue;
            txbInput.AppendText("pre ");
            //Handle parentheses pre line
            splitedInput[1] = PreHandleForInput(splitedInput[1]);
            //Split to recolor pre line
            string[] preSplit = splitedInput[1].Split(new[] { "~" }, StringSplitOptions.None);
            for (int j = 0; j < preSplit.Length; j++)
            {
                if (preSplit[j].Contains("&&"))
                {
                    txbInput.SelectionColor = Color.Maroon;
                    txbInput.AppendText(preSplit[j]);
                }
                else
                {
                    txbInput.SelectionColor = Color.Black;
                    txbInput.AppendText(preSplit[j]);
                }
            }
            txbInput.AppendText("\r\n");

            //Post line
            txbInput.SelectionColor = Color.Blue;
            txbInput.AppendText("post ");
            //Handle parentheses post line
            splitedInput[2] = PostHandleForInput(splitedInput[2]);
            //Split to recolor post line
            string[] postSlpit = splitedInput[2].Split(new[] { "~" }, StringSplitOptions.None);
            if (handlerModel.typeID == FormalLanguageType.TYPE_1)
            {
                for (int k = 0; k < postSlpit.Length; k++)
                {
                    if (postSlpit[k].Contains("||") ||
                        postSlpit[k].Contains("&&"))
                    {
                        txbInput.SelectionColor = Color.Maroon;
                        txbInput.AppendText(postSlpit[k]);
                    }
                    else
                    {
                        txbInput.SelectionColor = Color.Black;
                        txbInput.AppendText(postSlpit[k]);
                    }
                }
            }
            else
            {
                string postValue = postSlpit[0];
                txbInput.SelectionColor = Color.Peru;
                txbInput.AppendText(handlerModel.outputMap.First().Key);
                
                string formatedPostValue = postValue.Replace(handlerModel.outputMap.First().Key, "")
                    .Replace("VM", "VM ")
                    .Replace("TT", "TT ")
                    .Replace("TH", " TH ");
                txbInput.SelectionColor = Color.Black;
                txbInput.AppendText(formatedPostValue);
            }
        }
    }
}
