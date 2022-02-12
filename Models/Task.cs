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

        public int Tid { get => tid; set => tid = value; }
        public string Title { get => title; set => title = value; }
        public string Txt { get => txt; set => txt = value; }
        public bool Completed { get => completed; set => completed = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public User Creator { get => creator; set => creator = value; }

        public List<User> RegTo { get => regTo; set => regTo = value; }
    }
}