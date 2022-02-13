using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace toBee_Serverside.Models
{
    public class DBServices
    {
        public SqlDataAdapter da;
        public System.Data.DataTable dt;

        // create the SQL connection
        public SqlConnection connect(String conString)
        {
            // read the connection string from the configuration file
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        // create SQL command line connection
        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

            return cmd;
        }

        // Execute the SQL Query 
        public int ExecuteSqlCommand(string commandStr)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("DBConnectionString"); // create the connection
            }

            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            cmd = CreateCommand(commandStr, con);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return 0;
                }
                else throw;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        // ~~~ Users Handling ~~~

        // GET single user
        public User GetUser(int uid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "SELECT * FROM Users_2022 U WHERE U.uid = " + uid; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                User u = new User();
                while (dr.Read())
                {   // Read till the end of the data into a row

                    u.Uid = Convert.ToInt32(dr["uid"]);
                    u.Nickname = (string)dr["nickname"];
                    u.FirstName = (string)dr["firstName"];
                    u.LastName = (string)dr["lastName"];
                    u.Mail = (string)dr["mail"];
                    u.PhoneNum = (string)dr["phoneNum"];
                    u.ImgURL = (string)dr["imgURL"];

                    break;
                }
                return u;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // POST User
        public int PostUser(User u)
        {
            string strCommand = "INSERT INTO Users_2022([nickname], [firstName] , [lastName], [mail], [phoneNum],  [imgURL] ) VALUES('" + u.Nickname + "', '" + u.FirstName + "', '" + u.LastName + "', '" + u.Mail + "', '" + u.PhoneNum + "', '" + u.ImgURL + "'); ";
            return ExecuteSqlCommand(strCommand);
        }
        // ~~~ Groups Handling ~~~

        // GET single Group
        public Group GetGroup(int gid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "SELECT * FROM Groups_2022 G WHERE G.gid = " + gid; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                Group g = new Group();
                while (dr.Read())
                {   // Read till the end of the data into a row
                    g.Gid = Convert.ToInt32(dr["gid"]);
                    g.Name = (string)dr["name"];
                    g.Description = (string)dr["description"];
                    g.ImgURL = (string)dr["imgURL"];

                    break;
                }
                return g;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        // POST Group
        public Group PostGroup(Group g)
        {
            SqlConnection con = null;
            int gid = -1;

            //Insert the Group and get its ID
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string description = g.Description.Replace("\'", "\'\'");
                string strCommand = "INSERT INTO Groups_2022([name], [description] , [imgURL] ) OUTPUT(Inserted.gid) VALUES('" + g.Name + "', '" + description + "', '" + g.ImgURL + "'); ";

                SqlCommand cmd = new SqlCommand(strCommand, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    gid = Convert.ToInt32(dr["gid"]);
                }
                if (gid == -1) throw new Exception("Data was not read properly from SQL"); //if ID of recipe is -1 it means something went wrong

            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return GetGroup(gid);
        }

        // POST User in Group
        public int PostUserInGroup(int gid , int uid)
        {
            string strCommand = "INSERT INTO User_In_Group_2022([gid], [uid]) VALUES('" + gid + "', '" + uid + "'); ";
            return ExecuteSqlCommand(strCommand);
        }

        // ~~~ Tasks Handling ~~~
    }
}