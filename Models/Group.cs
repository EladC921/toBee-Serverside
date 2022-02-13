﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace toBee_Serverside.Models
{
    public class Group
    {
        int gid;
        string name;
        string description;
        string imgURL;
        int acceptedRequestUserId;
        List<User> members;

        public int Gid { get => gid; set => gid = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string ImgURL { get => imgURL; set => imgURL = value; }
        public int AcceptedRequestUserId { get => acceptedRequestUserId; set => acceptedRequestUserId = value; }
        public List<User> Members { get => members; set => members = value; }

        public Group PostGroup()
        {
            DBServices ds = new DBServices();
            return ds.PostGroup(this);
        }

        public Group GetGroup(int gid)
        {
            DBServices ds = new DBServices();
            return ds.GetGroup(gid);
        }

        public int PostUserInGroup(int gid, int uid)
        {
            DBServices ds = new DBServices();
            return ds.PostUserInGroup(gid, uid);
        }

    }
}