# 개발 환경
> * C# 12.0 (서버), C# 9.0 (클라이언트) 
> * Unity 2022.3.30f1 Personal
> * Visual Studio 2022 Community
> * Protobuf v3.27.4
> <br>
> <br>솔루션(.sln) 파일을 여는 것이 편리하기에 Visual Studio 사용 추천.

<br>

## AlgorithmTest/
> 구현한 `암호 알고리즘 테스트` 
> <br> 테스트에 필요한 `테스트 벡터` 또한 포함됨.

<br>

## Server/
> `게임 서버`, `더미 클라이언트` 구현
> <br> `더미 클라이언트`는 각 블록 암호 알고리즘별 테스트를 용이하게 하기 위해 구현됨. 실제 클라이언트 아님.

<br>

## Client/
> 게임 클라이언트 구현
> <br> `Unity`로 개발되었기에 프로젝트 오픈에 `Unity`가 필요함.

<br>

## Common/
> `Protobuf` 사용 폴더
> <br> 통신에 필요한 모든 패킷을 정의함.