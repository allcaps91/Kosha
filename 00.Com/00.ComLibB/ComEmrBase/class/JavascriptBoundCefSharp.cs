namespace ComEmrBase
{
    public class JavascriptBoundCefSharp
    {
        public bool IsSigned { get; set; }
        private frmEasDesigner designer = null;

        public JavascriptBoundCefSharp(frmEasDesigner designer)
        {
            this.designer = designer;
        }

        /// <summary>
        ///  winform-login.js 에서 호출
        /// </summary>
        /// <param name="userId"></param>
        public void SetSigned(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                IsSigned = false;
            }
            else
            {
                IsSigned = true;
                designer.LoadUrl();
            }
        }
        /// <summary>
        /// view.js 에서 호출
        /// </summary>
        public void SetPrintPageLoadCompleted()
        {
           designer.Print();
        }

        /// <summary>
        /// 서식 수정 준비 완료 form-main.js에서 호출함
        /// </summary>
        public void ReadyFormMain()
        {
            designer.Edit();
        }

    }
}
