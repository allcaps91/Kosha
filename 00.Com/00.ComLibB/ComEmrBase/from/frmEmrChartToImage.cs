using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrChartToImage : Form
    {
        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public string mstrFormNo = "0";
        public string mstrUpdateNo = "0";
        public string mstrFormName = "";
        public string mstrProgForm = "";
        public EmrPatient p = null;
        public EmrForm frmFORM = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "V";

        public string mstrChartDate = "";
        public string mstrCurDate = "";

        public string mstrFORMZOOM = "";
        #endregion

        //private Form ActiveForm = null;
        //폼이 Close될 경우
        //public delegate void EventClosed();
        //public event EventClosed rEventClosed;

        //private string mCloseOption = "";

        //private EmrChartForm ActiveFormViewChart = null;

        #region //FormEmrMessage
        public FormEmrMessage mEmrCallForm = null;
        public void MsgSave(string strSaveFlag)
        {

        }
        public void MsgDelete()
        {

        }
        public void MsgClear()
        {

        }
        public void MsgPrint()
        {

        }
        #endregion


        //FormXml[] mFormXml = null;
        FormXml[] mFormXmlInit = null;

        private Font StringToFont(string font)
        {
            string[] parts = font.Split(':');
            if (parts.Length != 3)
                throw new ArgumentException("Not a valid font string", "font");

            Font loadedFont = new Font(parts[0], float.Parse(parts[1]), (FontStyle)int.Parse(parts[2]));
            return loadedFont;
        }

        public frmEmrChartToImage()
        {
            InitializeComponent();
        }

        public frmEmrChartToImage(EmrPatient pPara, string strFormNo, string strUpdateNo, string strEmrNo, string strChartDate, string strCurDate, FormXml[] pFormXml, string strFORMZOOM)
        {
            InitializeComponent();

            p = pPara;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mstrChartDate = strChartDate;
            mstrCurDate = strCurDate;
            mstrFORMZOOM = strFORMZOOM;

            mFormXmlInit = pFormXml;
        }

        public bool ConChartToImage(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strChartDate, string strCurDate)
        {
            bool rtnVal = false;
            return rtnVal;
        }
        private void frmEmrChartToImage_Load(object sender, EventArgs e)
        {

        }
    }
}
