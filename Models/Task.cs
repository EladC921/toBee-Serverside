using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace toBee_Serverside.Models
{
    public class Task
    {
        int tid;
        string title;
        string txt;
        bool completed;
        DateTime createdAt;
        DateTime dueDate;
        User creator;
        List<User> regTo;
        int gid;
        string gName;

        public int Tid { get => tid; set => tid = value; }
        public string Title { get => title; set => title = value; }
        public string Txt { get => txt; set => txt = value; }
        public bool Completed { get => completed; set => completed = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public User Creator { get => creator; set => creator = value; }
        public List<User> RegTo { get => regTo; set => regTo = value; }
        public int Gid { get => gid; set => gid = value; }
        public string GName { get => gName; set => gName = value; }
   
        public List<Task> GetTasksOfGroup(int gid)
        {
            DBServices ds = new DBServices();
            return ds.GetTasksOfGroup(gid);
        } 

        public List<Task> GetProfileTasksOfUser(int uid)
        {
            DBServices ds = new DBServices();
            return ds.GetProfileTasksOfUser(uid);
        } 
        
        public List<Task> GetAvailableTasksInAllGroups(int uid)
        {
            DBServices ds = new DBServices();
            return ds.GetAvailableTasksInAllGroups(uid);
        }   
        
        public List<Task> GetTasksOfRegUserInAllGroups(int uid)
        {
            DBServices ds = new DBServices();
            return ds.GetTasksOfRegUserInAllGroups(uid);
        }

        public List<Task> PostTask()
        {
            DBServices ds = new DBServices();
            return ds.PostTask(this);
        }

        public List<Task> AssignUserToTaskInGroup(int gid, int uid, int tid)
        {
            DBServices ds = new DBServices();
            return ds.AssignUserToTaskInGroup(gid, uid, tid);
        }

        public List<Task> CompleteTask(int gid, int uid, int tid)
        {
            DBServices ds = new DBServices();
            return ds.CompleteTask(gid, uid, tid);
        }
    }
}