using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmPersonalSelectionView
    /// File Name : frmPersonalSelectionView.cs
    /// Title or Description : 개인 선택진료 정보
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : 공통함수 READ_ROAD_JUSO를 clsVbfunc.GetRoadJuSo로 대체.
    ///                    공통함수 READ_BAS_Mail을 clsVbfunc.GetBASMail로 대체.
    ///                    공통함수 READ_BCODE_Name을 clsVbfunc.GetBCODENameCode로 대체
    ///                    공통함수 READ_BAS_Doctor를 clsVbfunc.GetBASDoctorName으로 대체
    ///                    공통함수 READ_INSA_Name을 clsVbfunc.GetInSaName으로 대체
    /// </summary>
    /// <history>  
    /// VB\Frm개인선택진료정보.frm(Frm개인선택진료정보) -> frmPersonalSelectionView.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\buppat\Frm개인선택진료정보.frm(Frm개인선택진료정보)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\buppat\\buppat.vbp
    /// </vbp>
    public partial class frmPersonalSelectionView : Form
    {
        private string gstrRetValue = "";

        public frmPersonalSelectionView()
        {
            InitializeComponent();
        }

        public frmPersonalSelectionView(string strRetValue)
        {
            InitializeComponent();
            gstrRetValue = strRetValue;
        }

        private void chkNonDelete_CheckedChanged(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            search();
        }
      
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            search();
        }

        private void search()
        {
            string strPano = "";
            string strDel = "";

            txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
            strPano = txtPano.Text;

            if (chkNonDelete.Checked == true)
            {
                strDel = "Y";
            }
            else
            {
                strDel = "N";
            }

            Pano_Info_DISP_MST(strPano);

            personal_History(strPano, ssView_Sheet1, strDel);
            personal_History2(strPano, ssView2_Sheet1, strDel);
        }

        private void frmPersonalSelectionView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            txtPano.Text = "";
            if (gstrRetValue != "")
            {
                txtPano.Text = VB.Format(gstrRetValue, "00000000");
                search();
            }

            gstrRetValue = "";
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (e.KeyCode == Keys.Enter)
            {
                search();
            }
        }

        private void Pano_Info_DISP_MST(string argPano)
        {
            //환자정보 체크

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            try
            {
                SQL = "";
                SQL = " SELECT a.Pano,a.SName,a.Jumin1,a.Jumin2,a.Jumin3, a.Juso,a.ZipCode1 || a.ZipCode1 AS ZipCode,a.Tel,a.Hphone, ";
                SQL = SQL + ComNum.VBLF + "  b.DelDate,a.BuildNo,a.RoadDetail,b.ROWID ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_PATIENT a, " + ComNum.DB_PMPA + "BAS_SELECT_MST b ";
                SQL = SQL + ComNum.VBLF + "   WHERE a.PANO =b.Pano(+) ";
                SQL = SQL + ComNum.VBLF + "    AND a.PANO ='" + argPano + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("환자정보가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssInfo_Sheet1.Cells[0, 0].Text = argPano;
                    ssInfo_Sheet1.Cells[0, 1].Text = dt.Rows[0]["SName"].ToString().Trim();

                    //주민암호화
                    if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                    {
                        ssInfo_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        ssInfo_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + dt.Rows[0]["Jumin2"].ToString().Trim();
                    }

                    if (dt.Rows[0]["BuildNo"].ToString().Trim() != "")
                    {
                        ssInfo_Sheet1.Cells[0, 3].Text = clsVbfunc.GetRoadJuSo(clsDB.DbCon, dt.Rows[0]["BuildNo"].ToString().Trim()) + " " + dt.Rows[0]["RoadDetail"].ToString().Trim();
                    }
                    else
                    {
                        ssInfo_Sheet1.Cells[0, 3].Text = clsVbfunc.GetBASMail(clsDB.DbCon, dt.Rows[0]["ZipCode"].ToString().Trim()) + " " + dt.Rows[0]["Juso"].ToString().Trim();
                    }
                    ssInfo_Sheet1.Cells[0, 4].Text = dt.Rows[0]["Tel"].ToString().Trim() + ComNum.VBLF + dt.Rows[0]["Hphone"].ToString().Trim();
                }

                lblReduction.Text = "";
                //재원상태 감액정보 읽기
                SQL = "";
                SQL = " SELECT GAMCODE From BAS_GAMF Where GAMJUMIN3 = '"
                        + clsAES.AES(dt.Rows[0]["Jumin1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim())) + "' ";
                SQL = SQL + ComNum.VBLF + " AND (GAMEND IS NULL OR GAMEND ='') ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                
                if(dt1.Rows.Count > 0)
                {
                    lblReduction.Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "2", "BAS_감액코드명", dt1.Rows[0]["GAMCODE"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void personal_History(string strPano, SheetView ssView_Sheet1, string strDel)
        {
            int nX = 0;
            int nReadx = 0;
            string strIO = "";
            string strUse = "";
            string strNew = "";
            string strOld = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "  SELECT Pano,SName,DECODE(Gubun,'O','외래','I','입원',GUBUN) Gubun,";
                SQL = SQL + ComNum.VBLF + " Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9, ";
                SQL = SQL + ComNum.VBLF + " Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9, ";
                SQL = SQL + ComNum.VBLF + " DeptCode,DrCode,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDate,'YYYY-MM-DD') SDate,TO_CHAR(EDate,'YYYY-MM-DD') EDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI:SS') EntDate,TO_CHAR(EntDate2,'YY-MM-DD HH24:MI:SS') EntDate2,EntSabun ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST  ";
                SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + strPano + "' ";
                if (strDel == "Y")
                {
                    SQL = SQL + ComNum.VBLF + "    AND (DelDate IS NULL OR DelDate ='') ";
                }
                SQL = SQL + ComNum.VBLF + "   ORDER BY Gubun,DrCode,SDate DESC";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                nReadx = dt.Rows.Count;
                ssView_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = nReadx;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (nX = 0; nX < nReadx - 1; nX++)
                {
                    strNew = dt.Rows[nX]["DrCode"].ToString().Trim() + dt.Rows[nX]["Gubun"].ToString().Trim();

                    if (strNew != strOld)
                    {
                        ssView_Sheet1.Cells[nX, 0].Text = dt.Rows[nX]["Pano"].ToString().Trim();
                        strIO = dt.Rows[nX]["Gubun"].ToString().Trim();
                        ssView_Sheet1.Cells[nX, 1].Text = strIO;
                        if (strIO == "외래")
                        {
                            ssView_Sheet1.Cells[nX, 1].BackColor = Color.FromArgb(255, 255, 128);
                        }
                        else if (strIO == "입원")
                        {
                            ssView_Sheet1.Cells[nX, 1].BackColor = Color.FromArgb(255, 255, 128);
                        }

                        ssView_Sheet1.Cells[nX, 2].Text = dt.Rows[nX]["DeptCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nX, 3].Text = dt.Rows[nX]["DrCode"].ToString().Trim();
                        ssView_Sheet1.Cells[nX, 4].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[nX]["DrCode"].ToString().Trim());
                    }

                    strUse = "Y";
                    if (dt.Rows[nX]["DelDate"].ToString().Trim() != "")
                    {
                        strUse = "";
                    }
                    ssView_Sheet1.Cells[nX, 5].Text = dt.Rows[nX]["SDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 6].Text = dt.Rows[nX]["EDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 7].Text = dt.Rows[nX]["DelDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 8].Text = dt.Rows[nX]["EntDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[nX]["EntSabun"].ToString().Trim());
                    ssView_Sheet1.Cells[nX, 10].Text = dt.Rows[nX]["EntDate2"].ToString().Trim();

                    ssView_Sheet1.Cells[nX, 12].Text = dt.Rows[nX]["Set1"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 13].Text = dt.Rows[nX]["Set2"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 14].Text = dt.Rows[nX]["Set3"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 15].Text = dt.Rows[nX]["Set4"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 16].Text = dt.Rows[nX]["Set5"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 17].Text = dt.Rows[nX]["Set6"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 18].Text = dt.Rows[nX]["Set7"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 19].Text = dt.Rows[nX]["Set8"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 20].Text = dt.Rows[nX]["Set9"].ToString().Trim();

                    ssView_Sheet1.Cells[nX, 22].Text = dt.Rows[nX]["Setc1"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 23].Text = dt.Rows[nX]["Setc2"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 24].Text = dt.Rows[nX]["Setc3"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 25].Text = dt.Rows[nX]["Setc4"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 26].Text = dt.Rows[nX]["Setc5"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 27].Text = dt.Rows[nX]["Setc6"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 28].Text = dt.Rows[nX]["Setc7"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 29].Text = dt.Rows[nX]["Setc8"].ToString().Trim();
                    ssView_Sheet1.Cells[nX, 30].Text = dt.Rows[nX]["Setc9"].ToString().Trim();

                    strOld = dt.Rows[nX]["DrCode"].ToString().Trim() + dt.Rows[nX]["Gubun"].ToString().Trim(); ;

                    if (strUse != "Y")
                    {
                        ssView_Sheet1.Cells[nX, 5, nX, 7].BackColor = Color.FromArgb(255, 155, 155);
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void personal_History2(string strPano, SheetView ssView2_Sheet1, string strDel)
        {
            int nX = 0;
            int nReadx = 0;
            string strIO = "";
            string strUse = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL = "  SELECT Pano,SName,DECODE(Gubun,'O','외래','I','입원',GUBUN) Gubun,";
                SQL = SQL + ComNum.VBLF + " Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9, ";
                SQL = SQL + ComNum.VBLF + " Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9, ";
                SQL = SQL + ComNum.VBLF + " DeptCode,DrCode,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDate,'YYYY-MM-DD') SDate,TO_CHAR(EDate,'YYYY-MM-DD') EDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(DelDate,'YYYY-MM-DD') DelDate, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EntDate,'YYYY-MM-DD HH24:MI:SS') EntDate,TO_CHAR(EntDate2,'YY-MM-DD HH24:MI:SS') EntDate2,EntSabun ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SELECT_MST  ";
                SQL = SQL + ComNum.VBLF + "   WHERE Pano='" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY Gubun,DrCode,EntDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                nReadx = dt.Rows.Count;
                ssView2_Sheet1.RowCount = 0;
                ssView2_Sheet1.RowCount = nReadx;
                ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (nX = 0; nX < nReadx - 1; nX++)
                {
                    ssView2_Sheet1.Cells[nX, 0].Text = dt.Rows[nX]["Pano"].ToString().Trim();
                    strIO = dt.Rows[nX]["Gubun"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 1].Text = strIO;
                    if (strIO == "외래")
                    {
                        ssView2_Sheet1.Cells[nX, 1].BackColor = Color.FromArgb(255, 255, 128);
                    }
                    else if (strIO == "입원")
                    {
                        ssView2_Sheet1.Cells[nX, 1].BackColor = Color.FromArgb(187, 255, 187);
                    }

                    ssView2_Sheet1.Cells[nX, 2].Text = dt.Rows[nX]["DeptCode"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 3].Text = dt.Rows[nX]["DrCode"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 4].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[nX]["DrCode"].ToString().Trim());
                    strUse = "Y";
                    if (dt.Rows[nX]["DelDate"].ToString().Trim() != "")
                    {
                        strUse = "";
                    }
                    ssView2_Sheet1.Cells[nX, 5].Text = dt.Rows[nX]["SDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 6].Text = dt.Rows[nX]["EDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 7].Text = dt.Rows[nX]["DelDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 8].Text = dt.Rows[nX]["EntDate"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 9].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[nX]["EntSabun"].ToString().Trim());
                    ssView2_Sheet1.Cells[nX, 10].Text = dt.Rows[nX]["EntDate2"].ToString().Trim();

                    ssView2_Sheet1.Cells[nX, 12].Text = dt.Rows[nX]["Set1"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 13].Text = dt.Rows[nX]["Set2"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 14].Text = dt.Rows[nX]["Set3"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 15].Text = dt.Rows[nX]["Set4"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 16].Text = dt.Rows[nX]["Set5"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 17].Text = dt.Rows[nX]["Set6"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 18].Text = dt.Rows[nX]["Set7"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 19].Text = dt.Rows[nX]["Set8"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 20].Text = dt.Rows[nX]["Set9"].ToString().Trim();

                    ssView2_Sheet1.Cells[nX, 22].Text = dt.Rows[nX]["Setc1"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 23].Text = dt.Rows[nX]["Setc2"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 24].Text = dt.Rows[nX]["Setc3"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 25].Text = dt.Rows[nX]["Setc4"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 26].Text = dt.Rows[nX]["Setc5"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 27].Text = dt.Rows[nX]["Setc6"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 28].Text = dt.Rows[nX]["Setc7"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 29].Text = dt.Rows[nX]["Setc8"].ToString().Trim();
                    ssView2_Sheet1.Cells[nX, 30].Text = dt.Rows[nX]["Setc9"].ToString().Trim();

                    if (strUse != "Y")
                    {
                        ssView2_Sheet1.Cells[nX, 5, nX, 7].BackColor = Color.FromArgb(255, 155, 155);
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
