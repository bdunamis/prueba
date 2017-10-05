using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EventosEncuesta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            generar();
        }

        private void generar()
        {
            try
            {
                String cq = @" SELECT r.correlativo_contrato as Contrato, r.personas as TotalPersonas, r.tipo as Tiempo,
                                r.hora_servicio  as hora, r.inicio_reserva_catering as Fecha,  
                                c.first_name as Primer_Nombre, ifnull(c.last_name,'') as Segundo_Nombre, c.phone_work as Telefono, ea.email_address as Email,
                                cs.description as Descripcion, cs.name as Salon, cs.restaurante as Restaurante,  ce.name as Ejecutivo, cta.actividad as Tipo_Evento
                                FROM  
                                cebollines_banquetescrm.email_addresses ea, cebollines_banquetescrm.email_addr_bean_rel br, 
                                 cebollines_banquetescrm.contacts c, contacts_ceeservaciones_c cr, cebollines_banquetescrm.ceb_reservaciones r, 
                                 ceb_saloneseservaciones_c csr, ceb_salones cs, ceb_ejecutivo  ce, ceb_tipoactividad cta
                                WHERE ea.id = br.email_address_id
                                AND br.bean_id = c.id
                                AND cr.contacts_ceescontacts_ida = c.id
                                AND cr.contacts_ceervaciones_idb = r.id
                                AND csr.ceb_saloneservaciones_idb = r.id  
                                AND csr.ceb_saloneseb_salones_ida = cs.id 
                                AND r.assigned_user_id = ce.assigned_user_id
                                AND r.actividad = cta.id
                                AND r.status='confirmado' 
                                AND ea.deleted = 0 AND br.deleted = 0 AND c.deleted = 0 AND cr.deleted = 0 AND r.deleted = 0 AND ce.deleted= 0
                                AND r.correlativo_contrato <>'' AND ea.email_address <> ''
                                AND DATE_FORMAT(ADDTIME(r.inicio_reserva_catering,'06:00'), '%Y-%m-%d') BETWEEN "; 
                cq = cq + "'" + regresarfecha(dateTimePicker1.Text)+"' AND '" + regresarfecha(dateTimePicker2.Text) +"'";


                MySqlConnection pconn1 = mycn.mycon("cebollines_banquetescrm");
                DataTable table = new DataTable();
                MySqlDataAdapter daeventos = new MySqlDataAdapter(cq, pconn1);
                daeventos.SelectCommand.CommandTimeout = 0;
                daeventos.Fill(table);
                DataSet dsSeries = new DataSet();
                daeventos.Fill(dsSeries, "documentos");
                dgvdetalle.DataSource = dsSeries;
                dgvdetalle.DataMember = "documentos";

            }
            catch (Exception Exepcion)
            {

                MessageBox.Show("Se ha producido un error al Generar el reporte :  \n\n" + Exepcion.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        static string regresarfecha(String fecha)
        {
            String fecha2;
            if (fecha.Length == 10)
            {
                fecha2 = fecha.Substring(6, 4) + '-' + fecha.Substring(3, 2) + '-' + fecha.Substring(0, 2);
            }
            else
            {
                fecha2 = fecha.Substring(5, 4) + '-' + fecha.Substring(2, 2) + "-0" + fecha.Substring(0, 1);
            }
            
            return fecha2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            trasladarinfo();
        }

        private void trasladarinfo()
        {          
            MySqlConnection pconn1 = mycn.mycon2("vpscebol_encuesta");
            String cq1 = @"insert into lime_tokens_718755 
                            (firstname, lastname, email, emailstatus, language, sent, remindersent,remindercount, completed, usesleft,
                            attribute_1, attribute_4, attribute_6, attribute_7, attribute_8, attribute_9, attribute_10, attribute_11, attribute_12 )
                            values (@primernombre, @segundonombre, @email, 'OK', 'es', 'N', 'N',  0, 'N', 1, 
                             @contrato, @tel, @ejecutiva, @tiempo, @tipoevento, @res, @salon, @hora, @fecha) ";
            MySqlCommand objcomandosql = new MySqlCommand(cq1, pconn1);
            foreach (DataGridViewRow row in dgvdetalle.Rows)
            {

                String fecha = row.Cells["Fecha"].Value.ToString();
                fecha = fecha.Substring(0, 10);
                String contrato = (row.Cells["Contrato"].Value.ToString());
                if (validarcontrato(contrato) == false)
                {
                    String contrato2=  row.Cells["Contrato"].Value.ToString();
                    objcomandosql.Parameters.Clear();

                    objcomandosql.Parameters.AddWithValue("@primernombre", row.Cells["Primer_Nombre"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@segundonombre", row.Cells["Segundo_Nombre"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@email", row.Cells["Email"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@contrato", row.Cells["Contrato"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@tel", row.Cells["Telefono"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@ejecutiva", row.Cells["Ejecutivo"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@tiempo", row.Cells["Tiempo"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@tipoevento", row.Cells["Tipo_Evento"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@salon", row.Cells["Salon"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@res", row.Cells["Restaurante"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@hora", row.Cells["hora"].Value.ToString());
                    objcomandosql.Parameters.AddWithValue("@fecha", fecha);
                    objcomandosql.ExecuteNonQuery();


                    MySqlConnection p = mycn.mycon("cebollines_banquetescrm");
                    String query = @"insert into ceb_envenc 
                            (contrato, enviado)
                            values (" ;
                    query = query + contrato2 + ", 1) ";
                    MySqlCommand cmdmysql = new MySqlCommand(query, p);
                    cmdmysql.ExecuteNonQuery();
                }
                else
                {

                }

            }

            MessageBox.Show("Traslado finalizado");
        }


        public static int revisar_caracter(string correo)
        {

            // Split the string on line breaks.
            // ... The return value from Split is a string array.
            string[] lines = Regex.Split(correo, "@");

            if (lines.Length > 3)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }


        private static bool validarcontrato(String contrato)
        {
            using (MySqlConnection conn = mycn.mycon("cebollines_banquetescrm"))
            {
                String query = " select * from ceb_envenc where contrato = @contrato ";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("contrato", contrato);
               

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0){
                    return false;
                }

                else
                {
                    return true;
                }
                    
            } 

        }

        private void button2_Click(object sender, EventArgs e)
        {
            trasladarinfo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    } 
}
