protoc.exe -I=./ --csharp_out=./ ./Enum_Security.proto
if ERRORLEVEL 1 PAUSE

protoc.exe -I=./ --csharp_out=./ ./Protocol_Security.proto 
if ERRORLEVEL 1 PAUSE



