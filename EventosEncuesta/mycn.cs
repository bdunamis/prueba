using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace EventosEncuesta
{
    class mycn
    {

        public static MySqlConnection mycon(String nombrebd)
        //public static MySqlConnection ObtenerConexion(String server, String db, String sa, String pw)
        {

            //MySqlConnection conectar = new MySqlConnection("server=" + server + ";port=3306;database= " + db + ";UID=" + sa + ";pwd=" + pw + ";");
            //MySqlConnection conectar = new MySqlConnection("server=192.168.100.224;port=3306;database=cebollines_banquetescrm;UID=root;pwd=cebollines;");
            //SqlConnection Conn = new SqlConnection(@" Data Source = " + serverbd + ";Initial catalog = " + namebd + "; User Id=" + userbd + "; Password=" + passwordbd + ";");
            String conexion;
            conexion = "server=190.106.215.173;database=" + nombrebd + ";uid=root;pwd=admindb@1234";
            MySqlConnection conectar = new MySqlConnection(conexion);

            conectar.Open();
            return conectar;
        }



        public static MySqlConnection mycon2(String nombrebd)
        //public static MySqlConnection ObtenerConexion(String server, String db, String sa, String pw)
        {

            //MySqlConnection conectar = new MySqlConnection("server=" + server + ";port=3306;database= " + db + ";UID=" + sa + ";pwd=" + pw + ";");
            //MySqlConnection conectar = new MySqlConnection("server=192.168.100.224;port=3306;database=cebollines_banquetescrm;UID=root;pwd=cebollines;");
            //SqlConnection Conn = new SqlConnection(@" Data Source = " + serverbd + ";Initial catalog = " + namebd + "; User Id=" + userbd + "; Password=" + passwordbd + ";");
            String conexion;
            conexion = "server=www.portalcebollines.com;database=vpscebol_encuesta;uid=vpscebol_encuest;pwd=lp0417162";
            MySqlConnection conectar = new MySqlConnection(conexion);

            conectar.Open();
            return conectar;
        }
    }
}
