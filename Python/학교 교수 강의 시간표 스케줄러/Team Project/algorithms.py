import numpy

class Node:
    def __init__(self, data):
        self.data = data
        self.next = None

class LinkedList:
    def __init__(self):
        dummy = Node("dummy")   # head를 dummy(무의미한 노드)로 설정
        self.head = dummy       # head   
        self.tail = dummy       # tail
        self.current = None     # 현재 노드
        self.before = None      # 이전 노드
        self.num_of_data = 0    # 연결된 노드 수

    def append(self, data):     # 데이터 추가
        new_node = Node(data)
        self.tail.next = new_node
        self.tail = new_node
        self.num_of_data += 1

    def delete(self):           # 데이터 삭제
        pop_data = self.current.data

        if self.current is self.tail:
            self.tail = self.before

        self.before.next = self.current.next
        self.current = self.before
        self.num_of_data -= 1

        return pop_data

    def first(self):            # 맨 앞의 노드 탐색
        if self.num_of_data == 0:
            return None

        self.before = self.head
        self.current = self.head.next
        
        return self.current.data
    
    def next(self):             # 다음 노드 탐색
        if self.current.next == None:
            return None
        
        self.before = self.current
        self.current = self.current.next
        
        return self.current.data
    
class Lecture:
    def __init__(self):
        self.classCode = ''          # 과목코드
        self.className = ''          # 과목명
        self.classRoom = []          # 강의실
        self.score = 0               # 학점
        self.classTime = 0           # 이론시간
        self.researchTime = 0        # 실습시간
        self.classProperty = []      # 수업속성(HW, SW, 이론)
        self. professor = ''         # 교수
        self.track = ''              # 트랙
        self.classNum = 0            # 분반
        self.classLimit = 0          # 수강정원
        self.relation = []           # 연관된 수업 목록
        self.timeTable = [[['Vacant'] for col in range(18)] for row in range(5)] # 시간표 (시간 여부, 강의실) 'Occupied' : 차있음, 'Vacant' : 비어있음, 'Mine' : 내 강의 시간

    def setClassRelation(self, relatedClass):
        self.relation.append(relatedClass) # 연관된 수업 목록

class ClassRoom:
    def __init__(self):
        self.Building = '' # 건물
        self.roomNum = '' # 강의실 번호
        self.accommodableLimit = 0 # 강의실 인원
        self.isResearch = False # 실습실 여부
        self.name = '' # 강의실명
        self.property = '' # 강의실 속성(HW, SW, 이론, 담헌)
        self.timeTable = [[None for col in range(18)] for row in range(5)] # 시간표 (시간 여부, 강의실) 'Occupied' : 차있음, 'Vacant' : 비어있음, 'Mine' : 내 강의 시간

class ColorList:
    def __init__(self):
        self.index = 0
        self.color = []

        i = 0
        while True:
            tempColor = list((numpy.random.choice(256), numpy.random.choice(256), numpy.random.choice(256)))
            if tempColor in self.color:
                continue
            else :
                self.color.append(tempColor)
                i += 1

            if i > 30 :
                break