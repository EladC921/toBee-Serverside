using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace toBee_Serverside.Models
{
    public class User
    {
        int uid;
        string nickname;
        string firstName;
        string lastName;
        string mail;
        string phoneNum;
        string imgURL;

        public int Uid { get => uid; set => uid = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Mail { get => mail; set => mail = value; }
        public string PhoneNum { get => phoneNum; set => phoneNum = value; }
        public string ImgURL { get => imgURL; set => imgURL = value; }

        public int PostUser()
        {
            DBServices ds = new DBServices();
            return ds.PostUser(this);
        }

        public User GetUser(int uid)
        {
            DBServices ds = new DBServices();
            return ds.GetUser(uid);
        }
    }
}