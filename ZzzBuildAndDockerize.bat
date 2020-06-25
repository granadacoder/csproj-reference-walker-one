

docker build -f scaffolding\docker\ConsoleApp.docker -t myconsoleapp/latest .

REM docker run -i --rm myconsoleapp/latest
REM OR
REM docker run --env ASPNETCORE_ENVIRONMENT=Development  -i --rm myconsoleapp/latest

REM No "port forwarding" with a console app
REM  -p 55555:52400