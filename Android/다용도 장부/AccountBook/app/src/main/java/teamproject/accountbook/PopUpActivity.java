package teamproject.accountbook;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.DatePickerDialog;
import android.app.Dialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.app.DialogFragment;
import android.view.Menu;
import android.view.View;
import android.view.Window;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Toast;

import java.util.Calendar;

public class PopUpActivity extends Activity {
    EditText editCount;
    EditText editDate;
    EditText editUnitPrice;
    EditText editTotalPrice;
    RadioGroup selectGroup;
    static PopUpActivity.DatePickerFragment myDatePicker;

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        requestWindowFeature(Window.FEATURE_NO_TITLE); // 타이틀 바 없애기
        setContentView(R.layout.popup_activity);
        editCount = findViewById(R.id.count);
        editDate = findViewById(R.id.date);
        editUnitPrice = findViewById(R.id.UnitPrice);
        editTotalPrice = findViewById(R.id.TotalPrice);
        selectGroup = findViewById(R.id.choiceMode);
        Intent intent = getIntent();
        if (intent.getStringExtra("Date") != null) {
            editCount.setText(Integer.toString(intent.getIntExtra("Count", 0)));
            editDate.setText(intent.getStringExtra("Date"));
            editUnitPrice.setText("0");
            editTotalPrice.setText("0");
        }
        selectGroup.check(R.id.income_select);

        editUnitPrice.setFocusable(false);
        editTotalPrice.setFocusable(false);
    }


    public void onClick_Add(View view) { // "확인" 버튼 클릭 시
        Intent intent = new Intent();
        if (editDate.getText().toString().isEmpty()) intent.putExtra("Date",
                Calendar.getInstance().get(Calendar.YEAR) + "-" + (Calendar.getInstance().get(Calendar.MONTH) + 1) + "-" + Calendar.getInstance().get(Calendar.DATE));
        else intent.putExtra("Date", editDate.getText().toString());
        if (editCount.getText().toString().isEmpty()) intent.putExtra("Count", 0);
        else intent.putExtra("Count", Integer.valueOf(editCount.getText().toString()));
        if (editUnitPrice.getText().toString().isEmpty()) intent.putExtra("UnitPrice", 0);
        else intent.putExtra("UnitPrice", Integer.valueOf(editUnitPrice.getText().toString()));
        if (editTotalPrice.getText().toString().isEmpty()) intent.putExtra("TotalPrice", 0);
        else intent.putExtra("TotalPrice", Integer.valueOf(editTotalPrice.getText().toString()));
        int id = selectGroup.getCheckedRadioButtonId();
        RadioButton radioButton = findViewById(id);
        intent.putExtra("radioSelect", radioButton.getText().toString());
        //Toast.makeText(getApplicationContext(), radioButton.getText().toString(), Toast.LENGTH_SHORT).show();
        setResult(RESULT_OK, intent);
        finish();
    }

    public void onClick_selectInOut(View view) { // 수입/지출 EditText 클릭 시, 호출 함수
        Toast.makeText(getApplicationContext(), "단가/총가 중 하나만 입력해주십시오.", Toast.LENGTH_SHORT).show();
        switch (view.getId()) {
            case R.id.UnitPrice:
                editUnitPrice.setText("");
                editUnitPrice.setFocusable(true);
                editUnitPrice.setFocusableInTouchMode(true);
                editUnitPrice.requestFocus();
                editTotalPrice.setText("0");
                editTotalPrice.setFocusable(false);
                editTotalPrice.setFocusableInTouchMode(false);
                break;
            case R.id.TotalPrice:
                editTotalPrice.setText("");
                editTotalPrice.setFocusable(true);
                editTotalPrice.setFocusableInTouchMode(true);
                editTotalPrice.requestFocus();
                editUnitPrice.setText("0");
                editUnitPrice.setFocusable(false);
                editUnitPrice.setFocusableInTouchMode(false);
                break;
        }
    }

    public void onClick_Close(View view) {
        finish();
    } // "닫기" 버튼 클릭 시

    /*---------------------------------추가된 부분 시작------------------------------------------*/
    public void openDatePicker(View view) {
        if (myDatePicker == null) {
            myDatePicker = new DatePickerFragment();
            myDatePicker.setEditTextForDatePicker(editDate);
            myDatePicker.show(getFragmentManager(), "DatePicker");
        }
    }

    public static class DatePickerFragment extends DialogFragment implements DatePickerDialog.OnDateSetListener {
        EditText tmpDate;
        Calendar cal = Calendar.getInstance();
        int year = cal.get(Calendar.YEAR);
        int mon = cal.get(Calendar.MONTH);
        int day = cal.get(Calendar.DATE);

        public void setEditTextForDatePicker(EditText date) { //날짜를 표시할 뷰 설정
            tmpDate = date;
            if (!tmpDate.getText().toString().isEmpty()) {
                String tmp = tmpDate.getText().toString();
                year = Integer.valueOf(tmp.split("-")[0]);
                mon = Integer.valueOf(tmp.split("-")[1]) - 1;
                day = Integer.valueOf(tmp.split("-")[2]);
            }
        }

        @Override
        public Dialog onCreateDialog(Bundle savedInstanceState) { //다이얼로그 생성
            return new DatePickerDialog(getActivity(), AlertDialog.THEME_HOLO_DARK,
                    this, year, mon, day);
        }

        @Override
        public void onOptionsMenuClosed(Menu menu) {
            myDatePicker = null;
            super.onOptionsMenuClosed(menu);
        }

        @Override
        public void onDismiss(DialogInterface dialog) {
            myDatePicker = null;
            super.onDismiss(dialog);
        }

        @Override
        public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) { //선택된 날짜로 뷰에 표시
            tmpDate.setText(year + "-" + (monthOfYear + 1) + "-" + dayOfMonth);
            myDatePicker = null;
        }
    }
    /*---------------------------------추가된 부분 끝------------------------------------------*/
}
