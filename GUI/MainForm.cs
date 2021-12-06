using DataMapper;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using MenuStripGUI.Utils;
using MenuStripGUI.Handler;

namespace MenuStripGUI
{
    public partial class MainForm : Form
    {
        public string selected_txb { get; private set; }

        public MainForm()
        {
            selected_txb = "";
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateCode();
            txbInput.SelectionColor = Color.Black;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txbInput.SelectionColor = Color.Black;
            if (txbInput.Text == "" && txbOutput.Text == "")
                return;
            DialogResult dialogResult = MessageBox.Show("Do you want to save this one", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = String.Format("{0}|.txt", txbName.Text);
                if (savefile.Filter == "")
                    savefile.Filter = "Program|.txt";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(savefile.FileName);
                    sw.WriteLine(txbOutput.Text);
                    sw.Close();
                    DialogResult dialogResult2 = MessageBox.Show("Save succesful", "Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dialogResult2 == DialogResult.OK)
                    {

                    }
                    txbName.Text = "";
                    txbInput.Text = "";
                    txbOutput.Text = "";
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                txbName.Text = "";
                txbInput.Text = "";
                txbOutput.Text = "";
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = String.Format("{0}|.txt", txbName.Text);
            if (savefile.Filter == "")
                savefile.Filter = "Program|.txt";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(savefile.FileName);
                sw.WriteLine(txbInput.Text);
                sw.Close();
                DialogResult dialogResult = MessageBox.Show("Save succesful", "Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                }
            }
        }
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            try
            {
                txbInput.SelectionColor = Color.Black;
                string file_name = openFileDialog1.FileName;
                string read_file = File.ReadAllText(file_name);
                txbInput.Text = read_file;
                //lấy tên file, cắt đuôi và bỏ dấu cách
                txbName.Text = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.LastIndexOf(".")).Replace("", string.Empty);
            }
            catch
            {

            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.ShowDialog();
            try
            {
                string file_name = openFileDialog1.FileName;
                string read_file = File.ReadAllText(file_name);
                txbInput.SelectionColor = Color.Black;
                txbInput.Text = read_file;
                txbName.Text = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.LastIndexOf(".")).Replace("", string.Empty);
            }
            catch
            {

            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Filter = String.Format("{0}(*.txt)|.txt", txbName.Text);
            if (savefile.Filter == "")
                savefile.Filter = "Program|.txt";
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(savefile.FileName);
                sw.WriteLine(txbOutput.Text);
                sw.Close();
                DialogResult dialogResult = MessageBox.Show("Save succesful", "Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                {
                }
            }
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            txbInput.SelectionColor = Color.Black;
            if (txbInput.Text == "" && txbOutput.Text == "")
                return;
            DialogResult dialogResult = MessageBox.Show("Do you want to save this one", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = String.Format("{0}|.txt", txbName.Text);
                if (savefile.Filter == "")
                    savefile.Filter = "Program|.txt";
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(savefile.FileName);
                    sw.WriteLine(txbOutput.Text);
                    sw.Close();
                    DialogResult dialogResult2 = MessageBox.Show("Save succesful", "Notify", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (dialogResult2 == DialogResult.OK)
                    {

                    }
                    txbName.Text = "";
                    txbInput.Text = "";
                    txbOutput.Text = "";
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                txbName.Text = "";
                txbInput.Text = "";
                txbOutput.Text = "";
            }
        }
        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult2 = MessageBox.Show(" This is a tool for generating programming language with: \r\n - Input text: a formal specification function (implicit) \r\n - Output text: a completing program which uses C# programming language. \r\n - Support operators: +, -, *, /, %, >, <, =, !=, >=, <=, !, &&, ||", "Formular Specification Generating Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (dialogResult2 == DialogResult.OK)
            {
            }
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult2 = MessageBox.Show(" This is a tool for generating programming language with: \r\n - Input text: a formal specification function (implicit) \r\n - Output text: a completing program which uses C# programming language. \r\n - Support operators: +, -, *, /, %, >, <, =, !=, >=, <=, !, &&, ||", "Formular Specification Generating Tool", MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (dialogResult2 == DialogResult.OK)
            {
            }
        }
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            if (selected_txb == "input")
            {
                Clipboard.SetText(txbInput.SelectedText);
                txbInput.SelectedText = "";
            }
            else if (selected_txb == "name")
            {
                Clipboard.SetText(txbName.SelectedText);
                txbName.SelectedText = "";
            }
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            if (selected_txb == "input")
            {
                Clipboard.SetText(txbInput.SelectedText);
            }
            else if (selected_txb == "name")
            {
                Clipboard.SetText(txbName.SelectedText);
            }
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            if (selected_txb == "input")
            {
                txbInput.SelectedText = Clipboard.GetText();
            }
            else if (selected_txb == "name")
            {
                txbName.SelectedText = Clipboard.GetText();
            }
        }

        private void txbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GenerateCode();
            }
        }

        private void txbInput_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "input";
        }

        private void txbName_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "name";
        }

        private void txbExeName_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "exe";
            openFileDialog1.ShowDialog();
            try
            {
                string file_name = openFileDialog1.FileName;
                string read_file = File.ReadAllText(file_name);
                txbOutput.Text = read_file;
            }
            catch
            {

            }

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            selected_txb = "";
        }

        private void txbGenerate_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "";
        }

        private void lbClassName_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "";
        }

        private void lbExeName_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "";
        }

        private void btnGenerate_MouseDown(object sender, MouseEventArgs e)
        {
            selected_txb = "";
        }
      
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_txb == "input")
            {
                Clipboard.SetText(txbInput.SelectedText);
                txbInput.SelectedText = "";
            }
            else if (selected_txb == "name")
            {
                Clipboard.SetText(txbName.SelectedText);
                txbName.SelectedText = "";
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_txb == "input")
            {
                Clipboard.SetText(txbInput.SelectedText);
            }
            else if (selected_txb == "name")
            {
                Clipboard.SetText(txbName.SelectedText);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected_txb == "input")
            {
                txbInput.SelectedText = Clipboard.GetText();
            }
            else if (selected_txb == "name")
            {
                txbName.SelectedText = Clipboard.GetText();
            }
        }
        private void GenerateCode()
        {
            string flagException = "";
            string className = txbName.Text.ToString();

            if (className == "")
            {
                className = "FormalSpecification";
                txbName.Text = className;
            }

            string input = txbInput.Text.ToString();
            input = StringHelper.ClearRedundantText(input);

            List<string> outputScript = DataMapper.DataMapper.Instance.GenerateOutputSource(ref flagException, className, input);
            InputHandler.Instance.handlerModel = DataMapper.DataMapper.Instance.handlerModel;
            if (flagException == "")
            {
                //Change color for output textbox
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


                //xử lý input textbox
                txbInput.Text = "";
                //tách input thành 3 dòng title, pre và post
                string[] input_split = input.Split(new[] { "pre", "post" }, StringSplitOptions.None);

                //title line
                //// xử lý title name
                string[] title_split = input_split[0].Split(new[] { "(", ")" }, StringSplitOptions.None);
                txbInput.SelectionColor = Color.DarkCyan;
                txbInput.AppendText(title_split[0]);
                //// xử lý title parametter
                string[] title_split_para = title_split[1].Split(new[] { ":", "," }, StringSplitOptions.None);
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
                ////xử lý title result
                string[] title_split_result = title_split[2].Split(new[] { ":", "," }, StringSplitOptions.None);
                txbInput.SelectionColor = Color.Black;
                txbInput.AppendText(title_split_result[0] + " : ");
                txbInput.SelectionColor = Color.Red;
                txbInput.AppendText(title_split_result[1] + "\r\n");
                //pre line
                txbInput.SelectionColor = Color.Blue;
                txbInput.AppendText("pre ");
                //xử lý biểu thức post (bỏ các dấu ngoặc dư thừa hoặc bổ sung nếu thiếu)
                input_split[1] = InputHandler.Instance.PreHandleForInput(input_split[1]);
                // tách biểu thức để đổi màu
                string[] pre_split = input_split[1].Split(new[] { "~" }, StringSplitOptions.None);
                for (int j = 0; j < pre_split.Length; j++)
                {
                    if (pre_split[j].Contains("&&"))
                    {
                        txbInput.SelectionColor = Color.Maroon;
                        txbInput.AppendText(pre_split[j]);
                    }
                    else
                    {
                        txbInput.SelectionColor = Color.Black;
                        txbInput.AppendText(pre_split[j]);
                    }
                }
                txbInput.AppendText("\r\n");

                //post line
                txbInput.SelectionColor = Color.Blue;
                txbInput.AppendText("post ");
                //xử lý biểu thức post (bỏ các dấu ngoặc dư thừa hoặc bổ sung nếu thiếu)
                input_split[2] = InputHandler.Instance.PostHandleForInput(input_split[2]);
                // tách biểu thức để đổi màu
                string[] post_split = input_split[2].Split(new[] { "~" }, StringSplitOptions.None);
                for (int k = 0; k < post_split.Length; k++)
                {
                    if (post_split[k].Contains("||") ||
                        post_split[k].Contains("&&"))
                    {
                        txbInput.SelectionColor = Color.Maroon;
                        txbInput.AppendText(post_split[k]);
                    }
                    else
                    {
                        txbInput.SelectionColor = Color.Black;
                        txbInput.AppendText(post_split[k]);

                    }
                }
                lbGenerateStatus.ForeColor = Color.Blue;
                lbGenerateStatus.Text = "Success!";
            }
            else
            {
                DialogResult result = MessageBox.Show(flagException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //this.Close();
                }
                else
                {
                    //this.Close();
                }
                lbGenerateStatus.ForeColor = Color.Red;
                lbGenerateStatus.Text = "Failed!";
            }
            txbOutput.SelectionColor = Color.Black;
            txbInput.SelectionColor = Color.Black;

        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            if (txbInput.CanUndo == true)
            {
                // Determines if the redo operation deletes text.
                if (txbInput.RedoActionName != "Delete")
                    // Perform the redo.
                    txbInput.Undo();
            }
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            if (txbInput.CanRedo == true)
            {
                // Determines if the redo operation deletes text.
                if (txbInput.RedoActionName != "Delete")
                    // Perform the redo.
                    txbInput.Redo();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            string Output = "Out.exe";
            Button ButtonObject = (Button)sender;

            lbStatus.Text = "";
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            //Make sure we generate an EXE, not a DLL
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = Output;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, txbOutput.Text);

            if (results.Errors.Count > 0)
            {
                lbStatus.ForeColor = Color.Red;
                lbStatus.Text = "Failed!";
                string error_string ="";
                foreach (CompilerError CompErr in results.Errors)
                {
                    error_string = lbStatus.Text +
                                "Line number " + CompErr.Line +
                                ", Error Number: " + CompErr.ErrorNumber +
                                ", '" + CompErr.ErrorText + ";" +
                                Environment.NewLine + Environment.NewLine;
                }
                DialogResult error = MessageBox.Show(error_string, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                //Successful Compile
                lbStatus.ForeColor = Color.Blue;
                lbStatus.Text = "Success!";
                //If we clicked run then launch our EXE
                if (ButtonObject.Text == "Run") Process.Start(Output);
            }
        }

        private void txbName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
                e.Handled = true;
        }

        private void txbInput_TextChanged(object sender, EventArgs e)
        {
            if (txbInput.Text == "")
            {
                txbInput.SelectionColor = Color.Black;
            }    
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txbName_ClassName(object sender, EventArgs e)
        {

        }
    }

}