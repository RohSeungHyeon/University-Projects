import sys
import openpyxl
import algorithms
import random
from PyQt5.QtWidgets import *
from PyQt5 import uic, QtGui

form_class = uic.loadUiType("TempUI.ui")[0]

class ProjectWindow(QMainWindow, form_class):
    def __init__(self):
        super().__init__()
        self.setupUi(self)
        self.setWindowTitle("인공지능 3팀")

        # 진행하는데 쓰일 총 강의 리스트, 강의실 리스트, 강의실 분류
        self.LectureList = []
        self.classRoomList = []

        # 담은 과목을 저장할 강의 리스트 및 어느 시간이 차있는지 알려줄 인덱스
        self.includedLectureList = []
        self.includedLectureTime = [] # (요일, 시간)

        # 테이블 관리하는데 쓰일 변수
        self.tW_lectureInfo_row = 0
        self.tW_lectureInfo_2_row = 0

        # 버튼 연결
        self.pBtn_ExportData.clicked.connect(self.pBtn_ExportData_Clicked)
        self.pBtn_ImportData.clicked.connect(self.pBtn_ImportData_Clicked)
        self.pBtn_Init.clicked.connect(self.pBtn_Init_Clicked)
        self.pBtn_Search.clicked.connect(self.pBtn_Search_Clicked)
        self.pBtn_StartAgent.clicked.connect(self.pBtn_StartAgent_Clicked)
        self.pBtn_excludeLecture.clicked.connect(self.pBtn_excludeLecture_Clicked)
        self.pBtn_includeLecture.clicked.connect(self.pBtn_includeLecture_Clicked)

        # 테이블 위젯 설정
        self.tW_lectureInfo.setSelectionBehavior(QTableWidget.SelectRows)
        self.tW_lectureInfo.setEditTriggers(QTableWidget.NoEditTriggers)
        self.tW_lectureInfo_2.setSelectionBehavior(QTableWidget.SelectRows)
        self.tW_lectureInfo_2.setEditTriggers(QTableWidget.NoEditTriggers)
        self.tW_TimeTable.setEditTriggers(QTableWidget.NoEditTriggers)

        # 콤보 박스 설정
        self.cB_Category.currentIndexChanged.connect(self.cB_Category_Changed)
        self.cB_CategoryItem.currentIndexChanged.connect(self.cB_CategoryItem_Changed)

        # 진행 전 버튼 막아두기
        self.pBtn_excludeLecture.setEnabled(False)
        self.pBtn_includeLecture.setEnabled(False)
        self.pBtn_ExportData.setEnabled(False)
        self.pBtn_Init.setEnabled(False)
        self.pBtn_Search.setEnabled(False)
        self.pBtn_StartAgent.setEnabled(False)

        # 시간표 테이블에 과목 일정을 넣을 때 사용할 색상 리스트
        self.colorList = algorithms.ColorList()
        

    def pBtn_ExportData_Clicked(self):
        # 엑셀파일 열기
        file = openpyxl.load_workbook("class_data.xlsx")
        sheet = file['시간표']

        # 비우기
        for row in sheet.rows:
            for cell in row:
                cell.value = None

        sheet['A1'] = '과목'
        sheet['B1'] = '분반'
        sheet['C1'] = '교수'
        sheet['D1'] = '강의실'
        sheet['E1'] = '시간'

        isFirst = True
        index = 1
        for lecture in self.LectureList:
            index += 1
            tempList =[]
            sheet['A' + str(index)].value = lecture.className
            sheet['B' + str(index)].value = lecture.classNum
            sheet['C' + str(index)].value = lecture.professor
            if len(lecture.classRoom) is 2:
                tempString = lecture.classRoom[0].Building + '/' + str(lecture.classRoom[0].roomNum) + '호' + ', ' + lecture.classRoom[1].Building + '/' + str(lecture.classRoom[1].roomNum) + '호'
            elif len(lecture.classRoom) is 1:
                tempString = lecture.classRoom[0].Building + '/' + str(lecture.classRoom[0].roomNum) + '호'
            else:
                tempString = None
            sheet['D' + str(index)].value = tempString
            tempString_2 = []
            for day in range(5):
                if day is 0:
                    dayString = '월/'
                elif day is 1:
                    dayString = '화/'
                elif day is 2:
                    dayString = '수/'
                elif day is 3:
                    dayString = '목/'
                elif day is 4:
                    dayString = '금/'
                for time in range(18):
                    timeString = ''
                    if (time + 1) // 2 < 10:
                        timeString = '0' + str((time + 2) // 2)
                    elif (time + 1) // 2 >= 10:
                        timeString = str((time + 2) // 2)
                    if (time + 1) % 2 is 1:
                        timeString += 'A'
                    elif (time + 1) % 2 is 0:
                        timeString += 'B'
                    if lecture.timeTable[day][time][0] is 'Mine':
                        tempString_2.append(dayString)
                        tempString_2.append(timeString)
                        tempString_2.append(',')
            sheet['E' + str(index)].value = ''.join(tempString_2)

        file.save('class_data.xlsx')
        file.close()

    def pBtn_ImportData_Clicked(self):
        # 엑셀파일 열기
        file = openpyxl.load_workbook("class_data.xlsx")
        sheet_1 = file["강의"]
        sheet_2 = file["강의실"]
        isFirst = True # 첫 행을 필터링하기 위함
        # 첫번째 반복문으로 강의 리스트 생성
        for row in sheet_1.rows:
            if isFirst is True:
                isFirst = False
                continue
            tempLecture = algorithms.Lecture()
            tempLecture.classCode = row[2].value
            tempLecture.className = row[3].value
            tempLecture.score = row[5].value
            tempLecture.classTime = int(row[6].value)
            tempLecture.researchTime = int(row[7].value)
            tempLecture.track = row[8].value
            tempLecture.classNum = row[10].value
            tempLecture.classLimit = row[11].value
            tempLecture.professor = row[29].value

            if str(row[32].value) in '1':
                tempLecture.classProperty.append('SW')
            elif str(row[32].value) in '2':
                tempLecture.classProperty.append('HW')
            elif str(row[32].value) in '3':
                tempLecture.classProperty.append('이론')
            elif '담헌' in str(row[32].value):
                tempLecture.classProperty.append('담헌')
            elif str(row[32].value) in '1, 3':
                tempLecture.classProperty += ['이론', 'SW']

            if '캡스톤디자인' in tempLecture.className:
                tempLecture.timeTable[2][10] = ('Mine', '')
                tempLecture.timeTable[2][11] = ('Mine', '')

            self.LectureList.append(tempLecture) # 강의 리스트에 추가

        isFirst = True # 첫 행을 필터링하기 위함
        # 두번째 반복문으로 강의실 리스트 생성
        for row in sheet_2.rows:
            if isFirst is True:
                isFirst = False
                continue
            tempClassroom = algorithms.ClassRoom()
            tempClassroom.Building = row[0].value
            tempClassroom.roomNum = row[1].value
            tempClassroom.accommodableLimit = row[2].value
            if row[3].value is 'o':
                tempClassroom.isResearch = True
            else:
                tempClassroom.isResearch = False
            tempClassroom.name = row[4].value
            tempClassroom.property = row[5].value

            self.classRoomList.append(tempClassroom)

        # 수업 속성 별 강의실 수 확인
        TotalTheoryClassroom = 0
        TotalSWClassroom = 0
        TotalHWClassroom = 0
        TotalDamHeonClassroom = 0
        for classroom in self.classRoomList:
            if classroom.property in '이론':
                TotalTheoryClassroom += 1
            elif classroom.property in 'SW':
                TotalSWClassroom += 1
            elif classroom.property in 'HW':
                TotalHWClassroom += 1
            elif classroom.property in '담헌':
                TotalDamHeonClassroom += 1

        file.close()

        self.timeTableForWholeClassroom = [[[TotalTheoryClassroom, TotalSWClassroom - 1, TotalHWClassroom, TotalDamHeonClassroom] for col in range(18)] for row in range(5)] # 수업 속성별 강의실 수 시간표 (이론, SW, HW, 담헌) SW에서 하나를 뺀 이유는 정원이 못쓸 정도의 강의실이 있기 때문 : 지능형영상처리실습실

        self.cB_Category.setCurrentIndex(0) # 카테고리 Default : 전체

        self.init_tW_lectureInfo() # 강의 리스트 내용을 강의 정보 테이블_1에 적용

        # 데이터를 가져왔으니 버튼 막기
        self.pBtn_ImportData.setEnabled(False)

        # 막혀 있던 버튼 중 필요한 버튼 활성화
        self.pBtn_excludeLecture.setEnabled(True)
        self.pBtn_includeLecture.setEnabled(True)
        self.pBtn_Init.setEnabled(True)
        self.pBtn_Search.setEnabled(True)
        self.pBtn_StartAgent.setEnabled(True)

    # 초기화
    def pBtn_Init_Clicked(self):
        self.tW_lectureInfo_row = 0
        self.tW_lectureInfo_2_row = 0
        self.tW_lectureInfo.setRowCount(self.tW_lectureInfo_row)
        self.tW_lectureInfo_2.setRowCount(self.tW_lectureInfo_2_row)
        self.LectureList = []
        self.classRoomList = []
        self.includedLectureList = []
        self.includedLectureTime = []
        self.colorList = algorithms.ColorList()
        self.tW_lectureInfo.clearContents()
        self.tW_lectureInfo_2.clearContents()
        self.tW_TimeTable.clearContents()
        self.tW_TimeTable.clearSpans()
        self.cB_CategoryItem.clear()
        self.cB_Category.setCurrentIndex(0)

        # 버튼 활성화
        self.pBtn_ImportData.setEnabled(True)

        # 버튼 비활성화
        self.pBtn_excludeLecture.setEnabled(False)
        self.pBtn_includeLecture.setEnabled(False)
        self.pBtn_ExportData.setEnabled(False)
        self.pBtn_Init.setEnabled(False)
        self.pBtn_Search.setEnabled(False)
        self.pBtn_StartAgent.setEnabled(False)

    # 검색 후 필터링
    def pBtn_Search_Clicked(self):
        searchText = str(self.txt_Search.toPlainText())
        if searchText is not '':
            self.tW_lectureInfo_row = 0
            self.tW_lectureInfo.clearContents()
            for lecture in self.LectureList:
                if searchText in lecture.classCode or searchText in lecture.className or searchText in lecture.professor:
                    self.tW_lectureInfo_row += 1          
                    self.tW_lectureInfo.setRowCount(self.tW_lectureInfo_row)

                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 0, QTableWidgetItem(lecture.classCode))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 1, QTableWidgetItem(lecture.className))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 2, QTableWidgetItem(lecture.classNum))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 3, QTableWidgetItem(lecture.professor))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 4, QTableWidgetItem(lecture.score))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 5, QTableWidgetItem(lecture.classLimit))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 6, QTableWidgetItem(lecture.track))


    # 에이전트 시작
    def pBtn_StartAgent_Clicked(self): 
        self.pBtn_StartAgent.setEnabled(False) # Agent를 시작했으니 더 할 수 없게 '에이전트 시작' 버튼 비활성화
        self.TimeAgent()
        self.ClassroomAgent()
        for lecture in self.LectureList:
            if len(lecture.classRoom) is 1:
                print(lecture.className + ', ' + str(lecture.classNum))
                print(lecture.timeTable)
        # print(self.timeTableForWholeClassroom)
        self.pBtn_ExportData.setEnabled(True) # Agent가 돌았으니 '데이터 내보내기' 버튼 활성화

    # 시간 배치 에이전트
    def TimeAgent(self):
        self.relateLectures()
        isDone = False # 수업 배치 Flag
        # 첫번째 수업 시간 배치
        for lecture in self.LectureList:
            if '캡스톤디자인' in lecture.className: # 공학설계, 졸업설계는 제외
                continue
            for day in range(self.checkLectureTimeOfDay(lecture), 5): # 요일마다
                if isDone is True:
                    isDone = False
                    break
                for time in range(0, 18, 2): # 1시간 간격마다
                    isAvailable = self.checkRelatedLecture_First(lecture, day, time) # 이론 수업 배치 가능여부 확인
                    if isAvailable is True: # 이론 수업 배치가 가능하면 시간 할당
                        self.giveLectureFirstTime(lecture, day, time)
                        isDone = True
                        break

        isDone = False
        # 두번째 수업 시간 배치
        for lecture in self.LectureList:
            lectureFirstTime = self.checkLectureFirstTime(lecture)
            lectureTimeLeastFull = self.checkLectureTimeOfDay(lecture)
            if lectureTimeLeastFull >= lectureFirstTime:
                startDay = lectureTimeLeastFull
            else:
                startDay = lectureFirstTime

            if '캡스톤디자인' in lecture.className: # 공학설계, 졸업설계는 제외
                continue
            for day in range(startDay, 5): # 요일마다
                if isDone is True:
                    isDone = False
                    break
                for time in range(0, 18, 2): # 1시간 간격마다
                    isAvailable = self.checkRelatedLecture_Second(lecture, day, time) # 실습 수업 배치 가능여부 확인
                    if isAvailable is True: # 실습 수업 배치가 가능하면 시간 할당
                        self.giveLectureSecondTime(lecture, day, time)
                        isDone = True
                        break
 
        self.tW_TimeTable_Update()

    # 강의실 배치 에이전트
    def ClassroomAgent(self):
        # 수업 속성별 리스트 생성
        theoryList = []
        swList = []
        hwList = []
        for classroom in self.classRoomList:
            if '이론' in classroom.property:
                theoryList.append(classroom)
            if 'SW' in classroom.property:
                swList.append(classroom)
            if 'HW' in classroom.property:
                hwList.append(classroom)

        theoryList.sort(key = lambda classroom : classroom.accommodableLimit)
        swList.sort(key = lambda classroom : classroom.accommodableLimit)
        hwList.sort(key = lambda classroom : classroom.accommodableLimit)

        # 첫번째 수업들 배치
        isDone = False # 강의실 배치 완료 여부
        for lecture in self.LectureList:
            if '이론' in lecture.classProperty: # 수업 속성에 이론이 있는 경우
                for day in range(5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in theoryList:
                            if '이론' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        classroom.timeTable[day][time] = 'Used'
                                        classroom.timeTable[day][time + 1] = 'Used'
                                        classroom.timeTable[day][time + 2] = 'Used'
                                        classroom.timeTable[day][time + 3] = 'Used'
                                        isDone = True
                                        break
            elif 'SW' in lecture.classProperty and len(lecture.classProperty) is 1: # 수업 속성에 SW만 있는 경우
                for day in range(5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in swList:
                            if 'SW' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        classroom.timeTable[day][time] = 'Used'
                                        classroom.timeTable[day][time + 1] = 'Used'
                                        classroom.timeTable[day][time + 2] = 'Used'
                                        classroom.timeTable[day][time + 3] = 'Used'
                                        isDone = True
                                        break
            elif 'HW' in lecture.classProperty: # 수업 속성에 HW이 있는 경우
                for day in range(5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in hwList:
                            if 'HW' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        classroom.timeTable[day][time] = 'Used'
                                        classroom.timeTable[day][time + 1] = 'Used'
                                        classroom.timeTable[day][time + 2] = 'Used'
                                        classroom.timeTable[day][time + 3] = 'Used'
                                        isDone = True
                                        break
            elif '담헌' in lecture.classProperty: # 수업 속성에 담헌이 있는 경우
                for day in range(5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(18):
                        if isDone is True:
                            break
                        for classroom in self.classRoomList:
                            if '담헌' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        if lecture.classTime is not 1:
                                            classroom.timeTable[day][time] = 'Used'
                                            classroom.timeTable[day][time + 1] = 'Used'
                                            classroom.timeTable[day][time + 2] = 'Used'
                                            classroom.timeTable[day][time + 3] = 'Used'
                                        else:
                                            classroom.timeTable[day][time] = 'Used'
                                            classroom.timeTable[day][time + 1] = 'Used'
                                        isDone = True
                                        break
        # 두번째 수업들 배치
        isDone = False # 강의실 배치 완료 여부
        for lecture in self.LectureList:
            if '이론' in lecture.classProperty and len(lecture.classProperty) is 1: # 수업 속성에 이론만 있는 경우
                for day in range(self.checkLectureFirstTime(lecture), 5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in theoryList:
                            if '이론' in classroom.property:
                                if lecture.classTime is 3:
                                    if lecture.timeTable[day][time][0] is 'Mine':
                                        if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                            lecture.classRoom.append(classroom)
                                            classroom.timeTable[day][time] = 'Used'
                                            classroom.timeTable[day][time + 1] = 'Used'
                                            isDone = True
                                            break
                                elif lecture.classTime is not 3:
                                    if lecture.timeTable[day][time][0] is 'Mine':
                                        if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                            lecture.classRoom.append(classroom)
                                            classroom.timeTable[day][time] = 'Used'
                                            classroom.timeTable[day][time + 1] = 'Used'
                                            classroom.timeTable[day][time + 2] = 'Used'
                                            classroom.timeTable[day][time + 3] = 'Used'
                                            isDone = True
                                            break
                                    
            elif 'SW' in lecture.classProperty: # 수업 속성에 SW이 있는 경우
                for day in range(self.checkLectureFirstTime(lecture), 5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in swList:
                            if 'SW' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        classroom.timeTable[day][time] = 'Used'
                                        classroom.timeTable[day][time + 1] = 'Used'
                                        classroom.timeTable[day][time + 2] = 'Used'
                                        classroom.timeTable[day][time + 3] = 'Used'
                                        isDone = True
                                        break
            elif 'HW' in lecture.classProperty: # 수업 속성에 HW이 있는 경우
                for day in range(self.checkLectureFirstTime(lecture), 5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in hwList:
                            if 'HW' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        classroom.timeTable[day][time] = 'Used'
                                        classroom.timeTable[day][time + 1] = 'Used'
                                        classroom.timeTable[day][time + 2] = 'Used'
                                        classroom.timeTable[day][time + 3] = 'Used'
                                        isDone = True
                                        break
            elif '담헌' in lecture.classProperty: # 수업 속성에 담헌이 있는 경우
                for day in range(self.checkLectureFirstTime(lecture), 5):
                    if isDone is True:
                        isDone = False
                        break
                    for time in range(0, 18, 2):
                        if isDone is True:
                            break
                        for classroom in self.classRoomList:
                            if '담헌' in classroom.property:
                                if lecture.timeTable[day][time][0] is 'Mine':
                                    if classroom.timeTable[day][time] is None and int(lecture.classLimit) <= classroom.accommodableLimit: # 강의에 시간이 할당되어있고, 강의실이 비어있으며, 정원을 초과하지 않으면
                                        lecture.classRoom.append(classroom)
                                        if lecture.classTime is not 1:
                                            classroom.timeTable[day][time] = 'Used'
                                            classroom.timeTable[day][time + 1] = 'Used'
                                            isDone = True
                                            break


    # 배치하고자 하는 시간에 이미 관련된 수업 중 꿰차고 있는 시간이 있는지 검사 (첫번째 수업 시간)
    def checkRelatedLecture_First(self, lecture, day, time):
        # if time > 11: # 오후 4시 이상이면 False 반환
        #     return False
        if day is 2 and time > 9: # 수요일 오후 2시 이상이면 False 반환
            return False
        if day is 4 and time > 9: # 금요일 오후 2시 이상이면 False 반환
            return False
        
        # 점심시간 예외 : False 반환
        if lecture.classTime is 1:
            if time in (7, 8, 9):
                return False
        elif lecture.classTime is not 1:
            if time in (5, 6, 7, 8, 9):
                return False

        if lecture.timeTable[day][time][0] is 'Occupied': # 배치하고자 하는 시간이 이미 차있으면 False 반환
            return False
        else: # 배치하고자 하는 시간이 비어있는 경우
            if lecture.classTime is 1: # 이론 수업이 1시간인 경우
                if time + 1 > 17: # 오후 6시를 초과하면 False 반환
                    return False
                # 배치하고자 하는 시간에 해당 수업 속성 강의실 중 빈 곳이 없으면 False 반환
                if '담헌' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][3] <= 0:
                        return False
            elif lecture.classTime is not 1: # 이론 수업이 2시간 또는 3시간인 경우
                if time + 3 > 17: # 오후 6시를 초과하면  False 반환
                    return False
                elif lecture.timeTable[day][time + 2][0] is 'Occupied': # 배치하고자 하는 시간 다음 1시간에 이미 차있으면 False 반환
                    return False
                # 배치하고자 하는 시간에 해당 수업 속성 강의실 중 빈 곳이 없으면 False 반환
                if '이론' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][0] <= 0 or self.timeTableForWholeClassroom[day][time + 2][0] <= 0:
                        return False
                elif 'SW' in lecture.classProperty and len(lecture.classProperty) is 1:
                    if self.timeTableForWholeClassroom[day][time][1] <= 0 or self.timeTableForWholeClassroom[day][time + 2][0] <= 0:
                        return False
                elif 'HW' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][2] <= 0 or self.timeTableForWholeClassroom[day][time + 2][2] <= 0:
                        return False
                elif '담헌' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][3] <= 0 or self.timeTableForWholeClassroom[day][time + 2][3] <= 0:
                        return False

        return True # 위 사항에 아무것도 해당하지 않으면 True 반환


    # 배치하고자 하는 시간에 이미 관련된 수업 중 꿰차고 있는 시간이 있는지 검사 (두번째 수업 시간)
    def checkRelatedLecture_Second(self, lecture, day, time):
        # if time > 11: # 오후 4시 이상이면 False 반환
        #     return False
        if day is 2 and time > 9: # 수요일 오후 2시 이상이면 False 반환
            return False
        if day is 4 and time > 9: # 금요일 오후 2시 이상이면 False 반환
            return False

        # 점심시간 예외 : False 반환
        if lecture.classTime is 3:
            if time in (7, 8, 9):
                return False
        elif lecture.researchTime is 2:
            if time in (5, 6, 7, 8, 9):
                return False

        if lecture.timeTable[day][time][0] is 'Occupied': # 배치하고자 하는 시간이 이미 차있으면 False 반환
            return False
        else: # 배치하고자 하는 시간이 비어있는 경우
            if lecture.classTime is 3: # 이론 수업이 3시간인 경우
                if time + 1 > 17: # 오후 6시를 초과하면 False 반환
                    return False
                # 배치하고자 하는 시간에 해당 수업 속성 강의실 중 빈 곳이 없으면 False 반환
                if '담헌' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][3] <= 0:
                        return False
                else: # 이론
                    if self.timeTableForWholeClassroom[day][time][0] <= 0:
                        return False
            elif lecture.researchTime is 2: # 이론 수업 시간과 실습 수업 시간이 각 2시간인 경우
                if time + 3 > 17: # 오후 6시를 초과하면  False 반환
                    return False
                elif lecture.timeTable[day][time + 2][0] is 'Occupied': # 배치하고자 하는 시간 다음 1시간에 이미 차있으면 False 반환
                    return False
                # 배치하고자 하는 시간에 해당 수업 속성 강의실 중 빈 곳이 없으면 False 반환
                if 'SW' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][1] <= 0 or self.timeTableForWholeClassroom[day][time + 2][1] <= 0:
                        return False
                elif 'HW' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][2] <= 0 or self.timeTableForWholeClassroom[day][time + 2][2] <= 0:
                        return False
                elif '이론' in lecture.classProperty:
                    if self.timeTableForWholeClassroom[day][time][0] <= 0 or self.timeTableForWholeClassroom[day][time + 2][0] <= 0:
                        return False

        return True # 위 사항에 아무것도 해당하지 않으면 True 반환    


    # 부여된 시간 만큼 첫번째 수업 시간 할당 및 연관된 강의들에게도 할당된 시간에 이미 차있다고 알림
    def giveLectureFirstTime(self, lecture, day = 0, time = 0):
        if lecture.classTime is 1:
            lecture.timeTable[day][time][0] = 'Mine'
            lecture.timeTable[day][time + 1][0] = 'Mine'
            # 수업 배치를 했으니 수업 시간 총 강의실 시간표에서 해당 시간에 해당하는 수업 속성 1 감소
            self.decreaseClassroom(day, time, lecture.classProperty[0])
            self.decreaseClassroom(day, time + 1, lecture.classProperty[0])
            for relateLecture in lecture.relation:
                relateLecture.timeTable[day][time][0] = 'Occupied'
                relateLecture.timeTable[day][time + 1][0] = 'Occupied'
        else:
            lecture.timeTable[day][time][0] = 'Mine'
            lecture.timeTable[day][time + 1][0] = 'Mine'
            lecture.timeTable[day][time + 2][0] = 'Mine'
            lecture.timeTable[day][time + 3][0] = 'Mine'
            # 수업 배치를 했으니 수업 시간 총 강의실 시간표에서 해당 시간에 해당하는 수업 속성 1 감소
            self.decreaseClassroom(day, time, lecture.classProperty[0])
            self.decreaseClassroom(day, time + 1, lecture.classProperty[0])
            self.decreaseClassroom(day, time + 2, lecture.classProperty[0])
            self.decreaseClassroom(day, time + 3, lecture.classProperty[0])
            for relateLecture in lecture.relation:
                relateLecture.timeTable[day][time][0] = 'Occupied'
                relateLecture.timeTable[day][time + 1][0] = 'Occupied'
                relateLecture.timeTable[day][time + 2][0] = 'Occupied'
                relateLecture.timeTable[day][time + 3][0] = 'Occupied'

    # 부여된 시간 만큼 두번째 수업 시간 할당
    def giveLectureSecondTime(self, lecture, day, time):
        if lecture.classTime is 3:
            lecture.timeTable[day][time][0] = 'Mine'
            lecture.timeTable[day][time + 1][0] = 'Mine'
            # 수업 배치를 했으니 수업 시간 총 강의실 시간표에서 해당 시간에 해당하는 수업 속성 1 감소
            self.decreaseClassroom(day, time, lecture.classProperty[0])
            self.decreaseClassroom(day, time + 1, lecture.classProperty[0])
            for relateLecture in lecture.relation:
                relateLecture.timeTable[day][time][0] = 'Occupied'
                relateLecture.timeTable[day][time + 1][0] = 'Occupied'
        elif lecture.researchTime is 2:
            lecture.timeTable[day][time][0] = 'Mine'
            lecture.timeTable[day][time + 1][0] = 'Mine'
            lecture.timeTable[day][time + 2][0] = 'Mine'
            lecture.timeTable[day][time + 3][0] = 'Mine'
            # 수업 배치를 했으니 수업 시간 총 강의실 시간표에서 해당 시간에 해당하는 수업 속성 1 감소
            if 'SW' in lecture.classProperty and '이론' in lecture.classProperty:
                self.decreaseClassroom(day, time, lecture.classProperty[1])
                self.decreaseClassroom(day, time + 1, lecture.classProperty[1])
                self.decreaseClassroom(day, time + 2, lecture.classProperty[1])
                self.decreaseClassroom(day, time + 3, lecture.classProperty[1])
            else:
                self.decreaseClassroom(day, time, lecture.classProperty[0])
                self.decreaseClassroom(day, time + 1, lecture.classProperty[0])
                self.decreaseClassroom(day, time + 2, lecture.classProperty[0])
                self.decreaseClassroom(day, time + 3, lecture.classProperty[0])

            for relateLecture in lecture.relation:
                relateLecture.timeTable[day][time][0] = 'Occupied'
                relateLecture.timeTable[day][time + 1][0] = 'Occupied'
                relateLecture.timeTable[day][time + 2][0] = 'Occupied'
                relateLecture.timeTable[day][time + 3][0] = 'Occupied'

    # 수업 속성별 강의실 시간표에서 강의실 수 1 감소
    def decreaseClassroom(self, day, time, property):
        if property in '이론':
            self.timeTableForWholeClassroom[day][time][0] -= 1
        if property in 'SW':
            self.timeTableForWholeClassroom[day][time][1] -= 1
        if property in 'HW':
            self.timeTableForWholeClassroom[day][time][2] -= 1
        if property in '담헌':
            self.timeTableForWholeClassroom[day][time][3] -= 1

    # 강의에서 이미 부여된 시간이 있으면 다음 요일 반환, 없으면 0 반환
    def checkLectureFirstTime(self, lecture):
        for day in range(0, 5):
            for time in range(18):
                if lecture.timeTable[day][time][0] is 'Mine':
                    return (day + 1)
        return 0

    # 하루에 특정 교수의 강의 시간이 4시간 넘는 날 제외한 날 반환, 없으면 0 반환
    def checkLectureTimeOfDay(self, lecture):
        timeofDay = []
        for day in range(5):
            increment = 0
            for relateLecture in lecture.relation:
                if relateLecture.professor is lecture.professor:
                    for time in range(18):
                        if relateLecture.timeTable[day][time][0] is 'Mine':
                            increment += 1
                        if lecture.timeTable[day][time][0] is 'Mine':
                            increment += 1
            timeofDay.append(increment)
        index = 0
        for time in timeofDay:
            if time < 6:
                return index
            else:
                index += 1
        return 0

    # 관련 있는 수업들 연관짓기
    def relateLectures(self):
        for lecture in self.LectureList:
            for targetLecture in self.LectureList:
                if targetLecture is lecture or targetLecture in lecture.relation:
                    continue
                if targetLecture.professor in lecture.professor:
                    index = self.LectureList.index(lecture)
                    self.LectureList[index].relation.append(targetLecture)
                elif targetLecture.track is not None and lecture.track is not None:
                    if targetLecture.track is lecture.track:
                        index = self.LectureList.index(lecture)
                        self.LectureList[index].relation.append(targetLecture)

        self.LectureList.sort(key = lambda lecture : len(lecture.relation))
        self.LectureList.reverse()
        

    # 과목 빼기
    def pBtn_excludeLecture_Clicked(self):
        selectedRow = self.tW_lectureInfo_2.currentRow()
        if selectedRow is not -1:
            for lecture in self.includedLectureList:
                if self.tW_lectureInfo_2.item(selectedRow,0).text() in lecture.classCode and self.tW_lectureInfo_2.item(selectedRow, 2).text() in lecture.classNum:
                    self.includedLectureList.remove(lecture)
                    for day in range(5):
                        for time in range(18):
                            if lecture.timeTable[day][time][0] is 'Mine':
                                self.includedLectureTime.remove((day, time))

            if self.tW_lectureInfo_2_row > 0 :
                self.tW_lectureInfo_2_row -= 1
            self.tW_lectureInfo_2.removeRow(selectedRow)

        self.tW_TimeTable_Update()

    # 과목 담기
    def pBtn_includeLecture_Clicked(self):
        selectedLecture = self.tW_lectureInfo.selectedItems()
        if len(selectedLecture) is not 0:
            if self.pBtn_includeLecture_check_Duplicate(selectedLecture) is False:
                tempTimeList = []
                for lecture in self.LectureList:
                    if selectedLecture[0].text() in lecture.classCode and selectedLecture[2].text() in lecture.classNum:
                        for day in range(5):
                            for time in range(18):
                                if lecture.timeTable[day][time][0] is 'Mine' and (day, time) not in self.includedLectureTime:
                                    tempTimeList.append((day, time))
                                elif lecture.timeTable[day][time][0] is 'Mine' and (day, time) in self.includedLectureTime:
                                    msg = QMessageBox()
                                    msg.setWindowTitle('중복')
                                    msg.setText('중복된 시간이 있어 추가할 수 없습니다.')
                                    msg.setStandardButtons(QMessageBox.Ok)
                                    msg.exec_()
                                    return None
                        self.includedLectureList.append(lecture)
                        self.includedLectureTime.extend(tempTimeList)
                        break
                self.tW_lectureInfo_2_row += 1          
                self.tW_lectureInfo_2.setRowCount(self.tW_lectureInfo_2_row)
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 0, QTableWidgetItem(selectedLecture[0].text()))
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 1, QTableWidgetItem(selectedLecture[1].text()))
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 2, QTableWidgetItem(selectedLecture[2].text()))
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 3, QTableWidgetItem(selectedLecture[3].text()))
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 4, QTableWidgetItem(selectedLecture[4].text()))
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 5, QTableWidgetItem(selectedLecture[5].text()))
                self.tW_lectureInfo_2.setItem(self.tW_lectureInfo_2_row - 1, 6, QTableWidgetItem(selectedLecture[6].text()))
        self.tW_TimeTable_Update()

    # 과목 담기 중 중복 확인
    def pBtn_includeLecture_check_Duplicate(self, selectedLecture):
        for row in range(self.tW_lectureInfo_2.rowCount()):
             if selectedLecture[0].text() in self.tW_lectureInfo_2.item(row, 0).text() and selectedLecture[2].text() in self.tW_lectureInfo_2.item(row, 2).text():
                return True
        return False

    # 시간표 갱신
    def tW_TimeTable_Update(self):
        self.tW_TimeTable.clearContents()
        self.tW_TimeTable.clearSpans()
        self.colorList.index = 0

        isDone = False # 수업 배치 Flag
        # 첫번째 수업 시간 배치
        for lecture in self.includedLectureList:
            tempColor = self.colorList.color[self.colorList.index]
            for day in range(5): # 요일마다
                if isDone is True:
                    isDone = False
                    break
                for time in range(0, 18, 2): # 1시간 간격마다
                    if lecture.timeTable[day][time][0] is 'Mine': # 해당 요일, 시간에 수업이 있는 경우
                        if lecture.classTime is not 1: # 첫번째 수업이 1시간이 아닌 경우
                            self.tW_TimeTable.setSpan(time, day, 4, 1)
                            if '캡스톤디자인' in lecture.className: # 공학설계, 졸업설계는 제외
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor))
                            else:
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor + ',' + lecture.classRoom[0].Building + '/' + str(lecture.classRoom[0].roomNum) + '호'))
                            self.tW_TimeTable.item(time, day).setBackground(QtGui.QColor(tempColor[0], tempColor[1], tempColor[2]))
                        else:
                            self.tW_TimeTable.setSpan(time, day, 2, 1)
                            if '캡스톤디자인' in lecture.className: # 공학설계, 졸업설계는 제외
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor))
                            else:
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor + ',' + lecture.classRoom[0].Building + '/' + str(lecture.classRoom[0].roomNum) + '호'))
                            self.tW_TimeTable.item(time, day).setBackground(QtGui.QColor(tempColor[0], tempColor[1], tempColor[2]))
                        isDone = True
                        self.colorList.index += 1
                        break

        self.colorList.index = 0
        # 두번째 수업 시간 배치
        for lecture in self.includedLectureList:
            tempColor = self.colorList.color[self.colorList.index]
            isDone = False # 수업 배치 Flag
            for day in range(self.checkLectureFirstTime(lecture), 5): # 요일마다
                if isDone is True:
                    isDone = False
                    break
                for time in range(0, 18, 2): # 1시간 간격마다
                    if lecture.timeTable[day][time][0] is 'Mine': # 해당 요일, 시간에 수업이 있는 경우
                        if lecture.researchTime is 2: # 실습이 있는 수업인 경우
                            self.tW_TimeTable.setSpan(time, day, 4, 1)
                            if '캡스톤디자인' in lecture.className: # 공학설계, 졸업설계는 제외
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor))
                            else:
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor + ',' + lecture.classRoom[1].Building + '/' + str(lecture.classRoom[1].roomNum) + '호'))
                            self.tW_TimeTable.item(time, day).setBackground(QtGui.QColor(tempColor[0], tempColor[1], tempColor[2]))
                        elif lecture.classTime is 3: # 이론만 있는 수업인 경우
                            self.tW_TimeTable.setSpan(time, day, 2, 1)
                            if '캡스톤디자인' in lecture.className: # 공학설계, 졸업설계는 제외
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor))
                            else:
                                self.tW_TimeTable.setItem(time, day, QTableWidgetItem(lecture.className + ',' + lecture.classNum + ',' + lecture.professor + ',' + lecture.classRoom[1].Building + '/' + str(lecture.classRoom[1].roomNum) + '호'))
                            self.tW_TimeTable.item(time, day).setBackground(QtGui.QColor(tempColor[0], tempColor[1], tempColor[2]))
                        isDone = True
                        self.colorList.index += 1
                        break

    # 강의 정보 테이블 설정
    def init_tW_lectureInfo(self):
        for lecture in self.LectureList:
            self.tW_lectureInfo_row += 1          
            self.tW_lectureInfo.setRowCount(self.tW_lectureInfo_row)

            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 0, QTableWidgetItem(lecture.classCode))
            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 1, QTableWidgetItem(lecture.className))
            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 2, QTableWidgetItem(lecture.classNum))
            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 3, QTableWidgetItem(lecture.professor))
            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 4, QTableWidgetItem(lecture.score))
            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 5, QTableWidgetItem(lecture.classLimit))
            self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 6, QTableWidgetItem(lecture.track))

    # 카테고리 리스트 설정
    def cB_Category_Changed(self):
        if self.cB_Category.currentIndex() is 0: # 전체
            self.cB_CategoryItem.clear()
            self.tW_lectureInfo.clearContents()
            self.tW_lectureInfo_row = 0
            for lecture in self.LectureList:
                self.tW_lectureInfo_row += 1          
                self.tW_lectureInfo.setRowCount(self.tW_lectureInfo_row)

                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 0, QTableWidgetItem(lecture.classCode))
                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 1, QTableWidgetItem(lecture.className))
                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 2, QTableWidgetItem(lecture.classNum))
                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 3, QTableWidgetItem(lecture.professor))
                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 4, QTableWidgetItem(lecture.score))
                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 5, QTableWidgetItem(lecture.classLimit))
                self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 6, QTableWidgetItem(lecture.track))
        elif self.cB_Category.currentIndex() is 1: # 과목명
            self.cB_CategoryItem.clear()
            for lecture in self.LectureList:
                if self.cB_CategoryItem.findText(lecture.className) is -1:
                    self.cB_CategoryItem.addItem(lecture.className)                               
        elif self.cB_Category.currentIndex() is 2: # 교수자
            self.cB_CategoryItem.clear()
            for lecture in self.LectureList:
                if self.cB_CategoryItem.findText(lecture.professor) is -1:
                    self.cB_CategoryItem.addItem(lecture.professor)

    # 카테코리 리스트에 따른 강의 테이블 정보 변경
    def cB_CategoryItem_Changed(self):
        self.tW_lectureInfo.clearContents()
        self.tW_lectureInfo_row = 0
        if self.cB_Category.currentIndex() is 1: # 과목명
            for lecture in self.LectureList:
                if lecture.className in str(self.cB_CategoryItem.currentText()):
                    self.tW_lectureInfo_row += 1          
                    self.tW_lectureInfo.setRowCount(self.tW_lectureInfo_row)

                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 0, QTableWidgetItem(lecture.classCode))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 1, QTableWidgetItem(lecture.className))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 2, QTableWidgetItem(lecture.classNum))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 3, QTableWidgetItem(lecture.professor))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 4, QTableWidgetItem(lecture.score))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 5, QTableWidgetItem(lecture.classLimit))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 6, QTableWidgetItem(lecture.track))
        elif self.cB_Category.currentIndex() is 2: # 교수자
            for lecture in self.LectureList:
                if lecture.professor in str(self.cB_CategoryItem.currentText()):
                    self.tW_lectureInfo_row += 1          
                    self.tW_lectureInfo.setRowCount(self.tW_lectureInfo_row)

                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 0, QTableWidgetItem(lecture.classCode))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 1, QTableWidgetItem(lecture.className))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 2, QTableWidgetItem(lecture.classNum))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 3, QTableWidgetItem(lecture.professor))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 4, QTableWidgetItem(lecture.score))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 5, QTableWidgetItem(lecture.classLimit))
                    self.tW_lectureInfo.setItem(self.tW_lectureInfo_row - 1, 6, QTableWidgetItem(lecture.track))



if __name__ == "__main__":
    app = QApplication(sys.argv)
    projectWindow = ProjectWindow()
    projectWindow.show()
    app.exec_()