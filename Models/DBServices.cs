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

        // GET single user by email
        public User GetUserByEmail(string mail)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "SELECT * FROM Users_2022 U WHERE U.mail LIKE " + mail; // SELECT query 

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

        // Edit User Profile
        public User EditUserProfile(User u)
        {
            string strCommand = "UPDATE Users_2022 SET firstName = '" + u.FirstName + "' , lastName = '" + u.LastName + "' WHERE uid = " + u.Uid;
            ExecuteSqlCommand(strCommand);
            return GetUser(u.Uid);
        }
          
        
        // Edit User Profile Image
        public int EditUserProfilePic(User u)
        {
            string strCommand = "UPDATE Users_2022 SET imgURL = '" + u.ImgURL + "' WHERE uid = " + u.Uid;
            return ExecuteSqlCommand(strCommand);
        }


        // ~~~ Groups Handling ~~~

        // GET single Group and its members 
        public Group GetGroup(int gid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "SELECT * FROM Groups_2022 G left join User_In_Group_2022 UIG on G.gid = UIG.gid left join Users_2022 U on UIG.uid = U.uid WHERE G.gid = " + gid; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                bool firstRow = true;
                Group g = new Group();
                while (dr.Read())
                {   // Read till the end of the data into a row
                    if (firstRow)
                    {
                        g.Gid = Convert.ToInt32(dr["gid"]);
                        g.Name = (string)dr["name"];
                        g.Description = (string)dr["description"];
                        g.ImgURL = (string)dr["imgURL"];
                        g.Members = new List<User>();

                        firstRow = false;
                    }

                    if (!(dr["uid"] is DBNull))
                    {
                        User u = new User();

                        u.Uid = Convert.ToInt32(dr["uid"]);
                        u.Nickname = (string)dr["nickname"];
                        u.FirstName = (string)dr["firstName"];
                        u.LastName = (string)dr["lastName"];
                        u.ImgURL = (string)dr["imgURL"];

                        g.Members.Add(u);
                    }
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

            string strCommand2 = "INSERT INTO User_In_Group_2022([gid], [uid]) VALUES('" + gid + "','" + g.CreatorId + "'); ";
            ExecuteSqlCommand(strCommand2);
            return GetGroup(gid);
        }

        // POST User in Group
        public Group PostUserInGroup(int gid, string nickname)
        {
            SqlConnection con = null;
            int uid = -1;

            //Insert the Group and get its ID
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string strCommand = "SELECT uid FROM Users_2022 WHERE nickname LIKE '" + nickname + "'";

                SqlCommand cmd = new SqlCommand(strCommand, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    uid = Convert.ToInt32(dr["uid"]);
                    break;
                }
                if (uid == -1) throw new Exception("Data was not read properly from SQL Or there is no such User"); //if ID of user is -1 it means something went wrong

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

            string strCommand2 = "INSERT INTO User_In_Group_2022([gid], [uid]) VALUES('" + gid + "', '" + uid + "'); ";
            ExecuteSqlCommand(strCommand2);
            return GetGroup(gid);
        }

        // GET Groups of one User
        public List<Group> GetGroupsOfUser(int uid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "SELECT G.* FROM User_In_Group_2022 UIG inner join Groups_2022 G on UIG.gid = G.gid WHERE UIG.uid = " + uid; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                List<Group> groups = new List<Group>();
                while (dr.Read())
                {
                    // Read till the end of the data into a row
                    Group g = new Group();
                    g.Gid = Convert.ToInt32(dr["gid"]);
                    g.Name = (string)dr["name"];
                    g.Description = (string)dr["description"];
                    g.ImgURL = (string)dr["imgURL"];

                    groups.Add(g);
                }
                return groups;

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


        // DEL User from Group
        public int DeleteUserFromGroup(int gid, int uid)
        {
            string strCommand = "DELETE FROM User_In_Group_2022 WHERE gid = " + gid + " AND uid = " + uid + " ";
            strCommand += "DELETE FROM User_Assigned_To_Task_2022 WHERE tid IN(SELECT tid FROM Group_Task_2022 WHERE gid = " + gid + ") AND uid = " + uid;
            return ExecuteSqlCommand(strCommand);
        }


        // ~~~ Tasks Handling ~~~

        // GET Tasks Of Group
        public List<Task> GetTasksOfGroup(int gid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "SELECT GT.gid, UAT.uid as 'regTo', UCT.uid as 'createdBy', Ucreated.firstName as 'createdBy FirstName', Ucreated.lastName as 'createdBy LastName' ,Uassigned.firstName as 'regTo FirstName', Uassigned.lastName as 'regTo LastName',T.* " +
                                    "FROM Group_Task_2022 GT inner join Groups_2022 G on G.gid = GT.gid inner join Tasks_2022 T on T.tid = GT.tid inner join User_Created_Task_2022 UCT on UCT.tid = T.tid " +
                                        "left join User_Assigned_To_Task_2022 UAT on UAT.tid = T.tid inner join Users_2022 Ucreated on Ucreated.uid = UCT.uid left join Users_2022 Uassigned on Uassigned.uid = UAT.uid " +
                                    "WHERE G.gid = " + gid;  // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                List<Task> tasks = new List<Task>();
                Task t = new Task();
                User u = new User();
                int currentId = -1;
                int newId = 0;
                var loop = true;

                while (loop)
                {
                    loop = dr.Read();
                    if (!loop) // if the reader has finished
                    {
                        if (t.Tid != 0) tasks.Add(t); // add the last task that had been read to the tasks list
                        break;
                    }

                    newId = Convert.ToInt32(dr["tid"]); // get the new task id 

                    if (currentId != newId) // if new task id is different then we moved on to the next task (or first line)
                    {
                        if (currentId != -1) tasks.Add(t);

                        t = new Task();
                        t.Tid = Convert.ToInt32(dr["tid"]);
                        currentId = t.Tid;

                        t.Title = (string)dr["title"];
                        t.Txt = (string)dr["txt"];
                        t.Completed = (Convert.ToInt32(dr["completed"]) == 0) ? false : true;
                        t.CreatedAt = Convert.ToDateTime(dr["createdAt"]);
                        t.DueDate = Convert.ToDateTime(dr["dueDate"]);
                        t.RegTo = new List<User>();

                        t.Creator = new User();
                        t.Creator.Uid = Convert.ToInt32(dr["createdBy"]);
                        t.Creator.FirstName = (string)dr["createdBy FirstName"];
                        t.Creator.LastName = (string)dr["createdBy LastName"];
                    }

                    if (!(dr["regTo"] is DBNull))
                    {
                        u = new User();
                        u.Uid = Convert.ToInt32(dr["regTo"]);
                        u.FirstName = (string)dr["regTo FirstName"];
                        u.LastName = (string)dr["regTo LastName"];
                        t.RegTo.Add(u);
                    }
                }
                return tasks;

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

        // GET Tasks in which User is assigned to In All of the Groups
        public List<Task> GetTasksOfRegUserInAllGroups(int uid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "DECLARE @uid AS INT " +
                                    "SET @uid = " + uid + " " +
                                    "SELECT GT.gid, G.name as 'Group Name' ,UAT.uid as 'regTo', UCT.uid as 'createdBy', Ucreated.firstName as 'createdBy FirstName', Ucreated.lastName as 'createdBy LastName' ,Uassigned.firstName as 'regTo FirstName', Uassigned.lastName as 'regTo LastName',T.* " +
                                    "FROM User_Assigned_To_Task_2022 UAT inner join Tasks_2022 T on UAT.tid = T.tid inner join User_Created_Task_2022 UCT on UCT.tid = T.tid " +
                                        "inner join Users_2022 Uassigned on Uassigned.uid = UAT.uid inner join Users_2022 Ucreated on Ucreated.uid = UCT.uid inner join Group_Task_2022 GT on GT.tid = T.tid inner join Groups_2022 G on GT.gid = G.gid " +
                                    "WHERE UAT.uid = @uid"; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                List<Task> tasks = new List<Task>();
                Task t = new Task();
                User u = new User();
                int currentId = -1;
                int newId = 0;
                var loop = true;

                while (loop)
                {
                    loop = dr.Read();
                    if (!loop) // if the reader has finished
                    {
                        if (t.Tid != 0) tasks.Add(t); // add the last task that had been read to the tasks list
                        break;
                    }

                    newId = Convert.ToInt32(dr["tid"]); // get the new task id 

                    if (currentId != newId) // if new task id is different then we moved on to the next task (or first line)
                    {
                        if (currentId != -1) tasks.Add(t);

                        t = new Task();
                        t.Tid = Convert.ToInt32(dr["tid"]);
                        currentId = t.Tid;

                        t.Title = (string)dr["title"];
                        t.Txt = (string)dr["txt"];
                        t.Completed = (Convert.ToInt32(dr["completed"]) == 0) ? false : true;
                        t.CreatedAt = Convert.ToDateTime(dr["createdAt"]);
                        t.DueDate = Convert.ToDateTime(dr["dueDate"]);
                        t.RegTo = new List<User>();

                        t.Creator = new User();
                        t.Creator.Uid = Convert.ToInt32(dr["createdBy"]);
                        t.Creator.FirstName = (string)dr["createdBy FirstName"];
                        t.Creator.LastName = (string)dr["createdBy LastName"];

                        t.Gid = Convert.ToInt32(dr["gid"]);
                        t.GName = (string)dr["Group Name"];
                    }

                    u = new User();
                    u.Uid = Convert.ToInt32(dr["regTo"]);
                    u.FirstName = (string)dr["regTo FirstName"];
                    u.LastName = (string)dr["regTo LastName"];
                    t.RegTo.Add(u);
                }
                return tasks;

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

        // GET Tasks That are not assigned to anyone
        public List<Task> GetAvailableTasksInAllGroups(int uid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "DECLARE @uid AS INT " +
                                    "SET @uid = " + uid + " " +
                                    "SELECT GT.gid, G.name as 'Group Name' ,UAT.uid as 'regTo', UCT.uid as 'createdBy', Ucreated.firstName as 'createdBy FirstName', Ucreated.lastName as 'createdBy LastName', T.* " +
                                    "FROM Tasks_2022 T left join User_Assigned_To_Task_2022 UAT on T.tid = UAT.tid left join User_Created_Task_2022 UCT on UCT.tid = T.tid left join Group_Task_2022 GT on T.tid = GT.tid left join Users_2022 Ucreated on Ucreated.uid = UCT.uid " +
                                        "left join User_In_Group_2022 UIG on GT.gid = UIG.gid left join Groups_2022 G on G.gid = GT.gid " +
                                    "WHERE UIG.uid = @uid and UAT.uid is NULL "; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                List<Task> tasks = new List<Task>();

                while (dr.Read())
                {

                    Task t = new Task();

                    t.Tid = Convert.ToInt32(dr["tid"]);

                    t.Title = (string)dr["title"];
                    t.Txt = (string)dr["txt"];
                    t.Completed = (Convert.ToInt32(dr["completed"]) == 0) ? false : true;
                    t.CreatedAt = Convert.ToDateTime(dr["createdAt"]);
                    t.DueDate = Convert.ToDateTime(dr["dueDate"]);

                    t.Creator = new User();
                    t.Creator.Uid = Convert.ToInt32(dr["createdBy"]);
                    t.Creator.FirstName = (string)dr["createdBy FirstName"];
                    t.Creator.LastName = (string)dr["createdBy LastName"];

                    t.Gid = Convert.ToInt32(dr["gid"]);
                    t.GName = (string)dr["Group Name"];

                    tasks.Add(t);
                }
                return tasks;

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

        // GET Tasks Of User In Groups
        public List<Task> GetProfileTasksOfUser(int uid)
        {
            SqlConnection con = null;

            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                string selectSTR = "DECLARE @uid AS INT " +
                                    "SET @uid = " + uid + " " +
                                    "SELECT T.* " +
                                    "FROM Tasks_2022 T inner join User_Created_Task_2022 UCT on UCT.tid = T.tid inner join Group_Task_2022 GT on T.tid = GT.tid inner join Users_2022 Ucreated on Ucreated.uid = UCT.uid " +
                                    "WHERE GT.gid = -1 and UCT.uid = @uid "; // SELECT query 

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection); // CommandBehavior.CloseConnection: the connection will be closed after reading has reached the end

                List<Task> tasks = new List<Task>();

                while (dr.Read())
                {

                    Task t = new Task();

                    t.Tid = Convert.ToInt32(dr["tid"]);

                    t.Title = (string)dr["title"];
                    t.Txt = (string)dr["txt"];
                    t.Completed = (Convert.ToInt32(dr["completed"]) == 0) ? false : true;
                    t.CreatedAt = Convert.ToDateTime(dr["createdAt"]);
                    t.DueDate = Convert.ToDateTime(dr["dueDate"]);

                    tasks.Add(t);
                }
                return tasks;

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

        // POST Task
        public List<Task> PostTask(Task t)
        {
            SqlConnection con = null;
            int tid = -1;

            //Insert the Group and get its ID
            try
            {
                con = connect("DBConnectionString"); // create a connection to the database using the connection String defined in the web config file

                var dueDate = t.DueDate.Date.ToString("yyyy-MM-dd HH:mm:ss");
                string title = t.Title.Replace("\'", "\'\'");
                string txt = t.Txt.Replace("\'", "\'\'");
                string strCommand = "INSERT INTO Tasks_2022([title], [txt], [dueDate]) OUTPUT(Inserted.tid) VALUES('" + title + "', '" + txt + "', '" + dueDate + "') ";

                SqlCommand cmd = new SqlCommand(strCommand, con);

                // get a reader
                SqlDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    tid = Convert.ToInt32(dr["tid"]);
                }
                if (tid == -1) throw new Exception("Data was not read properly from SQL"); //if ID of recipe is -1 it means something went wrong
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

            string strCommand2 = "INSERT INTO Group_Task_2022 VALUES('" + t.Gid + "', '" + tid + "') ";
            strCommand2 += "INSERT INTO User_Created_Task_2022 VALUES ('" + t.Creator.Uid + "', '" + tid + "'); ";
            ExecuteSqlCommand(strCommand2);

            return (t.Gid == -1) ? GetProfileTasksOfUser(t.Creator.Uid) : GetTasksOfGroup(t.Gid);
        }

        // Assign User to a Task
        public List<Task> AssignUserToTaskInGroup(int gid, int uid, int tid)
        {
            string strCommand = "INSERT INTO User_Assigned_To_Task_2022([uid], [tid]) VALUES('" + uid + "', '" +  tid + "')";
            ExecuteSqlCommand(strCommand);
            return GetTasksOfGroup(gid);
        }

        // Complete a Task (send gid = -1 if Profile)
        public List<Task> CompleteTask(int gid, int uid, int tid)
        {
            string strCommand = "UPDATE Tasks_2022 SET completed = 1 WHERE tid = " + tid;
            ExecuteSqlCommand(strCommand);
            return (gid == -1) ? GetProfileTasksOfUser(uid) : GetTasksOfGroup(gid);
        }
    }
}