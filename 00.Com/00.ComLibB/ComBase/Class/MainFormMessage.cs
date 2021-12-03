using System.Windows.Forms;

namespace ComBase
{
    public interface MainFormMessage
    {
        void MsgActivedForm(Form frm); //Active Form 이벤트
        void MsgUnloadForm(Form frm); //Unload Form 이벤트
        void MsgFormClear();   //폼을 초기화 한다.
        void MsgSendPara(string strPara);   //폼에 변수값을 전달한다.
    }
}
