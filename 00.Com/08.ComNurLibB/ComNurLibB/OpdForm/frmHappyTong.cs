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
    /// File Name       : frmHappyTong.cs
    /// Description     : 해피콜통계
    /// Author          : 유진호
    /// Create Date     : 2018-01-16
    /// <history>       
    /// D:\포항성모병원 VB Source(2017.11.20)\emr\emrprt\FrmHappyTong
    /// </history>
    /// </summary>
    public partial class frmHappyTong : Form
    {
        ComFunc CF = new ComFunc();
        clsQuery Query = new clsQuery();

        //2019-01-03 안정수, 영상의학과 사용자 구별을 위하여 추가함 
        private string GstrUserGubun = "";

        public frmHappyTong()
        {
            InitializeComponent();
        }

        private void frmHappyTong_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            
            //2019-01-03 안정수, 영상의학과 사용자 여부 Check
            //진정액팅 메뉴사용권한 체크            
            DataTable dt = Query.Get_BasBcode(clsDB.DbCon, "C#_XRAY_진정액팅사용자", clsType.User.IdNumber, "", "", "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                GstrUserGubun = "ok";
            }
            else
            {
                GstrUserGubun = "";
            }
            dt.Dispose();
            dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);
            initFrm();
            setCombo();
        }

        private void initFrm()
        {
            ssView_Sheet1.RowCount = 0;

            dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-10);
            dtpEDate.Value = Convert.ToDateTime(clsPublic.GstrSysTime);

            if (GstrUserGubun == "")
            {
                cboGubun1.Items.Clear();
                cboGubun1.Items.Add("**.전체");
                cboGubun1.Items.Add("01.특수검사실");
                cboGubun1.Items.Add("02.주사실");
                cboGubun1.Items.Add("03.외래스테이션");
                cboGubun1.Items.Add("04.XRAY 진정 명단");
                cboGubun1.Items.Add("05.내시경실");
                cboGubun1.Items.Add("06.내시경예약관리");
                cboGubun1.SelectedIndex = 0;
            }
            else
            {
                cboGubun1.Items.Clear();                
                cboGubun1.Items.Add("04.XRAY 진정 명단");
                cboGubun1.Items.Add("07.XRAY 조영제 명단");
                cboGubun1.SelectedIndex = 0;
            }

            cboGubun2.Items.Clear();
            cboGubun2.Items.Add("**.전체");
            cboGubun2.Items.Add("01.예약부도");
            cboGubun2.Items.Add("02.예약안내");
            cboGubun2.Items.Add("03.환자안부");
            cboGubun2.SelectedIndex = 0;
        }

        private void setCombo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                cboDept.Items.Clear();
                cboDept.Items.Add("**");

                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DEPTCODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                    }
                }

                cboDept.SelectedIndex = 0;

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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            btnSearchClick();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            btnPrintClick();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearchClick()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPANO = "";
            string strGUBUN1 = "";
            string strGUBUN2 = "";
            string strDEPT = "";

            Cursor.Current = Cursors.WaitCursor;
            ssView_Sheet1.RowCount = 0;

            try
            {
                strGUBUN2 = VB.Left(VB.Trim(cboGubun2.Text), 2);
                strGUBUN1 = VB.Left(VB.Trim(cboGubun1.Text), 2);
                strDEPT = VB.Trim(cboDept.Text);


                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT GUBUN, GUBUN2, BDATE, DEPTCODE, PANO, WRITESABUN, GUBUN3, (SELECT JUMIN2 FROM KOSMOS_PMPA.BAS_PATIENT WHERE A.PANO = PANO) JUMIN2";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_HAPPYCALL_OPD A";
                SQL = SQL + ComNum.VBLF + "  WHERE BDATE >= " + ComFunc.ConvOraToDate(dtpFDate.Value, "D");
                SQL = SQL + ComNum.VBLF + "    AND BDATE <= " + ComFunc.ConvOraToDate(dtpEDate.Value, "D");
                if (strGUBUN2 != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND GUBUN2 = '" + strGUBUN2 + "' ";
                }

                if (strGUBUN1 != "**")
                {                    
                    SQL = SQL + ComNum.VBLF + "  AND GUBUN = '" + strGUBUN1 + "' ";                    
                }

                if (strDEPT != "**")
                {
                    SQL = SQL + ComNum.VBLF + "  AND DEPTCODE = '" + strDEPT + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND GUBUN2 IS NOT NULL";
                SQL = SQL + ComNum.VBLF + "  ORDER BY GUBUN2, GUBUN, BDATE, DEPTCODE";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPANO = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 0].Text = READ_GUBUN2(dt.Rows[i]["GUBUN2"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 1].Text = READ_GUBUN1(dt.Rows[i]["GUBUN"].ToString().Trim());
                        if (VB.IsDate(dt.Rows[i]["BDATE"].ToString().Trim()) == true)
                        {
                            ssView_Sheet1.Cells[i, 2].Text = Convert.ToDateTime(dt.Rows[i]["BDATE"].ToString().Trim()).ToShortDateString();
                        }                        
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = strPANO; 
                        ssView_Sheet1.Cells[i, 5].Text = clsVbfunc.GetPatientName(clsDB.DbCon, strPANO);
                        ssView_Sheet1.Cells[i, 6].Text = clsVbfunc.READ_AGE_GESAN(clsDB.DbCon, strPANO) + "/" + clsVbfunc.READ_SEX(clsDB.DbCon, strPANO);
                        ssView_Sheet1.Cells[i, 7].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["WRITESABUN"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 8].Text = READ_GUBUN2(dt.Rows[i]["GUBUN3"].ToString().Trim());
                    }
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {                
                dt.Dispose();
                dt = null;                
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnPrintClick()
        {
            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            string SysDate = "";

            SysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb0 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/n/n/f1/C 외래 해피콜 통계" + "/n/n/n/n";
            strHead2 = "/l/f2" + "조회기간 : " + dtpFDate.Value.ToString("yyyy-MM-dd") + "~" + dtpEDate.Value.ToString("yyyy-MM-dd") + VB.Space(10) + "인쇄일자 : " + SysDate;

            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Margin.Left = 35;
            ssView_Sheet1.PrintInfo.Margin.Right = 0;
            ssView_Sheet1.PrintInfo.Margin.Top = 35;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 30;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = false;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);
        }

        private string READ_GUBUN1(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "01":
                    rtnVal = "특수검사실";
                    break;
                case "02":
                    rtnVal = "주사실";
                    break;
                case "03":
                    rtnVal = "외래 스테이션";
                    break;
                case "04":
                    rtnVal = "XRAY 진정 명단";
                    break;
                case "05":
                    rtnVal = "내시경실";
                    break;
                case "06":
                    rtnVal = "내시경예약관리";
                    break;
                case "07":
                    rtnVal = "XRAY 조영제 명단";
                    break;
            }

            return rtnVal;
        }

        private string READ_GUBUN2(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "01":
                    rtnVal = "예약부도";
                    break;
                case "02":
                    rtnVal = "예약안내";
                    break;
                case "03":
                    rtnVal = "환자안부";
                    break;
            }

            return rtnVal;
        }

        private string READ_GUBUN3(string strGubun)
        {
            string rtnVal = "";

            switch (strGubun)
            {
                case "1": rtnVal = "EKG";
                    break;
                case "2": rtnVal = "뇌파";
                    break;
                case "3": rtnVal = "ECHO";
                    break;
                case "4": rtnVal = "PFT";
                    break;
                case "5": rtnVal = "MCT";
                    break;
                case "6": rtnVal = "청력검사";
                    break;
                case "7": rtnVal = "ABI (동맥경화)";
                    break;
                case "8": rtnVal = "SKIN PRICK TEST";
                    break;
                case "9": rtnVal = "ABP(24HBP)";
                    break;
                case "10": rtnVal = "EKG Holter";
                    break;
                case "11": rtnVal = "EKG Treadmill";
                    break;
                case "12": rtnVal = "TCD";
                    break;
                case "13": rtnVal = "EMG";
                    break;
                case "15": rtnVal = "적외선 체온열 검사";
                    break;
                case "14": rtnVal = "VNG (어지로움증검사)";
                    break;
                case "16": rtnVal = "Aridol test";
                    break;
                case "22": rtnVal = "Tilt test";
                    break;
                case "23": rtnVal = "BCM";
                    break;

            }

            return rtnVal;
        }
    }
}
