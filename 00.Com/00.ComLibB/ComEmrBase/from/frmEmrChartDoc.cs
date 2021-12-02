using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComEmrBase
{
    public partial class frmEmrChartDoc : Form
    {
        //EmrPatient p = null;

        FormXml[] mFormXml = null;
        FormXml[] mFormXmlInit = null;

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public string mstrFormNo = "2185";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";
        #endregion

        public frmEmrChartDoc()
        {
            InitializeComponent();
        }

        public frmEmrChartDoc(string strFormNo, string strUpdateNo, string strEmrNo)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
        }

        private void frmEmrChartDoc_Load(object sender, EventArgs e)
        {
            panTopMenu.Controls.Clear();

            mFormXmlInit = FormDesignQuery.GetDataFormXml(mstrFormNo, mstrUpdateNo);
            if (mFormXmlInit == null)
            {
                return;
            }

            if (mFormXmlInit != null)
            {
                mFormXml = mFormXmlInit;
                for (int i = 0; i < mFormXml.Length; i++)
                {
                    if (mFormXml[i].strCONTROLPARENT == "Form1")
                    {
                        mFormXml[i].strCONTROLPARENT = "panChart";
                    }

                    if (mFormXml[i].strCONTROTYPE == "System.Windows.Forms.Panel")
                    {
                        mFormXml[i].strCONTROTYPE = "mtsPanel15.mPanel";
                    }
                }

                FormLoadControl.LoadControl(this, mFormXml, "panChart");
            }
        }

        




    }
}
