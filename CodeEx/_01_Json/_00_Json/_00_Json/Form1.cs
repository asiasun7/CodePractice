using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;

namespace _00_Json
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //이벤트 선언
            InitEvent();
        }
        /// <summary>
        /// Json Read 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitEvent()
        {
            button1.Click += UiBtn_CreateJson_Click;
            button2.Click += UiBtn_WriteJson_Click;
            button3.Click += UiBtn_ReadJson_Click;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
             
        }

        private void UiBtn_ReadJson_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            ReadJson();
        }

        private void UiBtn_WriteJson_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            WriteJson();
        }

        private void UiBtn_CreateJson_Click(object sender, EventArgs e)
        {
            CreateJson();
        }
        private void CreateJson()
        {
            string path = @"C:\test\test.json";
            if (!File.Exists(path))
            {
                using (File.Create(path))//파일이 없을때 생성
                {
                    MessageBox.Show("파일 생성 성공");
                }
            }
            else
            {//파일이 있다면 이미 있다고 알려줌
                MessageBox.Show("이미 파일이 존재합니다.");
            }
        }
        private void WriteJson()
        {
            string path = @"C:\test\test.json";

            if (File.Exists(path))//파일이 존재한다면
            {
                InputJson(path);
            }
        }
        private void InputJson(string path)
        {
            var users = new[] { "USER1", "USER2", "USER3", "USER4" };
            JObject dbSpec = new JObject(
                new JProperty("IP","127.0.0.1"),
                new JProperty("ID", "BEOMBEoMJOJO"),
                new JProperty("PW", "1234"),
                new JProperty("SID", "TEST"),
                new JProperty("DATABASE","TEST"));

            dbSpec.Add("USERS", JArray.FromObject(users));

            File.WriteAllText(path, dbSpec.ToString());

            richTextBox1.Text = dbSpec.ToString();
        }
        private void ReadJson()
        {
            string jsonFilePath = @"C:\test\test.json";
            string str = string.Empty;
            string users = string.Empty;

            using (StreamReader file = File.OpenText(jsonFilePath)) 
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject json = (JObject)JToken.ReadFrom(reader);
                DataBase _db = new DataBase();

                _db.IP = (string)json["IP"].ToString();
                _db.ID = (string)json["ID"].ToString();
                _db.PW = (string)json["PW"].ToString();
                _db.SID = (string)json["SID"].ToString();
                _db.DATABASE = (string)json["DATABASE"].ToString();


                var user = json.SelectToken("USERS");
                var cnt = user.Count();
                
                for(int idx = 0; idx < user.Count(); idx++)
                {
                    var name = user[idx].ToString();
                    if (idx ==0)
                    {
                        users += $"{name}"; 
                    }
                    else
                    {
                        users += $",{name}";
                    }
                }
                str = $"IP : {_db.IP}\n ID : {_db.ID}\n " +
                    $" PW : {_db.PW}\n SID : {_db.SID}\n DATABASE : {_db.DATABASE}";
                richTextBox1.Text = str;
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class DataBase
    {
        public string IP = string.Empty;
        public string ID = string.Empty;
        public string PW = string.Empty;
        public string SID = string.Empty;
        public string DATABASE = string.Empty;
    }
}
