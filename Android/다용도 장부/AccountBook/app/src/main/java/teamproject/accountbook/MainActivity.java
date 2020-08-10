package teamproject.accountbook;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.view.ActionMode;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Toast;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;

public class MainActivity extends AppCompatActivity {
    ActionMode mActionMode;
    private ListView m_ListView;
    private ArrayAdapter<String> m_Adapter;
    static final int GET_STRING = 1;
    int Lv_pos;
    File fileDir;
    ArrayList<File> fileDir_inside;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        fileDir = getFilesDir(); // Data/Data/files 경로

        // ListView에 어댑터 적용
        ArrayList<String> values = new ArrayList<>();

        m_Adapter = new ArrayAdapter<String>(this, android.R.layout.simple_list_item_1, values);

        m_ListView = (ListView) findViewById(R.id.AccountBookList);
        m_ListView.setAdapter(m_Adapter);

        // ListView 아이템 터치 시 이벤트를 처리할 리스너 설정
        m_ListView.setOnItemClickListener(onClickListItem);
        m_ListView.setOnItemLongClickListener(new AdapterView.OnItemLongClickListener() {
            @Override
            public boolean onItemLongClick(AdapterView parent, View view, int position, long id) {
                if (mActionMode != null) {
                    return false;
                }

                // 컨텍스트 액션 모드 시작
                Lv_pos = position;
                mActionMode = startSupportActionMode(modeCallBack);
                view.setSelected(true);
                return true;
            }
        });

        m_Adapter.notifyDataSetChanged();

    }

    private ActionMode.Callback modeCallBack = new ActionMode.Callback() {
        @Override
        public boolean onCreateActionMode(ActionMode mode, Menu menu) {
            //MenuInflater 객체를 이용하여 컨텍스트 메뉴 생성
            MenuInflater inflater = mode.getMenuInflater();
            inflater.inflate(R.menu.context_menu, menu);
            return true;
        }

        //onCreateActionMode()가 호출된 후에 호출.
        // 액션 메뉴를 refresh하는 목적으로 여러 번 호출될 수 있음
        @Override
        public boolean onPrepareActionMode(ActionMode mode, Menu menu) {
            return false; // 아무 것도 하지 않을 때 false 반환, 액션 메뉴가 업데이트 되면 true 반환
        }

        // 사용자가 액션 메뉴 항목을 클릭했을 때 호출
        @Override
        public boolean onActionItemClicked(ActionMode mode, MenuItem item) {
            if (item.getItemId() == R.id.action_DEL) {
                fileDir_inside.get(Lv_pos).delete(); // 파일 삭제
                fileDir_inside.remove(Lv_pos); // 리스트에서 제거
                m_Adapter.remove(m_Adapter.getItem(Lv_pos));
                mode.finish();
                Toast.makeText(getApplicationContext(), "장부 제거 성공!", Toast.LENGTH_SHORT).show();
                return true;
            } else {
                return false;
            }
        }

        // 사용자가 컨텍스트 액션 모드를 빠져나갈 때 호출
        @Override
        public void onDestroyActionMode(ActionMode mode) {
            mActionMode = null;
        }
    };

    private AdapterView.OnItemClickListener onClickListItem = new AdapterView.OnItemClickListener() {
        @Override
        public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
            // List 이벤트 발생 시
            Intent intent = new Intent(MainActivity.this, ABViewActivity.class);
            intent.putExtra("INPUT_ACCOUNT_BOOK", m_Adapter.getItem(position));
            startActivity(intent);
        }
    };

    @Override
    public boolean onCreateOptionsMenu(Menu menu) { // Create Action Bar
        getMenuInflater().inflate(R.menu.action_bar, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) { // Action Bar에서 선택 시
        if (item.getItemId() == R.id.action_ADD) {
            Intent intent = new Intent(MainActivity.this, addAccountBook.class);
            startActivityForResult(intent, GET_STRING);
            return true;
        } else {
            return super.onOptionsItemSelected(item);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) { // 항목 추가 다이얼로그 이후
        if (requestCode == GET_STRING) {
            if (resultCode == RESULT_OK) {
                if (m_Adapter.getPosition(data.getStringExtra("INPUT_ACCOUNT_BOOK")) == -1) { // 중복된 이름 없으면 생성
                    Intent intent = new Intent(MainActivity.this, ABViewActivity.class);
                    intent.putExtra("INPUT_ACCOUNT_BOOK", data.getStringExtra("INPUT_ACCOUNT_BOOK"));
                    startActivity(intent);
                } else {
                    Toast.makeText(getApplicationContext(), "중복된 이름이 있습니다.", Toast.LENGTH_SHORT).show();
                }
            }
        }
    }

    private void getFileList() { // 파일 리스트 호출
        m_Adapter.clear();
        if (fileDir.exists()) { // 내부에 파일이 존재한다면
            if (fileDir.isDirectory()) { // 파일이 디렉토리라면
                fileDir_inside = new ArrayList<>(Arrays.asList(fileDir.listFiles()));

                for (int i = 0; i < fileDir_inside.size(); i++) {
                    m_Adapter.add(fileDir_inside.get(i).getName());
                }
            }
        }
    }

    @Override
    protected void onStart() {
        super.onStart();
        getFileList();
    }
}
