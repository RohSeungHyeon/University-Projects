package teamproject.accountbook;

import java.util.ArrayList;

public class MainListView {
    private String m_Category;
    private String m_Date;
    private int m_Count;
    private int m_Income;
    private int m_Outcome;
    public ArrayList<ChildListView> Child;

    MainListView(String name) {
        setCategory(name);
        Child = new ArrayList<ChildListView>();
    }

    public void setCategory(String _Category) {
        m_Category = _Category;
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

    public String getCategory() {
        return m_Category;
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
