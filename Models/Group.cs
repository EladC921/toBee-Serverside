using System;
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
        List<User> members;
        int creatorId;

        public int Gid { get => gid; set => gid = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public string ImgURL { get => imgURL; set => imgURL = value; }
        public List<User> Members { get => members; set => members = value; }
        public int CreatorId { get => creatorId; set => creatorId = value; }

        public Group GetGroup(int gid)
        {
            DBServices ds = new DBServices();
            return ds.GetGroup(gid);
        }

        public List<Group> GetGroupsOfUser(int uid)
        {
            DBServices ds = new DBServices();
            return ds.GetGroupsOfUser(uid);
        }

        public Group PostGroup()
        {
            DBServices ds = new DBServices();
            return ds.PostGroup(this);
        }

        public Group PostUserInGroup(int gid, string nickname)
        {
            DBServices ds = new DBServices();
            return ds.PostUserInGroup(gid, nickname);
        }

        public int DeleteUserFromGroup(int gid, int uid)
        {
            DBServices ds = new DBServices();
            return ds.DeleteUserFromGroup(gid, uid);
        }
    }
}