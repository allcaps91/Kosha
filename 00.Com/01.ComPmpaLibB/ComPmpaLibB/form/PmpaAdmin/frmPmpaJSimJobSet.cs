using System;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    public partial class frmPmpaJSimJobSet : Form
    {
        ComFunc clsPF = new ComFunc();
        clsPmpaType clsPT = new clsPmpaType();

        public frmPmpaJSimJobSet()
        {
            InitializeComponent();
        }

        private void frmPmpaJSimJobSet_Load(object sender, EventArgs e)
        {
            Get_Data();
        }

        private void Get_Data()
        {
            clsPmpaType.JSIM.DISP = clsPF.Reg_Get_Setting("JSIM", "DISP"); ;
            clsPmpaType.JSIM.Next = clsPF.Reg_Get_Setting("JSIM", "NEXT");
            clsPmpaType.JSIM.MY = clsPF.Reg_Get_Setting("JSIM", "MY");
            clsPmpaType.JSIM.BackColor = clsPF.Reg_Get_Setting("JSIM", "COLOR");
            
            if (clsPmpaType.JSIM.DISP == "1")
            { rdbDisp1.Checked = true; }
            else
            { rdbDisp2.Checked = true; }
            
            if (clsPmpaType.JSIM.Next == "1")
            { rdbNext1.Checked = true; }
            else
            { rdbNext2.Checked = true; }
            
            if (clsPmpaType.JSIM.MY == "1")
            { rdbMy1.Checked = true; }
            else
            { rdbMy2.Checked = true; }

            if (clsPmpaType.JSIM.BackColor == "1")
            { rdbColor1.Checked = true; }
            else
            { rdbColor2.Checked = true; }

        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string strDisp = "1";
            string strNext = "1";
            string strMy = "1";
            string strColor = "1";

            if (rdbDisp2.Checked == true) { strDisp = "2"; }
            if (rdbNext2.Checked == true) { strNext = "2"; }
            if (rdbMy2.Checked == true) { strMy = "2"; }
            if (rdbColor2.Checked == true) { strColor = "2"; }

            clsPF.Reg_Save_Setting("JSIM", "DISP", strDisp);      //재원심사 완료시 자동 재원심사 화면표시 설정
            clsPF.Reg_Save_Setting("JSIM", "NEXT", strNext);      //다음환자 자동선태 설정
            clsPF.Reg_Save_Setting("JSIM", "MY", strMy);          //내환자 자동 설정
            clsPF.Reg_Save_Setting("JSIM", "COLOR", strColor);    //수가별 색상 표시

            clsPmpaType.JSIM.DISP = strDisp;
            clsPmpaType.JSIM.Next = strNext;
            clsPmpaType.JSIM.MY = strMy;
            clsPmpaType.JSIM.BackColor = strColor;

            ComFunc.MsgBox("설정이 완료되었습니다.");

            this.Close();
        }

    }
}


