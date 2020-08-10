using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEAM_07
{
    public partial class Form1 : Form
    {
        // variable for initialization
        static int m_schedulingOption = 0;
        static int m_numberOfProcess = 0;
        static bool[] m_isFilled = null;
        static Process[] m_processPtr = null;
        // variable for scheduling
        static int m_TimeQuantum = 0; // TimeQuantum 저장용
        static List<Process> m_QueueList = new List<Process>(); // index 0은 running, 나머지는 ready queue
        // variable for gantt chart
        static int m_currentTime = 0;
        static bool m_isStarted = false;
        static List<int> m_runningProcess = new List<int>();
        static int m_totalTime = 0;
        // variable for quiz
        enum QuizMode
        {
            NONE,
            EASY,
            HARD
        }
        static QuizMode m_quizMode = QuizMode.NONE;
        // variable for checking process access
        static List<List<int>> m_enteringList = new List<List<int>>();
        static List<int> m_exitingList = new List<int>();

        // ========== ========== ========== ========== ========== ========== ==========
        // ↓ GUI

        public Form1()
        {
            InitializeComponent();
        }

        private void BT_APPLY_Click(object sender, EventArgs e)
        {
            { // check & assign input value        
                string INPUT_NOP = TB_NOP.Text;
                if (CB_SO.SelectedIndex == -1)
                {
                    MessageBox.Show("Select [Scheduling Option]", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (INPUT_NOP.Length == 0)
                {
                    MessageBox.Show("Input [Number of Process(es)]", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (int.Parse(INPUT_NOP) <= 0)
                {
                    MessageBox.Show("[Number of Process(es)] must be larger than 0", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    m_schedulingOption = CB_SO.SelectedIndex;
                    m_numberOfProcess = int.Parse(INPUT_NOP);
                }
            }

            { // process(es) dynamic allocation
                m_processPtr = null;
                m_processPtr = new Process[m_numberOfProcess];
                for (int i = 0; i < m_numberOfProcess; ++i)
                {
                    m_processPtr[i] = new Process();
                    m_processPtr[i].i = i + 1;
                }

                m_isFilled = null;
                m_isFilled = new bool[m_numberOfProcess];
            }

            { // handle state of controls
                CB_SO.Enabled = false;
                TB_NOP.Enabled = false;
                BT_APPLY.Enabled = false;
                if (m_schedulingOption == 1)
                {
                    TB_TQ.Enabled = true;
                }

                CB_SN.Items.Clear();
                for (int i = 1; i <= m_numberOfProcess; ++i)
                {
                    CB_SN.Items.Add(i);
                }
                CB_SN.SelectedIndex = 0;
                CB_SN.Enabled = true;
                TB_AT.Enabled = true;
                TB_BT.Enabled = true;
                BT_ASSIGN.Enabled = true;

                CB_QUIZ.Items.Clear();
                for (int i = 1; i <= m_numberOfProcess; ++i)
                {
                    CB_QUIZ.Items.Add(i);
                }
            }
        }

        private void BT_ASSIGN_Click(object sender, EventArgs e)
        {
            int index = CB_SN.SelectedIndex;

            { // check & assign input value             
                if (index != -1)
                {
                    string INPUT_AT = TB_AT.Text;
                    string INPUT_BT = TB_BT.Text;
                    if (INPUT_AT.Length == 0 || INPUT_BT.Length == 0)
                    {
                        MessageBox.Show("Input [Arrival Time] and [Burst Time]", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (int.Parse(INPUT_AT) <= -1 || int.Parse(INPUT_BT) <= -1)
                    {
                        MessageBox.Show("[Arrival Time] and [Burst Time] must be larger than -1", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    m_processPtr[index].setArrivalTime(int.Parse(INPUT_AT));
                    m_processPtr[index].setBurstTime(int.Parse(INPUT_BT));
                    m_isFilled[index] = true;
                }
                if (index + 1 < m_numberOfProcess)
                {
                    CB_SN.SelectedIndex = index + 1;
                }
            }

            { // check condition to start
                foreach (bool b in m_isFilled)
                {
                    if (b == false)
                    {
                        return;
                    }
                }

                BT_START.Enabled = true;
            }
        }

        private void CB_SN_SelectedIndexChanged(object sender, EventArgs e)
        {
            TB_AT.Clear();
            TB_BT.Clear();
        }

        private void BT_START_Click(object sender, EventArgs e)
        {
            { // check input value (for RR)
                if (m_schedulingOption == 1)
                {
                    string INPUT_TQ = TB_TQ.Text;
                    if (INPUT_TQ.Length == 0)
                    {
                        MessageBox.Show("Input [Time Quantum]", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (int.Parse(INPUT_TQ) <= 0)
                    {
                        MessageBox.Show("[Time Quantum] must be larger than 0", "NOTICE",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    m_TimeQuantum = int.Parse(INPUT_TQ);
                                      
                    // 각 프로세스 별로 TimeQuantum 정보 저장
                    for (int i = 0; i < m_numberOfProcess; i++)
                    {
                        m_processPtr[i].setTimeQuantum(m_TimeQuantum);
                    }
                }
            }

            { // handle state of controls
                TB_TQ.Enabled = false;
                CB_SN.Enabled = false;
                TB_AT.Enabled = false;
                TB_BT.Enabled = false;
                BT_ASSIGN.Enabled = false;
                BT_START.Enabled = false;

                m_isStarted = true;
                m_totalTime = 0;

                BT_STEP.Enabled = true;
                BT_JUMP.Enabled = true;

                RB_NONE.Enabled = true;
                RB_EASY.Enabled = true;
                RB_HARD.Enabled = true;
            }

            { // 스케줄링 옵션에 따라 해당하는 스케줄링 알고리즘 실행
                switch (m_schedulingOption)
                {
                    case 0: // FCFS
                        SchedulingFCFS();
                        break;
                    case 1: // RR                    
                        SchedulingRR();
                        break;
                    case 2: // SPN
                        SchedulingSPN();
                        break;
                    case 3: // SRTN
                        SchedulingSRTN();
                        break;
                    case 4: // HRRN
                        SchedulingHRRN();
                        break;
                }
            }

            { // make result table
                LV_RESULT.BeginUpdate();
                for (int i = 0; i < m_numberOfProcess; ++i)
                {
                    ListViewItem LVI = new ListViewItem(m_processPtr[i].i.ToString());
                    LVI.SubItems.Add(m_processPtr[i].getArrivalTime().ToString());
                    LVI.SubItems.Add(m_processPtr[i].getBurstTime().ToString());
                    LVI.SubItems.Add(m_processPtr[i].getWaitingTime().ToString());
                    LVI.SubItems.Add(m_processPtr[i].getTurnaroundTime().ToString());
                    LVI.SubItems.Add(m_processPtr[i].getNormilizedTT().ToString());

                    LV_RESULT.Items.Add(LVI);
                }
                LV_RESULT.EndUpdate();
            }

            { // gantt chart when time = 0
                for(int i = 1; i <= m_numberOfProcess; ++i)
                {
                    CT_GANTT.Series["NOP"].Points.AddXY(i, 1, 1);
                }

                updateAccess();
            }
        }

        private void BT_RESET_Click(object sender, EventArgs e)
        {
            CB_SO.SelectedIndex = -1;
            CB_SO.Enabled = true;
            TB_NOP.Clear();
            TB_NOP.Enabled = true;
            BT_APPLY.Enabled = true;

            TB_TQ.Clear();
            TB_TQ.Enabled = false;

            CB_SN.ResetText();
            CB_SN.Items.Clear();
            CB_SN.Enabled = false;
            TB_AT.Clear();
            TB_AT.Enabled = false;
            TB_BT.Clear();
            TB_BT.Enabled = false;
            BT_ASSIGN.Enabled = false;

            BT_START.Enabled = false;
            BT_STEP.Enabled = false;
            BT_JUMP.Enabled = false;

            LV_RESULT.Items.Clear();

            m_currentTime = 0;
            m_isStarted = false;
            m_runningProcess.Clear();
            foreach (var s in CT_GANTT.Series)
            {
                s.Points.Clear();
            }
            LB_CT.Text = m_currentTime.ToString();
            LV_ENP.Items.Clear();
            m_enteringList.Clear();
            LV_EXP.Items.Clear();
            m_exitingList.Clear();         

            RB_NONE.Enabled = false;
            RB_NONE.Select();
            RB_EASY.Enabled = false;
            RB_HARD.Enabled = false;
            CB_QUIZ.Enabled = false;
            BT_QUIZ.Enabled = false;
        }

        private void BT_STEP_Click(object sender, EventArgs e)
        {
            // gantt chart in process
            if (m_isStarted && m_currentTime < m_totalTime)
            {
                { // gantt chart update
                    if (m_runningProcess[m_currentTime] != 0)
                    {
                        CT_GANTT.Series["NOP"].Points.AddXY(m_runningProcess[m_currentTime],
                        m_currentTime, m_currentTime + 1);
                    }
                    else
                    {
                        CT_GANTT.Series["NOP"].Points.AddXY(1,
                        m_currentTime + 1, m_currentTime + 1);
                    }
                    m_currentTime += 1;
                    LB_CT.Text = m_currentTime.ToString();

                    updateAccess();
                }

                { // quiz
                    switch (m_quizMode)
                    {
                        case QuizMode.NONE:
                            break;
                        case QuizMode.EASY:
                            if (m_currentTime % 6 == 5 && m_currentTime < m_totalTime)
                            {
                                processQuiz();
                            }
                            break;
                        case QuizMode.HARD:
                            if (m_currentTime % 3 == 2 && m_currentTime < m_totalTime)
                            {
                                processQuiz();
                            }
                            break;
                    }
                }
            }
            // gantt chart complete
            if(m_currentTime == m_totalTime)
            {
                BT_STEP.Enabled = false;
                BT_JUMP.Enabled = false;
            }
        }

        private void BT_JUMP_Click(object sender, EventArgs e)
        {
            // gantt chart in process
            if (m_isStarted && m_currentTime < m_totalTime)
            {
                { // gantt chart update
                    foreach (var s in CT_GANTT.Series)
                    {
                        s.Points.Clear();
                    }
                    m_currentTime = 0;
                    for (int i = 0; i < m_totalTime; ++i)
                    {
                        if (m_runningProcess[i] != 0)
                        {
                            CT_GANTT.Series["NOP"].Points.AddXY(m_runningProcess[i],
                                m_currentTime, m_currentTime + 1);
                        }
                        m_currentTime += 1;
                    }
                    LB_CT.Text = m_currentTime.ToString();

                    updateAccess();
                }

                { // gantt chart complete
                    BT_STEP.Enabled = false;
                    BT_JUMP.Enabled = false;
                }
            }
        }

        private void updateAccess()
        {
            LV_ENP.Items.Clear();
            LV_ENP.BeginUpdate();
            if (m_enteringList.Count > m_currentTime)
            {
                foreach (var i in m_enteringList[m_currentTime])
                {
                    if (i != 0)
                    {
                        LV_ENP.Items.Add(i.ToString());
                    }
                }
            }
            LV_ENP.EndUpdate();

            LV_EXP.Items.Clear();
            LV_EXP.BeginUpdate();
            if (m_exitingList.Count > m_currentTime)
            {
                if (m_exitingList[m_currentTime] != 0)
                {
                    LV_EXP.Items.Add(m_exitingList[m_currentTime].ToString());
                }
            }
            LV_EXP.EndUpdate();
        }

        private void RB_NONE_CheckedChanged(object sender, EventArgs e)
        {
            m_quizMode = QuizMode.NONE;
        }

        private void RB_EASY_CheckedChanged(object sender, EventArgs e)
        {
            m_quizMode = QuizMode.EASY;
        }

        private void RB_HARD_CheckedChanged(object sender, EventArgs e)
        {
            m_quizMode = QuizMode.HARD;
        }

        private void processQuiz()
        {
            BT_STEP.Enabled = false;
            BT_JUMP.Enabled = false;

            CB_QUIZ.Enabled = true;
            BT_QUIZ.Enabled = true;

            MessageBox.Show("QUIZ TIME !", "NOTICE",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BT_QUIZ_Click(object sender, EventArgs e)
        {
            if (CB_QUIZ.SelectedIndex == -1)
            {
                MessageBox.Show("SELECT ANSWER", "NOTICE",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            BT_STEP.Enabled = true;
            BT_JUMP.Enabled = true;

            CB_QUIZ.Enabled = false;
            BT_QUIZ.Enabled = false;

            if (CB_QUIZ.SelectedIndex + 1 == m_runningProcess[m_currentTime])
            {
                MessageBox.Show("IT'S RIGHT ^-^ !", "NOTICE",
                    MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            else
            {
                MessageBox.Show("IT'S WRONG T-T ...", "NOTICE",
                    MessageBoxButtons.OK, MessageBoxIcon.None);
            }
            CB_QUIZ.SelectedIndex = -1;

            BT_STEP.PerformClick();
        }

        // ↑ GUI
        // ========== ========== ========== ========== ========== ========== ==========
        // ↓ LOGIC

        public static int CheckWholeRTT() // 모든 프로세스의 남은 실행 시간의 합 계산
        {
            int wholeRTT = 0;
            for (int i = 0; i < m_processPtr.Length; i++)
            {
                wholeRTT += m_processPtr[i].getRemainingBurstTime();
            }
            return wholeRTT;
        }

        public static void SchedulingFCFS() // FCFS
        {
            int count = 0, j = 0;
            m_exitingList.Add(0);
            while (CheckWholeRTT() != 0) // 모든 프로세스의 남은 실행시간이 0이 될 때까지 반복
            {
                // count for checking process access
                m_enteringList.Add(new List<int>()); // 시간이 count일 때, 들어오는 프로세스 리스트 준비
                m_exitingList.Add(0); // 시간이 count일 때, 나가는 프로세스 리스트 준비

                // 각 프로세스 도착 시간에 맞춰 큐에 삽입
                for (int i = 0; i < m_processPtr.Length; i++)
                {
                    if (count == m_processPtr[i].getArrivalTime())
                    {
                        m_QueueList.Add(m_processPtr[i]);
                        // count for checking process access
                        m_enteringList[count].Add(m_processPtr[i].i);
                    }
                }

                if (m_QueueList.Count != 0) // 큐에 프로세스가 존재할 경우
                {
                    m_QueueList[0].setRemainingBurstTime(); // running 프로세스의 잔여 실행시간 1 감소
                    // count for gantt chart                    
                    m_runningProcess.Add(m_QueueList[0].i);
                    if (m_QueueList[0].getRemainingBurstTime() == 0) //running 프로세스의 잔여 실행시간이 0이면
                    {
                        m_QueueList[0].setTurnaroundTime(); // 프로세스 TT 계산
                        m_QueueList[0].setNormalizedTT(); // 프로세스 NTT 계산
                        // count for checking process access
                        m_exitingList[count + 1] = m_QueueList[0].i;
                        m_QueueList.RemoveAt(0); // running 프로세스 terminate
                        j = 0;
                    }
                    else { j = 1; }
                    // ready 프로세스들의 대기시간 1 증가
                    for (; j < m_QueueList.Count; j++)
                    {
                        m_QueueList[j].setWaitingTime();
                    }
                }
                // count for gantt chart  
                else // 큐에 프로세스가 존재하지 않으면
                {
                    m_runningProcess.Add(0);
                }
                count++; //실행시간 1 증가
                ++m_totalTime; //전체 실행시간 1 증가
            }

            // reset for gantt chart
            m_currentTime = 0; // 다음 시연을 위해 m_currentTime 0 초기화
        }

        public static void SchedulingRR() // RR
        {
            int count = 0, j = 0, processNum = 0;
            // count for checking process access
            m_exitingList.Add(0); // 보정

            while (CheckWholeRTT() != 0) // 모든 프로세스의 남은 실행시간이 0이 될 때까지 반복
            {
                // count for checking process access
                m_enteringList.Add(new List<int>()); // 시간이 count일 때, 들어오는 프로세스 리스트 준비
                m_exitingList.Add(0); // 시간이 count일 때, 나가는 프로세스 리스트 준비

                // 각 프로세스 도착 시간에 맞춰 큐에 삽입
                for (int i = 0; i < m_processPtr.Length; i++)
                {
                    if (count == m_processPtr[i].getArrivalTime())
                    {
                        m_QueueList.Add(m_processPtr[i]);
                        // count for checking process access
                        m_enteringList[count].Add(m_processPtr[i].i); // 들어오는 프로세스 리스트에 추가
                    }
                }
                if (m_QueueList.Count != 0) // 큐에 프로세스가 존재할 경우
                {
                    if (m_QueueList[0].getTimeQuantum() == 0) // running 프로세스의 남은 제한시간이 없을 경우, 큐의 맨 나중 순서로 이동
                    {
                        processNum = m_QueueList[0].i - 1;
                        m_processPtr[processNum].setTimeQuantum(m_TimeQuantum);
                        m_QueueList.RemoveAt(0);
                        m_QueueList.Add(m_processPtr[processNum]);
                    }

                    // count for gantt chart                    
                    m_runningProcess.Add(m_QueueList[0].i);

                    m_QueueList[0].setRemainingBurstTime(); // running 프로세스의 잔여 실행시간 1 감소
                    m_QueueList[0].passTimeQuantum(); // running 프로세스의 잔여 제한시간 1 감소

                    if (m_QueueList[0].getRemainingBurstTime() == 0) //running 프로세스의 잔여 실행시간이 0이면
                    {
                        m_QueueList[0].setTurnaroundTime(); // 프로세스 TT 계산
                        m_QueueList[0].setNormalizedTT(); // 프로세스 NTT 계산
                        // count for checking process access
                        m_exitingList[count + 1] = m_QueueList[0].i; // 나가는 프로세스 리스트에 추가
                        m_QueueList.RemoveAt(0); // running 프로세스 terminate
                        j = 0;
                    }
                    else { j = 1; }
                    // ready 프로세스들의 대기시간 1 증가
                    for (; j < m_QueueList.Count; j++)
                    {
                        m_QueueList[j].setWaitingTime();
                    }
                }
                // count for gantt chart 
                else // 큐에 프로세스가 존재하지 않으면
                {
                    m_runningProcess.Add(0);
                }
                count++; // 실행시간 1 증가
                ++m_totalTime; // 전체 실행시간 1 증가
            }
            // reset for gantt chart
            m_currentTime = 0; // 다음 시연을 위해 m_currentTime 0 초기화
        }

        public static void SchedulingSPN() // SPN
        {
            int count = 0, j = 0;
            bool NotRunning = true; // 이전 running 프로세스가 terminate 되었는지 안되었는지 저장하는 변수
            m_exitingList.Add(0); // 보정
            while (CheckWholeRTT() != 0) // 모든 프로세스의 남은 실행시간이 0이 될 때까지 반복
            {
                // count for checking process access
                m_enteringList.Add(new List<int>()); // 시간이 count일 때, 들어오는 프로세스 리스트 준비
                m_exitingList.Add(0); // 시간이 count일 때, 나가는 프로세스 리스트 준비
                // 각 프로세스 도착 시간에 맞춰 큐에 삽입
                for (int i = 0; i < m_processPtr.Length; i++)
                {
                    if (count == m_processPtr[i].getArrivalTime())
                    {
                        m_QueueList.Add(m_processPtr[i]);
                        // count for checking process access
                        m_enteringList[count].Add(m_processPtr[i].i); // 들어오는 프로세스 리스트에 추가
                    }
                }
                if (m_QueueList.Count != 0) // 큐에 프로세스가 존재할 경우
                {
                    if (NotRunning) // 이전 running 프로세스가 terminate 되었으면
                    {
                        m_QueueList.Sort(new SPNProcessCompare()); // 큐의 인덱스 0부터 SPN 기준으로 정렬
                        NotRunning = false; // 이제 running 프로세스가 새로 생겼으므로 false 저장
                    }
                    else // 이전 running 프로세스가 아직 terminate 되지 않았으면
                    {
                        m_QueueList.Sort(1, m_QueueList.Count - 1, new SPNProcessCompare()); // running 프로세스 외 모든 ready 프로세스 SPN 기준으로 정렬
                    }
                    m_QueueList[0].setRemainingBurstTime(); // running 프로세스의 잔여 실행시간 1 감소
                    // count for gantt chart                    
                    m_runningProcess.Add(m_QueueList[0].i);
                    if (m_QueueList[0].getRemainingBurstTime() == 0) //running 프로세스의 잔여 실행시간이 0이면
                    {
                        m_QueueList[0].setTurnaroundTime(); // 프로세스 TT 계산
                        m_QueueList[0].setNormalizedTT(); // 프로세스 NTT 계산
                        // count for checking process access
                        m_exitingList[count + 1] = m_QueueList[0].i; // 나가는 프로세스 리스트에 추가
                        m_QueueList.RemoveAt(0); // running 프로세스 terminate
                        NotRunning = true; // running 프로세스가 terminate 되었으니 true 저장
                        j = 0;
                    }
                    else { j = 1; }
                    // ready 프로세스들의 대기시간 1 증가
                    for (; j < m_QueueList.Count; j++)
                    {
                        m_QueueList[j].setWaitingTime();
                    }
                }
                // count for gantt chart 
                else // 큐에 프로세스가 존재하지 않으면
                {
                    m_runningProcess.Add(0);
                }
                count++; // 실행시간 1 증가
                ++m_totalTime; // 전체 실행 시간 1 증가
            }

            // reset for gantt chart
            m_currentTime = 0; // 다음 시연을 위해 m_currentTime 0 초기화
        }

        public static void SchedulingSRTN() // SRTN
        {
            int count = 0, j = 0;
            m_exitingList.Add(0); // 보정
            while (CheckWholeRTT() != 0) // 모든 프로세스의 남은 실행시간이 0이 될 때까지 반복
            {
                // count for checking process access
                m_enteringList.Add(new List<int>()); // 시간이 count일 때, 들어오는 프로세스 리스트 준비
                m_exitingList.Add(0); // 시간이 count일 때, 나가는 프로세스 리스트 준비
                // 각 프로세스 도착 시간에 맞춰 큐에 삽입
                for (int i = 0; i < m_processPtr.Length; i++)
                {
                    if (count == m_processPtr[i].getArrivalTime())
                    {
                        m_QueueList.Add(m_processPtr[i]);
                        // count for checking process access
                        m_enteringList[count].Add(m_processPtr[i].i); // 들어오는 프로세스 리스트에 추가
                    }
                }
                if (m_QueueList.Count != 0)
                {
                    m_QueueList.Sort(new SRTNProcessCompare()); // 모든 프로세스 SRTN 기준으로 정렬
                    m_QueueList[0].setRemainingBurstTime(); // running 프로세스의 잔여 실행시간 1 감소
                    // count for gantt chart                    
                    m_runningProcess.Add(m_QueueList[0].i);
                    if (m_QueueList[0].getRemainingBurstTime() == 0) //running 프로세스의 잔여 실행시간이 0이면
                    {
                        m_QueueList[0].setTurnaroundTime(); // 프로세스 TT 계산
                        m_QueueList[0].setNormalizedTT(); // 프로세스 NTT 계산
                        // count for checking process access
                        m_exitingList[count + 1] = m_QueueList[0].i; // 나가는 프로세스 리스트에 추가
                        m_QueueList.RemoveAt(0); // running 프로세스 terminate
                        j = 0;
                    }
                    else { j = 1; }
                    // ready 프로세스들의 대기시간 1 증가
                    for (; j < m_QueueList.Count; j++)
                    {
                        m_QueueList[j].setWaitingTime();
                    }
                }
                // count for gantt chart 
                else // 큐에 프로세스가 존재하지 않으면
                {
                    m_runningProcess.Add(0);
                }
                count++; // 실행시간 1 증가
                ++m_totalTime; // 전체 실행시간 1 증가
            }

            // reset for gantt chart
            m_currentTime = 0; // 다음 시연을 위해 m_currentTime 0 초기화
        }

        public static void SchedulingHRRN() // HRRN
        {
            int count = 0, j = 0;
            bool NotRunning = true; // 이전 running 프로세스가 terminate 되었는지 안되었는지 저장하는 변수
            m_exitingList.Add(0); // 보정
            while (CheckWholeRTT() != 0) // 모든 프로세스의 남은 실행시간이 0이 될 때까지 반복
            {
                // count for checking process access
                m_enteringList.Add(new List<int>()); // 시간이 count일 때, 들어오는 프로세스 리스트 준비
                m_exitingList.Add(0); // 시간이 count일 때, 나가는 프로세스 리스트 준비
                // 각 프로세스 도착 시간에 맞춰 큐에 삽입
                for (int i = 0; i < m_processPtr.Length; i++)
                {
                    if (count == m_processPtr[i].getArrivalTime())
                    {
                        m_QueueList.Add(m_processPtr[i]);
                        // count for checking process access
                        m_enteringList[count].Add(m_processPtr[i].i); // 들어오는 프로세스 리스트에 추가
                        m_QueueList[m_QueueList.Count - 1].setResponseRatio(); // ResponseRatio 설정
                    }
                }
                if (m_QueueList.Count != 0) // 큐에 프로세스가 존재할 경우
                {
                    if (NotRunning) // 이전 running 프로세스가 terminate 되었으면
                    {
                        m_QueueList.Sort(new HRRNProcessCompare()); // 큐의 인덱스 0부터 HRRN 기준으로 정렬
                        NotRunning = false; // 이제 running 프로세스가 새로 생겼으므로 false 저장
                    }
                    else // 이전 running 프로세스가 아직 terminate 되지 않았으면
                    {
                        m_QueueList.Sort(1, m_QueueList.Count - 1, new HRRNProcessCompare()); // running 프로세스 외 모든 ready 프로세스 HRRN 기준으로 정렬
                    }

                    m_QueueList[0].setRemainingBurstTime(); // running 프로세스의 잔여 실행시간 1 감소
                    // count for gantt chart                    
                    m_runningProcess.Add(m_QueueList[0].i);
                    if (m_QueueList[0].getRemainingBurstTime() == 0) //running 프로세스의 잔여 실행시간이 0이면
                    {
                        m_QueueList[0].setTurnaroundTime(); // 프로세스 TT 계산
                        m_QueueList[0].setNormalizedTT(); // 프로세스 NTT 계산
                        // count for checking process access
                        m_exitingList[count + 1] = m_QueueList[0].i; // 나가는 프로세스 리스트에 추가
                        m_QueueList.RemoveAt(0); // running 프로세스 terminate
                        NotRunning = true; // running 프로세스가 terminate 되었으니 true 저장
                        j = 0;
                    }
                    else { j = 1; }
                    // ready 프로세스들의 대기시간 1 증가 및 대기시간 증가에 따른 ResponseRatio 변경
                    for (; j < m_QueueList.Count; j++)
                    {
                        m_QueueList[j].setWaitingTime();
                        m_QueueList[j].setResponseRatio();
                    }
                }
                // count for gantt chart 
                else // 큐에 프로세스가 존재하지 않으면
                {
                    m_runningProcess.Add(0);
                }
                count++; // 실행시간 증가
                ++m_totalTime; // 전체 실행시간 증가
            }

            // reset for gantt chart
            m_currentTime = 0; // 다음 시연을 위해 m_currentTime 0 초기화
        }

    }
}
