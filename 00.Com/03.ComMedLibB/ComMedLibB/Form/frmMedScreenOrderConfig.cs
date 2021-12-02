using ComBase;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : frmMedScreenOrderConfig.cs
    /// Description     : 처방검토 환경설정
    /// Author          : 박성완
    /// Create Date     : 2019-05-17
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    public partial class frmMedScreenOrderConfig : Form
    {
        private string pUserId;
        private string pInstCD;
        private string pModuleID;
        private string pModuleNM;
        private string pModuleGubun;

        public frmMedScreenOrderConfig()
        {
            InitializeComponent();
        }

        private void frmMedScreenOrderConfig_Load(object sender, EventArgs e)
        {
            //숨김처리
            if (Debugger.IsAttached == false)
            {
                ssEnvConfig.ActiveSheet.Columns[0].Visible = false;
                ssEnvConfig.ActiveSheet.Columns[4].Visible = false;
                ssEnvConfig.ActiveSheet.Columns[12].Visible = false;
                ssEnvConfig.ActiveSheet.Columns[13].Visible = false;
                ssEnvConfig.ActiveSheet.Columns[14].Visible = false;

                ssEnvExceptDrug.ActiveSheet.Columns[0].Visible = false;
            }


            pUserId = clsType.User.Sabun;

            //현재 사번은 약제팀장, 이민영약사, 권성희 약사 세명만 폼 사용하며 슈퍼유저로만 전체 컨트롤
            //pUserId = "07790";
            //pUserId = "43062";
            pUserId = "SUPERUSER";
            //pUserId = "99999";

            lblSabun.Text = pUserId;

            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            ComDbB.PsmhDb pDbCon = clsDB.DbCon;

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_DRUG.up_ScreenCheckModule";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("pUserId", OracleDbType.Varchar2, 5, pUserId, ParameterDirection.Input);
            cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

            OracleDataAdapter ODA = new OracleDataAdapter(cmd);
            ODA.Fill(ds);

            ssEnvConfig.ActiveSheet.Rows.Count = 0;
            ssEnvConfig.ActiveSheet.Rows.Count = 1;

            if (ds.Tables.Count > 0)
            {
                int nDataCnt = ds.Tables[0].Rows.Count;

                ssEnvConfig.ActiveSheet.Rows.Count = nDataCnt;


                ssEnvConfig.DataSource = ds.Tables[0];

                //셀타입지정
                ssEnvConfig.ActiveSheet.Columns[2, 3].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                FarPoint.Win.Spread.CellType.NumberCellType noCelltype = new FarPoint.Win.Spread.CellType.NumberCellType();
                noCelltype.MinimumValue = 0;
                noCelltype.MaximumValue = 999;
                noCelltype.FixedPoint = false;               
                ssEnvConfig.ActiveSheet.Columns[6, 11].CellType = noCelltype;

                for (int i = 0; i < nDataCnt; i++)
                {
                    ssEnvConfig.ActiveSheet.Cells[i, 2].Text = (ssEnvConfig.ActiveSheet.Cells[i, 2].Text == "Y") ? "True" : "False";
                    ssEnvConfig.ActiveSheet.Cells[i, 3].Text = (ssEnvConfig.ActiveSheet.Cells[i, 3].Text == "Y") ? "True" : "False";
                    ssEnvConfig.ActiveSheet.SetRowHeight(i, ComNum.SPDROWHT);
                    if (ssEnvConfig.ActiveSheet.Cells[i, 2].Text == "True")
                    {
                        ssEnvConfig.ActiveSheet.Cells[i, 3].Text = "True";
                        ssEnvConfig.ActiveSheet.Cells[i, 3].Locked = true;
                    }                    
                }

                if (pUserId != "SUPERUSER")
                {
                    ssEnvConfig.ActiveSheet.Columns[2].Locked = true;                                    
                }
                else
                {
                    ssEnvConfig.ActiveSheet.Columns[3].Visible = false;
                }

                ssEnvConfig.ActiveSheet.Columns[1].Width = ssEnvConfig.ActiveSheet.Columns[1].GetPreferredWidth();
                ssEnvConfig.ActiveSheet.Columns[2].Width = 80;
                ssEnvConfig.ActiveSheet.Columns[3].Width = 80;
            }

            ODA.Dispose();
            ODA = null;
            cmd.Dispose();
            cmd = null;
            ds.Dispose();
            ds = null;

            //검토등급 Code -> Value ComboBox셋팅         
            
            for (int i = 0; i < ssEnvConfig.ActiveSheet.Rows.Count; i++)
            {
                string strRefId = ssEnvConfig.ActiveSheet.Cells[i, 13].Text;
                if (string.IsNullOrEmpty(strRefId))
                {
                    continue;
                }

                FarPoint.Win.Spread.CellType.ComboBoxCellType comboBoxCellType = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                cmd = new OracleCommand();
                ds = new DataSet();

                //환경설정옵션
                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.UP_KCODEDEFINITION";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("I_CODETYPE", OracleDbType.Varchar2, 3, strRefId.Trim(), ParameterDirection.Input);
                cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                ODA = new OracleDataAdapter(cmd);
                ODA.Fill(ds);

                DataTable dt = null;
                dt = ds.Tables[0];

                string[] ComboBoxItem = new string[dt.Rows.Count];
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    ComboBoxItem[j] = dt.Rows[j]["CODEDESCRIPTION2"].ToString();
                    if (dt.Rows[j]["CODEVALUE"].ToString().Trim() == ssEnvConfig.ActiveSheet.Cells[i, 5].Text) 
                    {
                        ssEnvConfig.ActiveSheet.Cells[i, 5].Text = dt.Rows[j]["CODEDESCRIPTION2"].ToString();
                    }
                }

                comboBoxCellType.Items = ComboBoxItem;
                ssEnvConfig.ActiveSheet.Cells[i, 5].CellType = comboBoxCellType;
                ssEnvConfig.ActiveSheet.Cells[i, 5].Locked = false;

                ODA.Dispose();
                ODA = null;
                cmd.Dispose();
                cmd = null;
                ds.Dispose();
                ds = null;
            }

            //for (int i = 0; i < ssEnvConfig.ActiveSheet.Columns.Count; i++)
            //{
            //    ssEnvConfig.ActiveSheet.Columns[i].Visible = true;
            //}
        }

        private void ssEnvConfig_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //모듈명 클릭 시
            if (e.Column == 1 && e.ColumnHeader == false)
            {
                pInstCD = ssEnvConfig.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                pModuleID = ssEnvConfig.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                pModuleNM = ssEnvConfig.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                pModuleGubun = ssEnvConfig.ActiveSheet.Cells[e.Row, 12].Text.Trim();

                ssEnvConfig.ActiveSheet.Cells[0, 1, ssEnvConfig.ActiveSheet.Rows.Count - 1, 1].BackColor = Color.White;
                ssEnvConfig.ActiveSheet.Cells[e.Row, 1].BackColor = Color.SkyBlue;

                OracleCommand cmd = new OracleCommand();
                ComDbB.PsmhDb pDbCon = clsDB.DbCon;
                DataSet ds = new DataSet();

                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.up_ScreenEnvExceptDrug";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pInstCD", OracleDbType.Varchar2, 20, pInstCD, ParameterDirection.Input);
                cmd.Parameters.Add("pUserId", OracleDbType.Varchar2, 20, pUserId, ParameterDirection.Input);
                cmd.Parameters.Add("pModuleID", OracleDbType.Varchar2, 20, pModuleID, ParameterDirection.Input);
                cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                OracleDataAdapter ODA = new OracleDataAdapter(cmd);
                ODA.Fill(ds);

                cmd.Dispose();
                cmd = null;

                if (ds.Tables.Count > 0)
                {
                    ssEnvExceptDrug.DataSource = ds.Tables[0];

                    ssEnvExceptDrug.ActiveSheet.Columns[1].Width = ssEnvExceptDrug.ActiveSheet.Columns[1].GetPreferredWidth();
                    ssEnvExceptDrug.ActiveSheet.Columns[2].Width = ssEnvExceptDrug.ActiveSheet.Columns[2].GetPreferredWidth
                        () + 20;
                }


                ds.Dispose();
                ds = null;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            DataTable dt = null;

            string USERID = "";
            string INSTCD = "";
            string MODULEID = "";
            string CHECKVALUE = "";
            string LV = "";
            string DUPALLOWDAYS = "";
            string WEIGHTLV = "";
            string PREGAGEMIN = "";
            string PREGAGEMAX = "";
            string ADDEDVALUESMIN = "";
            string ADDEDVALUESMAX = "";
            //string CURRENTFLAG = "";
            //string INPUTDT = "";
            OracleCommand cmd = new OracleCommand();
            DataSet ds = new DataSet();
            ComDbB.PsmhDb pDbCon = clsDB.DbCon;

            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < ssEnvConfig.ActiveSheet.RowCount; i++)
                {
                    USERID = pUserId;
                    INSTCD = ssEnvConfig.ActiveSheet.Cells[i, 4].Text;
                    MODULEID = ssEnvConfig.ActiveSheet.Cells[i, 0].Text;
                    if (USERID == "SUPERUSER")
                    {
                        CHECKVALUE = (ssEnvConfig.ActiveSheet.Cells[i, 2].Text == "True" ? "Y" : "N");
                    }
                    else
                    {
                        if (ssEnvConfig.ActiveSheet.Cells[i, 2].Text == "True")
                        {
                            continue;
                        }
                        CHECKVALUE = (ssEnvConfig.ActiveSheet.Cells[i, 3].Text == "True" ? "Y" : "N");
                    }
                    
                    //등급 Value -> Code
                    LV = ssEnvConfig.ActiveSheet.Cells[i, 5].Text;
                    if (string.IsNullOrEmpty(LV))
                    {
                        LV = "";
                    }
                    else
                    {
                        string strRefId = ssEnvConfig.ActiveSheet.Cells[i, 13].Text;

                        cmd = new OracleCommand();
                        ds = new DataSet();

                        cmd.Connection = pDbCon.Con;
                        cmd.InitialLONGFetchSize = 1000;
                        cmd.CommandText = "KOSMOS_DRUG.UP_KCODEDEFINITION";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("I_CODETYPE", OracleDbType.Varchar2, 3, strRefId.Trim(), ParameterDirection.Input);
                        cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                        OracleDataAdapter ODA = new OracleDataAdapter(cmd);
                        ODA.Fill(ds);

                        DataTable dt1 = null;
                        dt1 = ds.Tables[0];

                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            if (dt1.Rows[j]["CODEDESCRIPTION2"].ToString().Trim() == ssEnvConfig.ActiveSheet.Cells[i, 5].Text)
                            {
                                LV = dt1.Rows[j]["CODEVALUE"].ToString();
                            }
                        }

                        ODA.Dispose();
                        ODA = null;
                        cmd.Dispose();
                        cmd = null;
                        ds.Dispose();
                        ds = null;
                    }

                    DUPALLOWDAYS = (string.IsNullOrEmpty(ssEnvConfig.ActiveSheet.Cells[i, 6].Text) == true ? "" : ssEnvConfig.ActiveSheet.Cells[i, 6].Text);
                    WEIGHTLV = (string.IsNullOrEmpty(ssEnvConfig.ActiveSheet.Cells[i, 7].Text) == true ? "" : ssEnvConfig.ActiveSheet.Cells[i, 7].Text);
                    PREGAGEMIN = (string.IsNullOrEmpty(ssEnvConfig.ActiveSheet.Cells[i, 8].Text) == true ? "" : ssEnvConfig.ActiveSheet.Cells[i, 8].Text);
                    PREGAGEMAX = (string.IsNullOrEmpty(ssEnvConfig.ActiveSheet.Cells[i, 9].Text) == true ? "" : ssEnvConfig.ActiveSheet.Cells[i, 9].Text);
                    ADDEDVALUESMIN = (string.IsNullOrEmpty(ssEnvConfig.ActiveSheet.Cells[i, 10].Text) == true ? "" : ssEnvConfig.ActiveSheet.Cells[i, 10].Text);
                    ADDEDVALUESMAX = (string.IsNullOrEmpty(ssEnvConfig.ActiveSheet.Cells[i, 11].Text) == true ? "" : ssEnvConfig.ActiveSheet.Cells[i, 11].Text);
                    //CURRENTFLAG = "Y";

                    SQL = " SELECT INPUTDT, CHECKVALUE, LV, DUPALLOWDAYS, WEIGHTLV, PREGAGEMIN, PREGAGEMAX, ADDEDVALUESMIN, ADDEDVALUESMAX ";
                    SQL += ComNum.VBLF + " FROM KOSMOS_DRUG.SCNENV ";
                    SQL += ComNum.VBLF + " WHERE USERID = '" + USERID + "' ";
                    SQL += ComNum.VBLF + " AND MODULEID = '" + MODULEID + "' ";
                    SQL += ComNum.VBLF + " AND CURRENTFLAG = 'Y' ";
                    SQL += ComNum.VBLF + " ORDER BY INPUTDT DESC ";
                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("환경설정 조회중 문제가 발생했습니다");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //데이터가 없으면 삽입
                    if (dt.Rows.Count == 0)
                    {
                        SQL = " INSERT INTO KOSMOS_DRUG.SCNENV (USERID,MODULEID,CHECKVALUE,LV,DUPALLOWDAYS,WEIGHTLV,PREGAGEMIN,PREGAGEMAX,ADDEDVALUESMIN,ADDEDVALUESMAX,CURRENTFLAG,INPUTDT) ";
                        SQL += ComNum.VBLF + "VALUES('" + USERID + "' ";
                        SQL += ComNum.VBLF + "      ,'" + MODULEID + "' ";
                        SQL += ComNum.VBLF + "      ,'" + CHECKVALUE + "' ";
                        SQL += ComNum.VBLF + "      ,'" + LV + "' ";
                        SQL += ComNum.VBLF + "      ,'" + DUPALLOWDAYS + "' ";
                        SQL += ComNum.VBLF + "      ,'" + WEIGHTLV + "' ";
                        SQL += ComNum.VBLF + "      ,'" + PREGAGEMIN + "' ";
                        SQL += ComNum.VBLF + "      ,'" + PREGAGEMAX + "' ";
                        SQL += ComNum.VBLF + "      ,'" + ADDEDVALUESMIN + "' ";
                        SQL += ComNum.VBLF + "      ,'" + ADDEDVALUESMAX + "' ";
                        SQL += ComNum.VBLF + "      ,'Y' ";
                        SQL += ComNum.VBLF + "      ,sysdate) ";
                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            dt.Dispose();
                            dt = null;
                            Cursor.Current = Cursors.Default;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("환경설정 INSERT 문제가 발생했습니다");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                    else
                    {
                        //값이 하나라도 다르면
                        if (CHECKVALUE != dt.Rows[0]["CHECKVALUE"].ToString() || LV != dt.Rows[0]["LV"].ToString() || DUPALLOWDAYS != dt.Rows[0]["DUPALLOWDAYS"].ToString()
                            || WEIGHTLV != dt.Rows[0]["WEIGHTLV"].ToString() || PREGAGEMIN != dt.Rows[0]["PREGAGEMIN"].ToString() || PREGAGEMAX != dt.Rows[0]["PREGAGEMAX"].ToString()
                            || ADDEDVALUESMIN != dt.Rows[0]["ADDEDVALUESMIN"].ToString() || ADDEDVALUESMAX != dt.Rows[0]["ADDEDVALUESMAX"].ToString())
                        {
                            SQL = " UPDATE KOSMOS_DRUG.SCNENV ";
                            SQL += ComNum.VBLF + " SET CURRENTFLAG = 'N' ";
                            SQL += ComNum.VBLF + " WHERE USERID = '" + USERID + "' ";
                            SQL += ComNum.VBLF + " AND MODULEID = '" + MODULEID + "' ";
                            SQL += ComNum.VBLF + " AND CHECKVALUE = 'Y' ";
                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                dt.Dispose();
                                dt = null;
                                Cursor.Current = Cursors.Default;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("환경설정 UPDATE 문제가 발생했습니다");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }

                            SQL = " INSERT INTO KOSMOS_DRUG.SCNENV (USERID,MODULEID,CHECKVALUE,LV,DUPALLOWDAYS,WEIGHTLV,PREGAGEMIN,PREGAGEMAX,ADDEDVALUESMIN,ADDEDVALUESMAX,CURRENTFLAG,INPUTDT) ";
                            SQL += ComNum.VBLF + "VALUES('" + USERID + "' ";
                            SQL += ComNum.VBLF + "      ,'" + MODULEID + "' ";
                            SQL += ComNum.VBLF + "      ,'" + CHECKVALUE + "' ";
                            SQL += ComNum.VBLF + "      ,'" + LV + "' ";
                            SQL += ComNum.VBLF + "      ,'" + DUPALLOWDAYS + "' ";
                            SQL += ComNum.VBLF + "      ,'" + WEIGHTLV + "' ";
                            SQL += ComNum.VBLF + "      ,'" + PREGAGEMIN + "' ";
                            SQL += ComNum.VBLF + "      ,'" + PREGAGEMAX + "' ";
                            SQL += ComNum.VBLF + "      ,'" + ADDEDVALUESMIN + "' ";
                            SQL += ComNum.VBLF + "      ,'" + ADDEDVALUESMAX + "' ";
                            SQL += ComNum.VBLF + "      ,'Y' ";
                            SQL += ComNum.VBLF + "      ,sysdate) ";
                            SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                dt.Dispose();
                                dt = null;
                                Cursor.Current = Cursors.Default;
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("환경설정 INSERT 문제가 발생했습니다");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                    }
                }

                dt.Dispose();
                dt = null;
                clsDB.setCommitTran(clsDB.DbCon);
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                MessageBox.Show(ex.Message + "\n환경설정 트랜잭션 오류");
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        private void btnExceptDrug_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pModuleID))
            {
                MessageBox.Show("위에서 해당 모듈명을 클릭해주십시오.");
                return;
            }

            if (pInstCD != "PH")
            {
                MessageBox.Show("ALERT여부 체크 후 제외약품 등록 가능합니다(환경설정값 저장이 되어야 합니다.)");
                return;
            }

            if (ssEnvConfig.ActiveSheet.Cells[ssEnvConfig.ActiveSheet.ActiveRowIndex, 12].Text == "H" && pUserId != "SUPERUSER")
            {
                MessageBox.Show("심평원 모듈은 SUPERUSER만 제외약품 등록이 가능합니다.");
                return;
            }

            frmMedScreenExceptDrug frm = new frmMedScreenExceptDrug(pInstCD, pUserId, pModuleID, pModuleNM, pModuleGubun);
            frm.ShowDialog();
        }

        private void ssEnvConfig_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            //슈퍼유저 2칼럼이나 일반유저 3칼럼 체크설정에 따라 기관코드 설정해준다.
            if (e.Column == 2 || e.Column == 3) 
            {
                if (ssEnvConfig.ActiveSheet.Cells[ssEnvConfig.ActiveSheet.ActiveRowIndex, e.Column].Text == "True")
                {
                    ssEnvConfig.ActiveSheet.Cells[ssEnvConfig.ActiveSheet.ActiveRowIndex, 4].Text = "PH";
                    pInstCD = "PH";
                }
                else
                {
                    ssEnvConfig.ActiveSheet.Cells[ssEnvConfig.ActiveSheet.ActiveRowIndex, 4].Text = "";
                    pInstCD = "";
                }
            }
        }
    }
}
