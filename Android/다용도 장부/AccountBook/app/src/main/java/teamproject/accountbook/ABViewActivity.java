package teamproject.accountbook;

import android.content.Context;
import android.content.Intent;
import android.graphics.Point;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.ContextMenu;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ExpandableListView;
import android.widget.FrameLayout;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;

import static android.app.Activity.RESULT_OK;

public class ABViewActivity extends AppCompatActivity {
    private ExpandableListView mainList;
    ExpandAdapter mAdapter;
    static final int GET_STRING = 1;
    static final int FOR_MODIFY_CHILD = 2;
    static final int FOR_MODIFY_GROUP = 3;
    static final int FOR_ADD = 4;
    int childposition;
    int groupposition;
    String FILE_NAME;

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
        setContentView(R.layout.activity_abview);
        totalIncome = findViewById(R.id.edit_TotalIncome);
        totalOutcome = findViewById(R.id.edit_TotalOutcome);
        totalProfit = findViewById(R.id.edit_Profit);

        Intent intent = getIntent();
        FILE_NAME = intent.getExtras().getString("INPUT_ACCOUNT_BOOK"); // MainAcitivy로부터 File 이름 불러옴
        setTitle("< " + FILE_NAME + " >");

        // ExpandableListView에 Adapter 적용
        mainList = (ExpandableListView) findViewById(R.id.MainListView);
        mAdapter = new ExpandAdapter(getApplicationContext(), R.layout.main_listview_item, R.layout.child_listview_item);
        mainList.setAdapter(mAdapter);

        loadItemsFromFile(); // 파일로부터 Item 저장
        saveItemsToFile(); // 파일에 Item 저장

        mAdapter.notifyDataSetChanged();
        updateTotal(); // 총수입, 총지출, 순이익 Update

        // 항목별 총수량, 수입, 지출 Update
        for (int i = 0; i < mAdapter.getGroupCount(); i++) {
            updateGroupTotal(i);
            mAdapter.getGroup(i).setDate("소계");
        }
        registerForContextMenu(mainList);

    }

    // 리스트뷰 height 공간에 맞게 변경하기
    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        fl = findViewById(R.id.abview_FL);
        ll = findViewById(R.id.abview_TL);

        flSize = fl.getHeight();
        llSize = ll.getHeight();

        //Log.i("리스트뷰 높이 : ", Float.toString(flSize) + ", " + Float.toString(llSize));

        params = mainList.getLayoutParams();

        params.height=(int) (flSize-llSize);
        mainList.setLayoutParams(params);

        mainList.setIndicatorBounds(mainList.getWidth()-mainList.getWidth()/10, 0);
        //Log.i("리스트뷰 너비 : ", Integer.toString(mainList.getWidth()));
    }

    public void onClickAddCategory(View view) { // 항목 추가 버튼 클릭 시
        Intent intent = new Intent(ABViewActivity.this, addCategory.class);
        startActivityForResult(intent, GET_STRING);
    }

    private void updateTotal() { //순이익 총수익 총지출 업데이트 함수
        int income = mAdapter.getALLIncome();
        int outcome = mAdapter.getALLOutcome();
        int profit = income - outcome;
        totalIncome.setText(Integer.toString(income));
        totalOutcome.setText(Integer.toString(outcome));
        totalProfit.setText(Integer.toString(profit));

    }

    private void updateGroupTotal(int groupPosition) { //하나의 항목의 수량, 지출, 수입 합 업데이트
        int income = mAdapter.getIndexIncome(groupPosition);
        int outcome = mAdapter.getIndexOutcome(groupPosition);
        int count = mAdapter.getIndexCount(groupPosition);

        mAdapter.getGroup(groupPosition).setCount(count);
        mAdapter.getGroup(groupPosition).setIncome(income);
        mAdapter.getGroup(groupPosition).setOutcome(outcome);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == GET_STRING) { // addCategory 메인 액티비티로부터 결과를 받아옴
            if (resultCode == RESULT_OK) {
                if (mAdapter.addGroupView(new MainListView(data.getStringExtra("INPUT_CATEGORY")))) {
                    mAdapter.notifyDataSetChanged();
                    Toast.makeText(getApplicationContext(), "항목 추가 성공", Toast.LENGTH_SHORT).show();
                } else {
                    Toast.makeText(getApplicationContext(), "항목 이름 중복", Toast.LENGTH_SHORT).show();
                }
            } else {
                Toast.makeText(getApplicationContext(), "항목 추가 실패", Toast.LENGTH_SHORT).show();
            }
        } else if (requestCode == FOR_MODIFY_CHILD) { // 항목 내용 수정 시
            if (resultCode == RESULT_OK) {
                int unitPrice = data.getIntExtra("UnitPrice", 0);
                int totalPrice = data.getIntExtra("TotalPrice", 0);
                int count = data.getIntExtra("Count", 0);
                String select = data.getStringExtra("radioSelect");
                int income = 0, outcome = 0;
                if(select.equals("수입")) income = unitPrice * count + totalPrice;
                else outcome = unitPrice * count + totalPrice;
                mAdapter.getChild(groupposition, childposition).setCount(count);
                mAdapter.getChild(groupposition, childposition).setIncome(income);
                mAdapter.getChild(groupposition, childposition).setOutcome(outcome);
                mAdapter.getChild(groupposition, childposition).setDate(data.getStringExtra("Date"));
                mAdapter.notifyDataSetChanged();
                updateTotal();
                updateGroupTotal(groupposition);
            }
        } else if (requestCode == FOR_ADD) { // 항목 내용 추가 시
            if (resultCode == RESULT_OK) {
                String tmpDate = data.getStringExtra("Date");
                int unitPrice = data.getIntExtra("UnitPrice", 0);
                int totalPrice = data.getIntExtra("TotalPrice", 0);
                int count = data.getIntExtra("Count", 0);
                String select = data.getStringExtra("radioSelect");
                int income = 0, outcome = 0;
                if(select.equals("수입")) income = unitPrice * count + totalPrice;
                else outcome = unitPrice * count + totalPrice;
                ChildListView tmpChild = new ChildListView(tmpDate, count, income, outcome);
                mAdapter.addChildView(groupposition, tmpChild);
                mAdapter.notifyDataSetChanged();
                updateTotal();
                updateGroupTotal(groupposition);
            }
        } else if (requestCode == FOR_MODIFY_GROUP) { // 항목 수정 시
            if (resultCode == RESULT_OK) {
                String modified_Name = data.getStringExtra("INPUT_CATEGORY");
                mAdapter.getGroup(groupposition).setCategory(modified_Name);
                mAdapter.notifyDataSetChanged();
            }
        }
        saveItemsToFile();
    }

    public boolean onCreateOptionsMenu(Menu menu) { // Create Action Bar
        getMenuInflater().inflate(R.menu.listview_action_bar, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) { // Action Bar에서 선택 시
        if (item.getItemId() == R.id.action_Filter) {
            String category = "";
            for (int i = 0; i < mAdapter.getGroupCount(); i++) {
                category = category + "/" + mAdapter.getGroup(i).getCategory();
            }
            Intent intent = new Intent(ABViewActivity.this, SelectFilterOption.class);
            intent.putExtra("FileName", FILE_NAME);
            intent.putExtra("ALLCategory", category);
            startActivity(intent);
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }

    public void onCreateContextMenu(ContextMenu menu, View v, ContextMenu.ContextMenuInfo menuInfo) {
        super.onCreateContextMenu(menu, v, menuInfo);
        ExpandableListView.ExpandableListContextMenuInfo info = (ExpandableListView.ExpandableListContextMenuInfo) menuInfo;
        int itemType = ExpandableListView.getPackedPositionType(info.packedPosition);
        if (itemType == ExpandableListView.PACKED_POSITION_TYPE_GROUP) {
            getMenuInflater().inflate(R.menu.context_menu_group, menu);
        } else if (itemType == ExpandableListView.PACKED_POSITION_TYPE_CHILD) {
            getMenuInflater().inflate(R.menu.context_menu_child, menu);
        }
    }

    public boolean onContextItemSelected(MenuItem item) {
        ExpandableListView.ExpandableListContextMenuInfo info = (ExpandableListView.ExpandableListContextMenuInfo) item.getMenuInfo();
        Intent intent;
        switch (item.getItemId()) {
            case R.id.child_modify:
                childposition = ExpandableListView.getPackedPositionChild(info.packedPosition);
                groupposition = ExpandableListView.getPackedPositionGroup(info.packedPosition);
                intent = new Intent(ABViewActivity.this, PopUpActivity.class);
                intent.putExtra("Date", mAdapter.getChild(groupposition, childposition).getDate());
                intent.putExtra("Count", mAdapter.getChild(groupposition, childposition).getCount());
                startActivityForResult(intent, FOR_MODIFY_CHILD);
                break;
            case R.id.group_modify:
                childposition = ExpandableListView.getPackedPositionChild(info.packedPosition);
                groupposition = ExpandableListView.getPackedPositionGroup(info.packedPosition);
                intent = new Intent(ABViewActivity.this, ModifyCategory.class);
                intent.putExtra("prev_groupName", mAdapter.getGroup(groupposition).getCategory());
                Toast.makeText(getApplicationContext(), "항목 수정", Toast.LENGTH_SHORT).show();
                startActivityForResult(intent, FOR_MODIFY_GROUP);
                break;
            case R.id.del_child:
                childposition = ExpandableListView.getPackedPositionChild(info.packedPosition);
                groupposition = ExpandableListView.getPackedPositionGroup(info.packedPosition);
                mAdapter.removeChild(groupposition, childposition);
                mAdapter.notifyDataSetChanged();
                updateTotal();
                updateGroupTotal(groupposition);
                Toast.makeText(getApplicationContext(), "항목 내역 삭제", Toast.LENGTH_SHORT).show();
                break;
            case R.id.del_group:
                childposition = ExpandableListView.getPackedPositionChild(info.packedPosition);
                groupposition = ExpandableListView.getPackedPositionGroup(info.packedPosition);
                mAdapter.removeGroup(groupposition);
                mAdapter.notifyDataSetChanged();
                updateTotal();
                Toast.makeText(getApplicationContext(), "항목 삭제", Toast.LENGTH_SHORT).show();
                break;
            case R.id.add:
                childposition = ExpandableListView.getPackedPositionChild(info.packedPosition);
                groupposition = ExpandableListView.getPackedPositionGroup(info.packedPosition);
                intent = new Intent(ABViewActivity.this, PopUpActivity.class);
                Toast.makeText(getApplicationContext(), "항목 내역 추가", Toast.LENGTH_SHORT).show();
                startActivityForResult(intent, FOR_ADD);
                break;
            default:
                return super.onContextItemSelected(item);
        }
        return true;
    }

    private void saveItemsToFile() { // 데이터를 파일에 저장하는 함수
        File file = new File(getFilesDir(), FILE_NAME);
        FileWriter fw = null;
        BufferedWriter bufwr = null;

        try {
            // Open file
            fw = new FileWriter(file);
            bufwr = new BufferedWriter(fw);

            for (int i = 0; i < mAdapter.getGroupCount(); i++) {
                bufwr.write("0 " + mAdapter.getGroup(i).getCategory());
                bufwr.newLine();

                for (int j = 0; j < mAdapter.getGroup(i).Child.size(); j++) {
                    bufwr.write("1 " + mAdapter.getChild(i, j).getDate() + " " + mAdapter.getChild(i, j).getCount() + " " +
                            mAdapter.getChild(i, j).getIncome() + " " + mAdapter.getChild(i, j).getOutcome());
                    bufwr.newLine();
                }
            }

            // write data to the file.
            bufwr.flush();
        } catch (Exception e) {
            e.printStackTrace();
        }

        try {
            // close file.
            if (bufwr != null) {
                bufwr.close();
            }

            if (fw != null) {
                fw.close();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void loadItemsFromFile() { // ArrayList Item을 파일로부터 읽어들이는 함수 작성
        File file = new File(getFilesDir(), FILE_NAME);
        FileReader fr = null;
        BufferedReader bufrd = null;
        String strArr[];
        String str;
        int i = -1;

        if (file.exists()) {
            try {
                //open file.
                fr = new FileReader(file);
                bufrd = new BufferedReader(fr);

                while ((str = bufrd.readLine()) != null) {
                    strArr = str.split(" ");
                    if (strArr[0].equals("0")) {
                        i++;
                        mAdapter.addGroupView(new MainListView(strArr[1]));
                    } else if (strArr[0].equals("1")) {
                        mAdapter.addChildView(i, new ChildListView(strArr[1], Integer.parseInt(strArr[2]), Integer.parseInt(strArr[3]), Integer.parseInt(strArr[4])));
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
        saveItemsToFile();
    }
}
