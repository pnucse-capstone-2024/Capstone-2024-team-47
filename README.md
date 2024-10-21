### 1. 연구 소개
#### 1.1. 연구 배경
> 컴퓨터 산업이 발전함에 따라 컴퓨터 게임 산업 역시 급속도로 성장했다. 초기에 단순한 콘솔 게임으로 시작된 게임 산업은 하드웨어 성능의 향상과 네트워크 기술의 발전에 힘입어 다양한 장르의 고품질 게임을 제공하는 산업으로 성장했다. 특히 인터넷의 보급과 함께 멀티플레이어 게임에 대한 관심이 증가했으며, 네트워크를 통해 다른 사용자들과 실시간으로 게임을 즐길 수 있게 되었다. 이러한 환경 변화는 게임 개발에 있어 네트워크 통신 기술의 중요성을 부각하였고, 유니티(Unity)와 같은 게임 엔진이 멀티플레이어 게임 개발을 위한 다양한 네트워킹 기능을 제공하는 계기가 되었다.
멀티플레이어 게임의 수요 증가로 인해 관련 보안 기술의 필요성 또한 증가했다. 멀티플레이어 게임에서는 치트 및 해킹 방지가 중요하다. 일부 플레이어가 게임 내에서 불법적인 방법으로 부당한 이익을 취하려는 시도는 게임의 공정성을 훼손하고 다른 플레이어의 경험을 저해하기 때문이다. 이러한 문제는 특히 경쟁적인 요소가 강한 FPS나 MMORPG에서 더욱 두드러진다. FPS 장르의 경우, 에임봇(aimbot, 상대 위치에 플레이어의 에임이 고정됨)이나 월핵(wallhack, 벽 뒤의 상대 플레이어가 보임) 같은 치트 프로그램은 게임의 밸런스를 무너뜨릴 수 있다. MMORPG에서는 아이템 복제나 골드 파밍을 통해 게임 경제를 망치는 행위가 발생할 수 있다. 이외에도 게임 계정 정보 탈취나 개인 정보 유출을 노리는 피싱 공격도 증가하고 있어, 게임 보안 기술은 단순한 치트 방지를 넘어선 광범위한 보안 이슈를 다루게 되었다.
게임 보안 기술은 클라이언트 보안과 네트워크 보안으로 구분된다. 이 중 네트워크 보안은 웹 서비스의 네트워크 보안과 유사하지만, 멀티플레이어 게임은 웹 서비스보다 훨씬 낮은 네트워크 지연 속도를 유지해야 한다는 특성이 있다. 특히, FPS나 MMORPG와 같이 여러 유저가 실시간으로 상호작용을 해야 하는 경우 해당 특성이 더욱 부각된다. 하지만, 다양한 멀티플레이어 게임에서 어떤 암호 알고리즘이 구체적으로 적용되었는지는 크게 알려진 바가 없다. 따라서 본 연구에서는 멀티플레이어 게임에서 안전한 통신 환경을 구축하고, 각 블록 암호 알고리즘별 성능을 비교해 볼 것이다.

#### 1.2. 연구 목표
> 본 연구에서는 유니티를 기반으로 개발된 MMORPG 게임에서 안전한 게임 통신 환경을 구현하고, 경량 암호 알고리즘을 포함한 다양한 블록 암호 알고리즘을 적용하여 각 알고리즘의 성능을 비교하려 한다. 이를 위해 일반적인 MMORPG 서버 환경과 최대한 유사하게 구현된 서버와 클라이언트를 만들고, 보안 통신을 위한 핸드셰이크(Handshake) 과정을 수행한다. 이를 통해 멀티플레이어 게임 환경에서 적합한 암호 알고리즘을 탐색하고, 유니티를 활용한 멀티플레이어 게임 개발 시 적용할 수 있는 최적의 보안 솔루션을 제시하고자 한다.

<br>

#### 1.3. 주요 내용
##### 각 암호 알고리즘을 표준 및 구현 명세서에 맞게 구현
```
> 전자 서명 알고리즘 : ECDSA(P-256, SHA-256)
> 키 교환 알고리즘 : ECDH(P-256, SHA-256)
> 키 유도 함수 : HKDF(SHA-256)
> 블록 암호 알고리즘 : AES, ARIA, HIGHT, SPECK, TWINE
> 블록 암호 운영모드 : ECB, CBC, CFB, OFB, CTR, GCM.
```

> ![AlgorithmTest](https://github.com/user-attachments/assets/97e31878-173b-4025-96a9-11c00c1f7e66)

<br>

##### 실제 MMORPG 게임 환경을 위한 게임 서버 / 클라이언트 구현
```
> 서버: C# IOCP를 활용한 고성능/비동기 서버
> 클라이언트: Unity 2D
``` 
>
> ![Server](https://github.com/user-attachments/assets/7c73b25f-4b08-4d86-a389-d0f3d844d3a7)
>
> ![Client](https://github.com/user-attachments/assets/d51b687f-f3d6-49c0-8b61-4248a25e756d)

<br> 

##### 안전한 통신을 위한 Handshake Protocol 설계 및 구현 
```
> Cipher Suite 교환, 서명 인증, 키 교환 및 유도가 포함
```
> ![Handshake Protocol](https://github.com/user-attachments/assets/d29b655d-2eeb-4ebe-9d65-e9492e3a06f7)
>
> ![CA](https://github.com/user-attachments/assets/b2d9cc36-585d-40fa-b074-3a9033f37ead)

<br>

### 2. 상세설계
#### 2.1. 시스템 구성도
```
> 전통적인 서버 - 클라이언트 구조, 서버에 각 클라이언트가 연결됨
```
> ![ServerClientArchitecture](https://github.com/user-attachments/assets/3a03dee2-f535-40a7-a625-00333d3eb35b)

<br>

#### 2.1. 개발 환경
> * C# 12.0 (서버), C# 9.0 (클라이언트) 
> * Unity 2022.3.30f1 Personal
> * Visual Studio 2022 Community

<br>

### 3. 설치 및 사용 방법
    .
    ├── ...
    ├── dist                     # 실행 파일 폴더
    │   ├── Client1              # Client1 실행 파일 폴더
    │   │   ├── ...
    │   │   ├── Client1.exe      # Client1 실행 파일
    │   ├── Client2              # Client2 실행 파일 폴더
    │   │   ├── ...
    │   │   ├── Client2.exe      # Client2 실행 파일
    │   ├── Server               # Server 실행 파일 폴더
    │   │   ├── ...
    │   │   ├── Server.exe       # Server 실행 파일
    │   └── ...              
    └── ...
> 1. `Server.exe` 실행
> 2. `Client1.exe`, `Client2.exe` 실행 
> <br> 서버는 정해진 포트에 소켓이 바인딩되기에 하나만 실행되어야 함.
> <br> 클라이언트는 하나의 실행 파일을 여러 번 실행해도 됨.

이외 소스코드별 실행 방법은 `src/` 에 존재하는 각 폴더 참조

<br>

### 4. 소개 및 시연 영상
> 공사중...

<br>

### 5. 팀 소개
> * 이름: 고세화
> * 연락처: kosh7707@gmail.com 
> * 역할: 암호 알고리즘 구현, Handshake Protocol 설계 및 구현, 게임 서버 및 클라이언트 구현, 보안 통신 구현
