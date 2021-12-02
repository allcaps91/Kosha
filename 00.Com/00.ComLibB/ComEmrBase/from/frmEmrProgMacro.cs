using System;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrProgMacro : Form
    {
        string gStrId = "";
        string gStrIdIdx = "";
        string gStrFormNo = "";
        string gStrMaxLength = "";
        //string gStrMaxRow = "";
        string gStrLabel = "";
        //string pGrpNo = "";

        //이벤트를 전달할 경우
        //public delegate void SetMacroAll(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx);
        //public event SetMacroAll rSetMacroAll;
        //public delegate void SetMacroOne(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx);
        //public event SetMacroOne rSetMacroOne;
        ////폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //string mstrS = "";
        //string mstrO = "";
        //string mstrA = "";
        //string mstrP = "";
        //string mstrG = "";
        string mstrTag = "";

        Control mFrm = null;

        public frmEmrProgMacro()
        {
            InitializeComponent();
        }

        public frmEmrProgMacro(Control pfrm, string strTag)
        {
            InitializeComponent();
            mFrm = pfrm;
            mstrTag = strTag;
        }

        public frmEmrProgMacro(string strStrId, string strStrIdIdx, string strStrFormNo, string strStrMaxLength, string strStrLabel)
        {
            InitializeComponent();

            gStrId = strStrId;
            gStrIdIdx = strStrIdIdx;
            gStrFormNo = strStrFormNo;
            gStrMaxLength = strStrMaxLength;
            gStrLabel = strStrLabel;
        }

        private void frmEmrProgMacro_Load(object sender, EventArgs e)
        {

        }
    }
}
