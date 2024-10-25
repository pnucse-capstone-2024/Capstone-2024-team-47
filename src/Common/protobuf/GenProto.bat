protoc.exe -I=./ --csharp_out=./ ./Enum.proto
if ERRORLEVEL 1 PAUSE

protoc.exe -I=./ --csharp_out=./ ./Struct.proto
if ERRORLEVEL 1 PAUSE

protoc.exe -I=./ --csharp_out=./ ./Protocol.proto 
if ERRORLEVEL 1 PAUSE




