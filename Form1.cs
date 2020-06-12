using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace Termodinamica
{
    public partial class Compresion : Form
    {
        private FileStream fs;

        private double k=0.1;
        private double Vol = 0.05, Pre=0.1, pf,vf,pi=3,vi;
        private Queue<double> Presiones = new Queue<double>();
        private Queue<double> Volumenes = new Queue<double>();
        public Compresion()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double masa, div;
                double Pabs1, Pabs2;
                double v1, v2;
                for (int j = 0; j < int.Parse(txtbIt.Text); j++)
                {
                    div = 1;
                    for (int k = 0; k < j; k++)
                    {
                        div = div * 2;
                    }
                    masa = 1000 / div;
                    Pabs1 = 1.98;
                    v1 = 0.05;
                    dgvDatos.Rows.Add(j + 1, "", "", "", "", "", "");
                    Presiones.Enqueue(Pabs1);
                    Volumenes.Enqueue(v1);
                    for (double i = div; i > 0; i--)
                    {
                        Pabs2 = (masa * (i - 1) * 9.8 / 10000) + 1;
                        v2 = Pabs1 * v1 / Pabs2;
                        dgvDatos.Rows.Add("", masa * (i), masa * (i - 1), Pabs1, Pabs2, "Expansion", v1, v2);

                        v1 = v2;
                        Pabs1 = Pabs2;
                        Presiones.Enqueue(Pabs1);
                        Volumenes.Enqueue(v1);
                    }
                    for (int i = 0; i <= div - 1; i++)
                    {
                        Pabs2 = masa * (i + 1) * 9.8 / 10000 + 1;
                        v2 = Pabs1 * v1 / Pabs2;
                        dgvDatos.Rows.Add("", masa * (i), masa * (i + 1), Pabs1, Pabs2, "Compresion", v1, v2);
                        Presiones.Enqueue(Pabs1);
                        Volumenes.Enqueue(v1);
                        v1 = v2;
                        Pabs1 = Pabs2;
                        Presiones.Enqueue(Pabs1);
                        Volumenes.Enqueue(v1);
                    }
                    
                }
                txtbIt.Text = "";
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            dgvDatos.Rows.Clear();
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            txtbIt.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {

            StreamWriter sw = null;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo Excel (.csv)|*.csv";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fs = new FileStream(saveFileDialog.FileName, FileMode.CreateNew, FileAccess.Write);
                    sw = new StreamWriter(fs);
                    sw.WriteLine("Ciclo", "mi", "mf", "Pabsi", "Pabsf", "Proceso", "Vi", "Vf");
                    for (int i = 0; i < dgvDatos.Rows.Count; i++)
                    {
                        sw.WriteLine(dgvDatos.Rows[i].Cells[0].Value + "," + dgvDatos.Rows[i].Cells[1].Value + "," + dgvDatos.Rows[i].Cells[2].Value + "," + dgvDatos.Rows[i].Cells[3].Value + "," + dgvDatos.Rows[i].Cells[4].Value + "," + dgvDatos.Rows[i].Cells[5].Value + "," + dgvDatos.Rows[i].Cells[6].Value + "," + dgvDatos.Rows[i].Cells[7].Value);
                    }

                }
                catch (IOException error)
                {
                    MessageBox.Show("Error: " + error.Message);
                }
                finally
                {
                    sw.Close();
                    fs.Close();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (chart1.Visible == false)
            {
                label1.Visible = false;
                label2.Visible = true;
                Calcular.Visible = false;
                chart1.Visible = true;
                dgvDatos.Visible = false;
                button3.Text = "Tabla";
                button4.Visible = true;
                button2.Visible = false;
                button5.Visible = true;
                button6.Visible = true;
                txtbIt.Visible = false;
            }
            else
            {
                label2.Visible = false;
                label1.Visible = true;
                Calcular.Visible = true;
                txtbIt.Visible = true;
                chart1.Visible = false;
                dgvDatos.Visible = true;
                button3.Text = "Grafica";
                button4.Visible = false;
                button5.Visible = false;
                button6.Visible = false;
                button2.Visible = true;
            }

        }
      
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Pre = k / Vol;
            chart1.Series[0].Points.AddXY(Vol, Pre);
            Vol = Vol + 0.0001;
            if (Vol >= 0.1001 || Vol <= 0.045)
            {
                timer2.Start();
                timer1.Stop();
            }
                       
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("*Para ver un ciclo gráficamente es necesario introducir el número de ciclos en la hoja de cálculo."+"\n*Un gran número de ciclos podría tardar bastante, o en su defecto, trabar el programa \n*El botón 'Guardar' almacena los datos en una hoja de Excel \n*El botón 'Limpiar' borra los datos de la tabla y de la gráfica \n*El botón 'Detener' no puede retomar la el proceso del ciclo, pero si el 'infinito'\n\n\t\tContacto: astridvergara14@icloud.com");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("EL propósito de este programa es resolver analítica y gráficamente: \n\n1 kg de aire se encuentra en un sistema isotérmico émbolo-pistón, inicialmente hay una masa de 1 kg, se retira y el aire se expande; se recoloca la masa provocando la compresión.\nDespués el número de bloques se duplican manteniendo la masa de 1 kg, se retiran y recolocan una a una\nAl ser un proceso isotérmico de expansión-compresión, es reversible.");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Imagen imagen = new Imagen();
            imagen.Show();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           timer1.Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            timer3.Start();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (Volumenes.Count != 0)
            {
                vf = Volumenes.Dequeue();
                pf = Presiones.Dequeue();
                if (pi > pf)
                {
                    chart1.Series[0].Points.AddXY(vf, pf);
                }
                else
                {
                 
                        chart1.Series[1].Points.AddXY(vf, pf);
                    
                }
                pi = pf;
                vi = vf;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            Pre = k / Vol;
            chart1.Series[1].Points.AddXY(Vol, Pre);
            Vol = Vol - 0.00001;
            if (Vol >= 0.1001 || Vol <= 0.049)
            {
                timer2.Stop();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            Volumenes.Clear();
            Presiones.Clear();
        }
    }
}