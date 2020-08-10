package teamproject.accountbook;

public class ChildListView {
    private String m_Date;
    private int m_Count;
    private int m_Income;
    private int m_Outcome;

    ChildListView() {
    }

    ChildListView(String _Date, int _Count, int _Income, int _Outcome) {
        m_Date = _Date;
        m_Count = _Count;
        m_Income = _Income;
        m_Outcome = _Outcome;
    }

    public void setDate(String _Date) {
        m_Date = _Date;
    }

    public void setCount(int _Count) {
        m_Count = _Count;
    }

    public void setIncome(int _Income) {
        m_Income = _Income;
    }

    public void setOutcome(int _Outcome) {
        m_Outcome = _Outcome;
    }
    public String getDate() {
        return m_Date;
    }

    public int getCount() {
        return m_Count;
    }

    public int getIncome() {
        return m_Income;
    }

    public int getOutcome() {
        return m_Outcome;
    }
}
