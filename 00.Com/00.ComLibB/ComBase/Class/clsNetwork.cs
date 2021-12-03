using Microsoft.Win32;
using System;
using System.Data;
using System.Windows.Forms;         //messagebox

namespace ComBase
{
    public class Network : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        //string SQL;
        //long rowcounter;
        //string strValue;

        //DataTable dt = null;        
        //string SqlErr = ""; //������ �޴� ����
        //int intRowAffected = 0; //����� Row �޴� ����

        //����� IP_ADDRESS �ҷ����� Fuction
        public string Read_IPAddress_SQL()
        {
            string SQL = "";
            long rowcounter = 0;
            string strValue = "";

            DataTable dt = null;
            string SqlErr = ""; //������ �޴� ����            

            try
            {
                SQL = "";
                SQL += " SELECT SYS_CONTEXT('USERENV','IP_ADDRESS') IPADDRESS     \r";
                SQL += "   FROM DUAL                                              \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("��ȸ�� ������ �߻��߽��ϴ�");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //�����α� ����
                    strValue = "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("�ش� DATA�� �����ϴ�.");
                    strValue = "";
                }

                rowcounter = 0;

                rowcounter = dt.Rows.Count;

                if (rowcounter == 1)
                {
                    strValue = dt.Rows[0]["IPADDRESS"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //�����α� ����
                return strValue;
            }
        }

        //��ǻ�� ���� ��ġ�� ������Ʈ������ �о����
        public string Read_Reg_Position()
        {
            //string SQL;
            //long rowcounter;
            //string strValue;

            //DataTable dt = null;
            //string SqlErr = ""; //������ �޴� ����
            //int intRowAffected = 0; //����� Row �޴� ����
            //* ������Ʈ�� ���ð� ��������
            const string RegRoot = @"SOFTWARE\VMS\3.0\SiteClient\General\";
            string setting = RegRoot;// +@"Setting";
            string sUserName = "";
            string sUserPartName = "";

            RegistryKey reg = Registry.LocalMachine;
            //reg = reOpenSubKey(setting, true);

            if (reg != null)
            {
                //üũ�� ����Ʈ �ڽ� ������ �� �Է�
                //sUserName = Convert.ToString(reGetValue("UserName"));
                //sUserPartName = Convert.ToString(reGetValue("UserPartName"));
            }
            //reClose();


            return sUserPartName + " / " + sUserName;

        }

        //����� IP_ADDRESS �ҷ����� Fuction
        public string Read_SESSIONID_SQL()
        {
            string SQL = "";
            long rowcounter = 0;
            string strValue = "";

            DataTable dt = null;
            string SqlErr = ""; //������ �޴� ����
            
            try
            {
                SQL = "";
                SQL += " SELECT SYS_CONTEXT('USERENV','SESSIONID') SESSIONID      \r";
                SQL += "   FROM DUAL                                              \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("��ȸ�� ������ �߻��߽��ϴ�");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //�����α� ����
                    strValue = "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("�ش� DATA�� �����ϴ�.");
                    strValue = "";
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter == 1)
                {
                    strValue = dt.Rows[0]["SESSIONID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return strValue;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //�����α� ����
                return strValue;
            }
        }

        //���Ǻ� LOCKȯ�� ��� �����
        public void DELETE_SESSIONID_SQL()
        {
            string SQL = "";
            
            string SqlErr = ""; //������ �޴� ����
            int intRowAffected = 0; //����� Row �޴� ����
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += " DELETE                                          \r";
                SQL += "   FROM BASIC.BG_LOCK_PATIENT                    \r";
                SQL += "  WHERE SESSIONID = USERENV('SESSIONID')         \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //�����α� ����
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //�����α� ����
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
