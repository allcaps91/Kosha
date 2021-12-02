namespace ComEmrBase
{
    public interface FormEmrMessage
    {
        void MsgSave(string strSaveFlag); //저장 이벤트
        void MsgDelete(); //삭제 이벤트
        void MsgClear(); //Clear 이벤트
        void MsgPrint(); //Print 이벤트
    }
}
