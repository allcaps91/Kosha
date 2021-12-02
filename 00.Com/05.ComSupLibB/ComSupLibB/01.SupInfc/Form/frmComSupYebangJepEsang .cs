using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : MedOrder
    /// File Name       : frmComSupYebangJepEsang.cs
    /// Description     : 예방접종 후 이상반응 발생 보고서
    /// Author          : 김욱동
    /// Create Date     : 2021-03-08
    /// <history> 
    /// 예방접종 후 이상반응 발생 보고서
    /// </summary>
    public partial class frmComSupYebangJepEsang : Form
    
    {
        public enum OrientationType
        {
            가로, 세로
        }
        public OrientationType Orientation = OrientationType.가로;

        public Int32 MarginHeader { get; set; } = 0;
        public Int32 MarginFooter { get; set; } = 0;
        public Int32 MarginTop { get; set; } = 0;
        public Int32 MarginBottom { get; set; } = 0;
        public Int32 MarginLeft { get; set; } = 0;
        public Int32 MarginRight { get; set; } = 0;

        public int PrintStartPage { get; set; } = 0;
        public int PrintEndPage { get; set; } = 0;
        public bool ShowColumnHead { get; set; } = true;
        public bool ShowRowHead { get; set; } = false;
        public bool ShowGrid { get; set; } = true;
        public bool ShowBorder { get; set; } = true;
        public bool ShowShoadows { get; set; } = false;
        public bool ShowColor { get; set; } = false;
        public bool IsSmartPrint { get; set; } = false;
        public float ZoomFator { get; set; } = 1f;
        private string GstrPANO = "";
        private string GstrROWID = "";
        private double GdblIpdNO_OCS = 0;
        private string GstrGbnER = "";
        private bool GbolEXINFECT = false;

        private string GstrAge = "";
        private string GstrDept = "";
        private string GstrDrSabun = "";
        private string GstrWard = "";
        private string GstrRoom = "";
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();

        public delegate void SendText(string strText);


        public delegate void EventClosed();
        public frmComSupYebangJepEsang()
        {
            InitializeComponent();
        }

        public frmComSupYebangJepEsang(MainFormMessage pform)
        {
            InitializeComponent();
        }

        public frmComSupYebangJepEsang(MainFormMessage pform, string strPANO)
        {
            InitializeComponent();
            GstrPANO = strPANO;
            Cursor.Current = Cursors.WaitCursor;
            print();
            Cursor.Current = Cursors.Default;
        }


        public frmComSupYebangJepEsang(string strPANO)
        {
            InitializeComponent();
            GstrPANO = strPANO;
            Cursor.Current = Cursors.WaitCursor;
            print();
            Cursor.Current = Cursors.Default;
        }

        private void frmComSupYebangJepEsang_Load(object sender, EventArgs e)
        {
            this.Close();
        }


        void print()
        {

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string strAge = "";

            Cursor.Current = Cursors.WaitCursor;


            SQL = "";
            SQL = "SELECT";
            SQL = SQL + ComNum.VBLF + "     SNAME, PNAME, JUMIN1, JUMIN2 , JIKUP, SEX, TEL, HPHONE, GBFOREIGNER,";
            SQL = SQL + ComNum.VBLF + "     A.ZIPCODE1, A.ZIPCODE2, B.ZIPNAME1 || ' ' || B.ZIPNAME2 ||' ' || B.ZIPNAME3 AS ZIPNAME, JUSO ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT A, " + ComNum.DB_PMPA + "BAS_ZIPSNEW B ";
            SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + GstrPANO + "' ";
            SQL = SQL + ComNum.VBLF + "         AND A.ZIPCODE1 = B.ZIPCODE1(+) ";
            SQL = SQL + ComNum.VBLF + "         AND A.ZIPCODE2 = B.ZIPCODE2(+) ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {

                //환자명
                ssView2_Sheet1.Cells[2, 2].Text = dt.Rows[0]["SNAME"].ToString().Trim();
                //등록번호
                ssView2_Sheet1.Cells[2, 4].Text = ComFunc.SetAutoZero(GstrPANO, 8);

                //성별
                ssView2_Sheet1.Cells[3, 10].Value = dt.Rows[0]["SEX"].ToString().Trim() == "M" ? true : false;
                ssView2_Sheet1.Cells[4, 10].Value = dt.Rows[0]["SEX"].ToString().Trim() == "F" ? true : false;

                //직업
                ssView2_Sheet1.Cells[3, 7].Text = clsVbfunc.GetJikupName(clsDB.DbCon, dt.Rows[0]["JIKUP"].ToString().Trim());

                clsAES.Read_Jumin_AES(clsDB.DbCon, GstrPANO);

                //주민등록번호
                ssView2_Sheet1.Cells[2, 8].Text = clsAES.GstrAesJumin1 + "-" + clsAES.GstrAesJumin2;

                //연령
                strAge = ComFunc.AgeCalc(clsDB.DbCon, clsAES.GstrAesJumin1 + clsAES.GstrAesJumin2).ToString();

                if (VB.Val(strAge) < 19)
                {
                    ssView2_Sheet1.Cells[3, 5].Text = dt.Rows[0]["PNAME"].ToString().Trim();
                }


                GstrAge = strAge;

                //우편번호
                ssView2_Sheet1.Cells[6, 3].Text = dt.Rows[0]["ZIPCODE1"].ToString().Trim() + "-" + dt.Rows[0]["ZIPCODE2"].ToString().Trim();

                //전화번호
                ssView2_Sheet1.Cells[4, 2].Text = dt.Rows[0]["HPHONE"].ToString().Trim() + "/" + dt.Rows[0]["TEL"].ToString().Trim();

                //주소
                ssView2_Sheet1.Cells[5, 2].Text = dt.Rows[0]["ZIPNAME"].ToString().Trim() + " " + dt.Rows[0]["JUSO"].ToString().Trim();

                //의사성명
                ssView2_Sheet1.Cells[43, 3].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, clsType.User.Sabun);
                ssView2_Sheet1.Cells[43, 8].Text = clsVbfunc.GetOCSDoctorDRBUNHO(clsDB.DbCon, clsType.User.Sabun);
            }

            dt.Dispose();
            dt = null;

            

            bool PrePrint = false;
            using (clsSpread sp = new clsSpread())
            {
                ComFunc.ReadSysDate(clsDB.DbCon);
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;
                string strHeader = string.Empty;
                string strFooter = string.Empty;

                setMargin = new clsSpread.SpdPrint_Margin(50, MarginFooter, MarginTop, MarginBottom, MarginLeft, MarginRight);
                setOption = new clsSpread.SpdPrint_Option(Orientation == OrientationType.세로 ? PrintOrientation.Landscape : PrintOrientation.Portrait, PrintType.All, PrintStartPage, PrintEndPage
                , false, false, ShowGrid, ShowBorder, ShowShoadows, ShowColor, IsSmartPrint, ZoomFator);

                sp.setSpdPrint(ssView2, PrePrint, setMargin, setOption, strHeader, strFooter);
               ComFunc.Delay(500);
            }
        }


    }
}
