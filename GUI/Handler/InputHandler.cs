using MenuStripGUI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (handlerModel.typeID == FormalLanguageType.IMPLICIT)
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
                        {   //mỗi biểu thức nhỏ xử lý giống pre handle exception
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
    }
}
