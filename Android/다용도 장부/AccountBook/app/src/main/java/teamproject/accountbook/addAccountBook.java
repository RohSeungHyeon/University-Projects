package teamproject.accountbook;

import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.widget.EditText;
import android.widget.Toast;

public class addAccountBook extends Activity {
    EditText editAccountBook;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_NO_TITLE); // 타이틀 바 없애기
        setContentView(R.layout.activity_add_account_book);
        editAccountBook = (EditText) findViewById(R.id.edtAccountBook);
    }

    public void onClick_AB_Add(View view) { // "추가하기" 클릭 시, 결과값 MainActivity에게 전달하고 종료
        Intent intent = new Intent(addAccountBook.this, MainActivity.class);
        if (!editAccountBook.getText().toString().isEmpty()) {
            intent.putExtra("INPUT_ACCOUNT_BOOK", editAccountBook.getText().toString());
            setResult(RESULT_OK, intent);
        } else {
            Toast.makeText(getApplicationContext(), "이름은 공백으로 지정할 수 없습니다.", Toast.LENGTH_SHORT).show();
        }
        finish();
    }

    public void onClick_AB_Close(View view) {
        finish();
    }
}
