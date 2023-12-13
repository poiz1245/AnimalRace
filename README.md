### ⌨️개발 내용

### 1. 호스트로 로비 및 룸 생성

- Host-Client 모드로 게임모드 설정
- Host로 접속시 Lobby 생성
- RoomName 입력 받아 Room 생성(Maxuser 설정 가능)

### 2. 클라이언트로 생성된 룸에 참가

- 이미 생성되어 있는 RoomName 입력하여 Client로 Room 참가 가능

### 3. 동기화

- Room참가시 RoomUser생성하여 각 유저가 선택한 카트를 서로가 볼 수 있게 구현
- Room에 있는 User들의 상태를 체크하여 모두 Ready상태 일 때 호스트에게 Play버튼 활성화, Play버튼 클릭 시 Track씬으로 이동
- Photon Fusion의 MechanimAnimator 컴포넌트 사용하여 애니메이션 동기화
- RPC(원격프로시저호출)사용하여 각 플레이어 닉네임을 플레이어 오브젝트 위에 활성화
- RPC(원격프로시저호출)사용하여 피시쉬 라인을 가장 먼저 통과한 플레이어 이름 각 플레이어 UI에 활성화
