package teamproject.accountbook;

import android.content.Intent;
import android.provider.ContactsContract;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.ContextMenu;
import android.view.Display;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ExpandableListView;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.sql.Array;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Date;

public class FilterResult extends AppCompatActivity {
    private ExpandableListView mainList;
    ExpandAdapter mAdapter;
    String FILE_NAME;
    String DateFilter;
    String CategoryFilter;
    ArrayList<String> categoryList;

    TextView totalIncome;
    TextView totalOutcome;
    TextView totalProfit;

    // 리스트뷰 크기 설정
    View fl;
    float flSize;
    View ll;
    float llSize;
    ViewGroup.LayoutParams params;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_filter_result);
        totalIncome = findViewById(R.id.edit_TotalIncome);
        totalOutcome = findViewById(R.id.edit_TotalOutcome);
        totalProfit = findViewById(R.id.edit_Profit);

        Intent intent = getIntent();
        FILE_NAME = intent.getExtras().getString("INPUT_ACCOUNT_BOOK"); // 파일 이름 저장
        DateFilter = intent.getExtras().getString("Date");  // 날짜 필터용 문자열 저장
        CategoryFilter = intent.getExtras().getString("Category");  // 항목 필터용 문자열 저장
        String CategoryArr[] = CategoryFilter.split("/");
        categoryList = new ArrayList<>(Arrays.asList(CategoryArr));
        mainList = (ExpandableListView) findViewById(R.id.MainListView);
        mAdapter = new ExpandAdapter(getApplicationContext(), R.layout.main_listview_item, R.layout.child_listview_item);
        mainList.setAdapter(mAdapter);
        setTitle("기간: " + DateFilter.split("/")[0] + " ~ " + DateFilter.split("/")[1]);
        loadItemsFromFile();  // 날짜/항목 필터를 통해 걸러진 아이템들을 불러옴

        mAdapter.notifyDataSetChanged();
        updateTotal();
        for (int i = 0; i < mAdapter.getGroupCount(); i++)
            updateGroupTotal(i);
    }

    // 리스트뷰 height 공간에 맞게 변경하기
    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        fl = findViewById(R.id.filterResult_FL);
        ll = findViewById(R.id.filterResult_TL);

        flSize = fl.getHeight();
        llSize = ll.getHeight();

        //Log.i("리스트뷰 높이 : ", Float.toString(flSize) + ", " + Float.toString(llSize));

        params = mainList.getLayoutParams();

        params.height=(int) (flSize-llSize);
        mainList.setLayoutParams(params);

        mainList.setIndicatorBounds(mainList.getWidth()-mainList.getWidth()/10, 0);
        //Log.i("리스트뷰 높이 : ", Integer.toString(params.height));
    }

    private void updateTotal() { //순이익 총수익 총지출 업데이트 함수
        int income = mAdapter.getALLIncome();
        int outcome = mAdapter.getALLOutcome();
        int profit = income - outcome;
        totalIncome.setText(Integer.toString(income));
        totalOutcome.setText(Integer.toString(outcome));
        totalProfit.setText(Integer.toString(profit));
    }

    private boolean isCategory(String _Category) { // 항목 필터 함수
        return categoryList.contains(_Category);
    }

    private boolean isDate(String _Date) { // 날짜 필터 함수
        String DateArr[] = DateFilter.split("/");
        SimpleDateFormat transFormat = new SimpleDateFormat("yyyy-MM-dd");
        Date start, end, compareDate;
        try {
            start = transFormat.parse(DateArr[0]);
            end = transFormat.parse(DateArr[1]);
            compareDate = transFormat.parse(_Date);

            if (compareDate.compareTo(start) >= 0 && compareDate.compareTo(end) <= 0) {
                return true;
            }
        } catch (ParseException e) {
            e.printStackTrace();
        }
        return false;
    }

    private void updateGroupTotal(int groupPosition) { //하나의 항목의 수량, 지출, 수입 합 업데이트
        int income = mAdapter.getIndexIncome(groupPosition);
        int outcome = mAdapter.getIndexOutcome(groupPosition);
        int count = mAdapter.getIndexCount(groupPosition);

        mAdapter.getGroup(groupPosition).setCount(count);
        mAdapter.getGroup(groupPosition).setIncome(income);
        mAdapter.getGroup(groupPosition).setOutcome(outcome);
    }

    private void loadItemsFromFile() { // ArrayList Item을 파일로부터 읽어들이는 함수 작성(필터링을 통해 필요한 항목, 항목내역을 걸러서 저장)
        File file = new File(getFilesDir(), FILE_NAME);
        FileReader fr = null;
        BufferedReader bufrd = null;
        String strArr[];
        String str;
        String nowCategory = "";
        int i = -1;
        int pos = 0;

        if (file.exists()) {
            try {
                //open file.
                fr = new FileReader(file);
                bufrd = new BufferedReader(fr);

                while ((str = bufrd.readLine()) != null) {
                    strArr = str.split(" ");
                    if (strArr[0].equals("0")) {
                        if (isCategory(strArr[1])) {
                            nowCategory = strArr[1];
                            mAdapter.addGroupView(new MainListView(strArr[1]));
                        } else nowCategory = "";
                    } else if (strArr[0].equals("1")) {
                        if (nowCategory.length() == 0) continue;
                        if (isDate(strArr[1])) {
                            for (pos = 0; pos < mAdapter.getGroupCount(); pos++) {
                                if (mAdapter.getGroup(pos).getCategory().equals(nowCategory)) break;
                            }
                            mAdapter.addChildView(pos, new ChildListView(strArr[1], Integer.parseInt(strArr[2]), Integer.parseInt(strArr[3]), Integer.parseInt(strArr[4])));
                        }
                    }
                }

                bufrd.close();
                fr.close();

            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    @Override
    protected void onDestroy() { // 종료 시, 파일에 데이터 저장
        super.onDestroy();
    }
}
