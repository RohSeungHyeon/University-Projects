package teamproject.accountbook;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.app.DialogFragment;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.os.Bundle;
import android.util.SparseBooleanArray;
import android.view.View;
import android.view.ViewGroup;
import android.view.Window;
import android.widget.ArrayAdapter;
import android.widget.CheckBox;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import java.util.ArrayList;
import java.util.Calendar;

public class SelectFilterOption extends Activity {
    ListView listview;
    ArrayAdapter adapter;
    ArrayList<String> items;
    EditText startDate;
    EditText endDate;
    String FileName;
    static SelectFilterOption.DatePickerFragment myDatePicker;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE); // 타이틀 바 없애기
        setContentView(R.layout.select_fliter_option);
        items = new ArrayList<String>();
        adapter = new ArrayAdapter(this, android.R.layout.simple_list_item_multiple_choice, items) {
            @Override
            public View getView(int position, View convertView, ViewGroup parent) {
                View view = super.getView(position, convertView, parent);
                TextView tv = (TextView) view.findViewById(android.R.id.text1);
                tv.setTextColor(Color.rgb(46, 46, 46));

                return view;
            }
        };
        listview = (ListView) findViewById(R.id.select_category);
        listview.setAdapter(adapter);
        startDate = findViewById(R.id.startdate);
        endDate = findViewById(R.id.enddate);
        Intent intent = getIntent();
        FileName = intent.getStringExtra("FileName");
        String tmp = intent.getStringExtra("ALLCategory");

        String[] category = tmp.split("/");
        for (String str : category) if (!str.isEmpty()) items.add(str);

        Calendar cal = Calendar.getInstance();
        int year = cal.get(Calendar.YEAR);
        int mon = cal.get(Calendar.MONTH);
        int day = cal.get(Calendar.DATE);
        String start = "1970-01-01";
        String end = year + "-" + (mon + 1) + "-" + day;
        startDate.setText(start);
        endDate.setText(end);

    }

    public void onClick_FilterSet(View view) { // "확인" 버튼 클릭 시
        SparseBooleanArray checkedItems = listview.getCheckedItemPositions();
        String tmp = "";
        String start, end;
        for (int i = 0; i < adapter.getCount(); i++)
            if (checkedItems.get(i)) tmp = tmp + "/" + items.get(i);

        Intent intent = new Intent(SelectFilterOption.this, FilterResult.class);
        intent.putExtra("INPUT_ACCOUNT_BOOK", FileName);
        start = startDate.getText().toString();
        end = endDate.getText().toString();
        if (start.compareTo(end) > 0) start = end;
        intent.putExtra("Date", start + "/" + end);
        intent.putExtra("Category", tmp);
        startActivity(intent);
        finish();
    }

    public void onClick_NoFilter(View view) {
        finish();
    } // "닫기" 버튼 클릭 시

    public void pickStartDate(View view) { // 시작 날짜 EditText 클릭 시
        if (myDatePicker == null) {
            myDatePicker = new SelectFilterOption.DatePickerFragment();
            myDatePicker.setEditTextForDatePicker(startDate);
            myDatePicker.show(getFragmentManager(), "DatePicker");
        }
    }

    public void pickEndDate(View view) { // 종료 날짜 EditText 클릭 시
        if (myDatePicker == null) {
            myDatePicker = new SelectFilterOption.DatePickerFragment();
            myDatePicker.setEditTextForDatePicker(endDate);
            myDatePicker.show(getFragmentManager(), "DatePicker");

        }
    }

    public void checkAll(View view) {
        int size = adapter.getCount();
        for (int i = 0; i < size; i++)
            listview.setItemChecked(i, true);
    }

    public void checkClear(View view) {
        int size = adapter.getCount();
        for (int i = 0; i < size; i++)
            listview.setItemChecked(i, false);
    }

    public static class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
        EditText tmpDate;
        Calendar cal = Calendar.getInstance();
        int year = cal.get(Calendar.YEAR);
        int mon = cal.get(Calendar.MONTH);
        int day = cal.get(Calendar.DATE);

        public void setEditTextForDatePicker(EditText date) { //날짜를 표시할 뷰 설정
            tmpDate = date;
        }

        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) { //다이얼로그 생성
            return new DatePickerDialog(getActivity(), AlertDialog.THEME_HOLO_DARK,
                    this, year, mon, day);
        }

        @Override
        public void onDismiss(DialogInterface dialog) {
            myDatePicker = null;
            super.onDismiss(dialog);
        }

        @Override
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) { //선택된 날짜로 뷰에 표시
            tmpDate.setText(year + "-" + (monthOfYear + 1) + "-" + dayOfMonth);
        }
    }
}
