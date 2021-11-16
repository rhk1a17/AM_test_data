using System;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using AM_test_data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AM_test_data.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ConnectSQL("1");
            return View();
        }

        public IActionResult Lap_2()
        {
            ConnectSQL("2");
            return View();
        }

        public IActionResult Lap_3()
        {
            ConnectSQL("3");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<SqlData> ConnectSQL(string lap_number_input)
        {
            SqlConnectionStringBuilder sql = new SqlConnectionStringBuilder();

            // new list
            List<SqlData> dataList = new List<SqlData>();

            //SQL query to retrieve latest row from sql table
            string retrieve = String.Format("SELECT * FROM track_data_imported WHERE LapNumber = {0};",lap_number_input);

            // SQL login data
            sql.DataSource = "aston-martin-test.database.windows.net";   // Server name from azure
            sql.UserID = "rhk1a17"; // ID to access DB
            sql.Password = "Noobdotanoob1@#";   //password to access DB
            sql.InitialCatalog = "AM_TestData";  //Database name

            using (SqlConnection sqlConn = new SqlConnection(sql.ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(retrieve, sqlConn);
                try
                {
                    sqlConn.Open();
                    sqlCommand.ExecuteNonQuery();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dataList.Add(new SqlData()
                            {
                                index = reader.GetInt32(0),
                                lap_number = reader.GetInt32(1),
                                lap_dist = reader.GetDouble(2),
                                speed = reader.GetDouble(3),
                                lat_accel = reader.GetDouble(4),
                                lon_accel = reader.GetDouble(5),
                                FRH = reader.GetDouble(6),  
                                RRH = reader.GetDouble(7),
                                yaw = reader.GetDouble(8),  
                                steer = reader.GetDouble(9),
                                roll = reader.GetDouble(10),
                                flap_angle = reader.GetDouble(11),
                                break_force = reader.GetDouble(12),
                                throt_padel = reader.GetDouble(13),
                                CzF = reader.GetDouble(14),
                                CzR = reader.GetDouble(15),
                                Cx = reader.GetDouble(16),
                                corner_phase = reader.GetValue(17),
                                GPS_lat = reader.GetDouble(18),
                                GPS_lon = reader.GetDouble(19),
                                
                            }); ;
                        }

                    }
                }

                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                }
                sqlConn.Close();
            }

            List<DataPoint> dataPoints = new List<DataPoint>();
            foreach (SqlData element in dataList)
            {
                dataPoints.Add(new DataPoint(element.lap_dist, element.speed));
            }
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return dataList; 
        }
        public string testing_front_df()
        {
            var sqldata = ConnectSQL("1");
            var result = string.Empty;
            foreach (SqlData element in sqldata)
            {
                result = element.CzF.ToString() + " front downforce";
            }
            return result;
        }

        public class SqlData
        {
            public int index
            {
                get;
                set;
            }

            public int lap_number
            {
                get;
                set;
            }

            public double lap_dist
            {
                get;
                set;
            }

            public double speed
            {
                get;
                set;
            }

            public double lat_accel
            {
                get;
                set;
            }

            public double lon_accel
            {
                get;
                set;
            }

            public double FRH 
            {
                get;
                set;
            }

            public double RRH
            {
                get;
                set;
            }

            public double yaw
            {
                get;
                set;
            }

            public double steer
            {
                get;
                set;
            }

            public double roll
            {
                get;
                set;
            }

            public double flap_angle
            {
                get;
                set;
            }

            public double break_force
            {
                get;
                set;
            }

            public double throt_padel
            {
                get;
                set;
            }

            public double CzF
            {
                get;
                set;
            }

            public double CzR
            {
                get;
                set;
            }

            public double Cx
            {
                get;
                set;
            }

            public object corner_phase
            {
                get;
                set;
            }

            public double GPS_lat
            {
                get;
                set;
            }

            public double GPS_lon
            {
                get;
                set;
            }
        }
    }
}