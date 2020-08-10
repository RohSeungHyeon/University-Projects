package teamproject.accountbook;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.widget.EditText;

public class ModifyCategory extends Activity {
    EditText editCategory;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        String tmp = getIntent().getStringExtra("prev_groupName");
        requestWindowFeature(Window.FEATURE_NO_TITLE); // 타이틀 바 없애기
        setContentView(R.layout.category_modify);
        editCategory = (EditText) findViewById(R.id.edtCategory);
        editCategory.setText(tmp);
    }

    public void onClick_Add(View view) { // "추가하기" 클릭 시, 결과값 ABViewActivity에게 전달하고 종료
        Intent intent = new Intent();
        intent.putExtra("INPUT_CATEGORY", editCategory.getText().toString());
        setResult(RESULT_OK, intent);
        finish();
    }

    public void onClick_Close(View view) {
        finish();
    }
}
