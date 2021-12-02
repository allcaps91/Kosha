using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewChartHelp.cs
    /// Description     : 성명, 생년월일로 차트검색
    /// Author          : 안정수
    /// Create Date     : 2017-08-14
    /// Update History  : 2017-10-24
    /// 이벤트 처리 부분 수정, 실제 테스트 필요함
    /// <history> 
    /// 실제 테스트 필요
    /// d:\psmh\OPD\oumsad\OUMSAD25.FRM(FrmChartHelp) => frmPmpaViewChartHelp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\OUMSAD25.FRM(FrmChartHelp)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewChartHelp : Form
    {
        string[] strData = new string[100];
        int nReadCount = 0;
        string FstrJob = "";

        public delegate void SendsValue(string GstrValue);
        public event SendsValue SendsValueX;

        public frmPmpaViewChartHelp()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnEvent);

            this.txtName.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtBirth.GotFocus += new EventHandler(eControl_GotFocus);

            this.txtBirth.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtName.LostFocus += new EventHandler(eControl_LostFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            txtName.Text = "";
            txtBirth.Text = "";

            ssList_Sheet1.Rows.Count = 0;
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            if (sender == this.txtName)
            {
                txtName.ImeMode = ImeMode.Hangul;
            }

            else if (sender == this.txtBirth)
            {
                if (txtName.Text.Length < 2)
                {
                    txtName.Text = "";
                    txtName.Focus();
                }
            }
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtBirth)
            {
                nReadCount = 0;

                if (clsPmpaPb.GstrPanoGbn == "P")
                {
                    Bas_Patient_Read(); //환자 마스타를 검색함                    
                }
                else
                {
                    OCS_EtcPano_Read(); //수탁검사 마스타                    
                }

                //자료가 1건이면
                if (nReadCount == 1)
                {
                    SendsValueX(strData[0]);
                    return;
                }

                //자료가 1건도 없으면
                if (nReadCount == 0)
                {
                    SendsValueX("");
                    this.Close();
                    return;
                }
            }

            else if (sender == this.txtName)
            {
                txtName.ImeMode = ImeMode.Hangul;
            }
        }

        void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtBirth_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void OCS_EtcPano_Read()
        {
            string strshow = "";
            string strSname = "";
            string strPano = "";
            string strJumin1 = "";
            string strJumin2 = "";
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                             ";
            SQL += ComNum.VBLF + "  Pano,Sname,Jumin1,Jumin2,Jumin3                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ETCPANO              ";
            SQL += ComNum.VBLF + "WHERE 1=1                                          ";
            SQL += ComNum.VBLF + "      AND Sname = '" + txtName.Text + "'           ";
            SQL += ComNum.VBLF + "      AND Jumin1 = '" + txtBirth.Text + "'         ";
            SQL += ComNum.VBLF + "ORDER BY Sname,Jumin1                              ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당하는 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 1)
                {
                    txtPan.Text = "커서로 이동후 Space를 친후 Enter를 치세요";
                    txtPan.ForeColor = Color.Blue;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strSname = dt.Rows[i]["Sname"].ToString().Trim();
                    strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                    strJumin2 = clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());

                    for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = strPano;
                        ssList_Sheet1.Cells[i, 1].Text = strSname;
                        ssList_Sheet1.Cells[i, 2].Text = strJumin1 + "-" + strJumin2;
                        nReadCount += 1;
                        strshow = strPano + " " + strSname + " " + strJumin1 + "-" + strJumin2;
                        strData[nReadCount - 1] = strshow;
                    }

                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;

        }

        void Bas_Patient_Read()
        {
            string strshow = "";
            string strSname = "";
            string strPano = "";
            string strJumin1 = "";
            string strJumin2 = "";
            int i = 0;

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  Pano,Sname,Jumin1,Jumin2,Jumin3";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_Patient";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Sname = '" + txtName.Text + "'";
            SQL += ComNum.VBLF + "      AND Jumin1 = '" + txtBirth.Text + "'";
            SQL += ComNum.VBLF + "ORDER BY Sname,Jumin1";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("해당하는 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 1)
                {
                    txtPan.Text = "커서로 이동후 Space를 친후 Enter를 치세요";
                    txtPan.ForeColor = Color.Blue;
                }

                if (dt.Rows.Count > 0)
                {
                    ssList_Sheet1.Rows.Count = dt.Rows.Count;

                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strSname = dt.Rows[i]["Sname"].ToString().Trim();
                    strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();

                    //주민암호화
                    if (dt.Rows[i]["Jumin3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                    }

                    strshow = strPano + " " + strSname + " " + strJumin1 + "-" + strJumin2;

                    for (i = 0; i < ssList_Sheet1.Rows.Count; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = strPano;
                        ssList_Sheet1.Cells[i, 1].Text = strSname;
                        ssList_Sheet1.Cells[i, 2].Text = strJumin1 + "-" + strJumin2;
                        nReadCount += 1;

                        strshow = strPano + " " + strSname + " " + strJumin1 + "-" + strJumin2;
                        strData[nReadCount - 1] = strshow;
                    }

                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            dt.Dispose();
            dt = null;
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SendsValueX(strData[e.Row]);
            this.Close();
        }

        void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            SendsValueX(strData[e.Row]);
            this.Close();
        }


    }
}

