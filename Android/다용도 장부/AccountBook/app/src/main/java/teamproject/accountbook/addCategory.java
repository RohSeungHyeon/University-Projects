package teamproject.accountbook;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.widget.EditText;
import android.widget.Toast;

public class addCategory extends Activity {
    EditText editCategory;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_NO_TITLE); // 타이틀 바 없애기
        setContentView(R.layout.category_add);
        editCategory = (EditText) findViewById(R.id.edtCategory);
    }

    public void onClick_Add(View view) { // "추가하기" 클릭 시, 결과값 ABViewActivity에게 전달하고 종료
        if (editCategory.getText().toString().isEmpty()) {
            Toast.makeText(getApplicationContext(), "항목은 빈 칸이 될 수 없습니다.", Toast.LENGTH_SHORT).show();
        } else {
            Intent intent = new Intent();
            intent.putExtra("INPUT_CATEGORY", editCategory.getText().toString());
            setResult(RESULT_OK, intent);
            finish();
        }
    }

    public void onClick_Close(View view) {
        finish();
    }
}
