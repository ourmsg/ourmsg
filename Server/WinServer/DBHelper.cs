using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;

using IMLibrary3.Organization;
using IMLibrary3.Protocol ;
using IMLibrary.SqlData;

namespace OurMsgServer
{
    class DBHelper
    {
        /// <summary>
        /// 资源管理器
        /// </summary>
        public static  ResourceManager resourceManager = new  ResourceManager("OurMsgServer.Properties.Settings",Assembly.GetExecutingAssembly());

        /// <summary>
        /// 应用程序配置器
        /// </summary>
        public static OurMsgServer.Properties.Settings settings = new OurMsgServer.Properties.Settings();

        #region 组织机构管理

        #region 创建新用户
        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="card"></param>
        public static int  CreateUserVcard(UserVcard card)
        {
            string sql = "insert into Users(userID,userName,GroupID,orderID,sex,Password,FaceIndex,CreateRooms,isAdmin,isSendSMS,isEditUserData,isSendNotice,isBroadcast,Vcard)"
                         + "values(@userID,@userName,@GroupID,@orderID,@sex,@Password,@FaceIndex,@CreateRooms,@isAdmin,@isSendSMS,@isEditUserData,@isSendNotice,@isBroadcast,@Vcard)";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@userID",card.UserID ),
                            new System.Data.SqlClient.SqlParameter("@userName",card.UserName ),
                            new System.Data.SqlClient.SqlParameter("@GroupID",card.GroupID ),
                            new System.Data.SqlClient.SqlParameter("@orderID",card.OrderID ),
                            new System.Data.SqlClient.SqlParameter("@sex", card.Sex),
                            new System.Data.SqlClient.SqlParameter("@Password", card.Password),
                            new System.Data.SqlClient.SqlParameter("@FaceIndex", card.FaceIndex),
                            new System.Data.SqlClient.SqlParameter("@CreateRooms", card.CreateRooms),
                            new System.Data.SqlClient.SqlParameter("@isAdmin", card.isAdmin),
                            new System.Data.SqlClient.SqlParameter("@isSendSMS", card.isSendSMS),
                            new System.Data.SqlClient.SqlParameter("@isEditUserData", card.isEditUserData),
                            new System.Data.SqlClient.SqlParameter("@isSendNotice", card.isSendNotice),
                            new System.Data.SqlClient.SqlParameter("@isBroadcast", card.isBroadcast),
                            new System.Data.SqlClient.SqlParameter("@Vcard",Factory.CreateXMLMsg(card)),
                          };
         return    IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #region 更新用户信息
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="card"></param>
        public static int  UpdateUserVcard(UserVcard card)
        {
            string sql = "update Users set userName=@userName,GroupID=@GroupID,orderID=@orderID,sex=@sex,Password=@Password,FaceIndex=@FaceIndex,"
            + "CreateRooms=@CreateRooms,isAdmin=@isAdmin,isSendSMS=@isSendSMS,isEditUserData=@isEditUserData,isSendNotice=@isSendNotice,isBroadcast=@isBroadcast,Vcard=@Vcard"
                       + " where userID=@userID";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@userID",card.UserID ),
                            new System.Data.SqlClient.SqlParameter("@userName",card.UserName ),
                            new System.Data.SqlClient.SqlParameter("@GroupID",card.GroupID ),
                            new System.Data.SqlClient.SqlParameter("@orderID",card.OrderID ),
                            new System.Data.SqlClient.SqlParameter("@sex", card.Sex),
                            new System.Data.SqlClient.SqlParameter("@Password", card.Password),
                            new System.Data.SqlClient.SqlParameter("@FaceIndex", card.FaceIndex),
                            new System.Data.SqlClient.SqlParameter("@CreateRooms", card.CreateRooms),
                            new System.Data.SqlClient.SqlParameter("@isAdmin", card.isAdmin),
                            new System.Data.SqlClient.SqlParameter("@isSendSMS", card.isSendSMS),
                            new System.Data.SqlClient.SqlParameter("@isEditUserData", card.isEditUserData),
                            new System.Data.SqlClient.SqlParameter("@isSendNotice", card.isSendNotice),
                            new System.Data.SqlClient.SqlParameter("@isBroadcast", card.isBroadcast),
                            new System.Data.SqlClient.SqlParameter("@Vcard",Factory.CreateXMLMsg(card)),
                          };
           return  IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #region 删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userID"></param>
        public static int  DelUser(string userID)
        {
            string sql = "delete Users where userID=@userID";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@userID", userID), };
          return   IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #region 创建分组
        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="group"></param>
        public static int  CreateGroupVcard(GroupVcard group)
        {
            string sql = "insert into Groups(GroupID,GroupName,SuperiorId,orderID)"
                         + "values(@GroupID,@GroupName,@SuperiorId,@orderID)";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@GroupID",group.GroupID ),
                            new System.Data.SqlClient.SqlParameter("@GroupName",group.GroupName ),
                            new System.Data.SqlClient.SqlParameter("@SuperiorId",group.SuperiorID  ),
                            new System.Data.SqlClient.SqlParameter("@orderID",group.OrderID ),
                          };
           return  IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #region 更新分组信息
        /// <summary>
        /// 更新分组信息
        /// </summary>
        /// <param name="group"></param>
        public static int UpdateGroupVcard(GroupVcard group)
        {
            string sql = "update Groups set GroupName=@GroupName,SuperiorId=@SuperiorId,orderID=@orderID where GroupID=@GroupID";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@GroupID",group.GroupID ),
                            new System.Data.SqlClient.SqlParameter("@GroupName",group.GroupName ),
                            new System.Data.SqlClient.SqlParameter("@SuperiorId",group.SuperiorID  ),
                            new System.Data.SqlClient.SqlParameter("@orderID",group.OrderID ),
                          };
            return IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #region 删除分组
        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupID"></param>
        public static int  DelGroup(string groupID)
        {
            string sql = "delete Groups where GroupID=@GroupID";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@GroupID", groupID), };
           return  IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #endregion

        #region 将所有数据库中用户读入内存

        #region 将所有数据库中用户详细信息读入内存
        /// <summary>
        /// 将所有数据库中用户详细信息读入内存
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, UserVcard> GetUserVcards()
        {
            Dictionary<string, UserVcard> users = new Dictionary<string, UserVcard>();
            System.Data.SqlClient.SqlDataReader dr = DataAccess.GetReaderBySql("select * from users");
            if (dr != null)
            {
                while (dr.Read())
                {
                    UserVcard user = new UserVcard();

                    user.UserID = dr["UserID"].ToString().Trim();
                    user.UserName = dr["UserName"] is DBNull ? user.UserID : dr["UserName"].ToString().Trim();
                    user.Password = dr["Password"] is DBNull ? "E10ADC3949BA59ABBE56E057F20F883E" : dr["Password"].ToString().Trim();//123456
                    user.Sex = Convert.ToByte(dr["Sex"] is DBNull ? 0 : dr["Sex"]);
                    user.FaceIndex = Convert.ToInt32(dr["FaceIndex"] is DBNull ? 0 : dr["FaceIndex"]);
                    user.GroupID = dr["GroupID"] is DBNull ? "" : dr["GroupID"].ToString().Trim();
                    user.OrderID = Convert.ToInt32(dr["OrderID"] is DBNull ? 0 : dr["OrderID"]);
                    user.isAdmin = Convert.ToBoolean(dr["isAdmin"] is DBNull ? false : dr["isAdmin"]);
                    user.isBroadcast = Convert.ToBoolean(dr["isBroadcast"] is DBNull ? false : dr["isBroadcast"]);
                    user.isEditUserData = Convert.ToBoolean(dr["isEditUserData"] is DBNull ? false : dr["isEditUserData"]);
                    user.isSendNotice = Convert.ToBoolean(dr["isSendNotice"] is DBNull ? false : dr["isSendNotice"]);
                    user.isSendSMS = Convert.ToBoolean(dr["isSendSMS"] is DBNull ? false : dr["isSendSMS"]);
                    user.CreateRooms = Convert.ToInt32(dr["CreateRooms"] is DBNull ? 0 : dr["CreateRooms"]);
                    user.LastIP = dr["LastIP"] is DBNull ? "" : dr["LastIP"].ToString().Trim();
                    user.LastDateTime = dr["LastDateTime"] is DBNull ? "" : dr["LastDateTime"].ToString().Trim();
                    string cardstring = dr["Vcard"] is DBNull ? "<x x=\"\"/>" : dr["Vcard"].ToString().Trim();
                    UserVcard card = Factory.CreateInstanceObject(cardstring) as UserVcard;//鸡生蛋还是蛋生鸡?肯定有一个在前:)
                    if (card != null)//扩展资料，可以添加很多
                    {
                        user.Mail = card.Mail;
                        user.OfficePhone = card.OfficePhone;
                        user.Phone = card.Phone;
                        user.Post = card.Post;
                        user.Remark = card.Remark;
                        user.Job = card.Job;
                        user.Birthday = card.Birthday;
                    }
                    users.Add(user.UserID, user);
                }
                dr.Close();
            }
            dr.Dispose();
            return users;
        }
        #endregion

        #region 将所有数据库中用户基本信息读入内存
        /// <summary>
        /// 将所有数据库中用户基本信息读入内存
        /// </summary>
        /// <returns></returns>
        private  static Dictionary<string, User> GetUsers()
        {
            Dictionary<string, User>   users = new Dictionary<string, User>();
            System.Data.SqlClient.SqlDataReader dr = DataAccess.GetReaderBySql("select * from users");
            if (dr != null)
            {
                while (dr.Read())
                {
                    User user = new User();

                    user.UserID = dr["UserID"].ToString().Trim();
                    user.UserName = dr["UserName"] is DBNull ? user.UserID : dr["UserName"].ToString().Trim();
                    //user.Sex = Convert.ToByte(dr["Sex"] is DBNull ? 0 : dr["Sex"]);
                    //user.FaceIndex = Convert.ToInt32(dr["FaceIndex"] is DBNull ? 0 : dr["FaceIndex"]);
                    user.GroupID = dr["GroupID"] is DBNull ? "" : dr["GroupID"].ToString().Trim();
                    user.OrderID = Convert.ToInt32(dr["OrderID"] is DBNull ? 0 : dr["OrderID"]);

                    users.Add(user.UserID, user);
                }
                dr.Close();
            } 
            dr.Dispose();
            return users;
        }
        #endregion

        #endregion

        #region 将所有数据库中分组基本信息读入内存
        /// <summary>
        /// 将所有数据库中分组基本信息读入内存
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, Group> GetGroups()
        {
            Dictionary<string, Group> Groups = new Dictionary<string, Group>(); 
            System.Data.SqlClient.SqlDataReader dr = DataAccess.GetReaderBySql("select * from groups");
            if (dr != null)
            {
                while (dr.Read())
                {
                    Group group = new Group();
                    group.GroupID = dr["GroupID"].ToString().Trim();
                    group.GroupName = dr["GroupName"] is DBNull ? group.GroupID : dr["GroupName"].ToString().Trim();
                    group.SuperiorID = dr["SuperiorID"] is DBNull ? "" : dr["SuperiorID"].ToString().Trim();
                    group.OrderID = Convert.ToInt32(dr["OrderID"] is DBNull ? 0 : dr["OrderID"]);
                   
                    Groups.Add(group.GroupID, group);
                }
                dr.Close();
            }
            dr.Dispose();
            return Groups;
        }
        #endregion

        #region 将所有数据库中群组读入内存
        /// <summary>
        /// 将所有数据库中群组读入内存
        /// </summary>
        /// <returns></returns>
        public  static Dictionary<string, Room> GetRooms()
        {
            Dictionary<string, Room>Rooms = new Dictionary<string, Room>(); 
            System.Data.SqlClient.SqlDataReader dr = DataAccess.GetReaderBySql("select * from Rooms");
            if (dr != null)
            {
                while (dr.Read())
                {
                    Room room = new Room();
                    room.RoomID = dr["RoomID"].ToString().Trim();
                    room.RoomName = dr["RoomName"] is DBNull ? room.RoomID : dr["RoomName"].ToString().Trim();
                    room.CreateUserID = dr["CreateUserID"] is DBNull ? "" : dr["CreateUserID"].ToString().Trim();
                    room.UserIDs = dr["Users"] is DBNull ? "" : dr["Users"].ToString().Trim();
                    room.Notice = dr["Notice"] is DBNull ? "" : dr["Notice"].ToString().Trim();
                   
                    Rooms.Add(room.RoomID, room);
                }
                dr.Close();
            }
            dr.Dispose();
            return Rooms;
        }
        #endregion

        #region 重设企业组织架构版本信息

        /// <summary>
        /// 重设企业组织架构版本信息
        /// </summary>
        public static IMLibrary3.Protocol.OrgVersion ResetOrgVersion()
        {
            IMLibrary3.Protocol.OrgVersion OrgVersion = new IMLibrary3.Protocol.OrgVersion();
            string GroupsVersion = "";
            string UsersVersion = "";
            int sendGroupsCount =80;//每次发送分组信息数量（太多发送不到对方，太少服务器负载增加）
            int sendUsersCount = 80;//每次发送用户信息数量（太多发送不到对方，太少服务器负载增加）

            Dictionary<string, Group> Groups = GetGroups();//获得所有分组信息
            if (Groups != null)
            {
                IMLibrary3.Protocol.DownloadGroups pGroups = new IMLibrary3.Protocol.DownloadGroups();
                pGroups.type = IMLibrary3.Protocol.type.result;

                int i = 0;
                foreach (Group group in Groups.Values)
                {
                    pGroups.Data.Add(group);
                    i++;
                    if (i == sendGroupsCount)
                    {
                        GroupsVersion += IMLibrary3.Protocol.Factory.CreateXMLMsg(pGroups);
                        OrgVersion.GroupsXML.Add(IMLibrary3.Protocol.Factory.CreateXMLMsg(pGroups));
                        pGroups.Data = new List<object>();//分组信息清零
                        i = 0;
                    }
                }
                if (pGroups.Data.Count > 0)//剩下的分组信息
                {
                    GroupsVersion += IMLibrary3.Protocol.Factory.CreateXMLMsg(pGroups);
                    OrgVersion.GroupsXML.Add(IMLibrary3.Protocol.Factory.CreateXMLMsg(pGroups));
                }

                OrgVersion.GroupsCount = Groups.Count;
                OrgVersion.GroupsVersion = IMLibrary3.Security.Hasher.GetMD5Hash(Encoding.Default.GetBytes(GroupsVersion));
            }

            Dictionary<string, User> Users = GetUsers();//获得所有用户信息
            if (Users != null)
            {
                IMLibrary3.Protocol.DownloadUsers pUsers = new IMLibrary3.Protocol.DownloadUsers();
                pUsers.type = IMLibrary3.Protocol.type.result;

                int i = 0;
                foreach (User user in Users.Values)
                {
                    pUsers.Data.Add(user);
                    i++;
                    if (i == sendUsersCount)
                    {
                        UsersVersion += IMLibrary3.Protocol.Factory.CreateXMLMsg(pUsers);
                        OrgVersion.UsersXML.Add(IMLibrary3.Protocol.Factory.CreateXMLMsg(pUsers));
                        pUsers.Data = new List<object>();//用户信息清零
                        i = 0;
                    }
                }
                if (pUsers.Data.Count > 0)//剩下的用户信息
                {
                    UsersVersion += IMLibrary3.Protocol.Factory.CreateXMLMsg(pUsers);
                    OrgVersion.UsersXML.Add(IMLibrary3.Protocol.Factory.CreateXMLMsg(pUsers));
                }

                OrgVersion.UsersCount = Users.Count;
                OrgVersion.UsersVersion = IMLibrary3.Security.Hasher.GetMD5Hash(Encoding.Default.GetBytes(UsersVersion)); 
            }

            return OrgVersion;
        }
        #endregion

        #region 离线消息管理

        #region 添加离线消息到数据库
        /// <summary>
        /// 添加离线消息到数据库
        /// </summary>
        /// <param name="msg"></param>
        public static void addMessageToDB(IMLibrary3.Protocol.Message msg)
        {
            string sql = "insert into RecordMsg(userID,MessageType,froms,tos,datetime,MessageXML) values(@userID,@MessageType,@froms,@tos,@datetime,@MessageXML)";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@userID", msg.to ),
                            new System.Data.SqlClient.SqlParameter("@MessageType",msg.MessageType ),
                            new System.Data.SqlClient.SqlParameter("@froms",msg.from ),
                            new System.Data.SqlClient.SqlParameter("@tos", msg.to ),
                            new System.Data.SqlClient.SqlParameter("@datetime", DateTime.Now.ToString()),
                            new System.Data.SqlClient.SqlParameter("@MessageXML", IMLibrary3.Protocol.Factory.CreateXMLMsg(msg)),
                          };
            IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }

        /// <summary>
        /// 添加离线消息到数据库
        /// </summary>
        /// <param name="msg"></param>
        public static void addMessageToDB(string userID, string from, string to, IMLibrary3.Enmu.MessageType messageType, string xml)
        {
            string sql = "insert into RecordMsg(userID,MessageType,froms,tos,datetime,MessageXML) values(@userID,@MessageType,@froms,@tos,@datetime,@MessageXML)";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@userID", userID),
                            new System.Data.SqlClient.SqlParameter("@MessageType",messageType ),
                            new System.Data.SqlClient.SqlParameter("@froms", from ),
                            new System.Data.SqlClient.SqlParameter("@tos",  to ),
                            new System.Data.SqlClient.SqlParameter("@datetime", DateTime.Now.ToString()),
                            new System.Data.SqlClient.SqlParameter("@MessageXML",xml),
                          };
            IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion

        #region 获得用户离线消息
        /// <summary>
        /// 获得用户离线消息
        /// </summary>
        /// <param name="userID">用户帐号</param>
        /// <param name="top">消息最后条数</param>
        /// <returns></returns>
        public static List<string> GetOfflineMessage(string userID, int top)
        {
            List<string> messageList = new List<string>();
            string sql = string.Format("select top {0} MessageXML from RecordMsg where userID='{1}'", top, userID);

            System.Data.SqlClient.SqlDataReader dr = IMLibrary.SqlData.DataAccess.GetReaderBySql(sql);
            if (dr != null)
            {
                while (dr.Read())
                    messageList.Add(dr["MessageXML"] is DBNull ? "" : dr["MessageXML"].ToString());
                dr.Close();
            }
            dr.Dispose();

            sql = string.Format("delete  from RecordMsg where userID='{0}'", userID);

            IMLibrary.SqlData.DataAccess.ExecSql(sql);
            return messageList;
        }
        #endregion

        #endregion

        #region 群管理

        /// <summary>
        /// 创建群
        /// </summary>
        /// <param name="room">群</param>
        public static bool CreateRoom(IMLibrary3.Organization.Room room)
        {
            try
            {
                string sql = "insert into Rooms(RoomID,RoomName,Notice,Users,CreateUserID,CreateDateTime)"
                    + " values(@RoomID,@RoomName,@Notice,@Users,@CreateUserID,@CreateDateTime)";

                System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@RoomID", room.RoomID ),
                            new System.Data.SqlClient.SqlParameter("@RoomName", room.RoomName ),
                            new System.Data.SqlClient.SqlParameter("@Notice", room.Notice ),
                            new System.Data.SqlClient.SqlParameter("@Users", room.UserIDs  ),
                            new System.Data.SqlClient.SqlParameter("@CreateUserID", room.CreateUserID ),
                            new System.Data.SqlClient.SqlParameter("@CreateDateTime", DateTime.Now),
                          };
                IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
                return true;
            }
          catch  {return false;}
        }

        /// <summary>
        ///  更新群
        /// </summary>
        /// <param name="room">群</param>
        public static bool UpdateRoom(IMLibrary3.Organization.Room room)
        {
            try
            {
                string sql = "update  Rooms set RoomName=@RoomName,Notice=@Notice,Users=@Users" 
                               +  "  where (RoomID=@RoomID)";

                System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@RoomID", room.RoomID ),
                            new System.Data.SqlClient.SqlParameter("@RoomName", room.RoomName ),
                            new System.Data.SqlClient.SqlParameter("@Notice", room.Notice ),
                            new System.Data.SqlClient.SqlParameter("@Users", room.UserIDs  ),
                          };
                IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
                return true;
            }
            catch { return false; }
        }

        #endregion

        #region 更新用户密码
        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="pssword">用户密码</param>
        public static int UpdatePassword(string userID, string Password)
        {
            string sql = "update Users set Password=@Password  where userID=@userID";
            System.Data.SqlClient.SqlParameter[] sqlpar = new System.Data.SqlClient.SqlParameter[]
                          { new System.Data.SqlClient.SqlParameter("@userID",userID),
                            new System.Data.SqlClient.SqlParameter("@Password",Password),
  
                          };
            return IMLibrary.SqlData.DataAccess.ExecSql(sql, sqlpar);
        }
        #endregion
    }
}
