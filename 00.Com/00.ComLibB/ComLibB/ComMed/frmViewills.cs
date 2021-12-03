using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Description : 질병코드조회
/// Author : 이상훈
/// Create Date : 2017.07.03
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmViewIlls.frm, FrmKCD6_ViewIllss "/>

namespace ComLibB
{
    public partial class frmViewills : Form 
    { 
        string SQL = ""; 
        DataTable dt = null;
        int rowcounter;
        string SqlErr = ""; //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        int nSelect;
        string strSelect = "ALL";

        clsSpread SP = new clsSpread();
        clsOrdFunction OF = new clsOrdFunction();


        string FstrIllSort = "";

        //TreeNode RootNode, SubNode, LastNode;
        //string OrdName;
        //string KeyVal;
        //string OrdNo;

        public delegate void Spread_DoubleClick(string illCode, string illNameE, bool blnRO);
        public event Spread_DoubleClick SSillsDoubleClick;

        public frmViewills()
        {
            InitializeComponent();
        }

        private void frmViewills_Load(object sender, EventArgs e)
        {
            this.Location = new Point(10, 10);

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)//폼 권한 조회
            {
                this.Close();
                return;
            }

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            rdoGb1.Checked = true;
            rdoGb2.Checked = false;
            rdoGb3.Checked = false;
            rdoSort1.Checked = true;
            rdoSort2.Checked = false;
            nSelect = 2;

            trvOutlineIlls.Visible = false;
            trvOutlineIlls.BringToFront();
            trvOutlineIlls.Top = 64;
            trvOutlineIlls.Left = 0;

            this.Text = "";

            //권한 또는 부모폼에 따른 화면설정
            fn_Authority_Disp();

            btnView1_Click(btnView1, new EventArgs());

            //btnSearch.BackColor = Color.FromArgb(190, 255, 190);  //연두
            //btnSearch.BackColor = Color.FromArgb(255, 200, 200);    //핑크

            //2020-09-07, 심사팀일 경우만 상병한글명 표기란 보이도록 
            if(clsType.User.JobGroup == "JOB009001"
                || clsType.User.JobGroup == "JOB009002" 
                || clsType.User.JobGroup == "JOB009003")
            {
                txtIllKName.Visible = true;
            }
            else
            {
                txtIllKName.Visible = false;
            }
        }

        /// <summary>
        /// 권한 또는 부모폼에 따른 화면설정
        /// </summary>
        private void fn_Authority_Disp()
        {
            if (clsOrdFunction.GstrGbJob == "ER")
            {
                chkGbER.Visible = true;
                chkGbER.Checked = true;
                btnView6.Visible = true;
            }            
            else
            {
                if (clsPublic.GstrDeptCode == "OS")
                {
                    chkDetail.Checked = false;
                }
                chkGbER.Visible = false;
                chkGbER.Checked = false;
                btnView6.Visible = false;
            }
        }

        private void btnView_Color_Clear()
        {
            btnView1.BackColor = Color.White;
            btnView2.BackColor = Color.White;
            btnView3.BackColor = Color.White;
            btnView4.BackColor = Color.White;
            btnView5.BackColor = Color.White;
            btnView6.BackColor = Color.White;

            btnView1.ForeColor = Color.Black;
            btnView2.ForeColor = Color.Black;
            btnView3.ForeColor = Color.Black;
            btnView4.ForeColor = Color.Black;
            btnView5.ForeColor = Color.Black;
            btnView6.ForeColor = Color.Black;
        }

        /// <summary>
        /// 개인별
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView1_Click(object sender, EventArgs e)
        {
            nSelect = 1;

            btnView_Color_Clear();
            btnView1.BackColor = Color.RoyalBlue;
            btnView1.ForeColor = Color.White;

            btnDelete.Enabled = true;
            if (clsPublic.GstrDeptCode == "OS")
            {
                btnDel.Enabled = true;
                btnDel.Visible = true;
            }

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, strSelect, "");
        }

        /// <summary>
        /// 과별
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView2_Click(object sender, EventArgs e)
        {
            nSelect = 2;

            btnView_Color_Clear();
            btnView2.BackColor = Color.RoyalBlue;
            btnView2.ForeColor = Color.White;

            btnDelete.Enabled = true;
            if (clsPublic.GstrDeptCode == "OS")
            {
                btnDel.Enabled = false;
                btnDel.Visible = false;
            }

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, strSelect, "");
        }

        /// <summary>
        /// 전체
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnView3_Click(object sender, EventArgs e)
        {
            nSelect = 3; 

            btnView_Color_Clear();
            btnView3.BackColor = Color.RoyalBlue;
            btnView3.ForeColor = Color.White;

            btnDelete.Enabled = false;
            if (clsPublic.GstrDeptCode == "OS")
            {
                btnDel.Enabled = false;
                btnDel.Visible = false;
            }    

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, strSelect, "");
        }

        private void btnView4_Click(object sender, EventArgs e)
        {
            nSelect = 4;

            btnView_Color_Clear();
            btnView4.BackColor = Color.RoyalBlue;
            btnView4.ForeColor = Color.White;

            btnDelete.Enabled = false;
            if (clsPublic.GstrDeptCode == "OS")
            {
                btnDel.Enabled = false;
                btnDel.Visible = false;
            }
        }

        private void btnView5_Click(object sender, EventArgs e)
        {
            btnView_Color_Clear();
            btnView5.BackColor = Color.RoyalBlue;
            btnView5.ForeColor = Color.White;
            txtSearch.Focus();

            btnDelete.Enabled = false;
            if (clsPublic.GstrDeptCode == "OS")
            {
                btnDel.Enabled = false;
                btnDel.Visible = false;
            }
        }

        private void rdoSort_Color_Clear()
        {
            rdoSort1.BackColor = Color.White;
            rdoSort2.BackColor = Color.White;
            rdoSort1.ForeColor = Color.Black;
            rdoSort2.ForeColor = Color.Black;
        }

        private void rdoSort1_Click(object sender, EventArgs e)
        {
            rdoSort_Color_Clear();
            rdoSort1.BackColor = Color.RoyalBlue;
            rdoSort1.ForeColor = Color.White;

            FstrIllSort = "코드순";

            if (nSelect == 1)
            {
                fn_Read_ills(nSelect, clsOrdFunction.GstrDrCode, strSelect);
            }
            else if (nSelect == 1)
            {
                fn_Read_ills(nSelect, clsOrdFunction.GstrDrCode, strSelect);
            }
            else
            {
                fn_Read_ills(nSelect, " ", strSelect);
            }
        }

        private void rdoSort2_Click(object sender, EventArgs e)
        {
            rdoSort_Color_Clear();
            rdoSort2.BackColor = Color.RoyalBlue;
            rdoSort2.ForeColor = Color.White;

            FstrIllSort = "명순";

            if (nSelect == 1)
            {
                fn_Read_ills(nSelect, clsOrdFunction.GstrDrCode, strSelect);
            }
            else if (nSelect == 1)
            {
                fn_Read_ills(nSelect, clsOrdFunction.GstrDrCode, strSelect);
            }
            else
            {
                fn_Read_ills(nSelect, " ", strSelect);
            }
        }

        private void btnKor_Color_Clear()
        {
            btn가.BackColor = Color.White;
            btn나.BackColor = Color.White;
            btn다.BackColor = Color.White;
            btn라.BackColor = Color.White;
            btn마.BackColor = Color.White;
            btn바.BackColor = Color.White;
            btn사.BackColor = Color.White;
            btn아.BackColor = Color.White;
            btn자.BackColor = Color.White;
            btn차.BackColor = Color.White;
            btn카.BackColor = Color.White;
            btn타.BackColor = Color.White;
            btn파.BackColor = Color.White;
            btn하.BackColor = Color.White;
            
            btn가.ForeColor = Color.Black;
            btn나.ForeColor = Color.Black;
            btn다.ForeColor = Color.Black;
            btn라.ForeColor = Color.Black;
            btn마.ForeColor = Color.Black;
            btn바.ForeColor = Color.Black;
            btn사.ForeColor = Color.Black;
            btn아.ForeColor = Color.Black;
            btn자.ForeColor = Color.Black;
            btn차.ForeColor = Color.Black;
            btn카.ForeColor = Color.Black;
            btn타.ForeColor = Color.Black;
            btn파.ForeColor = Color.Black;
            btn하.ForeColor = Color.Black;
        }

        private void Read_ills(int nSelect, string DrCode, string DeptCode, string Gubun, string Gubun2)
        {
            if (nSelect == 1)   //개인
            {
                fn_Read_ills(nSelect, clsOrdFunction.GstrDrCode, Gubun, Gubun2);
            }
            else if (nSelect == 2)  //과별
            {
                fn_Read_ills(nSelect, clsPublic.GstrDeptCode, Gubun, Gubun2);
            }
            else if (nSelect == 3)  //전체
            {
                fn_Read_ills(nSelect, "ALL", Gubun, Gubun2);
            }
            //else if (nSelect == 4)  //계통별(사용 안함)
            //{
            //    //fn_OutLineIlls_Init();
            //}
            else if (nSelect == 4)  //중증상병 조회
            {
                fn_Read_ills(nSelect, "ALL", Gubun, Gubun2);
            }
        }

        private void btn가_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn가.ForeColor = Color.White;
            btn가.BackColor = Color.RoyalBlue;

            strSelect = "가";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "가", "나");
        }

        private void btn나_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn나.ForeColor = Color.White;
            btn나.BackColor = Color.RoyalBlue;

            strSelect = "나";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "나", "다");
        }

        private void btn다_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn다.ForeColor = Color.White;
            btn다.BackColor = Color.RoyalBlue;

            strSelect = "다";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "다", "라");
        }

        private void btn라_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn라.ForeColor = Color.White;
            btn라.BackColor = Color.RoyalBlue;

            strSelect = "라";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "라", "마");
        }

        private void btn마_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn마.ForeColor = Color.White;
            btn마.BackColor = Color.RoyalBlue;

            strSelect = "마";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "마", "바");
        }

        private void btn바_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn바.ForeColor = Color.White;
            btn바.BackColor = Color.RoyalBlue;

            strSelect = "바";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "바", "사");
        }

        private void btn사_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn사.ForeColor = Color.White;
            btn사.BackColor = Color.RoyalBlue;

            strSelect = "사";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "사", "아");
        }

        private void btn아_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn아.ForeColor = Color.White;
            btn아.BackColor = Color.RoyalBlue;

            strSelect = "아";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "아", "자");
        }

        private void btn자_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn자.ForeColor = Color.White;
            btn자.BackColor = Color.RoyalBlue;

            strSelect = "자";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "자", "차");
        }

        private void btn차_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn차.ForeColor = Color.White;
            btn차.BackColor = Color.RoyalBlue;

            strSelect = "차";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "차", "카");
        }

        private void btn카_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn카.ForeColor = Color.White;
            btn카.BackColor = Color.RoyalBlue;

            strSelect = "카";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "카", "타");
        }

        private void btn타_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn타.ForeColor = Color.White;
            btn타.BackColor = Color.RoyalBlue;

            strSelect = "타";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "타", "파");
        }

        private void btn파_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn파.ForeColor = Color.White;
            btn파.BackColor = Color.RoyalBlue;

            strSelect = "파";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "파", "");
        }

        private void btn하_Click(object sender, EventArgs e)
        {
            btnKor_Color_Clear();
            btn하.ForeColor = Color.White;
            btn하.BackColor = Color.RoyalBlue;

            strSelect = "하";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "하", "");
        }

        private void btnEng_Color_Clear()
        {  
            btnA.BackColor = Color.White;
            btnB.BackColor = Color.White;
            btnC.BackColor = Color.White;
            btnD.BackColor = Color.White;
            btnE.BackColor = Color.White;
            btnF.BackColor = Color.White;
            btnG.BackColor = Color.White;
            btnH.BackColor = Color.White;
            btnI.BackColor = Color.White;
            btnJ.BackColor = Color.White;
            btnK.BackColor = Color.White;
            btnL.BackColor = Color.White;
            btnM.BackColor = Color.White;
            btnN.BackColor = Color.White;
            btnO.BackColor = Color.White;
            btnP.BackColor = Color.White;
            btnQ.BackColor = Color.White;
            btnR.BackColor = Color.White;
            btnS.BackColor = Color.White;
            btnT.BackColor = Color.White;
            btnU.BackColor = Color.White;
            btnV.BackColor = Color.White;
            btnW.BackColor = Color.White;
            btnX.BackColor = Color.White;
            btnY.BackColor = Color.White;
            btnZ.BackColor = Color.White;
            btnAll.BackColor = Color.White;

            btnA.ForeColor = Color.Black;
            btnB.ForeColor = Color.Black;
            btnC.ForeColor = Color.Black;
            btnD.ForeColor = Color.Black;
            btnE.ForeColor = Color.Black;
            btnF.ForeColor = Color.Black;
            btnG.ForeColor = Color.Black;
            btnH.ForeColor = Color.Black;
            btnI.ForeColor = Color.Black;
            btnJ.ForeColor = Color.Black;
            btnK.ForeColor = Color.Black;
            btnL.ForeColor = Color.Black;
            btnM.ForeColor = Color.Black;
            btnN.ForeColor = Color.Black;
            btnO.ForeColor = Color.Black;
            btnP.ForeColor = Color.Black;
            btnQ.ForeColor = Color.Black;
            btnR.ForeColor = Color.Black;
            btnS.ForeColor = Color.Black;
            btnT.ForeColor = Color.Black;
            btnU.ForeColor = Color.Black;
            btnV.ForeColor = Color.Black;
            btnW.ForeColor = Color.Black;
            btnX.ForeColor = Color.Black;
            btnY.ForeColor = Color.Black;
            btnZ.ForeColor = Color.Black;
            btnAll.ForeColor = Color.Black;
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnA.ForeColor = Color.White;
            btnA.BackColor = Color.RoyalBlue;

            strSelect = "A";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "A", "");
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnB.ForeColor = Color.White;
            btnB.BackColor = Color.RoyalBlue;

            strSelect = "B";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "B", "");
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnC.ForeColor = Color.White;
            btnC.BackColor = Color.RoyalBlue;

            strSelect = "C";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "C", "");
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnD.ForeColor = Color.White;
            btnD.BackColor = Color.RoyalBlue;

            strSelect = "D";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "D", "");
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnE.ForeColor = Color.White;
            btnE.BackColor = Color.RoyalBlue;

            strSelect = "E";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "E", "");
        }

        private void btnF_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnF.ForeColor = Color.White;
            btnF.BackColor = Color.RoyalBlue;

            strSelect = "F";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "F", "");
        }

        private void btnG_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnG.ForeColor = Color.White;
            btnG.BackColor = Color.RoyalBlue;

            strSelect = "G";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "G", "");
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnH.ForeColor = Color.White;
            btnH.BackColor = Color.RoyalBlue;

            strSelect = "H";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "H", "");
        }

        private void btnI_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnI.ForeColor = Color.White;
            btnI.BackColor = Color.RoyalBlue;

            strSelect = "I";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "I", "");
        }

        private void btnJ_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnJ.ForeColor = Color.White;
            btnJ.BackColor = Color.RoyalBlue;

            strSelect = "J";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "J", "");
        }

        private void btnK_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnK.ForeColor = Color.White;
            btnK.BackColor = Color.RoyalBlue;

            strSelect = "K";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "K", "");
        }

        private void btnL_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnL.ForeColor = Color.White;
            btnL.BackColor = Color.RoyalBlue;

            strSelect = "L";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "L", "");
        }

        private void btnM_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnM.ForeColor = Color.White;
            btnM.BackColor = Color.RoyalBlue;

            strSelect = "M";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "M", "");
        }

        private void btnN_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnN.ForeColor = Color.White;
            btnN.BackColor = Color.RoyalBlue;

            strSelect = "N";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "N", "");
        }

        private void btnO_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnO.ForeColor = Color.White;
            btnO.BackColor = Color.RoyalBlue;

            strSelect = "O";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "O", "");
        }

        private void btnP_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnP.ForeColor = Color.White;
            btnP.BackColor = Color.RoyalBlue;

            strSelect = "P";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "P", "");
        }

        private void btnQ_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnQ.ForeColor = Color.White;
            btnQ.BackColor = Color.RoyalBlue;

            strSelect = "Q";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "Q", "");
        }

        private void btnR_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnR.ForeColor = Color.White;
            btnR.BackColor = Color.RoyalBlue;

            strSelect = "R";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "R", "");
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnS.ForeColor = Color.White;
            btnS.BackColor = Color.RoyalBlue;

            strSelect = "S";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "S", "");
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnT.ForeColor = Color.White;
            btnT.BackColor = Color.RoyalBlue;

            strSelect = "T";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "T", "");
        }

        private void btnU_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnU.ForeColor = Color.White;
            btnU.BackColor = Color.RoyalBlue;

            strSelect = "U";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "U", "");

        }

        private void btnV_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnV.ForeColor = Color.White;
            btnV.BackColor = Color.RoyalBlue;

            strSelect = "V";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "V", "");
        }

        private void btnW_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnW.ForeColor = Color.White;
            btnW.BackColor = Color.RoyalBlue;

            strSelect = "W";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "W", "");
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnX.ForeColor = Color.White;
            btnX.BackColor = Color.RoyalBlue;

            strSelect = "X";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "X", "");
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnY.ForeColor = Color.White;
            btnY.BackColor = Color.RoyalBlue;

            strSelect = "Y";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "Y", "");
        }

        private void btnZ_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnZ.ForeColor = Color.White;
            btnZ.BackColor = Color.RoyalBlue;

            strSelect = "Z";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "Z", "");
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            btnEng_Color_Clear();
            btnAll.ForeColor = Color.White;
            btnAll.BackColor = Color.RoyalBlue;

            strSelect = "ALL";

            Read_ills(nSelect, clsOrdFunction.GstrDrCode, clsPublic.GstrDeptCode, "ALL", "");
        }

        /// <summary>
        /// 코드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGb1_Click(object sender, EventArgs e)
        {
            pnlEng.Visible = true;
            pnlKor.Visible = false;
            txtSearch.Focus();
        }

        /// <summary>
        /// 한글
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGb2_Click(object sender, EventArgs e)
        {
            pnlKor.Visible = true;
            pnlEng.Visible = false;
            btn가.Focus();
        }

        /// <summary>
        /// 영문
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGb3_Click(object sender, EventArgs e)
        {
            pnlKor.Visible = false;
            pnlEng.Visible = true;
            btnA.Focus();
        }

        private void SSills_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strillCode="";
            string strillName = "";
            string strILLMSG;

            FpSpread o = (FpSpread)sender;

            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.SSills)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column); //sort 정렬 기능 
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
            }

            if (SSills_Sheet1.RowCount == 0) return;

            strillCode = SSills.ActiveSheet.Cells[e.Row, 0].Text.Trim().ToUpper();

            if (SSills.ActiveSheet.Cells[e.Row, 3].Text.Trim() == "N")
            {
                strILLMSG = "";
                strILLMSG = "[불완전상병]" + "\r\r";
                strILLMSG += strillCode + " 는 불완전상병입니다. " + "\r\r";

                try
                {
                    SQL = "";
                    SQL += " SELECT ILLCODE, ILLNAMEE, ILLNAMEK                 \r";
                    SQL += "   FROM ADMIN.BAS_ILLS                        \r";
                    SQL += "  WHERE ILLCODE  LIKE '" + strillCode.Trim() + "%'  \r";
                    SQL += "    AND LENGTH(ILLCODE) <= 6                        \r";
                    SQL += "    AND (NOUSE <>'N' OR NOUSE IS NULL)              \r";
                    SQL += "    AND ILLCLASS = '1'                              \r";
                    SQL += "    AND DDATE IS NULL                               \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strILLMSG += " 사용가능 상병 아래 상병참조 바랍니다." + "\r\r";
                        strILLMSG += "========================================" + "\r\r";

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strILLMSG += "[" + VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim() + VB.Space(12), 12) + "] " + dt.Rows[i]["ILLNAMEE"].ToString().Trim() == "" ? dt.Rows[i]["ILLNAMEK"].ToString().Trim() : dt.Rows[i]["ILLNAMEE"].ToString().Trim() + "\r\r";
                        }

                        strILLMSG += "========================================" + "\r\r";

                        ComFunc.MsgBox(strILLMSG, "확인");
                    }

                    dt.Dispose();
                    dt = null;
                    return;
                }
                catch (Exception ex)
                {
                    if (dt != null)
                    {
                        dt.Dispose();
                        dt = null;
                    }

                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (SSills.ActiveSheet.Cells[e.Row, 7].Text.Trim() != "" || SSills.ActiveSheet.Cells[e.Row, 7].Text.Trim() != null)
                {
                    strILLMSG = "";
                    strILLMSG = "[삭제상병]" + "\r\r";
                    strILLMSG += strillCode + " 는 삭제 상병입니다. " + "\r\r";

                    try
                    {
                        SQL = "";
                        SQL += " SELECT ILLCODE, ILLNAMEE, ILLNAMEK                 \r";
                        SQL += "   FROM ADMIN.BAS_ILLS                        \r";
                        SQL += "  WHERE ILLCODE  LIKE '" + VB.Left(strillCode.Trim(), VB.Len(strillCode.Trim()) - 1) + "%'  \r";
                        SQL += "    AND LENGTH(ILLCODE) <= 6                        \r";
                        SQL += "    AND (NOUSE <>'N' OR NOUSE IS NULL)              \r";
                        SQL += "    AND ILLCLASS = '1'                              \r";
                        SQL += "    AND DDATE IS NULL                               \r";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strILLMSG += " 사용가능 상병 아래 상병참조 바랍니다." + "\r\r";
                            strILLMSG += "========================================" + "\r\r";

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                strILLMSG += "[" + VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim() + VB.Space(12), 12) + "] " + dt.Rows[i]["ILLNAMEE"].ToString().Trim() == "" ? dt.Rows[i]["ILLNAMEK"].ToString().Trim() : dt.Rows[i]["ILLNAMEE"].ToString().Trim() + "\r\r";
                            }

                            strILLMSG += "========================================" + "\r\r";

                            MessageBox.Show(strILLMSG, "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        dt.Dispose();
                        dt = null;
                        return;
                    }
                    catch (Exception ex)
                    {
                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }

                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }

                if (clsOrderEtc.CHECK_PNEUMONIA_ILL(clsDB.DbCon, strillCode.Trim()) == true && int.Parse(clsPublic.GstrAge) >= 18)
                {
                    FrmMsgPneumoniacs f = new FrmMsgPneumoniacs();
                    f.ShowDialog(this);
                    OF.fn_ClearMemory(f);
                }
            }

            strillName = (SSills.ActiveSheet.Cells[e.Row, 2].Text.Trim() == "") ? SSills.ActiveSheet.Cells[e.Row, 1].Text.Trim() : SSills.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            strillCode = SSills.ActiveSheet.Cells[e.Row, 0].Text.Trim();

            SSillsDoubleClick(strillCode, strillName, chkRO.Checked);
        }

        private void SSills_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //if (e.RowHeader == true)
            //{
            //    SP.setSpdSort(SSills, e.Column, true);
            //    return;
            //}

            if (SSills_Sheet1.RowCount == 0) return;

            string OrderName = (SSills.ActiveSheet.Cells[e.Row, 1].Text.Trim() == "") ? SSills.ActiveSheet.Cells[e.Row, 2].Text.Trim() : SSills.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            lblName.Text = "[" + SSills.ActiveSheet.Cells[e.Row, 0].Text.Trim() + "] : " + OrderName;

            //2020-09-07 안정수 추가
            txtIllKName.Text = "[" + SSills.ActiveSheet.Cells[e.Row, 0].Text.Trim() + "] : " + SSills.ActiveSheet.Cells[e.Row, 2].Text.Trim();

            if (SSills.ActiveSheet.Cells[e.Row, 0, e.Row, SSills.ActiveSheet.ColumnCount - 1].BackColor != Color.Aqua)
            {
                SSills.ActiveSheet.Cells[e.Row, 0, e.Row, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;
            }
            else
            {
                SSills.ActiveSheet.Cells[e.Row, 0, e.Row, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            }

            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < SSills_Sheet1.RowCount; i++)
                {
                    if (SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor == Color.Aqua)
                    {
                        SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {   
            string strillCode;
            string strGubun;
            int k;            
            
            k = 0;
            for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
            {
                if (SSills.ActiveSheet.Cells[i, 1].BackColor == Color.Aqua)
                {
                    k += 1;
                    break;
                }
            }

            if (k == 0)
            {
                MessageBox.Show("선택된 코드가 없습니다!!, 코드를 선택하신후 삭제 하십시오!!!");
                return;
            }

            if (nSelect != 1 && nSelect != 2) return;

            strGubun = (nSelect == 1 ? "개인 등록" : "과 등록");

            if (MessageBox.Show("선택 하신 상병을 " + strGubun + " 코드에서 삭제 하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
            {
                return;
            }

            for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
            {
                if (SSills.ActiveSheet.Cells[i, 1].BackColor == Color.Aqua)
                {
                    if (nSelect != 1 && nSelect != 2) return;

                    clsDB.setBeginTran(clsDB.DbCon);
                    

                    strillCode = SSills.Sheets[0].Cells[i, 0].Text.ToString().Trim();

                    try
                    {
                        SQL = "";
                        SQL += " DELETE FROM ADMIN.OCS_OILLDEF                         \r";
                        if (nSelect == 1)   //개인
                        {
                            SQL += "  WHERE DEPTDR = '" + clsOrdFunction.GstrDrCode + "'             \r";
                        }
                        else if (nSelect == 2)  //전체
                        {
                            SQL += "  WHERE DEPTDR = '" + clsPublic.GstrDeptCode + "'       \r";
                        }
                        SQL += "    AND ILLCODE = '" + strillCode.ToString() + "'           \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr + " : 상병 삭제중 오류 발생!!!");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        clsDB.setCommitTran(clsDB.DbCon);
                        ComFunc.MsgBox("삭제 하였습니다.");
                        Cursor.Current = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message + " : 상병 삭제중 오류 발생!!!");
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }
            MessageBox.Show("선택한 코드가 삭제 되었습니다!!! ");

            for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
            {
                if (SSills.ActiveSheet.Cells[i, 0].BackColor == Color.Aqua)
                {
                    SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                    SSills.ActiveSheet.Rows[i].Remove();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            SSills.ActiveSheet.RowCount = 0;

            try
            {
                SQL = "";
                SQL += " SELECT distinct IllCode                                                            \r";
                SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                SQL += "        CASE                                    \r";
                SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                SQL += "           ELSE ''                                    \r";
                SQL += "        END                                    \r";
                SQL += "        || IllNameE  \r";
                SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                SQL += "        CASE                                    \r";
                SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                SQL += "           ELSE ''                                    \r";
                SQL += "        END                                    \r";
                SQL += "        || ILLNAMEK  \r";
                SQL += "      , NoUse , ILLCODED, ";
                SQL += "        CASE                                    \r";
                SQL += "           WHEN GbV252 = '*' THEN '*'                                    \r";
                SQL += "           WHEN GbV352 = '*' THEN '*'                                    \r";
                SQL += "           ELSE ''                                    \r";
                SQL += "        END GbV252 ,                                   \r";
                SQL += "        GBVCODE, DDATE, IllNameE ILLNAMEE1, GBER          \r";
                SQL += "        ,KCD8          \r";
                SQL += "   FROM ADMIN.BAS_ILLS                                                        \r";
                SQL += "  WHERE 1 = 1                                                                       \r";
                SQL += "    AND (NOUSE <> 'N' OR NOUSE IS NULL)             \r";
                SQL += "    AND DDATE IS NULL                               \r";
                if (chkGbER.Checked == true)
                {
                    SQL += "    AND GBER = '*'                                                              \r";
                }
                SQL += "    AND (UPPER(IllNameE) Like '%' || '" + txtSearch.Text.Trim().ToUpper() + "' || '%'         \r";
                SQL += "     OR  UPPER(IllNameK) Like '%' || '" + txtSearch.Text.Trim().ToUpper() + "' || '%'         \r";
                SQL += "     OR  ILLCODE Like '%' || '" + txtSearch.Text.Trim().ToUpper() + "' || '%' )               \r";
                SQL += "    AND  IllClass = '1'                                                             \r";
                SQL += "  ORDER BY IllNameE                                                                 \r";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SP.Spread_All_Clear(SSills);
                    clsDB.DataTableToSpdRow(dt, SSills, 0, true);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["NOUSE"].ToString().Trim() == "N")
                        {
                            SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        else
                        {
                            SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
                        }

                        //2020-12-30 
                        if(dt.Rows[i]["KCD8"].ToString().Trim() != "*")
                        {
                            SSills.ActiveSheet.Rows[i].BackColor = Color.FromArgb(255, 113, 113);
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

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            SSills.ActiveSheet.ColumnHeader.Cells[0, 0, 0, SSills.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            Cursor.Current = Cursors.Default;
        }

        private void btnPersonalReg_Click(object sender, EventArgs e)
        {
            illComuse_Reg(clsOrdFunction.GstrDrCode);
        }

        /// <summary>
        /// 상병 개인등록
        /// </summary>
        /// <param name="argDeptDr"></param>
        private void illComuse_Reg(string strDeptDr)
        {
            string strillCode;
            int k;

            k = 0;
            for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
            {
                if (SSills.ActiveSheet.Cells[i, 1].BackColor == Color.Aqua)
                {
                    k += 1;
                    break;
                }
            }

            if (k == 0)
            {
                MessageBox.Show("선택된 코드가 없습니다!!, 코드를 선택하신후 등록 하십시오!!!");
                return;
            }

            for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
            {
                if (SSills.ActiveSheet.Cells[i, 1].BackColor == Color.Aqua)
                {
                    strillCode = SSills.ActiveSheet.Cells[i, 0].Text.ToString().Trim();
                    if (Read_ILLCODE_COMUSE(strDeptDr, strillCode) == false)
                    {
                        PersonalRed_Insert(strDeptDr, strillCode);
                    }
                    else
                    {
                        //Skip
                        //MessageBox.Show("이미 등록 되어 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            if (nSelect == 1)
            {
                MessageBox.Show("선택한 코드가 개인 등록되었습니다!!! ");
            }
            else if (nSelect == 2)
            {
                MessageBox.Show("선택한 코드가 과 등록되었습니다!!! ");
            }

            for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
            {
                SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            }
        }

        private bool Read_ILLCODE_COMUSE(string strDeptDrCode, string strillCode)
        {
            try
            {
                SQL = "";
                SQL += " SELECT ILLCODE                             \r";
                SQL += "   FROM ADMIN.OCS_OILLDEF              \r";
                SQL += "  WHERE DEPTDR = '" + strDeptDrCode + "'    \r";
                SQL += "    AND ILLCODE = '" + strillCode + "'      \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            
                return false;
            }
        }

        private void PersonalRed_Insert(string strDeptDrCode, string strillCode)
        {
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += " INSERT INTO ADMIN.OCS_OILLDEF              \r";
                SQL += "        (DEPTDR, ILLCODE)                        \r";
                SQL += " VALUES ('" + strDeptDrCode + "'                 \r";
                SQL += "       , '" + strillCode.ToString().Trim() + "') \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " : 상병 등록중 오류 발생!!!");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message + " : 상병 등록중 오류 발생!!!");
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnDeptReg_Click(object sender, EventArgs e)
        {
            illComuse_Reg(clsPublic.GstrDeptCode);
        }

        /// <summary>
        /// 상병 검색
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtSearch.Text != "" && e.KeyChar == (char)13)
            {
                e.Handled = true;
                btnSearch_Click(txtSearch, e);
            }
        }

        /// <summary>
        /// 상병 조회
        /// </summary>
        /// <param name="nGbn"></param>
        /// <param name="strDeptDr"></param>
        /// <param name="Gubun"></param>
        private void fn_Read_ills(int nGbn, string strDeptDr, string Gubun, string Gubun2 = "")
        {
            string strOld = "";
            string strFlag = "";
            DataSet ds = null;

            SSills_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                switch (nSelect)
                {   
                    case 3: //전체

                        
                        SQL = " SELECT IllCode                                                            \r";
                        SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END                                    \r";
                        SQL += "        || IllNameE  \r";
                        SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END                                    \r";
                        SQL += "        || ILLNAMEK  \r";
                        SQL += "      , NoUse , ILLCODED, ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '*'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '*'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END GbV252 ,                                   \r";
                        SQL += "        GBVCODE, DDATE, IllNameE ILLNAMEE1, GBER          \r";
                        SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ILLS                                              \r";
                        SQL += "  WHERE IllClass = '1'                                                              \r";
                        SQL += "    AND (NOUSE <>'N' OR NOUSE IS NULL)                                              \r";
                        if (chkGbER.Checked == true)
                        {
                            SQL += "    AND GBER = '*'                                                              \r";
                        }
                        if (rdoGb1.Checked == true)
                        {
                            if (Gubun != "ALL")
                            {
                                SQL += "    AND IllCode Like upper('" + Gubun + "' || '%')                          \r";
                            }
                        }
                        else if (rdoGb2.Checked == true)
                        {
                            if (Gubun != "ALL")
                            {
                                if (Gubun == "하")
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + Gubun + '%' + "'                \r";
                                }
                                else
                                {
                                    SQL += "    AND (IllNameK >= '" + Gubun + '%' + "'                              \r";
                                    SQL += "    AND  IllNameK < '" + Gubun2 + '%' + "')                             \r";
                                }
                            }
                        }
                        else if (rdoGb3.Checked == true)
                        {
                            if (Gubun != "ALL")
                            {
                                SQL += "    AND IllNameE Like upper('" + Gubun + "' || '%')                         \r";
                            }
                        }

                        if (chkDetail.Checked == false)
                        {
                            SQL += "   AND LENGTH(ILLCODE) <= '6'                                                   \r";
                        }
                        if (DateTime.Parse(clsPublic.GstrSysDate) < DateTime.Parse("2016-06-01"))
                        {
                            SQL += "   AND  ( KCDOLD ='*' OR KCD6  ='*' )                                           \r";
                        }
                        else if (DateTime.Parse(clsPublic.GstrSysDate) >= DateTime.Parse("2016-06-01"))
                        {
                            SQL += "   AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*')                              \r";
                        }

                        if (rdoSort1.Checked == true)
                        {
                            SQL += "  order by illcode                                                              \r";
                        }
                        else if (rdoSort2.Checked == true)
                        {
                            if (rdoGb2.Checked == true)
                            {
                                SQL += "  order by illnameK                                                         \r";
                            }
                            else if (rdoGb1.Checked == true || rdoGb3.Checked == true)
                            {
                                SQL += "  order by illnameE                                                         \r";
                            }
                        }
                        break;
                    case 4: //중증상병 

                        SQL = "";
                        //SQL += " SELECT IllCode                                                                     \r";
                        //SQL += "      , decode(GBVCODE, '*', '@', '') || DECODE(GBV252, '*', '★', '') || IllNameE  \r";
                        //SQL += "      , decode(GBVCODE, '*', '@', '') || DECODE(GBV252, '*', '★', '') || ILLNAMEK  \r";
                        //SQL += "      , NoUse , ILLCODED, GBV252, GBVCODE, DDATE, IllNameE IllNameE1                \r";
                        //SQL += "      , GBER                                                                        \r";
                        SQL += " SELECT IllCode                                                            \r";
                        SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END                                    \r";
                        SQL += "        || IllNameE  \r";
                        SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END                                    \r";
                        SQL += "        || ILLNAMEK  \r";
                        SQL += "      , NoUse , ILLCODED, ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '*'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '*'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END GbV252 ,                                   \r";
                        SQL += "        GBVCODE, DDATE, IllNameE ILLNAMEE1, GBER          \r";
                        SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ILLS                                              \r";
                        SQL += "  WHERE IllClass = '1'                                                              \r";
                        SQL += "    AND (NOUSE <>'N' OR NOUSE IS NULL)                                              \r";
                        //if (chkGbER.Checked == true)
                        //{
                            SQL += "    AND GBER = '*'                                                              \r";
                        //}
                        if (rdoGb1.Checked == true)
                        {
                            if (Gubun != "ALL" && Gubun.Trim() != "")
                            {
                                SQL += "    AND IllCode Like upper('" + Gubun + "' || '%')                          \r";
                            }
                        }
                        else if (rdoGb2.Checked == true)
                        {
                            if (Gubun != "ALL" && Gubun.Trim() != "")
                            {
                                if (Gubun == "하")
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + Gubun + '%' + "'                \r";
                                }
                                else
                                {
                                    SQL += "    AND (IllNameK >= '" + Gubun + '%' + "'                              \r";
                                    SQL += "    AND  IllNameK < '" + Gubun2 + '%' + "')                             \r";
                                }
                            }
                        }
                        else if (rdoGb3.Checked == true)
                        {
                            if (Gubun != "ALL" && Gubun.Trim() != "")
                            {
                                SQL += "    AND IllNameE Like upper('" + Gubun + "' || '%')                         \r";
                            }
                        }

                        if (chkDetail.Checked == false)
                        {
                            SQL += "   AND LENGTH(ILLCODE) <= '6'                                                   \r";
                        }
                        if (DateTime.Parse(clsPublic.GstrSysDate) < DateTime.Parse("2016-06-01"))
                        {
                            SQL += "   AND  ( KCDOLD ='*' OR KCD6  ='*' )                                           \r";
                        }
                        else if (DateTime.Parse(clsPublic.GstrSysDate) >= DateTime.Parse("2016-06-01"))
                        {
                            SQL += "   AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*')                              \r";
                        }

                        if (rdoSort1.Checked == true)
                        {
                            SQL += "  order by illcode                                                              \r";
                        }
                        else if (rdoSort2.Checked == true)
                        {
                            if (rdoGb2.Checked == true)
                            {
                                SQL += "  order by illnameK                                                         \r";
                            }
                            else if (rdoGb1.Checked == true || rdoGb3.Checked == true)
                            {
                                SQL += "  order by illnameE                                                         \r";
                            }
                        }
                        break;
                    default:
                        SQL = "";
                        //SQL += " SELECT A.IllCode                                                                   \r";
                        //SQL += "      , decode(GBVCODE, '*', '@', '') || DECODE(GBV252, '*', '★', '') || IllNameE  \r";
                        //SQL += "      , decode(GBVCODE, '*', '@', '') || DECODE(GBV252, '*', '★', '') || ILLNAMEK  \r";
                        //SQL += "      , B.NoUse ,B.ILLCODED, B.GBV252, B.GBVCODE, B.DDATE, IllNameE IllNameE1       \r";
                        //SQL += "      , GBER                                                                        \r";
                        SQL += " SELECT A.IllCode                                                            \r";
                        SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END                                    \r";
                        SQL += "        || IllNameE  \r";
                        SQL += "      , decode(GBVCODE, '*', '@', '') || ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN GbV252 = '*' THEN '★'                                    \r";
                        SQL += "           WHEN GbV352 = '*' THEN '★'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END                                    \r";
                        SQL += "        || ILLNAMEK  \r";
                        SQL += "      , B.NoUse , B.ILLCODED, ";
                        SQL += "        CASE                                    \r";
                        SQL += "           WHEN B.GbV252 = '*' THEN '*'                                    \r";
                        SQL += "           WHEN B.GbV352 = '*' THEN '*'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END GbV252 ,                                   \r";
                        SQL += "        B.GBVCODE, B.DDATE, B.IllNameE ILLNAMEE1, GBER          \r";
                        SQL += "   FROM ADMIN.OCS_OILLDEF A                                                    \r";
                        SQL += "      , ADMIN.BAS_ILLS   B                                                    \r";
                        if (nSelect == 1)
                        {
                            SQL += "  WHERE (A.DeptDr   = '" + clsType.User.Sabun + "'                              \r";
                            SQL += "     OR  A.DeptDr   = '" + strDeptDr + "')                                      \r";
                        }
                        else
                        {
                            SQL += "  WHERE A.DeptDr   = '" + strDeptDr + "'                                        \r";
                        }
                        SQL += "    AND A.IllCode  > ' '                                                            \r";
                        SQL += "    AND A.IllCode  = B.IllCode(+)                                                   \r";
                        SQL += "    AND B.IllCLASS = '1'                                                            \r";
                        if (chkGbER.Checked == true)
                        {
                            SQL += "    AND GBER = '*'                                                              \r";
                        }
                        if (rdoGb1.Checked == true) //코드
                        {
                            if (Gubun != "ALL" && Gubun.Trim() != "")
                            {
                                SQL += "    AND b.IllCode Like upper('" + Gubun + "' || '%')                        \r";
                            }
                        }
                        else if (rdoGb2.Checked == true) //한글
                        {
                            if (Gubun != "ALL" && Gubun.Trim() != "")
                            {
                                if (Gubun == "하")
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + Gubun + '%' + "'                \r";
                                }
                                else
                                {
                                    SQL += "    AND (IllNameK >= '" + Gubun + '%' + "'                              \r";
                                    SQL += "    AND  IllNameK < '" + Gubun2 + '%' + "')                             \r";
                                }
                            }
                        }
                        else if (rdoGb3.Checked == true) //영문
                        {
                            if (Gubun != "ALL" && Gubun.Trim() != "")
                            {
                                SQL += "    AND b.IllNameE Like upper('" + Gubun + "' || '%')                       \r";
                            }
                        }

                        if (chkDetail.Checked == false)
                        {
                            SQL += "   AND LENGTH(B.ILLCODE) <= '6'                                                 \r";
                        }
                        if (DateTime.Parse(clsPublic.GstrSysDate) < DateTime.Parse("2016-06-01"))
                        {
                            SQL += "   AND  ( KCDOLD ='*' OR KCD6  ='*' )                                           \r";
                        }
                        else if (DateTime.Parse(clsPublic.GstrSysDate) >= DateTime.Parse("2016-06-01"))
                        {
                            SQL += "   AND  ( KCDOLD ='*' OR KCD6  ='*' OR  KCD7 ='*')                              \r";
                        }

                        if (rdoSort1.Checked == true)   //코드별 정렬
                        {
                            SQL += "  order by illcode                                                              \r";
                        }
                        else if (rdoSort2.Checked == true)  //상병명별
                        {
                            if (rdoGb2.Checked == true) //한글
                            {
                                SQL += "  order by illnameK                                                         \r";
                            }
                            else if (rdoGb1.Checked == true || rdoGb3.Checked == true) //영문
                            {
                                SQL += "  order by illnameE                                                         \r";
                            }
                        }
                        break;
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                SqlErr = clsDB.GetDataSet(ref ds, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                //rowcounter = 0;
                //rowcounter = dt.Rows.Count;

                //SP.Spread_All_Clear(SSills);

                //SSills.ActiveSheet.RowCount = dt.Rows.Count;

                if (ds != null)
                {
                    clsDB.DataSetToSpd(ds, SSills);
                    //clsDB.DataTableToSpdRow(dt, SSills, 0, true);

                    strFlag = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //SSills.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["IllNameE"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["NoUse"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["ILLCODED"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["GBV252"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["GBVCODE"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["DDATE"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["IllNameE1"].ToString().Trim();
                        //SSills.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["GBER"].ToString().Trim();

                        if (dt.Rows[i]["NOUSE"].ToString().Trim() == "N")
                        {
                            //SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(190, 255, 190);
                            SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        else
                        {
                            //SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 200, 200);
                            SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
                        }

                        if (dt.Rows[i]["GBER"].ToString().Trim() == "*")
                        {
                            SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                        }

                        /*

                        switch (VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 1))
                        {
                            case "M":
                                if (strOld != VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 4))
                                {
                                    strOld = VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 4);
                                }
                                break;
                            case "S":
                                if (strOld != VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 4))
                                {
                                    strOld = VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 4);
                                    strFlag = (strFlag == "1" ? "2" : "1");
                                }
                                break;
                            default:
                                if (strOld != VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 3))
                                {
                                    strOld = VB.Left(dt.Rows[i]["ILLCODE"].ToString().Trim(), 3);
                                    strFlag = (strFlag == "1" ? "2" : "1");
                                }
                                break;
                        }

                        if (strFlag == "1")
                        {
                            //SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(128, 255, 255);
                            SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].ForeColor = Color.Blue;
                        }

                    */
                    }
                }
                ds.Dispose();
                ds = null;

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void fn_OutLineIlls_Init()
        {
            /*
            int i, j, K, n;
            string strName;
            string strCodeF;
            string strCodeT;
            TreeNode nodX = new TreeNode();

            trvOutlineIlls.Nodes.Clear();
            
            j = 1: n = 1
            For i = 1 To SS1.DataRowCnt
                SS1.Row = i
                SS1.Col = 1
                If Trim(SS1.Text) = "0" Then
                    if (rdoGb2.checked == true)
                    {

                    }
                        Case "KOR": SS1.Col = 2     '한글명
                        Case Else:  SS1.Col = 5     '영문명
                    End Select


                    Set nodX = OutlineIlls.Nodes.Add(, , "R" & j, Trim(SS1.Text))


                    For K = 1 To SS1.DataRowCnt
                        SS1.Row = K: SS1.Col = 1
                        If Val(Trim(SS1.Text)) = j Then
                            Select Case strKorEng
                                Case "KOR": SS1.Col = 2     '한글명
                                Case Else:  SS1.Col = 5     '영문명
                            End Select
                            strName = Trim(SS1.Text)
                            SS1.Col = 3: strCodeF = Trim(SS1.Text)
                            SS1.Col = 4: strCodeT = Trim(SS1.Text)


                            Set nodX = OutlineIlls.Nodes.Add("R" & j, tvwChild, "C" & n, RPadH(strName, 200) & RPadH(strCodeF, 6) & RPadH(strCodeT, 6))


                            n = n + 1
                        End If
                    Next K
                    j = j + 1
                End If


                SS1.Row = i: SS1.Col = 1
                If Trim(SS1.Text) > "0" Then Exit For
            Next i
            */
        }

        private void chkRO_Click(object sender, EventArgs e)
        {

        }

        private void btnView6_Click(object sender, EventArgs e)
        {
            nSelect = 4;

            btnView_Color_Clear();
            btnView6.BackColor = Color.RoyalBlue;
            btnView6.ForeColor = Color.White;

            btnDelete.Enabled = false;

            fn_Read_ills(nSelect, clsOrdFunction.GstrDrCode, strSelect);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (btnView1.BackColor != Color.RoyalBlue && btnView1.ForeColor != Color.White) return; 
            if (clsPublic.GstrDeptCode != "OS") return;
            if (SSills.ActiveSheet.RowCount == 0) return;

            if (MessageBox.Show("등록된 약속상병을 전체삭제 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            else
            {
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = "";
                SQL += " DELETE FROM ADMIN.OCS_OILLDEF                       \r";
                SQL += "  WHERE (DeptDr   = '" + clsType.User.Sabun + "'          \r";
                SQL += "     OR  DeptDr   = '" + clsOrdFunction.GstrDrCode + "')  \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr + " : 상병 삭제중 오류 발생!!!");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제 하였습니다.");
                Cursor.Current = Cursors.Default;                

                for (int i = 0; i < SSills.ActiveSheet.RowCount; i++)
                {
                    if (SSills.ActiveSheet.Cells[i, 0].BackColor == Color.Aqua)
                    {
                        SSills.ActiveSheet.Cells[i, 0, i, SSills.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                        SSills.ActiveSheet.Rows[i].Remove();
                    }
                }
            }

        }       
       
    }
}
