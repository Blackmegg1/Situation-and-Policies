using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace 网课查题
{
    public partial class Form1 : Form
    {
        private string answer;
        private string last = "";
        JArray jsonAnswer;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string res = Utils.findAnswer(textBox1.Text);


            JObject answer = JsonConvert.DeserializeObject<JObject>(res);
            if(answer["code"].Value<int>()!=200)
            {
                textBox3.Text = "找不到";
                textBox2.Text = "找不到";
                return;
            }
            textBox3.Text=answer["data"][0].Value<JObject>()["question"].Value<string>();
            textBox2.Text = answer["data"][0].Value<JObject>()["answer"].Value<string>();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Utils.init();
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IDataObject data = Clipboard.GetDataObject();

            // 将数据与指定的格式进行匹配，返回bool
            if (data.GetDataPresent(DataFormats.Text))
            {
 
                string text = (string)data.GetData(DataFormats.Text);
                if(text!= last)
                {
                    try
                    {
                       
                        for (int i=0;i<jsonAnswer.Count;i++)
                        {
                            string problem = jsonAnswer[i]["Body"].ToString();
                            if (problem.Contains(text))
                            {
                                textBox3.Text = problem;
                                JArray answerArray = jsonAnswer[i]["Answer"].Value<JArray>();
                                JArray optionsArray = jsonAnswer[i]["Options"].Value<JArray>();
                                string finalAnswer = "";
                                for (int j=0;j<answerArray.Count;j++)
                                {
                                    for(int m=0;m< optionsArray.Count;m++)
                                    {
                                        if(optionsArray[m]["key"].Value<string>()== answerArray[j].Value<string>())
                                        {
                                            finalAnswer += optionsArray[m]["value"].Value<string>() + "\r\n";
                                        }
                                    }
                                    

                                }
                                textBox2.Text = finalAnswer;
                                return;
                            }
                        }
                    }
                    catch (Exception exce)
                    {
                        Console.WriteLine("{0} Second exception.", exce.Message);
                        textBox3.Text = text;
                        textBox2.Text = "找不到";
                    }
                    textBox3.Text = text;
                    textBox2.Text = "找不到";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            answer = Utils.get("http://op.shanghaojia.icu:8081/problem.txt");//获取题库
            jsonAnswer = JsonConvert.DeserializeObject<JArray>(answer);
            MessageBox.Show("本次获取到的题目数量为" + jsonAnswer.Count);
            timer1.Enabled = true;
        }
    }
}
