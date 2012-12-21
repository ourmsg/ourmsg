using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using IMLibrary3.Data;
using IMLibrary3.Organization;
using IMLibrary3.Protocol;
using System.Data.SQLite;

namespace IMLibrary3 
{
    /// <summary>
    /// 操作本地缓存数据库
    /// </summary>
    public class OpeRecordDB
    {

        #region 登录认证信息管理

        #region 获得本地保存的用户登录认证信息
        /// <summary>
        /// 获得本地保存的用户登录认证信息
        /// </summary>
        /// <returns></returns>
        public static List<Auth> GetLoginAuths()
        {
            List<Auth> auths = null;

            string sql = "select * from log order by updateTime desc";
            string constr ="data source="+Application.StartupPath +"\\login"+".db";

            SQLiteConnection con = new SQLiteConnection(constr);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql);
            cmd.Connection = con;
            SQLiteDataReader dr = cmd.ExecuteReader();
            if (dr != null)
            {
                auths = new List<Auth>();
                while (dr.Read())
                {
                    Auth a =Factory.CreateInstanceObject(dr["auth"].ToString()) as Auth  ;
                    if (a != null)
                        auths.Add(a);
                }
                dr.Close();
            }
            dr.Dispose(); dr = null;
            cmd.Dispose(); cmd = null; 
            con.Close();con.Dispose(); con = null;

            return auths;
        }
        #endregion

        #region 保存用户登录认证信息
        /// <summary>
        /// 保存用户登录认证信息
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public static int SaveAuth(Auth auth)
        {
            string authXML=Factory.CreateXMLMsg(auth);
            string sql = string.Format ("update log set auth='{0}',updateTime='{2}' where userID='{1}'",authXML,auth.UserID,DateTime.Now.ToString());
            string constr = "data source=" + Application.StartupPath + "\\login" + ".db";

            SQLiteConnection con = new SQLiteConnection(constr);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql);
            cmd.Connection = con;
            int affected=cmd.ExecuteNonQuery();
            if (affected == 0)
            {
                sql = string.Format("insert into log(auth,userID,updateTime) values( '{0}','{1}','{2}')", authXML, auth.UserID, DateTime.Now.ToString());
                cmd.CommandText = sql;
                affected = cmd.ExecuteNonQuery();
            }
            cmd.Dispose(); cmd = null;
            con.Close(); con.Dispose(); con = null;
            return affected;
        }
        #endregion

        #region 删除用户登录认证信息
        /// <summary>
        /// 删除用户登录认证信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static int DeleteAuth(string  userID)
        {
            string sql = string.Format("delete from log where userID='{0}'", userID);
            string constr = "data source=" + Application.StartupPath + "\\login" + ".db";

            SQLiteConnection con = new SQLiteConnection(constr);
            con.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql);
            cmd.Connection = con;
            int affected = cmd.ExecuteNonQuery();
            cmd.Dispose(); cmd = null;
            con.Close(); con.Dispose(); con = null;
            return affected;
        }
        #endregion

        #endregion

        #region 消息管理

        #region 获得 DR 消息
        /// <summary>
        /// 获得 DR 消息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static IMLibrary3.Protocol.Message  GetDrMsg(System.Data.SQLite.SQLiteDataReader dr)
        {
            IMLibrary3.Protocol.Message msg = IMLibrary3.Protocol.Factory.CreateInstanceObject(dr["Vcard"].ToString()) as IMLibrary3.Protocol.Message;
            return msg;
        }
        #endregion 
         
        #region 获得消息集合
        /// <summary>
        /// 获得消息集合
        /// </summary>
        /// <param name="MsgInfoClass">消息类型</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<IMLibrary3.Protocol.Message> GetMsg(IMLibrary3.Enmu.MessageType MessageType, int pageIndex, int pageSize)
        {
            List<IMLibrary3.Protocol.Message> Msgs = null;

            System.Data.SQLite.SQLiteParameter messageType = new System.Data.SQLite.SQLiteParameter("MessageType", MessageType);

            string sql = "select * from MsgRecord where MessageType=@MessageType "
                 + " order by ID limit " + pageSize.ToString() + " offset " + ((pageIndex - 1) * pageSize).ToString();

            System.Data.SQLite.SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, messageType);

            if (dr != null)
            {
                Msgs = new List<IMLibrary3.Protocol.Message>();
                while (dr.Read())
                    Msgs.Add(GetDrMsg(dr));
            }
            dr.Close(); dr.Dispose();

            return Msgs;
        }
        #endregion

        #region 添加消息
        /// <summary>
        /// 添加消息
        /// </summary>
        public static void AddMsg(IMLibrary3.Protocol.Message Msg)
        {
            string sql = "insert into MsgRecord(froms,tos,MessageType,DateTime,Vcard)" +
                           "values(@froms,@tos,@MessageType,@DateTime,@Vcard)";
  
            System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@froms",Msg.from ),   
                     new System.Data.SQLite.SQLiteParameter("@tos",Msg.to ),   
                     new System.Data.SQLite.SQLiteParameter("@MessageType",Convert.ToByte( Msg.MessageType )),   
                     new System.Data.SQLite.SQLiteParameter("@DateTime",  Msg.DateTime==null?DateTime.Now.ToString():Msg.DateTime ),   
                     new System.Data.SQLite.SQLiteParameter("@Vcard",IMLibrary3.Protocol.Factory.CreateXMLMsg(Msg))  
                                       };
            try
            {
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
        }
        #endregion

        #region 删除消息
        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="userID">用户ID</param>
        public static void DeleteMsg(string userID)
        {
            try
            {
                System.Data.SQLite.SQLiteParameter uID = new System.Data.SQLite.SQLiteParameter("userID", userID);
                string sql = "delete from MsgRecord where froms=@userID or tos=@userID";
                SQLiteDBHelper.ExecuteNonQuery(sql, uID);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        #region 删除一条消息
        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="ID">索引</param>
        public static void DeleteMsg(int ID)
        {
            try
            {
                string sql = "delete from MsgRecord where ID=" + ID.ToString();
                SQLiteDBHelper.ExecuteNonQuery(sql, null);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion

        #region 删除所有消息
        /// <summary>
        /// 删除所有消息
        /// </summary>
        public static void DeleteAllMsg()
        {
            try
            {
                string sql = "delete from MsgRecord "  ;
                SQLiteDBHelper.ExecuteNonQuery(sql, null);
            }
            catch (Exception ex)
            {
                 throw new ArgumentException(  ex.Message,ex );
            }
        }
        #endregion

        #endregion

        #endregion

        #region 组织机构版本管理
        /// <summary>
        /// 获得组织机构版本
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static IMLibrary3.Protocol.OrgVersion GetOrgVersion(string UserID)
        {
            IMLibrary3.Protocol.OrgVersion orgVersion = null;

            #region 判断登录的用户本地数据库文件夹及文件是否存在，不存在返回空值
            //System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(UserID);
            //if (!dInfo.Exists)
            //    return null;
            //string FileNamePath = UserID + @"\Record.mdb";
            //if (!System.IO.File.Exists(FileNamePath))
            //    return null;

            ////如果本地数据库存在，则设置数据库连接字符串
            //DBFileNamePath = FileNamePath;
            //conStr = "data source=" + DBFileNamePath;
            #endregion

            try
            {
                string sql = "select * from OrgVersion";
                 System.Data.SQLite.SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, null );
                if (dr != null && dr.Read())
                {
                    orgVersion = new IMLibrary3.Protocol.OrgVersion();
                    orgVersion.GroupsVersion = Convert.ToString(dr["GroupsVersion"]).Trim();
                    orgVersion.UsersVersion = Convert.ToString(dr["UsersVersion"]).Trim();
                    orgVersion.RoomsVersion = Convert.ToString(dr["RoomsVersion"]).Trim();
                }
                dr.Close(); dr.Dispose();
                return orgVersion;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
            return null;

        }

      /// <summary>
      /// 更新本地数据库组织机构版本信息
      /// </summary>
        public static void UpdateOrgVersion(IMLibrary3.Protocol.OrgVersion orgVersion)
        {
            try
            {
                if (orgVersion.RoomsVersion == null)
                    orgVersion.RoomsVersion = "";
                if (orgVersion.GroupsVersion == null)
                    orgVersion.GroupsVersion = "";
                if (orgVersion.UsersVersion== null)
                    orgVersion.UsersVersion = "";

                string sql = "update OrgVersion set Password='',GroupsVersion=@GroupsVersion,UsersVersion=@UsersVersion,RoomsVersion=@RoomsVersion";

                System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@GroupsVersion",orgVersion.GroupsVersion  ),   
                     new System.Data.SQLite.SQLiteParameter("@UsersVersion",orgVersion.UsersVersion ),   
                     new System.Data.SQLite.SQLiteParameter("@RoomsVersion",orgVersion.RoomsVersion ),   
                                       };
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
        }
        #endregion

        #region 用户管理

        #region 获取用户资料
        /// <summary>
        /// 获取用户资料
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static  UserVcard GetUserVcard(string UserID)
        {
            UserVcard card=null;
            string sql = "select  Vcard from UsersVcard  where UserID=@UserID";
            System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@UserID",UserID )};
            System.Data.SQLite.SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, parameters);
            if (dr != null && dr.Read())
            {
                card= Factory.CreateInstanceObject(Convert.ToString(dr["Vcard"])) as UserVcard;
                dr.Close();
            }
            dr.Dispose();
            return card;
        }
        #endregion
            
        #region 更新用户Card，无数据则添加
        /// <summary>
        /// 更新用户Card，无数据则添加
        /// </summary>
        /// <param name="card"></param>
        public static void UpdateUserVcard(UserVcard card)
        {
            string sql = "update Users set UserName=@UserName,GroupID=@GroupID,OrderID=@OrderID where UserID=@UserID;"
                +"update UsersVcard set Vcard=@Vcard,LastUpdateTime=@LastUpdateTime where UserID=@UserID";
            System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@UserID",card.UserID ),   
                     new System.Data.SQLite.SQLiteParameter("@UserName",card.UserName  ),   
                     new System.Data.SQLite.SQLiteParameter("@GroupID",card.GroupID ),   
                     new System.Data.SQLite.SQLiteParameter("@OrderID",card.OrderID ),   
                     new System.Data.SQLite.SQLiteParameter("@Vcard",IMLibrary3.Protocol.Factory.CreateXMLMsg(card)),
                     new System.Data.SQLite.SQLiteParameter("@LastUpdateTime",DateTime.Now),
            };
            if (SQLiteDBHelper.ExecuteNonQuery(sql, parameters) == 0)
            {
                sql = "insert into Users(UserID,UserName,GroupID,OrderID) values(@UserID,@UserName,@GroupID,@OrderID);" +
                    "insert into UsersVcard(UserID,Vcard,LastUpdateTime) values(@UserID,@Vcard,@LastUpdateTime)";
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
        }
        #endregion

        #region 获得所有用户集合
        /// <summary>
        /// 获得所有用户集合
        /// </summary>
        public static List<exUser> GetUsers()
        {
            List<exUser> Users =  new List<exUser>(); 
            string sql = "select * from Users order by orderID";

            System.Data.SQLite.SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, null);
            if (dr != null)
            {
                while (dr.Read())
                {
                    exUser user = new exUser();
                    {
                        user.UserID = Convert.ToString(dr["UserID"]);
                        user.UserName = Convert.ToString(dr["UserName"]);
                        user.GroupID = Convert.ToString(dr["GroupID"]);
                        user.OrderID = Convert.ToInt32(dr["OrderID"]);
                    }
                    Users.Add(user);
                } 
                dr.Close();
            }
            dr.Dispose();
            return Users;
        }
        #endregion

        #region 添加用户

        /// <summary>
        ///  添加多个用户
        /// </summary>
        /// <param name="Users"></param>
        public static void AddUsers(List<exUser> Users)
        {
            #region 多次添加
            //try
            //{
            //    string sql = "insert into Users(UserID,UserName,GroupID,orderID) values(@UserID,@UserName,@GroupID,@orderID)";
            //    foreach (OurMsg.Organization.exUser user in Users)
            //    {
            //        System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
            //         new System.Data.SQLite.SQLiteParameter("@UserID",user.UserID ),   
            //         new System.Data.SQLite.SQLiteParameter("@UserName",user.UserName),   
            //         new System.Data.SQLite.SQLiteParameter("@GroupID",user.GroupID),   
            //         new System.Data.SQLite.SQLiteParameter("@orderID",user.OrderID),   
            //                           };
            //        SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Source + ex.Message);
            //}
            #endregion

            #region 一次性事务添加
            SQLiteConnection con = new SQLiteConnection(); //创建连接
            SQLiteCommand cmd = null;
            con.ConnectionString  = IMLibrary3.Data.SQLiteDBHelper.connectionString;
            try
            {
                con.Open();
                using (SQLiteTransaction dbTrans = con.BeginTransaction()) //使用事务
                {
                    using (cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "insert into Users(UserID,UserName,GroupID,orderID) values(?,?,?,?)";
                        SQLiteParameter Field1 = cmd.CreateParameter();   //添加字段
                        SQLiteParameter Field2 = cmd.CreateParameter();
                        SQLiteParameter Field3 = cmd.CreateParameter();
                        SQLiteParameter Field4 = cmd.CreateParameter();
                        cmd.Parameters.Add(Field1);
                        cmd.Parameters.Add(Field2);
                        cmd.Parameters.Add(Field3);
                        cmd.Parameters.Add(Field4);
                        foreach (exUser user in Users)
                        {
                            Field1.Value = user.UserID; //字符赋值
                            Field2.Value = user.UserName;
                            Field3.Value = user.GroupID;
                            Field4.Value = user.OrderID;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbTrans.Commit();     //提交事务执行
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
            finally
            {
                con.Close();
            }
            #endregion
        }

        //public static void ShowData()
        //{
        //    //查询从50条起的20条记录   
        //    string sql = "select * from test3 order by id desc limit 50 offset 20";

        //    using (SQLiteDataReader reader = SQLiteDBHelper.ExecuteReader(sql, null))
        //    {
        //        while (reader.Read())
        //        {
        //            Console.WriteLine("ID:{0},TypeName{1}", reader.GetInt64(0), reader.GetString(1));
        //        }
        //    }
        //}  

        #endregion

        #region 删除用户
        /// <summary>
        /// 删除用户
        /// </summary>
        public static void DeleteUser(string userID)
        {
            try
            {
                string sql = "delete from Users where userID=@userID;delete from UsersVcard  where userID=@userID";
                System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                         new System.Data.SQLite.SQLiteParameter("@UserID",userID ),
                };
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion

        #region 删除所有用户
        /// <summary>
        /// 删除所有用户
        /// </summary>
        public static void DeleteAllUser()
        {
            try
            {
                string sql = "delete from Users ";
                SQLiteDBHelper.ExecuteNonQuery(sql, null);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region 分组管理

        #region 更新分组Card，无数据则添加
        /// <summary>
        /// 更新分组Card，无数据则添加
        /// </summary>
        /// <param name="GroupID"></param>
        /// <param name="e"></param>
        public static void UpdateGroupVcard(GroupVcard  card)
        { 
            string sql = "update GroupsVcard set Vcard=@Vcard where GroupID=@GroupID";
            System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@GroupID",card.GroupID ),   
                     new System.Data.SQLite.SQLiteParameter("@Vcard",Factory.CreateXMLMsg(card)),
                     new System.Data.SQLite.SQLiteParameter("@GroupName",card.GroupName ),   
                     new System.Data.SQLite.SQLiteParameter("@SuperiorID",card.SuperiorID ),   
                     new System.Data.SQLite.SQLiteParameter("@OrderID",card.OrderID ),   
          };
            if (SQLiteDBHelper.ExecuteNonQuery(sql, parameters) == 0)
            {
                sql = "insert into Groups(GroupID,GroupName,SuperiorID,OrderID) values(@GroupID,@GroupName,@SuperiorID,@OrderID);"
                + "insert into GroupsVcard(GroupID,Vcard) values(@GroupID,@Vcard)";
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
        }
        #endregion

        #region 获得所有分组集合
        /// <summary>
        /// 获得所有分组集合
        /// </summary>
        public static List<exGroup> GetGroups()
        {
            List<exGroup> Groups = new List<exGroup>();
            string sql = "select * from Groups order by orderID  ";
            System.Data.SQLite.SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, null);
            if (dr != null)
            {
                while (dr.Read())
                {
                   exGroup group = new exGroup();
                    {
                        group.GroupID = Convert.ToString(dr["GroupID"]);
                        group.GroupName = Convert.ToString(dr["GroupName"]);
                        group.SuperiorID = Convert.ToString(dr["SuperiorID"]);
                        group.OrderID = Convert.ToInt32(dr["orderID"]);
                    }
                    Groups.Add(group);
                }
                dr.Close();
            }
            dr.Dispose();

            return Groups;
        }
        #endregion

        #region 添加分组
        /// <summary>
        ///  添加多个分组信息
        /// </summary>
        /// <param name="Groups"></param>
        public static void AddGroups(List<exGroup> Groups)
        {
            #region 一次性事务添加
            SQLiteConnection con = new SQLiteConnection(); //创建连接
            SQLiteCommand cmd = null;
            con.ConnectionString = IMLibrary3.Data.SQLiteDBHelper.connectionString;
            try
            {
                con.Open();
                using (SQLiteTransaction dbTrans = con.BeginTransaction()) //使用事务
                {
                    using (cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "insert into Groups(GroupID,GroupName,SuperiorID,orderID) values(?,?,?,?)";
                        SQLiteParameter Field1 = cmd.CreateParameter();   //添加字段
                        SQLiteParameter Field2 = cmd.CreateParameter();
                        SQLiteParameter Field3 = cmd.CreateParameter();
                        SQLiteParameter Field4 = cmd.CreateParameter();
                        cmd.Parameters.Add(Field1);
                        cmd.Parameters.Add(Field2);
                        cmd.Parameters.Add(Field3);
                        cmd.Parameters.Add(Field4);
                        foreach (exGroup group in Groups)
                        {
                            Field1.Value = group.GroupID; //字符赋值
                            Field2.Value = group.GroupName;
                            Field3.Value = group.SuperiorID;
                            Field4.Value = group.OrderID;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbTrans.Commit();     //提交事务执行
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
            finally
            {
                con.Close();
            }
            #endregion
        }
        #endregion 

        #region 删除分组
        /// <summary>
        /// 删除分组
        /// </summary>
        public static void DeleteGroup(string groupID)
        {
            try
            {
                string sql = "delete from Groups where groupID=@groupID;delete from GroupsVcard where groupID=@groupID";
                System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                         new System.Data.SQLite.SQLiteParameter("@groupID",groupID ),
                };
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion
         
        #region 删除所有分组
        /// <summary>
        /// 删除所有分组
        /// </summary>
        public static void DeleteAllGroup()
        {
            try
            {
                string sql = "delete from Groups ";
                SQLiteDBHelper.ExecuteNonQuery(sql, null);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion

        #region 查找分组
        public exGroup findGroup(string groupID,List<exGroup> Groups)
        {
            foreach (exGroup group in Groups)
                if (group.GroupID == groupID)
                    return group;
            return null;
        }
        #endregion

        #endregion

        #region 群

        #region 更新群Card，无数据则添加
        /// <summary>
        /// 更新群Card，无数据则添加
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="e"></param>
        public static void UpdateRoomVcard(string RoomID, IMLibrary3.Protocol.Element e)
        {
            //string sql = "delete from RoomsVcard  ";

            string sql = "update RoomsVcard set Vcard=@Vcard where RoomID=@RoomID";
            System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@RoomID",RoomID ),   
                     new System.Data.SQLite.SQLiteParameter("@Vcard",IMLibrary3.Protocol.Factory.CreateXMLMsg(e))};
            if (SQLiteDBHelper.ExecuteNonQuery(sql, parameters) ==0)
            {
                sql = "insert into RoomsVcard(RoomID,Vcard) values(@RoomID,@Vcard)";
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }
        }
        #endregion

        #region 添加或更新一个群信息
        /// <summary>
        ///  添加或更新一个群信息
        /// </summary>
        /// <param name="Groups"></param>
        public static void UpdateRooms(exRoom room)
        {
            #region 单次插入数据
            string sql = "insert into Rooms(RoomID,RoomName,Users, Notice,CreateUserID) values(@RoomID,@RoomName,@Users,@Notice,@CreateUserID)";
              System.Data.SQLite.SQLiteParameter[] parameters = new System.Data.SQLite.SQLiteParameter[]{   
                     new System.Data.SQLite.SQLiteParameter("@RoomID",room.RoomID ),   
                     new System.Data.SQLite.SQLiteParameter("@RoomName",room.RoomName ),   
                     new System.Data.SQLite.SQLiteParameter("@Users",room.UserIDs),   
                     new System.Data.SQLite.SQLiteParameter("@Notice",room.Notice ),   
                     new System.Data.SQLite.SQLiteParameter("@CreateUserID",room.CreateUserID ),   
                                       };
            try
            {
                SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
            }

            catch 
            {
                try
                {
                    sql = "update Rooms set RoomName=@RoomName,Users=@Users,Notice=@Notice,CreateUserID=@CreateUserID where  RoomID=@RoomID";

                    SQLiteDBHelper.ExecuteNonQuery(sql, parameters);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Source + ex.Message);
                }
            }
            #endregion

            #region 一次性事务添加
            //SQLiteConnection con = new SQLiteConnection(); //创建连接
            //SQLiteCommand cmd = null;
            //con.ConnectionString = conStr;
            //try
            //{
            //    con.Open();
            //    using (SQLiteTransaction dbTrans = con.BeginTransaction()) //使用事务
            //    {
            //        using (cmd = con.CreateCommand())
            //        {
            //            cmd.CommandText = "insert into Groups(GroupID,GroupName,SuperiorID,orderID) values(?,?,?,?)";
            //            SQLiteParameter Field1 = cmd.CreateParameter();   //添加字段
            //            SQLiteParameter Field2 = cmd.CreateParameter();
            //            SQLiteParameter Field3 = cmd.CreateParameter();
            //            SQLiteParameter Field4 = cmd.CreateParameter();
            //            cmd.Parameters.Add(Field1);
            //            cmd.Parameters.Add(Field2);
            //            cmd.Parameters.Add(Field3);
            //            cmd.Parameters.Add(Field4);
            //            foreach (OurMsg.Organization.exGroup group in Groups)
            //            {
            //                Field1.Value = group.GroupID; //字符赋值
            //                Field2.Value = group.GroupName;
            //                Field3.Value = group.SuperiorID;
            //                Field4.Value = group.OrderID;
            //                cmd.ExecuteNonQuery();
            //            }
            //        }
            //        dbTrans.Commit();     //提交事务执行
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Source + ex.Message);
            //}
            //finally
            //{
            //    con.Close();
            //}
            #endregion
        }
        #endregion 

        #region 获得所有群
        /// <summary>
        /// 获得所有群
        /// </summary>
        public static List<exRoom> GetRooms()
        {
            List<exRoom> Rooms = new List<exRoom>();
            string sql = "select * from Rooms";
            System.Data.SQLite.SQLiteDataReader dr = SQLiteDBHelper.ExecuteReader(sql, null);
            if (dr != null)
            {
                while (dr.Read())
                {
                    exRoom Room = new exRoom();
                    {
                        Room.RoomID = Convert.ToString(dr["RoomID"]);
                        Room.RoomName = Convert.ToString(dr["RoomName"]);
                        Room.Notice = Convert.ToString(dr["Notice"]);
                        Room.UserIDs = Convert.ToString(dr["Users"]);
                        Room.OrderID = Convert.ToInt32(dr["OrderID"]);
                        Room.CreateUserID = Convert.ToString(dr["CreateUserID"]);
                    }
                    Rooms.Add(Room);
                }
                dr.Close();
            }
            dr.Dispose();
            return Rooms;
        }
        #endregion

        #region 添加多个群
        /// <summary>
        ///  添加多个群
        /// </summary>
        /// <param name="Groups"></param>
        public static void AddRooms(List<exRoom > Rooms)
        {
            #region 一次性事务添加
            SQLiteConnection con = new SQLiteConnection(); //创建连接
            SQLiteCommand cmd = null;
            con.ConnectionString = IMLibrary3.Data.SQLiteDBHelper.connectionString;
            try
            {
                con.Open();
                using (SQLiteTransaction dbTrans = con.BeginTransaction()) //使用事务
                {
                    using (cmd = con.CreateCommand())
                    {
                        cmd.CommandText = "insert into Rooms(RoomID,RoomName,Notice,Users,CreateUserID) values(?,?,?,?,?)";
                        SQLiteParameter Field1 = cmd.CreateParameter();   //添加字段
                        SQLiteParameter Field2 = cmd.CreateParameter();
                        SQLiteParameter Field3 = cmd.CreateParameter();
                        SQLiteParameter Field4 = cmd.CreateParameter();
                        SQLiteParameter Field5 = cmd.CreateParameter();
                        cmd.Parameters.Add(Field1);
                        cmd.Parameters.Add(Field2);
                        cmd.Parameters.Add(Field3);
                        cmd.Parameters.Add(Field4);
                        cmd.Parameters.Add(Field5);
                        foreach (exRoom room in Rooms)
                        {
                            Field1.Value = room.RoomID; //字符赋值
                            Field2.Value = room.RoomName;
                            Field3.Value = room.Notice;
                            Field4.Value = room.UserIDs;
                            Field5.Value = room.CreateUserID;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    dbTrans.Commit();     //提交事务执行
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ex.Message);
            }
            finally
            {
                con.Close();
            }
            #endregion
        }
        #endregion 

        #region 删除所有群
        /// <summary>
        /// 删除所有群
        /// </summary>
        public static void DeleteAllRoom()
        {
            try
            {
                string sql = "delete from Rooms ";
                SQLiteDBHelper.ExecuteNonQuery(sql, null);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion

        #endregion
    }
}
