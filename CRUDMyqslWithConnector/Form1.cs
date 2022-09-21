using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUDMyqslWithConnector
{
    public partial class Form1 : Form
    {

        MySqlConnection connection = new MySqlConnection("server = localhost; uid = root; database = kontak");
        DataTable dataTable = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillDataKontak();

            tb_namaEdit.Enabled = false;
            tb_alamatEdit.Enabled = false;
            tb_id.Enabled = false;
            btn_save.Enabled = false;
        }

        public DataTable getDataKontak()
        {
            dataTable.Reset();
            dataTable = new DataTable();
            using (MySqlCommand command = new MySqlCommand("SELECT * FROM data_kontak", connection))
            {
                connection.Open();

                MySqlDataReader reader = command.ExecuteReader();
                dataTable.Load(reader);
            }
            return dataTable;
        }

        public void fillDataKontak()
        {
            dgv_dataKontak.DataSource = getDataKontak();

            DataGridViewButtonColumn colEditKontak = new DataGridViewButtonColumn();
            colEditKontak.UseColumnTextForButtonValue = true;
            colEditKontak.Text = "Edit";
            colEditKontak.Name = "";
            dgv_dataKontak.Columns.Add(colEditKontak);

            DataGridViewButtonColumn colDeleteKontak = new DataGridViewButtonColumn();
            colDeleteKontak.UseColumnTextForButtonValue = true;
            colDeleteKontak.Text = "Delete";
            colDeleteKontak.Name = "";
            dgv_dataKontak.Columns.Add(colDeleteKontak);

            connection.Close();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO data_kontak(nama, telepon) VALUE(@nama, @telepon)";
                cmd.Parameters.AddWithValue("@nama", tb_nama.Text);
                cmd.Parameters.AddWithValue("@telepon", tb_telepon.Text);
                cmd.ExecuteNonQuery();
                connection.Close();
                dgv_dataKontak.Columns.Clear();
                dataTable.Clear();
                fillDataKontak();
            } catch (Exception ex)
            {

            }
        }

        private void dgv_dataKontak_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                int id = Convert.ToInt32(dgv_dataKontak.CurrentCell.RowIndex.ToString());
                tb_id.Text = dgv_dataKontak.Rows[id].Cells[0].Value.ToString();
                tb_namaEdit.Text = dgv_dataKontak.Rows[id].Cells[1].Value.ToString();
                tb_alamatEdit.Text = dgv_dataKontak.Rows[id].Cells[2].Value.ToString();

                tb_namaEdit.Enabled = true;
                tb_alamatEdit.Enabled = true;
                btn_save.Enabled = true;
            }

            if (e.ColumnIndex == 4)
            {
                int id = Convert.ToInt32(dgv_dataKontak.CurrentCell.RowIndex.ToString());

                MySqlCommand cmd;
                connection.Open();

                try
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "DELETE FROM data_kontak WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", dgv_dataKontak.Rows[id].Cells[0].Value.ToString());
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    dgv_dataKontak.Columns.Clear();
                    dataTable.Clear();
                    fillDataKontak();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE data_kontak SET nama = @nama, telepon = @telepon WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", tb_id.Text);
                cmd.Parameters.AddWithValue("@nama", tb_namaEdit.Text);
                cmd.Parameters.AddWithValue("@telepon", tb_alamatEdit.Text);
                cmd.ExecuteNonQuery();
                connection.Close();
                dgv_dataKontak.Columns.Clear();
                dataTable.Clear();
                fillDataKontak();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
