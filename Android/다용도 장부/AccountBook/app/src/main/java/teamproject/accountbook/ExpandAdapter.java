package teamproject.accountbook;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;
import android.widget.TextView;

import java.util.ArrayList;

public class ExpandAdapter extends BaseExpandableListAdapter {
    private Context context;
    private int parentLayout = 0;
    private int chlidLayout = 0;
    private ArrayList<MainListView> DataList = new ArrayList<MainListView>();
    private LayoutInflater myinf = null;

    public ExpandAdapter(Context context, int _parentLayout, int _chlidLayout) {
        this.parentLayout = _parentLayout;
        this.chlidLayout = _chlidLayout;
        this.context = context;
        this.myinf = (LayoutInflater) context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
    }

    @Override
    public View getGroupView(int groupPosition, boolean isExpanded, View convertView, ViewGroup parent) {
        // TODO Auto-generated method stub
        if (convertView == null) {
            convertView = myinf.inflate(this.parentLayout, parent, false);
        }
        TextView CategoryEdit = (TextView) convertView.findViewById(R.id.pItem_Category);
        CategoryEdit.setText(DataList.get(groupPosition).getCategory());

        TextView DateEdit = (TextView) convertView.findViewById(R.id.pItem_Date);
        DateEdit.setText(DataList.get(groupPosition).getDate());

        TextView CountEdit = (TextView) convertView.findViewById(R.id.pItem_Count);
        CountEdit.setText("" + DataList.get(groupPosition).getCount());

        TextView IncomeEdit = (TextView) convertView.findViewById(R.id.pItem_Income);
        IncomeEdit.setText("" + DataList.get(groupPosition).getIncome());

        TextView OutcomeEdit = (TextView) convertView.findViewById(R.id.pItem_Outcome);
        OutcomeEdit.setText("" + DataList.get(groupPosition).getOutcome());

        return convertView;
    }

    @Override
    public View getChildView(int groupPosition, int childPosition, boolean isLastChild, View convertView, ViewGroup parent) {
        // TODO Auto-generated method stub
        if (convertView == null) {
            convertView = myinf.inflate(this.chlidLayout, parent, false);
        }

        TextView DateEdit = (TextView) convertView.findViewById(R.id.cItem_Date);
        DateEdit.setText(DataList.get(groupPosition).Child.get(childPosition).getDate());

        TextView CountEdit = (TextView) convertView.findViewById(R.id.cItem_Count);
        CountEdit.setText("" + DataList.get(groupPosition).Child.get(childPosition).getCount());

        TextView IncomeEdit = (TextView) convertView.findViewById(R.id.cItem_Income);
        IncomeEdit.setText("" + DataList.get(groupPosition).Child.get(childPosition).getIncome());

        TextView OutcomeEdit = (TextView) convertView.findViewById(R.id.cItem_Outcome);
        OutcomeEdit.setText("" + DataList.get(groupPosition).Child.get(childPosition).getOutcome());

        return convertView;
    }

    public boolean addGroupView(MainListView _view) {
        int count = 0;
        for (int i = 0; i < DataList.size(); i++) {
            if (DataList.get(i).getCategory().equals(_view.getCategory())) {
                return false;
            }
        }
        DataList.add(_view);
        return true;
    }

    public void addChildView(int groupPosition, ChildListView _view) {
        DataList.get(groupPosition).Child.add(_view);
    }

    public void setGroupView(int groupPosition, String name) {
        DataList.get(groupPosition).setCategory(name);
    }

    public void setChildView(int groupPosition, int childPosition, ChildListView _view) {
        DataList.get(groupPosition).Child.get(childPosition).setDate(_view.getDate());
        DataList.get(groupPosition).Child.get(childPosition).setCount(_view.getCount());
        DataList.get(groupPosition).Child.get(childPosition).setIncome(_view.getIncome());
        DataList.get(groupPosition).Child.get(childPosition).setOutcome(_view.getOutcome());
    }

    public void removeGroup(int groupPosition) {
        if (!DataList.isEmpty()) {
            if (!DataList.get(groupPosition).Child.isEmpty())
                DataList.get(groupPosition).Child.clear();
            DataList.remove(groupPosition);
        }
    }

    public void removeChild(int groupPosition, int childPosition) {
        if (!DataList.isEmpty())
            if (!DataList.get(groupPosition).Child.isEmpty()) {
                DataList.get(groupPosition).Child.remove(childPosition);
            }
    }

    /*---------------------------------추가된 부분 시작------------------------------------------*/
    public int getIndexIncome(int groupPosition) {
        int income = 0;
        MainListView m = DataList.get(groupPosition);
        for (int i = 0; i < m.Child.size(); i++)
            income += m.Child.get(i).getIncome();
        return income;
    }

    public int getIndexOutcome(int groupPosition) {
        int outcome = 0;
        MainListView m = DataList.get(groupPosition);
        for (int i = 0; i < m.Child.size(); i++)
            outcome += m.Child.get(i).getOutcome();
        return outcome;
    }

    public int getIndexCount(int groupPosition) {
        int count = 0;
        MainListView m = DataList.get(groupPosition);
        for (int i = 0; i < m.Child.size(); i++) {
            if (m.Child.get(i).getIncome() > 0) {
                count -= m.Child.get(i).getCount();
            } else {
                count += m.Child.get(i).getCount();
            }
        }
        return count;
    }

    public int getALLIncome() {
        int income = 0;
        for (MainListView m : DataList)
            for (int i = 0; i < m.Child.size(); i++)
                income += m.Child.get(i).getIncome();
        return income;
    }

    public int getALLOutcome() {
        int outcome = 0;
        for (MainListView m : DataList)
            for (int i = 0; i < m.Child.size(); i++)
                outcome += m.Child.get(i).getOutcome();
        return outcome;
    }

    /*---------------------------------추가된 부분 끝------------------------------------------*/
    @Override
    public boolean hasStableIds() {
        // TODO Auto-generated method stub
        return true;
    }

    @Override
    public boolean isChildSelectable(int groupPosition, int childPosition) {
        // TODO Auto-generated method stub
        return true;
    }

    @Override
    public ChildListView getChild(int groupPosition, int childPosition) {
        // TODO Auto-generated method stub
        return DataList.get(groupPosition).Child.get(childPosition);
    }

    @Override
    public long getChildId(int groupPosition, int childPosition) {
        // TODO Auto-generated method stub
        return childPosition;
    }

    @Override
    public int getChildrenCount(int groupPosition) {
        // TODO Auto-generated method stub
        return DataList.get(groupPosition).Child.size();
    }

    @Override
    public MainListView getGroup(int groupPosition) {
        // TODO Auto-generated method stub
        return DataList.get(groupPosition);
    }

    @Override
    public int getGroupCount() {
        // TODO Auto-generated method stub
        return DataList.size();
    }

    @Override
    public long getGroupId(int groupPosition) {
        // TODO Auto-generated method stub
        return groupPosition;
    }

}
