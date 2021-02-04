using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections;

namespace ModelAssy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
         private SQLiteConnection sql_con;
    
      

        private void Form1_Load(object sender, EventArgs e)
        {

            Data();

            SelectModel();

           
        }

        public void SelectModel()
        {

         SQLiteCommand sql_cmd;
         SQLiteDataAdapter DB;
         DataSet DS = new DataSet();
         DataTable DT = new DataTable();

            List<ModelMaster> modelM = new List<ModelMaster>();
        
            string json;
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();          
            string CommandText = "SELECT  Model_Id,Model_Name FROM  MasterModel ";

            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset(); 
            DB.Fill(DS);
            DT = DS.Tables[0];
            int count = DT.Rows.Count; 

            json = JsonConvert.SerializeObject(DS);

            var jsons = JsonConvert.DeserializeObject(json);
          
          

            for(int i=0; i < count; i++){
               
                comboBox1.DisplayMember ="Text";
                comboBox1.ValueMember = "Value";
                

                comboBox1.Items.Add(new {Text=DT.Rows[i]["Model_Name"],Value = DT.Rows[i]["Model_Id"] });

                //modelM.Add(new ModelMaster
                //{
                //    Model_Id = DT.Rows[i]["Model_Id"].ToString(),
                //    Model_Name = DT.Rows[i]["Model_Name"].ToString()

                //});
               
            }


            sql_con.Close(); 
            

        }

        public void Data()
        {


            SQLiteCommand sql_cmd;
            SQLiteDataAdapter DB;
            DataSet DS = new DataSet();
            DataTable DT = new DataTable();

            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT Drawing_No,Model_Name,Palette_Quantity,Box_Quantity,Unit_Code,Supplier_Name,Location_Code,Lot_No,Lot_Quantity,Actual_Stock,Status FROM Model_Lot INNER JOIN  MasterModel ON Model_Lot.Master_Model_Id = MasterModel.Model_Id";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dataGridView1.DataSource = DT;
            sql_con.Close(); 
        }

        private void SetConnection()
        {
            sql_con = new SQLiteConnection
                ("Data Source=Properties/DataSources/demoassy.db;Version=3;New=False;Compress=True;");
        }



        private void ExecuteQuery(string txtQuery)
        {

            SQLiteCommand sql_cmd;
           

            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }

       
        private void btnCloseF1_Click(object sender, EventArgs e)
        {

            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            add_Model f2 = new add_Model();

            f2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            string  lot_no, lot_qty;
            
            lot_no = textBox1.Text;
            lot_qty = textBox2.Text;
            int idz = comboBox1.SelectedIndex + 1;
            var se = comboBox2.Text;
            SetConnection();
            

            string txtQuery = "INSERT INTO Model_Lot(Master_Model_Id, Lot_No,Lot_Quantity,Actual_Stock,Status) VALUES ('" +idz+ "','" + this.textBox1.Text + "','" + this.textBox2.Text + "','" + this.textBox2.Text + "','"+se+"' ) ";

            if (lot_no == "") { MessageBox.Show("Please Input data"); } 
            else if (lot_qty == "") { MessageBox.Show("Please Input data"); } 
            else if (se == "") { MessageBox.Show("Please Input data"); }
            else
            {
                try
                {
                    ExecuteQuery(txtQuery);


                    MessageBox.Show("เพิ่มข้อมูลสำเร็จ");

                    Data();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }


            }
        }
           
           

           

        private void button3_Click(object sender, EventArgs e)
        {
            Data();
        }
      

    }
}
