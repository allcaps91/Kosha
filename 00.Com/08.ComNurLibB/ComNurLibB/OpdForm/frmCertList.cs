using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComNurLibB
{
    /// <summary>   
    /// File Name       : frmCertList.cs
    /// Description     : 진단서 리스트
    /// Author          : 유진호
    /// Create Date     : 2017-02-01
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmCertList
    /// </history>
    /// </summary>
    public partial class frmCertList : Form
    {
        ComFunc CF = new ComFunc();

        public frmCertList()
        {
            InitializeComponent();
        }

        private void frmCertList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-7);
            dtpTDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchClick()
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPANO  = "";
            string strSDATE = "";
            string strEDATE = "";
                
            //Dim StrTemp()   As String

            try
            {
                if (VB.Trim(txtPANO.Text) == "")
                {
                    ComFunc.MsgBox("등록번호가 공란입니다.");
                    return;
                }

                ssCerti_Sheet1.RowCount = 0;

                strPANO = VB.Trim(txtPANO.Text);
                strSDATE = dtpFDate.Value.ToShortDateString();
                strEDATE = dtpTDate.Value.ToShortDateString();
                
                //'증명서 조회 기능 확장 2012-10-11 이주형
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT '1' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI01 " ;                       //'01 일반진단서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '2' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI02 " ;                 //'02 상해진단서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '3' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI03 " ;                 //'03 병사용진단서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '8' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI08 " ;                 //'08 진료소견서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '12' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI12 " ;                //'12 진료회송서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '14' GUBUN, LSDATE, '' DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI14 " ;             //'14 건강진단서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '18' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI18 " ;                //'18 진료의뢰서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + " SELECT '4' GUBUN, ACTDATE, DEPTCODE, DRCODE, TO_CHAR(PRTDATE) SEND, '' MCNO FROM KOSMOS_PMPA.ETC_WONSELU " ;    //'진료사실증명서
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + "  SELECT '5' GUBUN, LSDATE, DEPTCODE, DRNAME, SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI05 " ;                     //'사망진단서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND LSDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND LSDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " UNION ALL " ;
                SQL = SQL + ComNum.VBLF + "  SELECT '26' GUBUN, BALDATE LSDATE, '' DEPTCODE, DRNAME, '' SEND, MCNO FROM KOSMOS_OCS.OCS_MCCERTIFI26 " ;      //'의료급여의뢰서
                SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + strPANO + "'" ;
                SQL = SQL + ComNum.VBLF + "   AND BALDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "   AND BALDATE <= " + ComFunc.ConvOraToDate(dtpTDate.Value, "D");
                SQL = SQL + ComNum.VBLF + " ORDER BY 2 DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if (dt.Rows.Count > 0)
                {
                    ssCerti_Sheet1.RowCount = dt.Rows.Count;
                    ssCerti_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCerti_Sheet1.Cells[i, 0].Text = strPANO;
                        ssCerti_Sheet1.Cells[i, 1].Text = CF.Read_Patient(clsDB.DbCon, strPANO, "2");
                        ssCerti_Sheet1.Cells[i, 2].Text = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPANO);
                        ssCerti_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssCerti_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        //'증명서 발급 확인 기능 확장 2012-10-11 이주형
                        switch (dt.Rows[i]["GUBUN"].ToString().Trim())
                        {
                            case "1":
                                ssCerti_Sheet1.Cells[i, 6].Text = "일반진단서";
                                break;                                
                            case "2":
                                ssCerti_Sheet1.Cells[i, 6].Text = "상해진단서";
                                break;
                            case "3":
                                ssCerti_Sheet1.Cells[i, 6].Text = "병사용진단서";
                                break;
                            case "8":
                                ssCerti_Sheet1.Cells[i, 6].Text = "진료소견서";
                                break;
                            case "12":
                                ssCerti_Sheet1.Cells[i, 6].Text = "진료회송서";
                                break;
                            case "14":
                                ssCerti_Sheet1.Cells[i, 6].Text = "건강진단서";
                                break;
                            case "18":
                                ssCerti_Sheet1.Cells[i, 6].Text = "진료의뢰서";
                                break;
                            case "26":
                                ssCerti_Sheet1.Cells[i, 6].Text = "의료급여의뢰서";
                                break;
                            case "5":
                                ssCerti_Sheet1.Cells[i, 6].Text = "사망진단서";
                                break;
                            case "4":
                                {
                                    //'진료사실증명서 테이블에 있는 사번으로 이름 가져옴
                                    SQL = "SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN = '" + dt.Rows[i]["DRNAME"].ToString().Trim() + "' ";
                                    SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                                    if (SqlErr != "")
                                    {
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        return;
                                    }

                                    if (dt2.Rows.Count > 0)
                                    {
                                        ssCerti_Sheet1.Cells[i, 4].Text = dt2.Rows[0]["KORNAME"].ToString().Trim();
                                    }
                                    dt2.Dispose();
                                    dt2 = null;

                                    ssCerti_Sheet1.Cells[i, 6].Text = "진료사실증명서";
                                }
                                break;
                        }
                        
                        ssCerti_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssCerti_Sheet1.Cells[i, 10].Text = dt.Rows[i]["MCNO"].ToString().Trim();
                        ssCerti_Sheet1.Cells[i, 11].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        if (VB.IsDate(dt.Rows[i]["LSDATE"].ToString().Trim()) == true)
                        {
                            ssCerti_Sheet1.Cells[i, 7].Text = Convert.ToDateTime(dt.Rows[i]["LSDATE"].ToString().Trim()).ToShortDateString();
                        }
                        

                        //'증명서 발급 및 원무과 전송 구분 추가 2013-01-07 이주형
                        //'증명서 발급구분 세분화(작성, 전송, 발급) 수정 2013-04-25 이주형
                        if (dt.Rows[i]["SEND"].ToString().Trim() == "Y")
                        {
                            ssCerti_Sheet1.Cells[i, 8].Text = "전송";
                            ssCerti_Sheet1.Cells[i, 8].BackColor = Color.FromArgb(210, 210, 250);
                        }
                        else if (dt.Rows[i]["SEND"].ToString().Trim() == "P")
                        {
                            ssCerti_Sheet1.Cells[i, 8].Text = "발급";
                            ssCerti_Sheet1.Cells[i, 8].BackColor = Color.FromArgb(157, 206, 255);
                        }                            
                        else if (dt.Rows[i]["GUBUN"].ToString().Trim() == "4" && dt.Rows[i]["SEND"].ToString().Trim() != "")
                        {
                            ssCerti_Sheet1.Cells[i, 8].Text = "발급";
                            ssCerti_Sheet1.Cells[i, 8].BackColor = Color.FromArgb(157, 206, 255);
                        }
                        else
                        {
                            ssCerti_Sheet1.Cells[i, 8].Text = "작성";
                            ssCerti_Sheet1.Cells[i, 8].BackColor = Color.FromArgb(255, 255, 255);                            
                        }                        
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void txtPANO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPANO.Text = ComFunc.LPAD(txtPANO.Text, 8, "0");
                txtSNAME.Text = CF.Read_Patient(clsDB.DbCon, txtPANO.Text, "2");
            }
        }
    }
}
